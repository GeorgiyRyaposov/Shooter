using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Models;
using Assets.Game.Scripts.Domain.Signals;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class WeaponSystem : IDisposable
    {
        private WeaponsList _weaponsList;
        private ExplosionRangeVisual.Factory _explosionFactory;
        private SignalBus _signalBus;

        private LayerMask _enemyMask;
        private Collider[] _hits = new Collider[10];

        public WeaponSystem(WeaponsList weaponsList, LayerMask enemyMask, ExplosionRangeVisual.Factory explosionFactory, SignalBus signalBus)
        {
            _weaponsList = weaponsList;
            _enemyMask = enemyMask;
            _explosionFactory = explosionFactory;

            _signalBus = signalBus;
            _signalBus.Subscribe<EnemyDown>(OnEnemyDown);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDown>(OnEnemyDown);
        }

        public void CreateContexts()
        {
            GameContext.Current.Weapons = _weaponsList.Items.Select(x => new WeaponContext(x)).ToList();
        }

        public void SelectDefaultWeapon()
        {
            GameContext.Current.SelectedWeapon = GameContext.Current.Weapons[0];
        }

        public void SwitchWeapon(bool next)
        {
            var index = GameContext.Current.Weapons.IndexOf(GameContext.Current.SelectedWeapon);
            index += next ? 1 : -1;

            if (index < 0)
            {
                index = GameContext.Current.Weapons.Count - 1;
            }
            else if (index >= GameContext.Current.Weapons.Count)
            {
                index = 0;
            }

            GameContext.Current.SelectedWeapon = GameContext.Current.Weapons[index];
        }

        public bool CanFire()
        {
            return Time.time - GameContext.Current.SelectedWeapon.LastShotAt > GameContext.Current.SelectedWeapon.Model.FireDelay;
        }

        public void OnFire(Ray fireRay)
        {
            GameContext.Current.SelectedWeapon.Fire();
            GameContext.Current.SelectedWeapon.LastShotAt = Time.time;

            switch (GameContext.Current.SelectedWeapon.Model.Bullet.Type)
            {
                case BulletType.Common:
                    OnFireCommon(fireRay);
                    break;

                case BulletType.Explosive:
                    OnFireExplosive(fireRay);
                    break;
            }
        }

        //TODO: move to Bullet class, use strategy patter
        private void OnFireCommon(Ray fireRay)
        {
            Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 10);

            if (Physics.Raycast(fireRay, out var hit, float.MaxValue, _enemyMask))
            {                
                var damage = (GameContext.Current.SelectedWeapon.Model.Bullet as CommonBullet).Damage;
                ApplyDamage(hit.collider, damage);
            }
        }
        private void OnFireExplosive(Ray fireRay)
        {
            Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 10);
            
            //hit floor, wall, or enemy
            if (!Physics.Raycast(fireRay, out var floorHit, float.MaxValue))
            {
                return;
            }

            var range = (GameContext.Current.SelectedWeapon.Model.Bullet as ExplosiveBullet).ExplosionRange;
            var radius = range * 0.5f;
            var rocketHitPoint = floorHit.point;
                        
            var explosion = _explosionFactory.Create();
            explosion.Show(rocketHitPoint, range);

            //check if hitted enemy in range
            var hitsCount = Physics.OverlapSphereNonAlloc(rocketHitPoint, radius, _hits);
            if (hitsCount > 0)
            {
                //Debug.DrawRay(rocketHitPoint, Vector3.up * 5, Color.yellow, 10);

                var explosiveBullet = (GameContext.Current.SelectedWeapon.Model.Bullet as ExplosiveBullet);
                for (int i = 0; i < hitsCount; i++)
                {
                    var hit = _hits[i];
                    var hitPoint = hit.bounds.center;

                    var distance = Vector3.Distance(hitPoint, rocketHitPoint);
                    var damage = Mathf.RoundToInt(Mathf.Lerp(explosiveBullet.MaxDamage, explosiveBullet.MinDamage, 
                                                        distance / radius));
                    ApplyDamage(hit, damage);

                    if (hit.attachedRigidbody != null)
                    {
                        hit.attachedRigidbody.AddExplosionForce(damage * 50, rocketHitPoint, radius);
                    }

                    //Debug.DrawRay(hitPoint, Vector3.up * 2, Color.magenta, 10);
                    //UnityEngine.Debug.Log($"dist {distance} / range {radius} : {damage}");
                    //Debug.DrawLine(rocketHitPoint, hitPoint, Color.green, 10);
                }
            }
        }

        private void OnEnemyDown(EnemyDown enemyDown)
        {
            var weapon = GameContext.Current.Weapons.Find(x => x.Model.Bullet.Type == enemyDown.KilledBy);
            AddScorePoints(weapon);
        }

        private void AddScorePoints(WeaponContext weapon)
        {
            GameContext.Current.Points += weapon.Model.Bullet.ScorePoints;
        }

        private void ApplyDamage(Collider hit, int damage)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyDamage(damage, GameContext.Current.SelectedWeapon.Model.Bullet.Type);
            }
        }
    }
}

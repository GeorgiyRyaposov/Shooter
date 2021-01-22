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
        private SignalBus _signalBus;

        public WeaponSystem(WeaponsList weaponsList, SignalBus signalBus)
        {
            _weaponsList = weaponsList;

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

            GameContext.Current.SelectedWeapon.Model.Bullet.Fire(fireRay);
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

        public void ApplyDamage(Collider hit, int damage)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                var modifiedDamage = Mathf.RoundToInt(damage * GameContext.Current.DamageModifier);
                enemy.ApplyDamage(modifiedDamage, GameContext.Current.SelectedWeapon.Model.Bullet.Type);
            }
        }
    }
}

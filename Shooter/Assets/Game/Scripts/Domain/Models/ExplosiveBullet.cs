using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "ExplosiveBullet", menuName = "Data/ExplosiveBullet")]
    public class ExplosiveBullet : Bullet
    {
        public override BulletType Type => BulletType.Explosive;

        public int MinDamage;
        public int MaxDamage;
        public float ExplosionRange;

        [System.NonSerialized] private WeaponSystem _weaponSystem;
        [System.NonSerialized] ExplosionRangeVisual.Factory _explosionFactory;
        
        private readonly Collider[] _hits = new Collider[10];

        [Inject]
        public void Construct(WeaponSystem weaponSystem, ExplosionRangeVisual.Factory explosionFactory)
        {
            _weaponSystem = weaponSystem;
            _explosionFactory = explosionFactory;
        }

        public override void Fire(Ray fireRay)
        {
            Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 10);

            //hit floor, wall, or enemy
            if (!Physics.Raycast(fireRay, out var floorHit, float.MaxValue))
            {
                return;
            }

            var range = ExplosionRange;
            var radius = range * 0.5f;
            var rocketHitPoint = floorHit.point;

            _explosionFactory.Create(rocketHitPoint, range);

            //check if hitted enemy in range
            var hitsCount = Physics.OverlapSphereNonAlloc(rocketHitPoint, radius, _hits);
            if (hitsCount > 0)
            {
                //Debug.DrawRay(rocketHitPoint, Vector3.up * 5, Color.yellow, 10);

                for (int i = 0; i < hitsCount; i++)
                {
                    var hit = _hits[i];
                    var hitPoint = hit.bounds.center;

                    var distance = Vector3.Distance(hitPoint, rocketHitPoint);
                    var damage = Mathf.RoundToInt(Mathf.Lerp(MaxDamage, MinDamage, distance / radius));
                    _weaponSystem.ApplyDamage(hit, damage);

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
    }
}

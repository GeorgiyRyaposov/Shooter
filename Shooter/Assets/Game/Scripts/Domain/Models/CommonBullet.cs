using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "CommonBullet", menuName = "Data/CommonBullet")]
    public class CommonBullet : Bullet
    {
        public override BulletType Type => BulletType.Common;
        public int Damage = 10;

        [System.NonSerialized] private LayerMask _enemyMask;
        [System.NonSerialized] private WeaponSystem _weaponSystem;
        [System.NonSerialized] private ExplosionRangeVisual.Factory _explosionFactory;

        [Inject]
        public void Construct(LayerMask enemyMask, WeaponSystem weaponSystem, ExplosionRangeVisual.Factory explosionFactory)
        {
            _enemyMask = enemyMask;
            _weaponSystem = weaponSystem;
            _explosionFactory = explosionFactory;
        }

        public override void Fire(Ray fireRay)
        {
            Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 10);

            if (Physics.Raycast(fireRay, out var hit, float.MaxValue, _enemyMask))
            {
                _explosionFactory.Create(hit.point, 0.1f);
                _weaponSystem.ApplyDamage(hit.collider, Damage);
            }
        }
    }
}

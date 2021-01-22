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

        [Inject]
        public void Construct(LayerMask enemyMask, WeaponSystem weaponSystem)
        {
            _enemyMask = enemyMask;
            _weaponSystem = weaponSystem;
        }

        public override void Fire(Ray fireRay)
        {
            Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 10);

            if (Physics.Raycast(fireRay, out var hit, float.MaxValue, _enemyMask))
            {
                _weaponSystem.ApplyDamage(hit.collider, Damage);
            }
        }
    }
}

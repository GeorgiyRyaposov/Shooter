using UnityEngine;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "ExplosiveBullet", menuName = "Data/ExplosiveBullet")]
    public class ExplosiveBullet : Bullet
    {
        public int MinDamage;
        public int MaxDamage;
        public float ExplosionRange;
        public override BulletType Type => BulletType.Explosive;
    }
}

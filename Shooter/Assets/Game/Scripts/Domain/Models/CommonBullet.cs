using UnityEngine;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "CommonBullet", menuName = "Data/CommonBullet")]
    public class CommonBullet : Bullet
    {
        public override BulletType Type => BulletType.Common;
        public int Damage = 10;
    }
}

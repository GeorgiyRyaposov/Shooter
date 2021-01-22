using UnityEngine;

namespace Assets.Game.Scripts.Domain.Models
{
    public class Bullet : ScriptableObject
    {
        public virtual BulletType Type { get; }

        public int ScorePoints;

        public virtual void Fire(Ray fireRay) { }
    }

    public enum BulletType
    {
        Common,
        Explosive
    }
}

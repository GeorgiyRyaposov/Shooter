using Assets.Game.Scripts.Domain.Views;
using UnityEngine;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon")]
    public class Weapon : ScriptableObject
    {
        public WeaponView ViewPrefab;
        public Bullet Bullet;
        public float FireDelay;
    }
}

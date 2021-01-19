using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Domain.Models
{
    [CreateAssetMenu(fileName = "WeaponsList", menuName = "Data/WeaponsList")]
    public class WeaponsList : ScriptableObject
    {
        public List<Weapon> Items;
    }
}

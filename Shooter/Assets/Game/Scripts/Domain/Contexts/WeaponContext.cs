﻿using Assets.Game.Scripts.Core.Common;
using Assets.Game.Scripts.Domain.Models;

namespace Assets.Game.Scripts.Domain.Contexts
{
    public class WeaponContext : ScriptableContext<Weapon>
    {
        public float LastShotAt;

        public WeaponContext(Weapon weapon) : base(weapon)
        {
            
        }
    }
}

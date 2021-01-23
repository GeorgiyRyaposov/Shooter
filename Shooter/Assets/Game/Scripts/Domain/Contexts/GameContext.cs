using Assets.Game.Scripts.Core.Common;
using System.Collections.Generic;
using UniRx;

namespace Assets.Game.Scripts.Domain.Contexts
{
    public class GameContext : Context
    {
        public static GameContext Current;

        public List<WeaponContext> Weapons = new List<WeaponContext>();
        public ReactiveProperty<WeaponContext> SelectedWeapon = new ReactiveProperty<WeaponContext>();

        public IntReactiveProperty Points = new IntReactiveProperty();

        public float DamageModifier = 1f;
    }
}

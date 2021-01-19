using Assets.Game.Scripts.Core.Common;
using System.Collections.Generic;

namespace Assets.Game.Scripts.Domain.Contexts
{
    public class GameContext : Context
    {
        public static GameContext Current;

        public List<WeaponContext> Weapons = new List<WeaponContext>();

        private WeaponContext _selectedWeapon;
        public WeaponContext SelectedWeapon
        {
            get { return _selectedWeapon; }
            set 
            { 
                _selectedWeapon = value;
                Notify(WeaponChangedEvent);
            }
        }

        private int _points;
        public int Points
        {
            get { return _points; }
            set 
            {
                _points = value;
                Notify(PointsChangedEvent);
            }
        }

        #region events

        public const int WeaponChangedEvent = 0;
        public const int PointsChangedEvent = 1;

        #endregion
    }
}

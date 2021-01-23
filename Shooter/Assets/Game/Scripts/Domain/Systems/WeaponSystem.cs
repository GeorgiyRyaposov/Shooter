using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Models;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class WeaponSystem 
    {
        private readonly WeaponsList _weaponsList;

        public WeaponSystem(WeaponsList weaponsList)
        {
            _weaponsList = weaponsList;
        }

        public void CreateContexts()
        {
            GameContext.Current.Weapons = _weaponsList.Items.Select(x => new WeaponContext(x)).ToList();
        }

        public void SelectDefaultWeapon()
        {
            GameContext.Current.SelectedWeapon.Value = GameContext.Current.Weapons[0];
        }

        public void SwitchWeapon(bool next)
        {
            var index = GameContext.Current.Weapons.IndexOf(GameContext.Current.SelectedWeapon.Value);
            index += next ? 1 : -1;

            if (index < 0)
            {
                index = GameContext.Current.Weapons.Count - 1;
            }
            else if (index >= GameContext.Current.Weapons.Count)
            {
                index = 0;
            }

            GameContext.Current.SelectedWeapon.Value = GameContext.Current.Weapons[index];
        }

        public bool CanFire()
        {
            return Time.time - GameContext.Current.SelectedWeapon.Value.LastShotAt > GameContext.Current.SelectedWeapon.Value.Model.FireDelay;
        }

        public void OnFire(Ray fireRay)
        {
            GameContext.Current.SelectedWeapon.Value.LastShotAt = Time.time;
            GameContext.Current.SelectedWeapon.Value.Model.Bullet.Fire(fireRay);
        }

        public void ApplyDamage(Collider hit, int damage)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                var modifiedDamage = Mathf.RoundToInt(damage * GameContext.Current.DamageModifier);
                enemy.ApplyDamage(modifiedDamage, GameContext.Current.SelectedWeapon.Value.Model.Bullet.Type);
            }
        }
    }
}

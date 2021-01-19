using Assets.Game.Scripts.Domain.Models;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Data/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public WeaponsList WeaponsList;
        public LayerMask EnemyMask = 0;

        public override void InstallBindings()
        {
            Container.BindInstance(WeaponsList);
            Container.BindInstance(EnemyMask);
        }
    }
}

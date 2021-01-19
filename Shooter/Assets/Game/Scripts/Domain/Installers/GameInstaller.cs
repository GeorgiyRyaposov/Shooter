using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Signals;
using Assets.Game.Scripts.Domain.Systems;
using Assets.Game.Scripts.Domain.Views;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public Protagonist Protagonist;
        public ExplosionRangeVisual ExplosionRangeVisual;

        [Header("Ui")]
        public Transform UiRoot;
        public StartScreenView StartScreenView;
        public SettingsView SettingsView;
        public PointsView PointsView;

        public override void InstallBindings()
        {
            InstallSignals();

            //install ui
            Container.BindInstance(StartScreenView);
            Container.BindInstance(SettingsView);
            Container.BindInstance(PointsView);

            //install game systems
            Container.BindInterfacesAndSelfTo<SettingsSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInputSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponSystem>().AsSingle();

            Container.BindInstance(Protagonist);

            //install factories
            Container.BindFactory<ExplosionRangeVisual, ExplosionRangeVisual.Factory>()
                .FromComponentInNewPrefab(ExplosionRangeVisual)
                .WithGameObjectName("ExplosionRangeVisual")
                .UnderTransformGroup("ExplosionRangeVisuals");
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<NewGameStarted>();
            Container.DeclareSignal<EnemyDown>();
        }
    }
}
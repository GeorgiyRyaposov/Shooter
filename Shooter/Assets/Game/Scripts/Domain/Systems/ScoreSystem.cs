using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Signals;
using System;
using Zenject;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class ScoreSystem : IDisposable
    {
        private readonly SignalBus _signalBus;

        public ScoreSystem(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<EnemyDown>(OnEnemyDown);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDown>(OnEnemyDown);
        }

        private void OnEnemyDown(EnemyDown enemyDown)
        {
            var weapon = GameContext.Current.Weapons.Find(x => x.Model.Bullet.Type == enemyDown.KilledBy);
            AddScorePoints(weapon);
        }

        private void AddScorePoints(WeaponContext weapon)
        {
            GameContext.Current.Points.Value += weapon.Model.Bullet.ScorePoints;
        }
    }
}

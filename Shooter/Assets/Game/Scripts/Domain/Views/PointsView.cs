using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Signals;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Views
{
    public class PointsView : MonoBehaviour, IDisposable
    {
#pragma warning disable 0649
        [SerializeField] private TextMeshProUGUI _text;
#pragma warning restore 0649

        private SignalBus _signalBus;
        private CompositeDisposable _disposables = new CompositeDisposable();

        private GameContext _context;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _signalBus.Subscribe<NewGameStarted>(OnNewGameStarted);
        }

        public void Dispose()
        {
            _disposables.Clear();
            _signalBus.Unsubscribe<NewGameStarted>(OnNewGameStarted);
        }

        private void OnNewGameStarted()
        {
            Attach(GameContext.Current);
        }

        private void Attach(GameContext context)
        {
            _context = context;
            _context.Points.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x => _text.text = $"{x} pts.")
                    .AddTo(_disposables);
        }
    }
}

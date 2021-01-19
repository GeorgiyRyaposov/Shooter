using UnityEngine;
using TMPro;
using Assets.Game.Scripts.Core.Common;
using Assets.Game.Scripts.Domain.Contexts;
using Zenject;
using System;
using Assets.Game.Scripts.Domain.Signals;

namespace Assets.Game.Scripts.Domain.Views
{
    public class PointsView : MonoBehaviour, IContextObserver, IDisposable
    {
#pragma warning disable 0649
        [SerializeField] private TextMeshProUGUI _text;
#pragma warning restore 0649

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _signalBus.Subscribe<NewGameStarted>(OnNewGameStarted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<NewGameStarted>(OnNewGameStarted);
        }

        private void OnNewGameStarted()
        {
            Attach(GameContext.Current);
            RefreshText();
        }


        #region IObserver
        private IContext _context;
        public void Attach(IContext context)
        {
            _context = context;
            _context.Attach(this);
        }

        public void Detach()
        {
            _context?.Detach(this);
        }

        public void Refresh(int propertyId)
        {
            switch (propertyId)
            {
                case GameContext.PointsChangedEvent:
                    RefreshText();
                    break;

                default:
                    break;
            }
        }
        #endregion

        private void RefreshText()
        {
            _text.text = $"{GameContext.Current.Points} pts.";
        }
    }
}

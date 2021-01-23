using Assets.Game.Scripts.Domain.Contexts;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Domain.Views
{
    public class SettingsView : MonoBehaviour, IDisposable
    {
        public Button ApplyButton => _applyButton;
        public Button RestoreDefaultsButton => _restoreDefaultsButton;
        public Button QuitButton => _quitButton;

#pragma warning disable 0649
        [SerializeField] private CustomSliderView _movementSpeedSlider;
        [SerializeField] private CustomSliderView _mouseSensitivitySlider;
        [SerializeField] private CustomSliderView _jumpHeightSlider;
        [SerializeField] private Button _restoreDefaultsButton;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _quitButton;
#pragma warning restore 0649

        private CompositeDisposable _disposables = new CompositeDisposable();
        private SettingsContext _context;

        public void Attach(SettingsContext context)
        {
            _context = context;

            _movementSpeedSlider.AddListener((value) => _context.MovementSpeed.Value = value);
            _mouseSensitivitySlider.AddListener((value) => _context.MouseSensitivity.Value = value);
            _jumpHeightSlider.AddListener((value) => _context.JumpHeight.Value = value);

            SubscribeSliderToContext(_context.MovementSpeed, _movementSpeedSlider);
            SubscribeSliderToContext(_context.MouseSensitivity, _mouseSensitivitySlider);
            SubscribeSliderToContext(_context.JumpHeight, _jumpHeightSlider);
        }

        private void SubscribeSliderToContext(FloatReactiveProperty property, CustomSliderView view)
        {
            property.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x => view.SetValueWithoutNotify(x))
                    .AddTo(_disposables);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _disposables.Clear();
        }
    }
}

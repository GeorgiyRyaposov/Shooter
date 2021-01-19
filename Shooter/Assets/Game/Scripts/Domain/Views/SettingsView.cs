using Assets.Game.Scripts.Core.Common;
using Assets.Game.Scripts.Domain.Contexts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Domain.Views
{
    public class SettingsView : MonoBehaviour, IContextObserver
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

        private SettingsContext _context;

        public void Attach(IContext context)
        {
            Detach();

            _context = context as SettingsContext;

            _movementSpeedSlider.AddListener((value) => _context.MovementSpeed = value);
            _mouseSensitivitySlider.AddListener((value) => _context.MouseSensitivity = value);
            _jumpHeightSlider.AddListener((value) => _context.JumpHeight = value);
            
            RefreshSliders();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Refresh(int propertyId)
        {
            switch (propertyId)
            {
                case SettingsContext.MovementSpeedEvent:
                    _movementSpeedSlider.SetValueWithoutNotify(_context.MovementSpeed);
                    break;

                case SettingsContext.MouseSensitivityEvent:
                    _mouseSensitivitySlider.SetValueWithoutNotify(_context.MouseSensitivity);
                    break;

                case SettingsContext.JumpHeightEvent:
                    _jumpHeightSlider.SetValueWithoutNotify(_context.JumpHeight);
                    break;
            }
        }
        private void RefreshSliders()
        {
            Refresh(SettingsContext.MovementSpeedEvent);
            Refresh(SettingsContext.MouseSensitivityEvent);
            Refresh(SettingsContext.JumpHeightEvent);
        }

        public void Detach()
        {
            _context?.Detach(this);
        }
    }
}

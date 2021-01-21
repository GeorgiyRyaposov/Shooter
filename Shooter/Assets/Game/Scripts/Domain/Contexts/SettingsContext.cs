using Assets.Game.Scripts.Core.Common;
using Assets.Game.Scripts.Domain.Models;
using UnityEngine;

namespace Assets.Game.Scripts.Domain.Contexts
{
    [System.Serializable]
    public class SettingsContext : Context<Settings>
    {
        //TODO: replace with UniRX
        public const int MovementSpeedEvent = 0;
        public const int MouseSensitivityEvent = 1;
        public const int JumpHeightEvent = 2;

        public SettingsContext() { }
        public SettingsContext(Settings settings) : base (settings) 
        {
            MovementSpeed = settings.MovementSpeed;
            MouseSensitivity = settings.MouseSensitivity;
            JumpHeight = settings.JumpHeight;
        }
        
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed
        {
            get { return _movementSpeed; }
            set 
            {
                var notify = _movementSpeed != value;
                _movementSpeed = value;

                if (notify)
                {
                    Notify(MovementSpeedEvent);
                }
            }
        }

        [SerializeField] private float _mouseSensitivity;
        public float MouseSensitivity
        {
            get { return _mouseSensitivity; }
            set 
            {
                var notify = _mouseSensitivity != value;
                _mouseSensitivity = value;

                if (notify)
                {
                    Notify(MouseSensitivityEvent);
                }
            }
        }

        [SerializeField] private float _jumpHeight;
        public float JumpHeight
        {
            get { return _jumpHeight; }
            set 
            {
                var notify = _jumpHeight != value;
                _jumpHeight = value;

                if (notify)
                {
                    Notify(JumpHeightEvent);
                }
            }
        }
    }
}

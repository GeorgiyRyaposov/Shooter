using Assets.Game.Scripts.Domain.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class GameInputSystem : ShooterInput.IPlayerActions
    {
        public event UnityAction FireEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction<bool> SwitchWeapon = delegate { };
        public event UnityAction<Vector2> MoveEvent = (v) => { };
        public event UnityAction<Vector2> LookEvent = (v) => { };
        public event UnityAction OpenMenuEvent = delegate { };

        private ShooterInput _shooterInput;

        public GameInputSystem()
        {
            _shooterInput = new ShooterInput();
            _shooterInput.Player.SetCallbacks(this);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                FireEvent.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                JumpEvent.Invoke();
            }
        }
        public void OnNextWeapon(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SwitchWeapon.Invoke(true);
            }
        }

        public void OnPreviousWeapon(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SwitchWeapon.Invoke(false);
            }
        }
        public void OnOpenMenu(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            { 
                OpenMenuEvent.Invoke(); 
            }
        }

        public void EnableGameplayInput()
        {
            _shooterInput.Player.Enable();
            _shooterInput.UI.Disable();
        }
        public void EnableUiInput()
        {
            _shooterInput.Player.Disable();
            _shooterInput.UI.Enable();
        }

        public bool IsStartKeyHit()
        {
            return _shooterInput.Player.Fire.triggered;
        }
    }
}

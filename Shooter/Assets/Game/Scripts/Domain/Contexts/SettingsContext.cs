using Assets.Game.Scripts.Core.Common;
using Assets.Game.Scripts.Domain.Models;
using UniRx;

namespace Assets.Game.Scripts.Domain.Contexts
{
    [System.Serializable]
    public class SettingsContext : Context<Settings>
    {
        public SettingsContext(Settings settings) : base (settings) 
        {
            MovementSpeed = new FloatReactiveProperty(settings.MovementSpeed);
            MouseSensitivity = new FloatReactiveProperty(settings.MouseSensitivity);
            JumpHeight = new FloatReactiveProperty(settings.JumpHeight);
        }
        
        public FloatReactiveProperty MovementSpeed;
        public FloatReactiveProperty MouseSensitivity;
        public FloatReactiveProperty JumpHeight;
    }
}

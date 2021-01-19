using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Domain.Views
{
    public class StartScreenView : MonoBehaviour
    {
        public Button StartGame;
        public Button Settings;
        public Button Quit;
        public Camera Camera;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

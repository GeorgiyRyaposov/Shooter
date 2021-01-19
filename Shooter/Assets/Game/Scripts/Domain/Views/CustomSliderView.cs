using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Domain.Views
{
    public class CustomSliderView : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _value;
#pragma warning restore 0649

        public void AddListener(UnityAction<float> call)
        {
            _slider.onValueChanged.RemoveAllListeners();

            _slider.onValueChanged.AddListener(UpdateText);
            _slider.onValueChanged.AddListener(call);
        }

        public void SetValueWithoutNotify(float value)
        {
            _slider.SetValueWithoutNotify(value);
            UpdateText(value);
        }

        private void UpdateText(float value)
        {
            _value.text = value.ToString("#0.0");
        }
    }
}

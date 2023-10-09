using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        public void SetMaxValue(float value)
        {
            slider.maxValue = value;
            SetValue(value);
        }

        public void SetValue(float value)
        {
            slider.value = value;
        }
    }
}

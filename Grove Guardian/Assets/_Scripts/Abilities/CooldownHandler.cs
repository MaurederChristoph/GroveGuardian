using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownHandler : MonoBehaviour {

    [SerializeField] private Slider _slider;

    public void StartCooldown(float cooldown) {
        StartCoroutine(UpdateSlider(cooldown));
    }

    private IEnumerator UpdateSlider(float cooldown) {
        var elapsedTime = 0.0f;
        var startValue = 1.0f;
        var endValue = 0.0f;

        while(elapsedTime < cooldown) {
            var currentSliderValue = Mathf.Lerp(startValue, endValue, elapsedTime / cooldown);
            _slider.value = currentSliderValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _slider.value = endValue;
    }
}

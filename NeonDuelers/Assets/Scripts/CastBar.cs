using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetCastTime(float castTime) {
        slider.maxValue = castTime;
        slider.value = 0f;

        fill.color = gradient.Evaluate(0f);
    }

    public void CastProgress(float curTime) {
        slider.value = slider.maxValue - curTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}

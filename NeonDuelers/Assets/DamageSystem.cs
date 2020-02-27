using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageType { Auto, Energy, Plasma }

public class DamageSystem : MonoBehaviour
{
    public static DamageSystem Instance;
    public DamageNumbers damageNumbers;
    public float defaultFontSize = 4;
    public float exponential = 0.1f;
    [Header ("Colors")]
    public Color autoColor;
    public Color energyColor;
    public Color plasmaColor;


    private void Awake() {
        if (!Instance) Instance = this;
    }


    public void SpawnDamageNumbers(float damage, DamageType damageType, Vector3 pos) {
        DamageNumbers numbers = Instantiate(damageNumbers, transform.position , Quaternion.identity, transform);
        numbers.rectTransform.position = pos + Vector3.up;
        switch (damageType) {
            case DamageType.Auto:
                numbers.text.color = autoColor;
                break;
            case DamageType.Energy:
                numbers.text.color = energyColor;
                break;
            case DamageType.Plasma:
                numbers.text.color = plasmaColor;
                break;
        }
        numbers.text.text = damage.ToString();
        numbers.text.fontSize = (exponential * Mathf.Pow(damage, 2f)) + defaultFontSize;
        numbers.animator.speed = Mathf.Pow(damage, exponential);
        Destroy(numbers.gameObject, 1/Mathf.Pow(damage, exponential));
    }
}

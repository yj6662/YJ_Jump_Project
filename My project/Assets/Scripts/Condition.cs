using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Value")] public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;

    [Header("UI")] public Image uiBar;

    private void Awake()
    {
        curValue = startValue;

        if (uiBar != null)
        {
            uiBar.fillAmount = GetPercentage();
        }
    }

    private void Update()
    {
        if (uiBar != null)
        {
            uiBar.fillAmount = GetPercentage();
        }
    }
    
    public float GetPercentage()
    {
        if (maxValue == 0) return 0f;
        return curValue / maxValue;
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffDuration : MonoBehaviour
{
    [Header("Duration")]
    private float totalDuration;
    private float remainDuration;
    private float buffEndTime;
    private bool isBuffing = false;

    [Header("UI")] public Image uiBar;

    private void Awake()
    {
        remainDuration = 0f;
        if (uiBar != null)
        {
            uiBar.fillAmount = GetPercentage();
        }

    }

    private void Update()
    {
        if (isBuffing)
        {
            remainDuration -= Time.deltaTime;
            UpdateUI();

            if (remainDuration <= 0f)
            {
                isBuffing = false;
                ClearUI();
            }
        }
    }
    
    private void UpdateUI()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private void ClearUI()
    {
        uiBar.fillAmount = 0f;
    }

    public void StartBuff(float duration)
    {
        isBuffing = true;
        totalDuration = duration;
        buffEndTime = Time.time + duration;
        remainDuration = duration;
        UpdateUI();
    }

    public float GetPercentage()
    {
        return remainDuration / totalDuration;
    }
}

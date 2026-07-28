using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private TextMeshProUGUI txtAdjust;
    [SerializeField] private Color[] color;

    private void Awake()
    {
        txtAdjust.gameObject.SetActive(false);
    }

    public void TickCountdownTimer() => txt.text = ConvertSecondsToTimeSpan(WifeSoul.instance.Resource_WifeSoul);

    public void TickAdjustmentTimer(bool operation, float val)
    {
        txtAdjust.text = ConvertSecondsToTimeSpan(val);
        txtAdjust.color = operation ? color[0] : color[1];
        
        // var fadeoutcolor = color;
        // fadeoutcolor.a = 0;
        // LeanTween.value(gameObject, updateValueExampleCallback, fadeoutcolor, color, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(2f);
    }

public void ChangeAdjustmentAlpha(Color alpha)
    {
        txtAdjust.color = alpha;
    }

    public static string ConvertSecondsToTimeSpan(float totalSeconds)
    {
        float m = TimeSpan.FromSeconds(totalSeconds).Minutes;
        float s = TimeSpan.FromSeconds(totalSeconds).Seconds;
        float ms = TimeSpan.FromSeconds(totalSeconds).Milliseconds;
        
        return m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");
    }
}


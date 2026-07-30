using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private TextMeshProUGUI txtAdjust;
    [SerializeField] private Color[] color;
    [SerializeField] private float textTransition;

    private void Awake()
    {
        txtAdjust.gameObject.SetActive(false);
    }
    
    public void TickCountdownTimer() => txt.text = ConvertSecondsToTimeSpan(WifeSoul.instance.Resource_WifeSoul);
    
    public void TickAdjustmentTimer(bool operation, float val)
    {
        if (LeanTween.isTweening(txtAdjust.gameObject))
            LeanTween.cancel(txtAdjust.gameObject);
        
        //1 = addition; 0 = subtraction
        txtAdjust.gameObject.SetActive(true);
        txtAdjust.text = operation ? "+" : "-" + ConvertSecondsToTimeSpan(val);
        
        Color currCol = txtAdjust.color = operation ? color[0] : color[1]; 
        txtAdjust.color = currCol;
        
        Color fadeoutColor = currCol;
        fadeoutColor.a = 0;
  
        LeanTween.value(txtAdjust.gameObject, ChangeAdjustmentAlpha, currCol, fadeoutColor, textTransition)
            .setEaseOutElastic();
        LeanTween.value(txtAdjust.gameObject, ChangeAdjustmentAlpha, fadeoutColor, currCol, textTransition)
            .setEaseOutElastic().setDelay(3f).setOnComplete(() => txtAdjust.gameObject.SetActive(false));
    }

    private void ChangeAdjustmentAlpha(Color val) => txtAdjust.color = val;

    private static string ConvertSecondsToTimeSpan(float totalSeconds)
    {
        float m = TimeSpan.FromSeconds(totalSeconds).Minutes;
        float s = TimeSpan.FromSeconds(totalSeconds).Seconds;
        float ms = TimeSpan.FromSeconds(totalSeconds).Milliseconds;
        
        return m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");
    }
}

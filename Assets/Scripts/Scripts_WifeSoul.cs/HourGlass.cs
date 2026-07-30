using UnityEngine;
using UnityEngine.UI;

public class HourGlass : MonoBehaviour
{

    [SerializeField] private Image hourGlassImage;
    [SerializeField] private Sprite[] hourGlassSprites;
    [SerializeField] private int time;
    
    public void Tick()
    {
       time = (int) Mathf.Clamp(((WifeSoul.instance.Resource_WifeSoul/WifeSoul.instance.Initial_WifeSoul)) * 10f, 0f, 9f);
        hourGlassImage.sprite = hourGlassSprites[time];
    }
}
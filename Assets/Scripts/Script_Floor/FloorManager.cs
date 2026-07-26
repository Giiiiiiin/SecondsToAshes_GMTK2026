using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private List<Floor> floors;
    [SerializeField] private int currFloor = 0;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Start()
    {
        currFloor = 0;
        UpdateFloor();
    }
    public void UpdateFloor()
    {
        currFloor++;
        combatManager.UpdateFloorLogic(floors[currFloor].totalEnemies, floors[currFloor]);

        StartCoroutine(TransitionFloors());
    }

    private IEnumerator TransitionFloors()
    {
        yield return FadeRoutine(canvasGroup.alpha, 1f, 1f);
        
        if (currFloor >= 0)
            floors[currFloor-1].gameObject.SetActive(false);
        if (!floors[currFloor].gameObject.activeSelf && currFloor < floors.Count)
            floors[currFloor].gameObject.SetActive(true);

        yield return (FadeRoutine(canvasGroup.alpha, 0f, 1f));
    }
    
    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            yield return null; 
        }
        canvasGroup.alpha = targetAlpha;
    }
}
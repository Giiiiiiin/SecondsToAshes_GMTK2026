using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//This class is attached to the UI Canvas that contains all interfaces, IPointerHandlers enable detection of these ui components.
public class AnimatedCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D[] cursorFrames;
    private bool isMouseOverUI;

    private void Start()
    {
        Cursor.SetCursor(cursorFrames[0], Vector2.zero, CursorMode.Auto);
    }
    
    private void OnEnable()
    {
         Player_DataHandler.ShootGun.AddListener(() => StartCoroutine(ShootCursor()));
    }
    
    private void OnDisable()
    {
        Player_DataHandler.ShootGun.RemoveListener(() => StartCoroutine(ShootCursor()));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered Ui");
        isMouseOverUI = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited Ui");
        isMouseOverUI = false;
        Cursor.SetCursor(cursorFrames[0], Vector2.zero, CursorMode.Auto);
    }

    private IEnumerator ShootCursor()
    {
        if (isMouseOverUI)
            yield break;
        
        Cursor.SetCursor(cursorFrames[2], Vector2.zero, CursorMode.Auto);
            yield return new WaitForSeconds(0.2f);
        Cursor.SetCursor(cursorFrames[0], Vector2.zero, CursorMode.Auto);
    }
}

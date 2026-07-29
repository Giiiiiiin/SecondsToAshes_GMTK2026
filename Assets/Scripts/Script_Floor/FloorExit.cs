using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorExit : MonoBehaviour
{
    [SerializeField] private FloorManager manager;
    private bool hasEntered = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasEntered) return;
        
        hasEntered = true;
        Debug.Log("Player has exited.");
        manager.UpdateFloor();
        
    }
}
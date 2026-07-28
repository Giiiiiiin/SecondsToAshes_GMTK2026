using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorExit : MonoBehaviour
{
    [SerializeField] private FloorManager manager;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player has exited.");
        manager.UpdateFloor();
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorExit : MonoBehaviour
{
    [SerializeField] private FloorManager manager;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player has exited.");
        manager.UpdateFloor();
    }
}
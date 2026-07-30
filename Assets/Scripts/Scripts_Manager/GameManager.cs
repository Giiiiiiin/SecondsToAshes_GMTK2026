using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Player_DataHandler playerDataHandler;
    public CanvasGroup loseScreen;
    public CanvasGroup winScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseGame()
    {
        playerDataHandler.DisablePlayer();
        loseScreen.alpha = 1;
    }

    public void WinGame()
    { 
        playerDataHandler.DisablePlayer();
        winScreen.alpha = 1;
    }
}
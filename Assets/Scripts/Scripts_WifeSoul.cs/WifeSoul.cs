using System;
using UnityEngine;

public class WifeSoul : MonoBehaviour
{
    [SerializeField] private float initial_WifeSoul;
    [SerializeField] private float resource_WifeSoul;
    [SerializeField] private HourGlass glass;
    private bool canTick = false;
    
    public static WifeSoul instance;
        
    public float Resource_WifeSoul
    {
        get { return resource_WifeSoul; }
    }
    
    public float Initial_WifeSoul
    {
        get { return initial_WifeSoul; }
    }

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

        resource_WifeSoul = initial_WifeSoul;
        canTick = true;
    }

    public void ReduceWifeSoul(float value)
    {
        resource_WifeSoul -= value;
        glass.Tick();
        
        if (IsWifeSoulDepleted())
        {
            GameManager.instance.LoseGame();
        }
    }
    
    public void IncreaseWifeSoul(float value)
    {
        resource_WifeSoul += value;
    }

    public bool IsWifeSoulDepleted()
    {
        return resource_WifeSoul <= 0;
    }

    void Update()
    {
        if (canTick)
            ReduceWifeSoul(Time.deltaTime);
    }
    
}
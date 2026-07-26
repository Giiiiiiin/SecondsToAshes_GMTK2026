using System;
using UnityEngine;

public class WifeSoul : MonoBehaviour
{
    private float resource_WifeSoul;
    public static WifeSoul instance;
    public float Resource_WifeSoul
    {
        get { return resource_WifeSoul; }
        set { resource_WifeSoul = value; }
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
    }
}
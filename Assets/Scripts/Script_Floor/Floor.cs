using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject _floorExit;
    [SerializeField] private List<CombatBase> _enemyList;
    [SerializeField] private List<CombatBase> _bossList;
    public int totalEnemies;

    public void Awake()
    {
        totalEnemies = _enemyList.Count;
    }
    
    public void WinFloor() => _floorExit.gameObject.SetActive(false);
}
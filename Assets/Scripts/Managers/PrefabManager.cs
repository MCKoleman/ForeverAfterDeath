using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Prefabs")]
    public GameObject[] enemyPrefabList;
    public GameObject entrancePrefab;
    public GameObject exitPrefab;

    [Header("Holders")]
    // Object holders
    public Transform projectileHolder;
    public Transform enemyHolder;
    public Transform levelObjHolder;
    public Transform levelGeoHolder;
    public Transform bossHolder;

    // Initializes the manager. Should only be called from GameManager
    public void Init()
    {
        //ResetLevel();
    }

    // Resets all gameObjects
    public void ResetLevel()
    {
        for(int i = projectileHolder.childCount - 1; i >= 0; i--)
            Destroy(projectileHolder.GetChild(i).gameObject);

        for (int i = enemyHolder.childCount - 1; i >= 0; i--)
            Destroy(enemyHolder.GetChild(i).gameObject);

        for (int i = levelGeoHolder.childCount - 1; i >= 0; i--)
            Destroy(levelGeoHolder.GetChild(i).gameObject);

        for (int i = levelObjHolder.childCount - 1; i >= 0; i--)
            Destroy(levelObjHolder.GetChild(i).gameObject);

        for (int i = bossHolder.childCount - 1; i >= 0; i--)
            Destroy(bossHolder.GetChild(i).gameObject);
    }

    // Returns the number of enemies 
    public int GetEnemyCount() { return enemyHolder.childCount; }

    // Returns whether any bosses are alive
    public bool IsBossAlive() { return bossHolder != null && bossHolder.childCount > 0; }
}

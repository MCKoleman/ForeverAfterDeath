using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    [Header("Prefabs")]
    public GameObject[] enemyPrefabList;
    public GameObject[] wallPrefabList;
    // WallType { DEFAULT = 0, TOP = 1, SIDE = 2, TOP_DOOR = 3, SIDE_DOOR = 4, FLOOR = 5 }
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

    // Returns the wall object of the given type
    public GameObject GetWallObject(GlobalVars.WallType wallType)
    {
        return wallPrefabList[Mathf.Clamp((int)(wallType - 1), 0, wallPrefabList.Length)];
    }

    // Returns the number of enemies 
    public int GetEnemyCount() { return enemyHolder.childCount; }

    // Returns whether any bosses are alive
    public bool IsBossAlive() { return bossHolder != null && bossHolder.childCount > 0; }
}

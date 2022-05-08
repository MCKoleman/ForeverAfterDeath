using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelContentList", menuName = "ScriptableObjects/LevelContentList", order = 1)]
[System.Serializable]
public class LevelContentList : ScriptableObject
{
    [System.Serializable]
    public struct ContentGameObject
    {
        public GlobalVars.ContentType content;
        public GameObject obj;
        public ContentGameObject(GlobalVars.ContentType _content, GameObject _obj)
        {
            content = _content;
            obj = _obj;
        }
    }

    [Header("Content lists")]
    [SerializeField]
    private WeightedGameObjectList enemyList = new WeightedGameObjectList();
    [SerializeField]
    private WeightedGameObjectList treasureList = new WeightedGameObjectList();
    [SerializeField]
    private WeightedGameObjectList wallObjList = new WeightedGameObjectList();

    [Header("List weights")]
    [SerializeField]
    private int nothingWeight;
    [SerializeField]
    private int enemyWeight;
    [SerializeField]
    private int treasureWeight;
    [SerializeField]
    private float wallObjWeight = 0.3f;

    [Header("Base weights")]
    [SerializeField, Range(0f, 1f)]
    private float baseNothingWeight = 1f;
    [SerializeField, Range(0f, 1f)]
    private float baseEnemyWeight = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float baseTreasureWeight = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float baseWallObjWeight = 0.1f;

    [Header("Room percent weights")]
    [SerializeField]
    private float enemyMod;
    [SerializeField]
    private float treasureMod;

    // Returns a random gameObject from all the lists based on the given random values
    public ContentGameObject GetRandomContent(float roomPercent, GlobalVars.NodePlace place)
    {
        // Change content generation based on node type
        switch (place)
        {
            case GlobalVars.NodePlace.CENTER:
                return GetRandomContent(roomPercent);
            case GlobalVars.NodePlace.EDGE:
                return GetRandomContent(roomPercent);
            case GlobalVars.NodePlace.CORNER:
                return GetRandomContent(roomPercent);
            case GlobalVars.NodePlace.WALL:
                return GetRandomWallContent(roomPercent);
            case GlobalVars.NodePlace.NORMAL:
                return GetRandomContent(roomPercent);
            default:
                return new ContentGameObject(GlobalVars.ContentType.NOTHING, null);
        }
    }

    // Gets a random wall object
    private ContentGameObject GetRandomWallContent(float roomPercent)
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand < baseWallObjWeight * Mathf.Pow(1 + wallObjWeight, 1 + roomPercent))
            return new ContentGameObject(GlobalVars.ContentType.WALL, wallObjList.GetRandomObject());
        else
            return new ContentGameObject(GlobalVars.ContentType.NOTHING, null);
    }

    // Gets random content with the given room percent
    private ContentGameObject GetRandomContent(float roomPercent)
    {
        // Find modifiers
        float kN = baseNothingWeight * nothingWeight;
        float kE = (baseEnemyWeight + GenManager.Instance.GetLevelNum() * enemyMod) * enemyWeight;
        float kT = Mathf.Clamp(baseTreasureWeight + GenManager.Instance.GetLevelNum() * treasureMod, 0.0f, 1.0f) * treasureWeight;

        float rand = Random.Range(0, kN + kE + kT);

        // Get random enemy
        if (rand < kE)
        {
            return new ContentGameObject(GlobalVars.ContentType.ENEMY, enemyList.GetRandomObject());
        }
        rand -= kE;

        // Get random treasure
        if (rand < kT)
        {
            return new ContentGameObject(GlobalVars.ContentType.TREASURE, treasureList.GetRandomObject());
        }

        // Spawn nothing
        return new ContentGameObject(GlobalVars.ContentType.NOTHING, null);
    }
}

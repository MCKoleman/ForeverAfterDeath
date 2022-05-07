using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GlobalVars : Singleton<GlobalVars>
{
    public enum SelectedAbility { DEFAULT = 0, ABILITY_1 = 1, ABILITY_2 = 2, ABILITY_3 = 3 };

    public static SelectedAbility GetNextAbility(SelectedAbility sel, float dir)
    {
        int maxVal = Enum.GetValues(typeof(SelectedAbility)).Cast<int>().Max();

        // Select next direction upward
        if (dir > 0.0f)
        {
            if ((byte)sel + 1 <= maxVal)
                return sel + 1;
            else
                return (SelectedAbility)1;
        }
        // Select next direction downward
        else if (dir < 0.0f)
        {
            if ((byte)sel - 1 > 0)
                return sel - 1;
            else
                return (SelectedAbility)maxVal;
        }
        // Don't change selection
        else
        {
            return sel;
        }
    }

    [System.Serializable]
    public enum ContentType { NOTHING, ENEMY, TREASURE, HAZARD, WALL };
    [System.Serializable]
    public enum NodePlace { NORMAL = 0, EDGE = 1, CORNER = 2, CENTER = 3, WALL = 4 }

    [System.Serializable]
    public enum Direction { MIDDLE = 0, TOP = 1, BOTTOM = 2, RIGHT = 3, LEFT = 4 }

    // NONE = 0_0000_0001,
    // TOP = 0_0000_0010, BOTTOM = 0_0000_0100, RIGHT = 0_0000_1000, LEFT = 0_0001_0000,
    // NOTOP = 0_0010_0000, NOBOTTOM = 0_0100_0000, NORIGHT = 0_1000_0000, NOLEFT = 1_0000_0000
    [System.Serializable]
    public enum RoomReq
    {
        NONE = 1,
        TOP_DOOR = 2, BOTTOM_DOOR = 4, RIGHT_DOOR = 8, LEFT_DOOR = 16,
        TOP_WALL = 32, BOTTOM_WALL = 64, RIGHT_WALL = 128, LEFT_WALL = 256,
        TOP_OPEN = 512, BOTTOM_OPEN = 1024, RIGHT_OPEN = 2048, LEFT_OPEN = 4096
    }

    // Returns whether the room with given flagType requires a special room
    public static bool DoesRequireSpecialReq(uint reqFlag)
    {
        // Open rooms are guaranteed to have a value of 512 or greater
        return reqFlag >= (uint)RoomReq.TOP_OPEN;
    }

    // Locks the requirement flag to not allow any variation, ie. if top is not required, don't allow it
    public static uint LockFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the TOP bit is 0, set the NOTOP bit to 1
        if ((newFlag & ((int)RoomReq.TOP_DOOR)) == 0)
            newFlag |= ((int)RoomReq.TOP_WALL);

        // If the BOTTOM bit is 0, set the NOBOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.BOTTOM_DOOR)) == 0)
            newFlag |= ((int)RoomReq.BOTTOM_WALL);

        // If the RIGHT bit is 0, set the NORIGHT bit to 1
        if ((newFlag & ((int)RoomReq.RIGHT_DOOR)) == 0)
            newFlag |= ((int)RoomReq.RIGHT_WALL);

        // If the LEFT bit is 0, set the NOLEFT bit to 1
        if ((newFlag & ((int)RoomReq.LEFT_DOOR)) == 0)
            newFlag |= ((int)RoomReq.LEFT_WALL);

        return newFlag;
    }

    // Locks the requirement flag to allow maximum variation, ie. if top is allowed, require it
    public static uint LockInverseFlagReqs(uint reqFlag)
    {
        uint newFlag = reqFlag;

        // If the NOTOP bit is 0, set the TOP bit to 1
        if ((newFlag & ((int)RoomReq.TOP_WALL)) == 0)
            newFlag |= ((int)RoomReq.TOP_DOOR);

        // If the NOBOTTOM bit is 0, set the BOTTOM bit to 1
        if ((newFlag & ((int)RoomReq.BOTTOM_WALL)) == 0)
            newFlag |= ((int)RoomReq.BOTTOM_DOOR);

        // If the NORIGHT bit is 0, set the RIGHT bit to 1
        if ((newFlag & ((int)RoomReq.RIGHT_WALL)) == 0)
            newFlag |= ((int)RoomReq.RIGHT_DOOR);

        // If the NOLEFT bit is 0, set the LEFT bit to 1
        if ((newFlag & ((int)RoomReq.LEFT_WALL)) == 0)
            newFlag |= ((int)RoomReq.LEFT_DOOR);

        return newFlag;
    }

    // Calculate the requirements flag from the given requirements
    public static uint CalcReqFlagFromReqs(List<RoomReq> _reqs)
    {
        // Make sure all elements are unique
        uint _reqFlag = 0;
        _reqs = _reqs.Distinct().ToList();

        // Add all elements of the room requirements list
        for (int i = 0; i < _reqs.Count; i++)
        {
            _reqFlag += (uint)_reqs[i];
        }

        return _reqFlag;
    }

    // Returns the negated requirement of the one given
    public static RoomReq GetNegatedReq(RoomReq req)
    {
        switch (req)
        {
            case RoomReq.TOP_DOOR:
                return RoomReq.TOP_WALL;
            case RoomReq.BOTTOM_DOOR:
                return RoomReq.BOTTOM_WALL;
            case RoomReq.RIGHT_DOOR:
                return RoomReq.RIGHT_WALL;
            case RoomReq.LEFT_DOOR:
                return RoomReq.LEFT_WALL;
            case RoomReq.TOP_WALL:
                return RoomReq.TOP_DOOR;
            case RoomReq.BOTTOM_WALL:
                return RoomReq.BOTTOM_DOOR;
            case RoomReq.RIGHT_WALL:
                return RoomReq.RIGHT_DOOR;
            case RoomReq.LEFT_WALL:
                return RoomReq.LEFT_DOOR;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }

    // Returns the opposite requirement of the one given
    public static RoomReq GetOppositeReq(RoomReq req)
    {
        switch (req)
        {
            case RoomReq.TOP_DOOR:
                return RoomReq.BOTTOM_DOOR;
            case RoomReq.BOTTOM_DOOR:
                return RoomReq.TOP_DOOR;
            case RoomReq.RIGHT_DOOR:
                return RoomReq.LEFT_DOOR;
            case RoomReq.LEFT_DOOR:
                return RoomReq.RIGHT_DOOR;
            case RoomReq.TOP_WALL:
                return RoomReq.BOTTOM_WALL;
            case RoomReq.BOTTOM_WALL:
                return RoomReq.TOP_WALL;
            case RoomReq.RIGHT_WALL:
                return RoomReq.LEFT_WALL;
            case RoomReq.LEFT_WALL:
                return RoomReq.RIGHT_WALL;
            case RoomReq.NONE:
            default:
                return RoomReq.NONE;
        }
    }
}

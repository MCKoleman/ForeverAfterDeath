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
}

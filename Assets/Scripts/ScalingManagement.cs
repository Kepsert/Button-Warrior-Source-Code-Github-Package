using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingManagement : MonoBehaviour
{
    public int ExpRequiredToLevelUp(int level)
    {
        return Mathf.RoundToInt((level / 0.3f) * 5);
    }

    public int MaxHealthIncrease(int level)
    {
        return level * 2;
    }

    public int GetHealth(int level)
    {
        return 18 + (level * 2);
    }
}

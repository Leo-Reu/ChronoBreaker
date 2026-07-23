using UnityEngine;
using System;

[Serializable]
public class GameSaveData
{
    public bool isMidBossClear;
    public bool isFinalBossClear;
    public float bestClearTime;
    public int deathCount;

    public GameSaveData()
    {
        isMidBossClear = false;
        isFinalBossClear = false;
        bestClearTime = 9999f;
        deathCount = 0;
    }
}

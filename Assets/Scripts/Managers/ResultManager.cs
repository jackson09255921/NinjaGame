using System;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager
{
    static ResultManager instance;
    public static ResultManager Instance
    {
        get
        {
            return instance ??= new ResultManager();
        }
    }

    class LevelData
    {
        internal int attempts;
        internal int fails;
        internal float totalTime;
        internal float clearTime;
        internal int clearItemCount;
        internal int maxItemCount;
    }

    readonly Dictionary<string, LevelData> dataMap = new();

    public void ResetAll()
    {
        dataMap.Clear();
    }

    public void Reset(string level)
    {
        dataMap.Remove(level);
    }

    public void Restart(string level, float time)
    {
        if (!dataMap.TryGetValue(level, out LevelData data))
        {
            dataMap[level] = data = new();
        }
        data.attempts += 1;
        data.totalTime += time;
    }

    public void Clear(string level, float time, int itemCount, int maxItemCount)
    {
        if (!dataMap.TryGetValue(level, out LevelData data))
        {
            dataMap[level] = data = new();
        }
        data.attempts += 1;
        data.totalTime += time;
        data.clearTime = Math.Min(data.clearTime, time);
        data.clearItemCount = itemCount;
        data.maxItemCount = maxItemCount;
    }

    public float GetTotalTime(string level)
    {
        return dataMap.TryGetValue(level, out LevelData data) ? data.totalTime : 0;
    }
}
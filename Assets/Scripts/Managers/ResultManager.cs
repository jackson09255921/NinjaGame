using System;
using System.Collections.Generic;
using System.Linq;

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
        internal int clearItemCount;
        internal int totalItemCount;
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
        data.attempts++;
        data.totalTime = time;
    }

    public void Clear(string level, float time, int itemCount, int totalItemCount)
    {
        if (!dataMap.TryGetValue(level, out LevelData data))
        {
            dataMap[level] = data = new();
        }
        data.attempts++;
        data.totalTime = time;
        data.clearItemCount = itemCount;
        data.totalItemCount = totalItemCount;
    }

    public int GetAttempts(string level)
    {
        return dataMap.TryGetValue(level, out LevelData data) ? data.attempts : 0;
    }

    public float GetTotalTimeAll()
    {
        return dataMap.Values.Sum(d => d.totalTime);
    }

    public float GetTotalTime(string level)
    {
        return dataMap.TryGetValue(level, out LevelData data) ? data.totalTime : 0;
    }

    public int GetClearItemCountAll()
    {
        return dataMap.Values.Sum(d => d.clearItemCount);
    }

    public int GetTotalItemCountAll()
    {
        return dataMap.Values.Sum(d => d.totalItemCount);
    }
}
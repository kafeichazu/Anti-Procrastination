using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelFixedTasksDatabase", menuName = "Game/LevelFixedTasksDatabase")]
public class LevelFixedTasksDatabase : ScriptableObject
{
    [System.Serializable]
    public class oneFixedTasksData
    {
        //public int Day;
        public int slotID;
        public int taskCardID;// 任务卡片ID列表
    }
    [System.Serializable]
    public class oneLevelFixedTasksData
    {
        public List<oneFixedTasksData> taskInOneDay;// 任务卡片ID列表
    }
    public List<oneLevelFixedTasksData> levelData = new List<oneLevelFixedTasksData>();
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/LevelDatabase")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class oneLevelData
    {
        public int levelEnergy;
        public int levelMood;
        public List<int> taskCards;// 任务卡片ID列表
    }
    public List<oneLevelData> levelData = new List<oneLevelData>();
}
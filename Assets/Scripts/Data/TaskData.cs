using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 任务卡片信息
/// 卡片基本信息不可变
/// </summary>
[System.Serializable]
public struct TaskCardInfo
{
    [Tooltip("任务ID")]
    public int taskID;

    [Tooltip("任务卡片颜色")]
    public Color cardColor;

    [Tooltip("固定任务名字")]
    public string fixedTaskName;

    [Tooltip("计划任务名字")]
    public string scheduledTaskName;

    [Tooltip("持续任务的话连续几个时间块")]
    [Range(1, 12)]
    public int timeBlockNum;

    [Tooltip("需要多少点才完成")]
    [Range(0, 10)]
    public int successPoint;

    [Tooltip("能否添加心情骰子")]
    public bool canAddMood;

    [Tooltip("能否添加精力骰子")]
    public bool canAddEnergy;

    [Tooltip("消耗多少心情")]
    [Range(0, 10)]
    public int consumeMood;

    [Tooltip("消耗多少精力")]
    [Range(0, 10)]
    public int consumeEnergy;

    // 显式构造函数
    public TaskCardInfo(Color cardColor, int taskID = 0, string fixedTask = "", string scheduledTask = "", int timeBlockNum = 1,
                       int successPoint = 0,
                       int consumeMood = 0, int consumeEnergy = 0)
    {
        this.cardColor = cardColor;
        this.taskID = taskID;
        this.fixedTaskName = fixedTask;
        this.scheduledTaskName = scheduledTask;
        this.timeBlockNum = timeBlockNum;
        this.successPoint = successPoint;
        this.canAddMood = true;
        this.canAddEnergy = true;
        this.consumeMood = consumeMood;
        this.consumeEnergy = consumeEnergy;
    }
}

[CreateAssetMenu(fileName = "TaskDatabase", menuName = "Game/TaskDatabase")]
public class TaskData : ScriptableObject
{
    public List<TaskCardInfo> taskPools = new List<TaskCardInfo>();

    public TaskCardInfo GetTaskById(int id)
    {
        foreach (var task in taskPools)
        {
            if (task.taskID == id)
                return task;
        }
        
        Debug.LogWarning($"Task with ID {id} not found in the task pool.");
        return new TaskCardInfo(Color.white);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 任务卡片信息
/// 卡片基本信息不可变
/// </summary>
[System.Serializable]
public struct TaskCardInfo
{
    [Tooltip("固定任务名字")]
    public string fixedTask;
    
    [Tooltip("计划任务名字")]
    public string scheduledTask;
    
    [Tooltip("持续任务的话连续几个时间块")]
    public int timeBlockNum;
    
    [Tooltip("需要多少点才完成")]
    [Range(0, 10)]
    public int successPoint;
    
    [Tooltip("能否添加心情骰子")]
    [Range(0, 5)]
    public int canAddMood;
    
    [Tooltip("能否添加精力骰子")]
    [Range(0, 5)]
    public int canAddEnergy;
    
    [Tooltip("消耗多少心情")]
    [Range(0, 10)]
    public int consumeMood;
    
    [Tooltip("消耗多少精力")]
    [Range(0, 10)]
    public int consumeEnergy;

    // 显式构造函数
    public TaskCardInfo(string fixedTask = "", string scheduledTask = "", int timeBlockNum = 1,
                       int successPoint = 0, int canAddMood = 0, int canAddEnergy = 0,
                       int consumeMood = 0, int consumeEnergy = 0)
    {
        this.fixedTask = fixedTask;
        this.scheduledTask = scheduledTask;
        this.timeBlockNum = timeBlockNum;
        this.successPoint = successPoint;
        this.canAddMood = canAddMood;
        this.canAddEnergy = canAddEnergy;
        this.consumeMood = consumeMood;
        this.consumeEnergy = consumeEnergy;
    }
}

public class TaskCard : MonoBehaviour
//, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField]
    public TaskCardInfo taskCardInfo;
    public bool isInSlot = false;

    public void InitInfo(TaskCardInfo info)
    {
        taskCardInfo = info;
        ShowInfo();
    }

    public bool SetTaskCheck(GameObject slotObj)
    {
        Slot slot = slotObj.GetComponent<Slot>();
        //TODO 还没添加连续任务块处理
        if (slot.isEmpty)
        {
            //注入信息到Slot中
            slot.taskCardInfo = taskCardInfo;
            slot.taskCard = this.gameObject;
            slot.isEmpty = false;
            isInSlot = true;
            //设置当前物体的父物体为Slot并且隐藏
            this.transform.SetParent(slotObj.transform);
            this.gameObject.SetActive(false);

            ShowInfo();
            return true;
        }
        else
        {
            return false;
        }
    }

    //显示信息
    public void ShowInfo()
    {
        
    }
}

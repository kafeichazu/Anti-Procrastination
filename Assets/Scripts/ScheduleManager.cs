using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
/*public class TimeSlot
{
    public string fixedTask = "";  //固定任务名字
    public string scheduledTask = "";  //计划任务名字
    public bool isContinue = false;  //是否是持续任务（连续好几个时间块）
    public int successPoint = 0;  //需要多少点才完成
    public int inputPoint = 0;  //输入了多少点
    public int consumeMood = 0;  //消耗多少心情
    public int consumeEnergy = 0;  //消耗多少精力
}
*/

public class TimeSlot
{
    public int taskID;                // 任务ID
    public string type = "";          // 任务类型: "fixed"、"love"、"scheduled"
    public string taskName = "";      // 任务名字
    public bool isContinue = false;   // 是否是持续任务
    public int successPoint = 0;      // 成功所需点数
    public int inputPoint = 0;        // 实际输入点数
    public int consumeMood = 0;       // 消耗的心情
    public int consumeEnergy = 0;     // 消耗的精力
    public int gainMood = 0;          // 获得的心情
    public int gainEnergy = 0;        // 获得的精力
}


[System.Serializable]
public class DaySchedule
{
    public TimeSlot[] DayPlan = new TimeSlot[12];
}

public class ScheduleManager : MonoBehaviour
{
    private int currentExecuteIndex = 0;
    public UnityEngine.UI.Button executeButton;  // 你需要将按钮拖拽绑定到这个字段
    public DaySchedule[] weekSchedule = new DaySchedule[7];

    public AttributeManager attributeManager;
    public List<TimeSlot> arrangedDaySchedule = new List<TimeSlot>();
    public LogPanelManager logPanelManager;

    // 传进来一天12个slot的数据：todayTaskCards
    // 传进来这是第几天：index（0-6）
    public void UpdateDayTasks(TimeSlot[] todayTaskCards, int index)
    {
        if (todayTaskCards == null || todayTaskCards.Length != 12)
        {
            Debug.LogWarning("传入的 todayTaskCards 为空或长度不为 12");
            return;
        }

        if (index < 0 || index >= weekSchedule.Length)
        {
            Debug.LogWarning("Index 超出 weekSchedule 范围");
            return;
        }

        for (int i = 0; i < 12; i++)
        {
            weekSchedule[index].DayPlan[i] = todayTaskCards[i];
        }
        Debug.Log($"已更新 weekSchedule[{index}] 的任务计划");

        // 传进来之后直接调用整合工具函数
        arrangeDayPlan(index);
    }

    // 整理合并
    public void arrangeDayPlan(int index)
    {
        if (index < 0 || index >= weekSchedule.Length)
        {
            Debug.LogWarning("Index 超出 weekSchedule 范围");
            return;
        }

        arrangedDaySchedule.Clear(); // 清空原来的计划

        TimeSlot[] dayPlan = weekSchedule[index].DayPlan;

        int i = 0;
        while (i < dayPlan.Length)
        {
            TimeSlot current = dayPlan[i];

            // 如果当前是空任务，跳过
            if (string.IsNullOrEmpty(current.taskName))
            {
                i++;
                continue;
            }

            // 复制当前项
            TimeSlot merged = new TimeSlot
            {
                taskID = current.taskID,
                type = current.type,
                taskName = current.taskName,
                successPoint = current.successPoint,
                inputPoint = current.inputPoint,
                consumeMood = current.consumeMood,
                consumeEnergy = current.consumeEnergy,
                gainMood = current.gainMood,
                gainEnergy = current.gainEnergy,
                isContinue = false  // 无论是否合并，最终都设置为 false
            };

            int j = i + 1;

            // 查找后续可以合并的项（固定任务或计划任务一致，且 isContinue 为 true）
            while (j < dayPlan.Length)
            {
                TimeSlot next = dayPlan[j];

                bool canMerge =
                    next.isContinue && !string.IsNullOrEmpty(current.taskName) && current.taskID == next.taskID;

                if (canMerge)
                {
                    j++;
                }
                else
                {
                    break;
                }
            }

            arrangedDaySchedule.Add(merged);
            i = j; // 跳过已合并的部分
        }
        Debug.Log($"已整理 DayPlan[{index}] 为 {arrangedDaySchedule.Count} 个任务段");
        currentExecuteIndex = 0;

        if (executeButton != null)
            executeButton.interactable = arrangedDaySchedule.Count > 0;
    }
    
    public void executeNextTask()
    {
        if (currentExecuteIndex < arrangedDaySchedule.Count)
        {
            TimeSlot task = arrangedDaySchedule[currentExecuteIndex];
            executeTimeSlot(task.taskID, task.type, task.taskName, task.successPoint, task.inputPoint, task.consumeMood, task.consumeEnergy, task.gainMood, task.gainEnergy);
            currentExecuteIndex++;

            if (currentExecuteIndex >= arrangedDaySchedule.Count)
            {
                GameManager.Instance.EnterLevel(GameManager.Instance.level);
                Debug.Log("所有任务已执行完毕！");
                if (executeButton != null)
                    executeButton.interactable = false;  // 禁用按钮
            }
        }
    }

    public void executeTimeSlot(int taskID, string taskType, string taskName, int successPoint, int inputPoint, int consumeMood, int consumeEnergy, int gainMood, int gainEnergy)
    {
        int total = 0;
        logPanelManager.AddLog("执行" + taskName + "任务，进行成功判定");
        string resultLog = "骰出：[ ";

        for (int i = 0; i < inputPoint; i++)
        {
            int roll = Random.Range(0, 2);  // 0 或 1
            total += roll;
            if (roll == 1)
            {
                resultLog += "正 ";
            }
            else
            {
                resultLog += "反 ";
            }
        }

        resultLog += "]，点数：" + total + "，成功需求：" + successPoint;
        logPanelManager.AddLog(resultLog);
        AttributeManager.Instance.Consume(consumeMood, consumeEnergy);

        if (total >= successPoint)
        {
            if (taskType == "Fixed" || taskType == "Scheduled")
            {
                logPanelManager.AddLog("你没有拖延地完成了任务！");
                logPanelManager.AddLog("获得了" + gainMood + "点心情和" + gainEnergy + "点精力！");
                AttributeManager.Instance.Consume(-gainMood, -gainEnergy);
                if (taskType == "Scheduled")
                {
                    AttributeManager.Instance.successedScheduleTask += 1;
                }
            }
            else if (taskType == "Love")
            {
                logPanelManager.AddLog("你来啦！小美和你相处很开心！");
                logPanelManager.AddLog("获得了" + gainMood + "点心情和" + gainEnergy + "点精力！");
                AttributeManager.Instance.Consume(-gainMood, -gainEnergy);
                AttributeManager.Instance.successedLoveTask += 1;
            }
        }
        else
        {
            if (taskType == "Fixed")
            {
                logPanelManager.AddLog("呜呜，虽然勉勉强强完成了，但你耗费了更多的心情和精力");
                //1.2倍
                int moreConsumedMood = Mathf.CeilToInt(consumeMood * 0.2f);
                int moreConsumedEnergy = Mathf.CeilToInt(consumeEnergy * 0.2f);
                AttributeManager.Instance.Consume(moreConsumedMood, moreConsumedEnergy);
            }
            else if (taskType == "Scheduled")
            {
                GameManager.Instance.InstantiateTaskCardById(taskID);
                logPanelManager.AddLog("啊哦，这个任务完成不了，重新安排时间吧");
            }
            else if (taskType == "Love")
            {
                logPanelManager.AddLog("你在做什么啊，小美好伤心！！");
            }
        }

        //Debug.Log(resultLog);
        //logPanelManager.AddLog(resultLog);
        logPanelManager.AddLog("————————————————");
    }
}


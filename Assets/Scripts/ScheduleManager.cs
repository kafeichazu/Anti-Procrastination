using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class TimeSlot
{
    public string fixedTask = "";  //固定任务名字
    public string scheduledTask = "";  //计划任务名字
    public bool isContinue = false;  //是否是持续任务（连续好几个时间块）
    public int successPoint = 0;  //需要多少点才完成
    public int inputPoint = 0;  //输入了多少点
    public int consumeMood = 0;  //消耗多少心情
    public int consumeEnergy = 0;  //消耗多少精力
}

[System.Serializable]
public class DaySchedule
{
    public TimeSlot[] DayPlan = new TimeSlot[12];
}

public class ScheduleManager : MonoBehaviour
{
    public DaySchedule[] weekSchedule = new DaySchedule[7];

    public AttributeManager attributeManager;
    public List<TimeSlot> arrangedDaySchedule = new List<TimeSlot>();
    //public LogPanelManager logPanelManager;

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
            if (string.IsNullOrEmpty(current.fixedTask) && string.IsNullOrEmpty(current.scheduledTask))
            {
                i++;
                continue;
            }

            // 复制当前项
            TimeSlot merged = new TimeSlot
            {
                fixedTask = current.fixedTask,
                scheduledTask = current.scheduledTask,
                successPoint = current.successPoint,
                inputPoint = current.inputPoint,
                consumeMood = current.consumeMood,
                consumeEnergy = current.consumeEnergy,
                isContinue = false  // 无论是否合并，最终都设置为 false
            };

            int j = i + 1;

            // 查找后续可以合并的项（固定任务或计划任务一致，且 isContinue 为 true）
            while (j < dayPlan.Length)
            {
                TimeSlot next = dayPlan[j];

                bool canMerge =
                    next.isContinue &&
                    (
                        (!string.IsNullOrEmpty(current.fixedTask) && current.fixedTask == next.fixedTask) ||
                        (!string.IsNullOrEmpty(current.scheduledTask) && current.scheduledTask == next.scheduledTask)
                    );

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
        foreach (TimeSlot task in arrangedDaySchedule)
        {
            executeTimeSlot(task.successPoint, task.inputPoint);
        }
    }

    public void executeTimeSlot(int successPoint, int inputPoint)
    {
        int total = 0;
        string resultLog = "执行任务：结果为 [ ";

        for (int i = 0; i < inputPoint; i++)
        {
            int roll = Random.Range(0, 2);  // 0 或 1
            total += roll;
            resultLog += roll + " ";
        }

        resultLog += "]，总和：" + total + "，成功需求：" + successPoint;

        if (total >= successPoint)
        {
            resultLog += " → ✅ 成功！";
        }
        else
        {
            resultLog += " → ❌ 失败！";
        }

        Debug.Log(resultLog);
        //logPanelManager.AddLog(resultLog);
    }
}


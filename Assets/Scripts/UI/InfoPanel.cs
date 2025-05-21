using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public void ShowInfo(TaskCardInfo taskCardInfo)
    {
        // 显示任务信息
        Debug.Log($"Fixed Task: {taskCardInfo.fixedTask}");
        Debug.Log($"Scheduled Task: {taskCardInfo.scheduledTask}");
        Debug.Log($"Time Block Number: {taskCardInfo.timeBlockNum}");
        Debug.Log($"Success Point: {taskCardInfo.successPoint}");
        Debug.Log($"Can Add Mood: {taskCardInfo.canAddMood}");
        Debug.Log($"Can Add Energy: {taskCardInfo.canAddEnergy}");
        Debug.Log($"Consume Mood: {taskCardInfo.consumeMood}");
        Debug.Log($"Consume Energy: {taskCardInfo.consumeEnergy}");
    }
}

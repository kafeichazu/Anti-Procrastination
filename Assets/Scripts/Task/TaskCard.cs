using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskCard : MonoBehaviour
//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public TaskCardInfo taskCardInfo;
    public bool isInSlot = false;

    public void InitInfo(TaskCardInfo info)
    {
        taskCardInfo = info;
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
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

}

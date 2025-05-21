using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskCard : MonoBehaviour
//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    public TaskCardInfo taskCardInfo;
    public bool isInSlot = false;

    private Image cardImage;
    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    public void InitInfo(TaskCardInfo info)
    {
        taskCardInfo = info;
        Color newColor = taskCardInfo.cardColor;
        newColor.a = 1f;
        cardImage.color = newColor;
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

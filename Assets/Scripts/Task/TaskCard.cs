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

    public bool SetTaskCheck(GameObject slotObj, TaskCardInfo taskCardInfo)
    {
        Slot slot = slotObj.GetComponent<Slot>();
        //添加连续任务块处理
        if (GameManager.Instance.level - 1 != slot.day)
        {
            return false;
        }
        if (!slot.isEmpty)
        {
            return false;
        }

        if (taskCardInfo.timeBlockNum > 1)
        {
            bool haveSpace = true;
            int day = slot.day;
            int slotID = slot.slotID;
            for (int i = 0; i < taskCardInfo.timeBlockNum; i++)
            {
                if (!GameManager.Instance.slots[day][slotID + i].isEmpty)
                {
                    haveSpace = false;
                    break;
                }
            }
            if (haveSpace)
            {
                for (int i = 0; i < taskCardInfo.timeBlockNum; i++)
                {
                    var insertSlot = GameManager.Instance.slots[day][slotID + i];
                    insertSlot.InsertCard(taskCardInfo, true);
                }
                GameManager.Instance.slots[day][slotID].isEmpty = false;
                GameManager.Instance.infoPanel.gameObject.SetActive(false);
                isInSlot = true;
                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (taskCardInfo.timeBlockIndex == 1)
        {
            //注入信息到Slot中
            slot.InsertCard(taskCardInfo);
            GameManager.Instance.infoPanel.gameObject.SetActive(false);
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

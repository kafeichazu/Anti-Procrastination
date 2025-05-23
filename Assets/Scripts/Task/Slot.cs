using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public TaskCardInfo taskCardInfo;
    //public GameObject taskCardPrefab;
    //public GameObject taskCard;
    public bool isEmpty = true;

    private Image slotImage;
    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void InsertCard(TaskCardInfo info)
    {
        taskCardInfo = info;
        isEmpty = false;
        Color newColor = taskCardInfo.cardColor;
        newColor.a = 1f;
        slotImage.color = newColor;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !isEmpty)
        {
            GameManager.Instance.InstantiateTaskCard(taskCardInfo);
            isEmpty = true;
            taskCardInfo = new TaskCardInfo();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && !isEmpty)
        {
            ShowInfo();
        }

    }

    //如果有包含任务，显示信息
    public void ShowInfo()
    {
        //显示任务信息面板，并且可以调整骰子

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            //TODO 改成自己的信息面板，而不是任务卡信息显示的那个
        }
    }
}

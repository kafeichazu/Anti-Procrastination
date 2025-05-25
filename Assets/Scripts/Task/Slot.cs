
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{
    public int day;
    public int slotID;

    public TaskCardInfo taskCardInfo;
    public GameObject infoPanel;
    //public GameObject taskCardPrefab;
    //public GameObject taskCard;
    public bool isEmpty = true;

    private Image slotImage;
    public bool canEdit = true;
    private bool isEditing = false;
    private void Awake()
    {
        slotImage = GetComponent<Image>();
        infoPanel = GameManager.Instance.slotInfoPanel;
    }

    public void InsertCard(TaskCardInfo info, bool isEmpty = false)
    {
        taskCardInfo = info;
        this.isEmpty = isEmpty;
        Color newColor = taskCardInfo.cardColor;
        newColor.a = 1f;
        slotImage.color = newColor;
        InsertCardArray(taskCardInfo);
    }

    //将任务卡片信息插入到任务卡片数组中
    public void InsertCardArray(TaskCardInfo info, int moodDiceNum = 0, int energyDiceNum = 0)
    {
        TimeSlot timeSlot = new TimeSlot();
        //timeSlot.fixedTask = info.fixedTaskName;
        //timeSlot.scheduledTask = info.scheduledTaskName;
        timeSlot.taskID = info.taskID;
        timeSlot.type = info.taskType.ToString();
        timeSlot.taskName = info.fixedTaskName == "" ? info.scheduledTaskName : info.fixedTaskName;
        
        timeSlot.isContinue = info.timeBlockNum > 1;
        timeSlot.successPoint = info.successPoint;
        timeSlot.consumeMood = moodDiceNum == 0 ? info.consumeMood : moodDiceNum;
        timeSlot.consumeEnergy = energyDiceNum == 0 ? info.consumeEnergy : energyDiceNum;
        timeSlot.inputPoint = timeSlot.consumeMood + timeSlot.consumeMood;

        GameManager.Instance.todayTaskCards[slotID] = timeSlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEmpty) return;
        if (!canEdit) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CancelCardInsertion();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            isEditing = true;
            infoPanel.GetComponent<InfoPanel>().slot = this;
            ShowInfo();
        }
    }

    public void CancelCardInsertion()
    {
        infoPanel.SetActive(false);
        GameManager.Instance.InstantiateTaskCard(taskCardInfo);
        //isEditing = false;
        //isEmpty = true;
        int x = day;
        int y = slotID;
        int timeNum = taskCardInfo.timeBlockNum;
        for (int i = 0; i < timeNum; i++)
        {
            var insertedSlot = GameManager.Instance.slots[x][y + i];
            insertedSlot.isEditing = false;
            insertedSlot.isEmpty = true;
            insertedSlot.canEdit = true;
            insertedSlot.taskCardInfo = new TaskCardInfo();
            insertedSlot.slotImage.color = Color.white;
            //上传的TimeSlot也要改
            var timeSlot = new TimeSlot();
            timeSlot.taskName = "";
            GameManager.Instance.todayTaskCards[y + i] = timeSlot;
        }
    }

    //如果有包含任务，显示信息
    public void ShowInfo()
    {
        //显示任务信息面板，并且可以调整骰子
        infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().ShowInfo(taskCardInfo);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            ShowInfo();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEmpty) return;
        if (!isEditing)
        {
            infoPanel.SetActive(false);
        }
    }
}

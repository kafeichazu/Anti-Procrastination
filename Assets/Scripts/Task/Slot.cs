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
    private bool isEditing = false;
    private void Awake()
    {
        slotImage = GetComponent<Image>();
        infoPanel = GameManager.Instance.slotInfoPanel;
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
        if (isEmpty) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.Instance.InstantiateTaskCard(taskCardInfo);
            isEmpty = true;
            taskCardInfo = new TaskCardInfo();
            slotImage.color = Color.white;
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.LogWarning("123");
            isEditing = true;
            ShowInfo();
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

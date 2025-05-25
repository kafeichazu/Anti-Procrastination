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

    

}

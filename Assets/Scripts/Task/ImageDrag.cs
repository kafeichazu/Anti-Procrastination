using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using DG.Tweening;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class ImageDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler ,IPointerExitHandler //, IDropHandler， IBeginDragHandler
{
    private Transform panel;      
    // Scroll View上的Scroll Rect组件
    private ScrollRect scrollRect; 
    private TaskCard taskCard;

    void Awake()
    {
        taskCard = this.GetComponent<TaskCard>();
        panel = this.transform.root.transform.Find("Panel");
        //注意面板中默认创建的ScrollView中间有空格
        scrollRect = panel.transform.Find("ScrollView").GetComponent<ScrollRect>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.infoPanel.gameObject.SetActive(true);
        GameManager.Instance.infoPanel.GetComponent<InfoPanel>().ShowInfo(taskCard.taskCardInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.infoPanel.gameObject.SetActive(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        GameObject canvas = GameObject.Find("Canvas");
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.position = eventData.position;
    }

    // 结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        //处理item拖拽结束的逻辑处理
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool isOverSlot = false;
        bool haveSpace = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                haveSpace = taskCard.SetTaskCheck(result.gameObject);
                isOverSlot = true;
                break;
            }
        }

        if (!isOverSlot || !haveSpace)
        {
            this.transform.SetParent(scrollRect.content);
            this.transform.SetAsFirstSibling();
        }
        
        Debug.Log("--------拖拽子物体结束-----------");

    }
}

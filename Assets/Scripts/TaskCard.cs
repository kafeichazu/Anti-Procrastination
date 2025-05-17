using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        // 如果没有CanvasGroup组件，添加一个
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 记录原始位置和父物体
        originalPosition = rectTransform.position;
        originalParent = transform.parent;
        
        // 设置可以穿透UI事件，便于检测下方物体
        canvasGroup.blocksRaycasts = false;
        
        // 将物体移到层级顶部以便拖动时显示在最上层
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 根据鼠标移动更新位置
        // 使用Input.mousePosition或eventData.position
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢复可以接收事件
        canvasGroup.blocksRaycasts = true;
        
        // 检测是否放置在了有slot标签的物体上
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        bool placedOnSlot = false;
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                // 放在了slot上
                transform.SetParent(result.gameObject.transform);
                rectTransform.anchoredPosition = Vector2.zero; // 居中放置
                placedOnSlot = true;
                break;
            }
        }
        
        // 如果没有放在slot上，回到原位置
        if (!placedOnSlot)
        {
            transform.SetParent(originalParent);
            rectTransform.position = originalPosition;
        }
    }
}

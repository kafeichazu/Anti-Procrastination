using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NonDraggableScrollRect : ScrollRect
{
    // 禁用拖拽
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }

    // 反转鼠标滚轮方向
    public override void OnScroll(PointerEventData eventData)
    {
        // 获取当前滚轮输入
        Vector2 scrollDelta = eventData.scrollDelta;
        
        // 反转Y轴（垂直滚动）或X轴（水平滚动）
        scrollDelta.y *= -1; // 垂直方向反转
        // scrollDelta.x *= -1; // 如果需要水平方向反转，取消注释此行
        
        // 调用基类方法，但传入反转后的滚轮输入
        base.OnScroll(new PointerEventData(EventSystem.current)
        {
            scrollDelta = scrollDelta
        });
    }
}
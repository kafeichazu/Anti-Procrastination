using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using DG.Tweening;
using System.Collections.Generic;
using System;

public class ImageDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler//, IDropHandler
{
    /// <summary>
    /// 拖拽的偏移量
    /// </summary>
    private Vector3 _touchOffset; 
    /// <summary>
    /// 场景中的Panel，设置拖拽过程中的父物体
    /// </summary>
    private Transform _panel;      
    /// <summary>
    /// Scroll View上的Scroll Rect组件
    /// </summary>
    private ScrollRect _scrollRect; 
    /// <summary>
    /// 拖拽的是否是子物体
    /// </summary>
    private bool _isDragItem;      

    void Awake()
    {

        Input.multiTouchEnabled = false;    //限制多指拖拽

        _panel = this.transform.root.transform.Find("Panel");

        //注意面板中默认创建的ScrollView中间有空格
        _scrollRect = _panel.transform.Find("ScrollView").GetComponent<ScrollRect>();

        _isDragItem = false;

    }

   
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        Vector2 touchDeltaPosition = Vector2.zero;
#if UNITY_EDITOR
        float delta_x = Input.GetAxis("Mouse X");
        float delta_y = Input.GetAxis("Mouse Y");
        touchDeltaPosition = new Vector2(delta_x, delta_y);

#elif UNITY_ANDROID || UNITY_IPHONE
        touchDeltaPosition = Input.GetTouch(0).deltaPosition;  
#endif
        //通过touchDeltaPosition去判断你的手指（鼠标）的移动方向，是和Scroll View同方向还是拖拽的方向
        if (Mathf.Abs(touchDeltaPosition.x) < Mathf.Abs(touchDeltaPosition.y))
        {
            //在这里区分是拖拽Item还是ScrollRect
            _isDragItem = true;

            this.transform.SetParent(_panel);//设置Item的父物体，为什么要在一开始确定是拖拽Item后就设置父物体？？你可以注掉试试

            this.transform.SetAsLastSibling();//设置为同父物体的最从底层，也就是不会被其同级遮挡。

            Vector3 uguiPos = new Vector3();   //定义一个接收返回的ugui坐标
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.gameObject.GetComponent<RectTransform>(),
                 eventData.position, Camera.main, out uguiPos))
            {
                //计算图片中心和鼠标点的差值
                _touchOffset = this.transform.position - uguiPos;
            }
        }
        else
        {
            _isDragItem = false;
            if (_scrollRect != null)
                //调用Scroll的OnBeginDrag方法，有了区分，就不会被item的拖拽事件屏蔽
            _scrollRect.OnBeginDrag(eventData);
        }     
    }

    public void OnDrag(PointerEventData eventData)
    {
            //OnDrag的方法都是在OnBeginDrag中区分的。
        if (_isDragItem)
        {
            Vector3 pos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.gameObject.GetComponent<RectTransform>(),
                eventData.position, Camera.main, out pos))
            {
                this.transform.position = pos + _touchOffset;
            }
        }
        else
        {
            if (_scrollRect != null)
                _scrollRect.OnDrag(eventData);
        }
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDragItem)
        {
            //处理item拖拽结束的逻辑处理
            Debug.Log("--------拖拽子物体结束-----------");
        }
        else
        {
            //Scroll Rect 拖拽结束
            if (_scrollRect != null)
                _scrollRect.OnEndDrag(eventData);
        }

    }
}

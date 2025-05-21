using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 任务卡片信息
/// 卡片基本信息不可变
/// </summary>
[System.Serializable]
public struct TaskCardInfo
{
    [Tooltip("固定任务名字")]
    public string fixedTask;
    
    [Tooltip("计划任务名字")]
    public string scheduledTask;
    
    [Tooltip("是否是持续任务（连续好几个时间块）")]
    public bool isContinue;
    
    [Tooltip("需要多少点才完成")]
    [Range(0, 10)]
    public int successPoint;
    
    [Tooltip("能否添加心情骰子")]
    [Range(0, 5)]
    public int canAddMood;
    
    [Tooltip("能否添加精力骰子")]
    [Range(0, 5)]
    public int canAddEnergy;
    
    [Tooltip("消耗多少心情")]
    [Range(0, 10)]
    public int consumeMood;
    
    [Tooltip("消耗多少精力")]
    [Range(0, 10)]
    public int consumeEnergy;

    // 显式构造函数
    public TaskCardInfo(string fixedTask = "", string scheduledTask = "", bool isContinue = false,
                       int successPoint = 0, int canAddMood = 0, int canAddEnergy = 0,
                       int consumeMood = 0, int consumeEnergy = 0)
    {
        this.fixedTask = fixedTask;
        this.scheduledTask = scheduledTask;
        this.isContinue = isContinue;
        this.successPoint = successPoint;
        this.canAddMood = canAddMood;
        this.canAddEnergy = canAddEnergy;
        this.consumeMood = consumeMood;
        this.consumeEnergy = consumeEnergy;
    }
}

public class TaskCard : MonoBehaviour
//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    [SerializeField]
    public TaskCardInfo taskCardInfo;

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

    //注入信息到Slot中
    public void injectInfo(TaskCardInfo info)
    {
        taskCardInfo = info;
        ShowInfo();
    }

    //显示信息
    public void ShowInfo()
    {
        
    }
}

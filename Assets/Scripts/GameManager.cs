using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例实例
    private static GameManager _instance;
    public AttributeManager attributeManager;
    public ScheduleManager scheduleManager;
    public GameObject gameSlots;
    public Slot[][] slots;

    //懒得写UIManager了，暂时用这个顶顶
    public GameObject infoPanel;
    public GameObject slotInfoPanel;
    public GameObject Card;
    //装任务卡片的父物体
    public GameObject Content;

    //卡片数据
    public TaskData taskDataBase;
    //关卡数据
    public LevelData levelDataBase;
    //固定任务数据
    public LevelFixedTasksDatabase fixedDataBase;


    //维护一个未完成的任务卡片列表
    public List<TaskCardInfo> taskCards;
    //今日已安排的任务卡片数组
    public TimeSlot[] todayTaskCards;
    public int level = 1;

    // 公共访问器
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 确保场景中只有一个实例存在
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 设置实例
        _instance = this;

        todayTaskCards = new TimeSlot[12];
        for (int i = 0; i < todayTaskCards.Length; i++)
        {
            todayTaskCards[i] = new TimeSlot();
            Debug.Log($"{todayTaskCards[i].taskName} initialized.");
        }

        // 防止场景切换时被销毁
        DontDestroyOnLoad(gameObject);

        InitGameData();

        EnterLevel(1);
    }

    public void EnterLevel(int level)
    {
        Debug.Log($"Entering level {level}...");
        var ldata = levelDataBase.levelData[level - 1];
        //List<TaskCardInfo> levelTasks = new List<TaskCardInfo>();
        attributeManager.totalEnergy += ldata.levelEnergy;
        attributeManager.totalMood += ldata.levelMood;

        foreach (int taskId in ldata.taskCards)
        {
            InstantiateTaskCardById(taskId);
        }
    }

    // 实例化任务卡片
    public void InstantiateTaskCard(TaskCardInfo taskInfo)
    {
        GameObject card = Instantiate(Card, Content.transform);
        TaskCard taskCard = card.GetComponent<TaskCard>();
        if (taskCard != null)
        {
            taskCard.InitInfo(taskInfo);
        }
        taskCards.Add(taskInfo);
    }

    public void InstantiateTaskCardById(int taskId)
    {
        TaskCardInfo taskInfo = taskDataBase.GetTaskById(taskId);
        GameObject card = Instantiate(Card, Content.transform);
        TaskCard taskCard = card.GetComponent<TaskCard>();
        if (taskCard != null)
        {
            taskCard.InitInfo(taskInfo);
        }
        taskCards.Add(taskInfo);
    }



    // 初始化游戏管理器的方法
    private void InitGameData()
    {
        Debug.Log("GameManager initialized!");
        slots = new Slot[gameSlots.transform.childCount][];
        for (int i = 0; i < gameSlots.transform.childCount; i++)
        {
            slots[i] = new Slot[12];
        }
        InitSlotID();
        InitFixedTasks();
        InitLoveTasks();
    }

    public void InitFixedTasks()
    {
        // for (int i = 0; i < fixedDataBase.levelData.Length; i++)
        // {
        //     LevelFixedTasks levelFixedTasks = fixedDataBase.levelFixedTasks[i];
        //     foreach (int taskId in levelFixedTasks.taskCards)
        //     {
        //         TaskCardInfo taskInfo = taskDataBase.GetTaskById(taskId);
        //         InstantiateTaskCard(taskInfo);
        //     }
        // }
    }

    public void InitLoveTasks()
    {

    }

    private void InitSlotID()
    {
        for (int i = 0; i < gameSlots.transform.childCount; i++)
        {
            GameObject daySlots = gameSlots.transform.GetChild(i).gameObject; ;
            for (int j = 0; j < daySlots.transform.childCount; j++)
            {
                GameObject gameSlot = daySlots.transform.GetChild(j).gameObject;
                slots[i][j] = gameSlot.GetComponent<Slot>();
                gameSlot.GetComponent<Slot>().day = i;
                gameSlot.GetComponent<Slot>().slotID = j;
                gameSlot.gameObject.name = i + "_" + j;
            }
            daySlots.gameObject.name = i.ToString();
        }

    }


    /// <summary>
    /// 显示任务信息面板
    /// </summary>
    public void ShowInfo(TaskCardInfo taskInfo)
    {
        infoPanel.SetActive(true);
        infoPanel.GetComponent<InfoPanel>().ShowInfo(taskInfo);
    }
    //关闭任务信息面板
    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    public void StartToday()
    {
        Debug.Log("today Plan Num " + todayTaskCards.Length);
        scheduleManager.UpdateDayTasks(todayTaskCards, level - 1);
        level++;
    }

}
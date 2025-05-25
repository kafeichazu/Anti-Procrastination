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
    //恋爱任务列表
    public int[][] loveTasks;

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

        var oneDayLoveTask = loveTasks[level - 1];
        for (int j = 0; j < oneDayLoveTask.Length; j++)
        {
            if (oneDayLoveTask[j] != 6184)
            {
                TaskCardInfo taskInfo = taskDataBase.GetTaskById(oneDayLoveTask[j]);
                bool setCheck = SetTaskCheck(slots[level - 1][j], taskInfo, true, false);
            }
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
        InitLoveTasks();
        InitFixedTasks();
    }

    public void InitFixedTasks()
    {
        for (int i = 0; i < 7; i++)
        {
            var levelFixedTasks = fixedDataBase.levelData[i];
            foreach (var task in levelFixedTasks.taskInOneDay)
            {
                Debug.Log($"LevelFixedTasks Num {i}{levelFixedTasks.taskInOneDay.Count}");
                TaskCardInfo taskInfo = taskDataBase.GetTaskById(task.taskCardID);
                if (taskInfo.taskType == TaskType.Love)
                {
                    loveTasks[i][task.slotID] = task.taskCardID;
                }
                else
                {
                    bool setCheck = SetTaskCheck(slots[i][task.slotID], taskInfo, true, false);
                }
            }
        }
    }

    public void InitLoveTasks()
    {
        loveTasks = new int[7][];
        for (int i = 0; i < 7; i++)
        {
            loveTasks[i] = new int[12];
            for (int j = 0; j < 12; j++)
            {
                loveTasks[i][j] = 6184;
            }
        }
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
        //EnterLevel(level);
    }

    public bool SetTaskCheck(Slot slot, TaskCardInfo taskCardInfo, bool isInit = false, bool canEdit = true)
    {
        //添加连续任务块处理
        if (level - 1 != slot.day && !isInit)
        {
            return false;
        }
        if (!slot.isEmpty)
        {
            return false;
        }

        if (taskCardInfo.timeBlockNum > 1)
        {
            bool haveSpace = true;
            int day = slot.day;
            int slotID = slot.slotID;
            for (int i = 0; i < taskCardInfo.timeBlockNum; i++)
            {
                if (!slots[day][slotID + i].isEmpty)
                {
                    haveSpace = false;
                    break;
                }
            }
            if (haveSpace)
            {
                for (int i = 0; i < taskCardInfo.timeBlockNum; i++)
                {
                    var insertSlot = slots[day][slotID + i];
                    insertSlot.InsertCard(taskCardInfo, false);
                    insertSlot.canEdit = false;
                }
                //slots[day][slotID].isEmpty = false;
                slots[day][slotID].canEdit = canEdit;
                infoPanel.gameObject.SetActive(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (taskCardInfo.timeBlockIndex == 1)
        {
            //注入信息到Slot中
            slot.InsertCard(taskCardInfo);
            slot.canEdit = canEdit;
            infoPanel.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

}
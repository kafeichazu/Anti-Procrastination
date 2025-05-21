using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例实例
    private static GameManager _instance;

    //懒得写UIManager了，暂时用这个顶顶
    public GameObject infoPanel;
    public GameObject Card;
    //装任务卡片的父物体
    public GameObject Content;

    //卡片数据
    public TaskData taskDataBase;
    //关卡数据
    public LevelData levelDataBase;


    //维护一个未完成的任务卡片列表
    public List<TaskCardInfo> taskCards;
    //今日已安排的任务卡片列表
    public TaskCardInfo[] todayTaskCards;
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

        // 防止场景切换时被销毁
        DontDestroyOnLoad(gameObject);

        // 初始化游戏管理器
        InitGameManager();
        
        EnterLevel(level);
    }

    public void EnterLevel(int level)
    {
        Debug.Log($"Entering level {level}...");
        var ldata = levelDataBase.levelData[level - 1];
        //List<TaskCardInfo> levelTasks = new List<TaskCardInfo>();

        foreach (int taskId in ldata.taskCards)
        {
            TaskCardInfo taskInfo = taskDataBase.GetTaskById(taskId);
            GameObject card = Instantiate(Card, Content.transform);
            TaskCard taskCard = card.GetComponent<TaskCard>();
            if (taskCard != null)
            {
                taskCard.InitInfo(taskInfo);
            }
            // Add the task to our level tasks list
            taskCards.Add(taskInfo);
        }

    }
    
    // 初始化游戏管理器的方法
    private void InitGameManager()
    {
        Debug.Log("GameManager initialized!");
    }


    
    
}
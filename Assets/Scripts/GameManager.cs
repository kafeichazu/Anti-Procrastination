using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例实例
    private static GameManager _instance;

    //懒得写UIManager了，暂时用这个顶顶
    public GameObject infoPanel;
    
    // 公共访问器
    public static GameManager Instance
    {
        get
        {
            // 如果实例不存在，尝试在场景中查找
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // 如果场景中没有找到，创建一个新的
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    
    // 在对象被初始化时调用
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
    }
    
    // 初始化游戏管理器的方法
    private void InitGameManager()
    {
        Debug.Log("GameManager initialized!");
        // 在这里添加所有需要的初始化代码
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // 你可以在这里添加游戏管理需要的公共方法和属性
    // 例如:
    
    // 游戏状态
    private bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
    }
    
    // 暂停游戏
    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }
    
    // 继续游戏
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
}
using UnityEngine;
using TMPro;

public class AttributeManager : MonoBehaviour
{
    public static AttributeManager Instance { get; private set; }
    public TextMeshProUGUI moodTMP;
    public TextMeshProUGUI energyTMP;
    public int totalMood = 10;
    public int totalEnergy = 10;

    public int successedScheduleTask = 0;
    public int successedLoveTask = 0;

    private void Awake()
    {
        // 保证唯一实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 如果已有实例，销毁当前多余的
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 可选：是否跨场景保留
    }

    /// <summary>
    /// 消耗情绪与体力值
    /// </summary>
    public void Consume(int mood, int energy)
    {
        totalMood -= mood;
        totalEnergy -= energy;
        totalMood = Mathf.Max(totalMood, 0);
        totalEnergy = Mathf.Max(totalEnergy, 0);
        moodTMP.text = totalMood.ToString();
        energyTMP.text = totalEnergy.ToString();
    }
}

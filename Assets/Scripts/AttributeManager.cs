using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public int totalMood = 100;
    public int totalEnergy = 100;
    
    // 用于外部调用减少值
    public void Consume(int mood, int energy)
    {
        totalMood -= mood;
        totalEnergy -= energy;
        totalMood = Mathf.Max(totalMood, 0);
        totalEnergy = Mathf.Max(totalEnergy, 0);
    }
}

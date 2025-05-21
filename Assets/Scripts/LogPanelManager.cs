using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogPanelManager : MonoBehaviour
{
    public Transform logContentParent;      // 挂在ScrollView的Content下
    public GameObject logItemPrefab;        // 一条日志的TMP预制体（TextMeshProUGUI）
    public ScrollRect scrollRect;  // 在 LogPanelManager 中添加

    /*
    public void AddLog(string message)
    {
        GameObject newLog = Instantiate(logItemPrefab, logContentParent);
        newLog.GetComponent<TMP_Text>().text = message;
    }
    */
    public void AddLog(string logMessage)
    {
        GameObject logItem = Instantiate(logItemPrefab, logContentParent);
        TextMeshProUGUI tmp = logItem.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = logMessage;
        }

        Canvas.ForceUpdateCanvases(); // 强制刷新
        scrollRect.verticalNormalizedPosition = 0f; // 滚到最底
    }
}




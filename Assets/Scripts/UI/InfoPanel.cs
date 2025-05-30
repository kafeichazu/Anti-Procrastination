using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public TaskCardInfo taskCardInfo;
    public TextMeshProUGUI taskName;
    public TextMeshProUGUI timeRequest;
    public TextMeshProUGUI passPoint;
    public TextMeshProUGUI taskDescription;
    public TextMeshProUGUI moodNum;
    public TextMeshProUGUI energyNum;

    //public Button moodAddButton;
    public Button moodSubButton;
    //public Button energyAddButton;
    public Button energySubButton;

    [HideInInspector]
    public Slot slot;
    public int totalDiceNum;
    public int moodDiceNum;
    public int energyDiceNum;

    public void ShowInfo(TaskCardInfo taskCardInfo,int moodDN = -1,int energyDN = -1)
    {
        this.taskCardInfo = taskCardInfo;
        // 显示任务信息
        taskName.text = taskCardInfo.fixedTaskName == ""? taskCardInfo.scheduledTaskName : taskCardInfo.fixedTaskName;
        timeRequest.text = taskCardInfo.timeBlockNum.ToString();
        passPoint.text = taskCardInfo.successPoint.ToString();
        //taskDescription.text = taskCardInfo.scheduledTask;
        moodNum.text = moodDN >= 0? moodDN.ToString() : taskCardInfo.consumeMood.ToString();
        energyNum.text = energyDN >= 0? energyDN.ToString() : taskCardInfo.consumeEnergy.ToString();

        //使用量默认为推荐
        moodDiceNum = taskCardInfo.consumeMood;
        energyDiceNum = taskCardInfo.consumeEnergy;
        totalDiceNum = moodDiceNum + energyDiceNum;
    }

    public void IncreaseMood()
    {
        moodDiceNum++;
        slot.moodDiceNum++;
        totalDiceNum++;
        moodNum.text = moodDiceNum.ToString();
        RefreshButton();
    }
    public void DecreaseMood()
    {
        if (moodDiceNum > taskCardInfo.consumeMood)
        {
            slot.moodDiceNum--;
            moodDiceNum--;
            totalDiceNum--;
            moodNum.text = moodDiceNum.ToString();
        }
        RefreshButton();
    }

    public void IncreaseEnergy()
    {
        slot.energyDiceNum++;
        energyDiceNum++;
        totalDiceNum++;
        energyNum.text = energyDiceNum.ToString();
        RefreshButton();
    }
    public void DecreaseEnergy()
    {
        if (energyDiceNum > taskCardInfo.consumeEnergy)
        {
            slot.energyDiceNum--;
            energyDiceNum--;
            totalDiceNum--;
            energyNum.text = energyDiceNum.ToString();
        }
        RefreshButton();
    }

    public void RefreshButton()
    {
        if (moodDiceNum > taskCardInfo.consumeMood)
        {
            moodSubButton.gameObject.SetActive(true);
        }
        else
        {
            moodSubButton.gameObject.SetActive(false);
        }

        if (energyDiceNum > taskCardInfo.consumeEnergy)
        {
            energySubButton.gameObject.SetActive(true);
        }
        else
        {
            energySubButton.gameObject.SetActive(false);
        }
    }

    public void Apply()
    {
        gameObject.SetActive(false);
        GameManager.Instance.startButton.SetActive(true);

        //填充信息到本日任务列表中
        slot.InsertCardArray(taskCardInfo, moodDiceNum, energyDiceNum);
        GameManager.Instance.isPlanDice = false;
    }

}

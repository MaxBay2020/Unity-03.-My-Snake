using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    public Text lastText;

    public Text bestText;

    public Toggle blueToggle;

    public Toggle yellowToggle;

    public Toggle boundaryModeToggle;

    public Toggle freeModeToggle;

    private void Awake()
    {
        lastText.text = "Last: Length " + PlayerPrefs.GetInt("LastLength", 0) + ", Score " + PlayerPrefs.GetInt("LastScore", 0);
        
        bestText.text = "Best: Length " + PlayerPrefs.GetInt("BestLength", 0) + ", Score " + PlayerPrefs.GetInt("BestScore", 0);
    }

    /// <summary>
    /// 开始游戏按钮功能；
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// 记录蓝色皮肤选择；
    /// </summary>
    /// <param name="isOn"></param>
    public void BlueSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }

    /// <summary>
    /// 记录黄色皮肤选择；
    /// </summary>
    /// <param name="isOn"></param>
    public void YellowSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }

    /// <summary>
    /// 记录边界模式选择；
    /// </summary>
    /// <param name="isOn"></param>
    public void BoundaryModeSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("Boundary", 1);
        }
    }

    /// <summary>
    /// 记录自由模式选择；
    /// </summary>
    /// <param name="isOn"></param>
    public void FreeModeSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("Boundary", 0);
        }
    }

    private void Start()
    {
        if(PlayerPrefs.GetString("sh", "sh01") == "sh01")
        {
            blueToggle.isOn = true;

            PlayerPrefs.SetString("sh", "sh01");

            PlayerPrefs.SetString("sb01", "sb0101");

            PlayerPrefs.SetString("sb02", "sb0102");
        }
        else
        {
            yellowToggle.isOn = true;

            PlayerPrefs.SetString("sh", "sh02");

            PlayerPrefs.SetString("sb01", "sb0201");

            PlayerPrefs.SetString("sb02", "sb0202");
        }

        if(PlayerPrefs.GetInt("Boundary", 1) == 1)
        {
            boundaryModeToggle.isOn = true;

            PlayerPrefs.SetInt("Boundary", 1);
        }
        else
        {
            freeModeToggle.isOn = true;

            PlayerPrefs.SetInt("Boundary", 0);
        }
    }
}

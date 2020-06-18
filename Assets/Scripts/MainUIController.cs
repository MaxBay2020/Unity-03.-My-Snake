using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public static MainUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public int score = 0;

    public int length = 0;

    public Text scoreText;

    public Text msgText;

    public Text lengthText;

    public Image bgImage;

    private Color tempColor;

    public bool isPause = false;

    public Image pauseImage;

    public Sprite[] pauseSprites;

    public bool hasBoundary = true;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Boundary", 1) == 0)
        {
            hasBoundary = false;

            foreach (Transform walls in bgImage.gameObject.transform)
            {
                walls.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    /// <summary>
    /// 更新分数和长度；
    /// </summary>
    /// <param name="s"></param>
    /// <param name="l"></param>
    public void updateUI(int s = 5, int l = 1)
    {
        score += s;

        length += l;

        scoreText.text = "Score: \n" + score;

        lengthText.text = "Length: \n" + length;
    }

    private void Update()
    {
        //按照一定分数切换背景颜色；
        switch (score/100)
        {
            case 0:
            case 1:
            case 2:
                break;

            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Phase" + 2;

                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Phase" + 3;

                break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Phase" + 4;

                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Phase" + 5;

                break;
            case 11:
            case 12:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Phase" + 6;

                break;
            default:
                ColorUtility.TryParseHtmlString("#FFEEAACC", out tempColor);

                bgImage.color = tempColor;

                msgText.text = "Free Phase";

                break;
        }
    }

    /// <summary>
    /// 游戏暂停功能
    /// </summary>
    public void Pause()
    {
        isPause = !isPause;

        if (isPause)
        {
            //时间冻结，相当于游戏暂停；
            Time.timeScale = 0;

            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            //恢复游戏运行；
            Time.timeScale = 1;

            pauseImage.sprite = pauseSprites[0];
        }
    }

    /// <summary>
    /// 回到主页功能；
    /// </summary>
    public void Home()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

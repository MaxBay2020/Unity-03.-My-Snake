using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour
{

    public int step;

    private int deltaX;

    private int deltaY;

    private Vector3 headPos;

    private float y;

    private float x;

    public float velocity = 0.5f;

    //存储蛇身的集合；
    public List<Transform> bodyList = new List<Transform>();

    public GameObject bodyPrefab;

    public Sprite[] bodySprites = new Sprite[2];

    private Transform canvas;

    private bool isDead = false;

    public GameObject dieEffect;

    public AudioClip eatClip, dieClip;


    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;

        //通过unity内部的Resources.Load()方法，来加载资源，参数不需要写Resources/及文件扩展名；
        GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh01"));

        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0102"));

        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }
    private void Start()
    {
        deltaX = step;

        deltaY = 0;

        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);

        InvokeRepeating("Move", 0, velocity);

    }

    private void Update()
    {
        //如果游戏暂停或死亡，将不能控制；
        if (MainUIController.instance.isPause || isDead)
        {
            return;
        }

        x = Input.GetAxisRaw("Horizontal");

        y = Input.GetAxisRaw("Vertical");

        if (y == 1 && deltaY != -step)  //向上走；
        {
            deltaX = 0;

            deltaY = step;

            //旋转；
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (y == -1 && deltaY != step) //向下走；
        {
            deltaX = 0;

            deltaY = -step;

            //旋转；
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);

        }
        if (x == 1 && deltaX != -step)  //向右走；
        {
            deltaX = step;

            deltaY = 0;

            //旋转；
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);

        }
        if (x == -1 && deltaX != step) //向左走；
        {
            deltaX = -step;

            deltaY = 0;

            //旋转；
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        //按住空格键，加速；
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.2f);
        }

        //抬起空格键，减速；
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }

    }

    /// <summary>
    /// 蛇移动；
    /// </summary>
    void Move()
    {
        //移动前，将蛇头的位置储存下来；
        headPos = gameObject.transform.localPosition;

        //开始移动，蛇头向期望方向移动；
        gameObject.transform.localPosition = new Vector3(headPos.x + deltaX, headPos.y + deltaY, headPos.z);

        if (bodyList.Count > 0)
        {
            //方法一：将最后一个蛇身（蛇尾）移动到移动前蛇头的位置；但是由于是双色蛇身，因此此方法会出现颜色（图片）错误；此方法弃用；
            //bodyList.Last().localPosition = headPos;

            //将蛇尾插入到蛇身list的第一个元素；
            //bodyList.Insert(0, bodyList.Last());

            //将蛇身list的最后一个元素（也就是蛇尾）移除；
            //bodyList.RemoveAt(bodyList.Count - 1);

            //方法二：从后往前移动蛇身，后一个蛇身都移动到它前一个蛇身的位置；由于是双色蛇身，因此采用此方法达到正确显示颜色（图片）的目的；
            for (int i = bodyList.Count - 2; i >= 0; i--)
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;

            }

            //第一个蛇身移动到移动前蛇头的位置；
            bodyList[0].localPosition = headPos;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //吃到食物的处理方法；
        if (collision.gameObject.CompareTag("Food"))
        {
            //分数增加；
            MainUIController.instance.updateUI();

            //销毁食物；
            Destroy(collision.gameObject);

            //蛇身+1；
            BodySpawn();

            if (Random.Range(0, 100) < 80)//有20%的机率生成食物的同时，生成奖励；
            {
                //生成新的食物，同时生成奖励；
                FoodMaker.instance.MakeFood(true);
            }
            else //有80%的机率只生成食物；
            {
                //生成新的食物，同时生成奖励；
                FoodMaker.instance.MakeFood(false);
            }

        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            //分数随机增加；
            MainUIController.instance.updateUI(Random.Range(5, 15) * 10);

            Destroy(collision.gameObject);

            BodySpawn();
        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            Die();
        }
        else
        {
            //边界模式
            if (MainUIController.instance.hasBoundary)
            {
                Die();
            }
            else
            {
                //自由模式；
                switch (collision.gameObject.name)
                {
                    case "TopCollider":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "BottomCollider":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "LeftCollider":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 180, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "RightCollider":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 240, transform.localPosition.y, transform.localPosition.z);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 生成蛇身；
    /// </summary>
    void BodySpawn()
    {
        //播放吃食物音效；
        AudioSource.PlayClipAtPoint(eatClip, new Vector3(0, 0, -10));

        int index = (bodyList.Count % 2 == 0) ? 0 : 1;

        GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);

        body.GetComponent<Image>().sprite = bodySprites[index];

        body.transform.SetParent(canvas, false);

        bodyList.Add(body.transform);
    }

    /// <summary>
    /// 死亡方法；
    /// </summary>
    public void Die()
    {            
        //播放死亡音效；
        AudioSource.PlayClipAtPoint(dieClip, new Vector3(0,0,-10));

        CancelInvoke();

        isDead = true;

        Instantiate(dieEffect);

        //使用unity提供的类来记录分数和长度；
        PlayerPrefs.SetInt("LastLength", MainUIController.instance.length);

        PlayerPrefs.SetInt("LastScore", MainUIController.instance.score);

        if (PlayerPrefs.GetInt("BestScore", 0) < MainUIController.instance.score)
        {
            PlayerPrefs.SetInt("BestLength", MainUIController.instance.length);

            PlayerPrefs.SetInt("BestScore", MainUIController.instance.score);

        }

        StartCoroutine(GameOver(1.5f));
    }

    /// <summary>
    /// 等待几秒之后进行下一步操作；
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator GameOver(float t)
    {
        yield return new WaitForSeconds(t);

        SceneManager.LoadScene("MenuScene");
    }
}

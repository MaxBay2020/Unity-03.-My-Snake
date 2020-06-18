using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    public static FoodMaker instance;

    public int maxX = 21;

    public int maxY = 11;

    public int xOffset = 7;

    public GameObject foodPrefab;

    public Sprite[] foodSprites;

    private Transform foodHolder;

    public GameObject rewardPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foodHolder = GameObject.Find("FoodHolder").transform;

        MakeFood(false);
    }

    /// <summary>
    /// 生成食物的方法；
    /// </summary>
    public void MakeFood(bool isReward)
    {
        int index = Random.Range(0, foodSprites.Length);

        GameObject food = Instantiate(foodPrefab);

        food.GetComponent<Image>().sprite = foodSprites[index];

        food.transform.SetParent(foodHolder, false);

        int x = Random.Range(-maxX + xOffset, maxX);

        int y = Random.Range(-maxY, maxY);

        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        if (isReward)
        {
            GameObject reward = Instantiate(rewardPrefab);

            reward.transform.SetParent(foodHolder, false);

            x = Random.Range(-maxX + xOffset, maxX);

            y = Random.Range(-maxY, maxY);

            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Food
    {
        public string tag;
        public GameObject pfToPool;
        public float baseFoodSize;
        public int spawnFrequency;
        public int poolSize;
    }

    [NonReorderable]
    [SerializeField] List<Food> foodList;

    [Header("Spawner Settings")]
    [SerializeField] Transform playerTransform;
    [SerializeField] int maxSpawn;
    [SerializeField] float minSpawnRadius;
    [SerializeField] float maxSpawnRadius;
    [SerializeField] public float minSizePercentile;
    [SerializeField] public float maxSizePercentile;


    //Pool variables
    List<string> frequencyList;
    public Dictionary<string, Queue<GameObject>> foodDic;

    //Variables for spawning of the objects
    private GameObject thisFood;
    private Queue<GameObject> thisQueue;
    private Vector3 dir, pos;
    private float distance;
    private Vector3 playerSize;

    //Singleton
    public static FoodSpawner spawner;

    private void Awake()
    {
        spawner = this;
    }
    void Start()
    {
        //Pooling of all enemy/food types
        foodDic = new Dictionary<string, Queue<GameObject>>();
        frequencyList = new List<string>();

        foreach (Food food in foodList)
        {
            //-----Setting frequency list
            for (int x = 0; x < food.spawnFrequency; x++)
            {
                frequencyList.Add(food.tag);
            }


            //-----Actual Pooling
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int x = 0; x < food.poolSize; x++)
            {
                GameObject foodObj = Instantiate(food.pfToPool);
                foodObj.SetActive(false);
                foodObj.transform.parent = gameObject.transform;
                objPool.Enqueue(foodObj);
            }

            foodDic.Add(food.tag, objPool);
        }

        SpawnFood(maxSpawn);
    }

    public Queue<GameObject> GetFoodQueue(string tagname)
    {
        if (foodDic.ContainsKey(tagname))
        {
            return foodDic[tagname];
        }
        else
        {
            return null;
        }
    }

    public void SpawnFood(int setSpawn)
    {
        playerSize = playerTransform.localScale;

        for (int x = 0; x < setSpawn; x++)
        {
            thisQueue = GetFoodQueue(frequencyList[Random.Range(0, frequencyList.Count)]);
            dir = Random.insideUnitCircle.normalized;
            distance = Random.Range(minSpawnRadius, maxSpawnRadius) * playerSize.x;
            pos = dir * distance;

            thisFood = thisQueue.Dequeue();
            thisFood.transform.position = playerTransform.position + new Vector3(pos.x, playerTransform.position.y, pos.y);
            thisFood.transform.localScale = playerTransform.localScale * Random.Range(minSizePercentile, maxSizePercentile);
            thisFood.SetActive(true);

            thisQueue.Enqueue(thisFood);
        }
    }
}

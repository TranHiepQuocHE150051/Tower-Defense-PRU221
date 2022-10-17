using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    public Transform way1;
    public GameObject circle;
    public GameObject square;
    public GameObject diamond;
    public GameObject hexagon;
    [SerializeField]
    public Transform monsterParent;

    private List<GameObject> monsters = new List<GameObject>();
    private List<Transform> paths = new List<Transform>();

    public GameObject[] normal;
    //public GameObject[] fast;
    //public GameObject[] boss;
    //public GameObject[] leader;

    int totalTurnSpawn = 2;
    float timeBetweenWave = 5f;
    float countDown = 5f;
    public bool isDoneSpawn = true;

    private int waveSpawn;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        waveSpawn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDoneSpawn)
        {
            return;
        }

        if (countDown <= 0)
        {           
            isDoneSpawn = false;
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWave;
            return;
        }

        countDown -= Time.deltaTime;
    }
    void SpawnMonster(GameObject monster, Transform path)
    {
        Vector3[] wayPoints = new Vector3[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            wayPoints[i] = path.GetChild(i).position;
        }
        GameObject newMonster = Instantiate(monster, monsterParent);
        newMonster.transform.position = path.GetChild(0).position;
        newMonster.GetComponent<MonsterController>().wayPoints = wayPoints;
    }
    IEnumerator SpawnWave()
    {
        CheckWaveSpawn();

        for (int i = 0; i < totalTurnSpawn; i++)
        {
            Transform path = paths[Random.Range(0, paths.Count)];
            foreach (GameObject monster in monsters)
            {
                SpawnMonster(monster, path);
                yield return new WaitForSeconds(1.5f);
            }

            yield return new WaitForSeconds(5f);
        }
      
            //totalTurnSpawn++;        

        isDoneSpawn = true;
    }
    void CheckWaveSpawn()
    {
        monsters = new List<GameObject>();
        paths = new List<Transform>();
        AddNormalMonster(5);
        AddFastMonster(5);
        AddBossMonster(2);
        AddLeaderMonster(1);
        AddWay1Path();
    }
    void AddNormalMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(circle);
        }
    }

    void AddLeaderMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //monsters.Add(leader[Random.Range(0, leader.Length)]);
            monsters.Add(hexagon);
        }
    }

    void AddFastMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(square);
        }
    }

    void AddBossMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(diamond);
        }
    }
    void AddWay1Path()
    {
        foreach (Transform path in way1)
        {
            paths.Add(path);
        }
    }
}

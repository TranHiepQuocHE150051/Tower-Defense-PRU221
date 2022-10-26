using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    public Transform way1;  
    [SerializeField]
    public Transform monsterParent;

    private List<GameObject> monsters = new List<GameObject>();
    private List<Transform> paths = new List<Transform>();

    public GameObject[] boss;
    public GameObject[] leader;
    public GameObject[] normal;

    int totalTurnSpawn = 2;
    float timeBetweenWave = 5f;
    float countDown = 5f;
    public bool isDoneSpawn = true;

    private int waveSpawn;
    public int WaveSpawn
    {
        get
        {
            return waveSpawn;
        }
        set
        {
            waveSpawn = value;
            //UIController.instance.txtWave.text = waveSpawn.ToString();

            //if (waveSpawn > 5)
            //{
            //    foreach (Transform tower in TowerManager.instance.towerParent)
            //    {
            //        if (tower.GetComponent<ArcherTowerController>() != null)
            //        {
            //            tower.GetComponent<ArcherTowerController>().damage += tower.GetComponent<ArcherTowerController>().damage * waveSpawn * 0.02f;
            //        }
            //        else if (tower.GetComponent<CanonTowerController>() != null)
            //        {
            //            tower.GetComponent<CanonTowerController>().damage += tower.GetComponent<CanonTowerController>().damage * waveSpawn * 0.02f;
            //        }
            //        else if (tower.GetComponent<MagicTowerController>() != null)
            //        {
            //            tower.GetComponent<MagicTowerController>().damage += tower.GetComponent<MagicTowerController>().damage * waveSpawn * 0.02f;
            //        }
            //        else if (tower.GetComponent<LightningTowerController>() != null)
            //        {
            //            tower.GetComponent<LightningTowerController>().damage += tower.GetComponent<LightningTowerController>().damage * waveSpawn * 0.02f;
            //        }
            //    }
            //}
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        WaveSpawn = 1;
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

            yield return new WaitForSeconds(8f);
        }

        WaveSpawn++;

        if (WaveSpawn % 2 == 0)
        {
            totalTurnSpawn++;
        }

        isDoneSpawn = true;
    }
    void CheckWaveSpawn()
    {
        monsters = new List<GameObject>();
        paths = new List<Transform>();
        if (WaveSpawn <= 5)
        {
            AddLeaderMonster(1);
            AddNormalMonster(3);
            AddWay1Path();
        }
        else if (WaveSpawn > 5 && WaveSpawn <= 10)
        {
            AddBossMonster(1);
            AddLeaderMonster(2);
            AddNormalMonster(3);
            AddWay1Path();
        }
        else if (WaveSpawn > 10 && WaveSpawn <= 20)
        {
            AddBossMonster(1);
            AddLeaderMonster(3);
            AddNormalMonster(7);
            AddWay1Path();        }
        else if (WaveSpawn > 20 && WaveSpawn <= 30)
        {
            AddBossMonster(2);
            AddLeaderMonster(5);
            AddNormalMonster(15);
            AddWay1Path();

        }
        else if (WaveSpawn > 30)
        {
            AddBossMonster(5);
            AddLeaderMonster(10);
            AddNormalMonster(30);
            AddWay1Path();
        }
    }
    void AddNormalMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(normal[Random.Range(0, normal.Length)]);
        }
    }

    void AddLeaderMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(leader[Random.Range(0, leader.Length)]);
        }
    }


    void AddBossMonster(int count)
    {
        for (int i = 0; i < count; i++)
        {
            monsters.Add(boss[0]);
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

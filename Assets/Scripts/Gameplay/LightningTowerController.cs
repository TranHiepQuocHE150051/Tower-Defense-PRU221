using DG.Tweening;
using SimpleJSON;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTowerController : MonoBehaviour
{
    public Transform spineLevelParent;

    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0f;

    public Transform attackRange;

    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            LoadDataTower();
            SetIdle();

            //attackRange.localScale = new Vector2(fireRange, fireRange);

            if (MonsterSpawnController.instance.WaveSpawn > 5)
            {
                damage += damage * MonsterSpawnController.instance.WaveSpawn * 2 / 100;
            }
        }
    }

    public float damage;
    private float fireRate;
    private float fireRange;
    public float price;
    public float priceToUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (monsters.Count > 0)
        {
            if (monsters[0] != null)
            {
                Shoot();
            }
            else
            {
                monsters.RemoveAt(0);
            }
        }
    }

    public JSONNode loadTextData(string path)
    {
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));

        return JSONArray.Parse(txt.text);
    }

    public void LoadDataTower()
    {
        JSONNode jsonNode = loadTextData("Data/LightningTower");
        foreach (JSONNode node in jsonNode)
        {
            if (Level == node["Level"].AsInt)
            {
                damage = node["Damage"].AsFloat;
                fireRate = node["FireRate"].AsFloat;
                fireRange = node["FireRange"].AsFloat;
                price = node["Price"].AsFloat;
            }
            if (node["Level"].AsInt == (Level + 1))
            {
                priceToUpgrade = node["Price"].AsFloat;
            }
        }
    }

    void Shoot()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(SetShoot());
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            monsters.Add(target.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            GameObject monster = target.gameObject;
            if (monster.GetComponentInParent<MonsterController>().Health <= 0)
            {
                if (monsters.IndexOf(monster) >= 0)
                {
                    monsters.Remove(monster);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Monster"))
        {
            monsters.Remove(target.gameObject);
        }
    }

    void SetIdle()
    {
        Transform spineLevel = spineLevelParent.GetChild(Level - 1);
        foreach(Transform spine in spineLevel)
        {
            spine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", true);
        }
    }

    IEnumerator SetShoot()
    {
        countDown = fireRate;

        Spine.TrackEntry trackEntry = new Spine.TrackEntry();

        Transform spineLevel = spineLevelParent.GetChild(Level - 1);
        foreach (Transform spine in spineLevel)
        {
            trackEntry = spine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack", false);
        }

        AudioController.instance.PlaySound("lightningShoot");

        yield return new WaitForSeconds(0.8f);

        foreach (GameObject monster in monsters)
        {
            if (monster.GetComponentInParent<MonsterController>().Health > 0)
            {
                monster.GetComponentInParent<MonsterController>().TakeDamage(damage);
            }
        }

        yield return new WaitForSpineAnimationComplete(trackEntry);

        SetIdle();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using SimpleJSON;

public class ArcherTowerController : MonoBehaviour
{
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    List<GameObject> monsters = new List<GameObject>();
    public GameObject arrow;
    float countDown = 0f;

    public Transform arrowParentLv1;
    public Transform arrowParentLv2;
    public Transform arrowParentLv3;
    public Transform arrowParentLv4;

    public Transform attackRange;

    float arrowSpeed = 5f;
    private ObjectPool bulletPool;
    [SerializeField]
    private int bulletPoolCount = 2;

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

    private void Awake()
    {
        bulletPool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        bulletPool.Initialize(arrow, bulletPoolCount);

    }

    // Update is called once per frame
    void Update()
    {
        if (monsters.Count > 0)
        {
            if (monsters[0] != null)
            {
                Shoot(monsters[0]);
            }
            else
            {
                monsters.RemoveAt(0);
            }
        }
        else
        {
            SetIdle();
        }
    }

    public JSONNode loadTextData(string path)
    {
        TextAsset txt = (TextAsset)Resources.Load(path, typeof(TextAsset));

        return JSONArray.Parse(txt.text);
    }

    public void LoadDataTower()
    {
        JSONNode jsonNode = loadTextData("Data/ArcherTower");
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

    void Shoot(GameObject monster)
    {
        Debug.Log("Shooting");
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(SetShoot(monster));
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
        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "idle", true);
        if (Level == 1 || Level == 2)
        {
            spineLevelParent.GetChild(Level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", true);
        }
    }

    IEnumerator SetShoot(GameObject monster)
    {
        countDown = fireRate;

        Transform spineLevel = spineLevelParent.GetChild(Level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();
        Spine.TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_start", false);
        if (Level == 1 || Level == 2)
        {
            spineLevelParent.GetChild(Level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack_start", false);
        }

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "attack_idle", true);
        if (Level == 1 || Level == 2)
        {
            spineLevelParent.GetChild(Level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack_idle", false);
        }

        //GameObject bullet = Instantiate(arrow, arrowParentLv1);
        GameObject bullet = bulletPool.CreateObject();
        bullet.transform.position = arrowParentLv1.position;
        bullet.transform.localRotation = arrowParentLv1.rotation;

        //bullet.transform.localPosition = new Vector3(0, 0, 0);
        Vector3 direct = monster.transform.position - bullet.transform.position;
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        //bullet.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float distance = Vector2.Distance(bullet.transform.position, monster.transform.position);
        float time = distance / arrowSpeed;
        AudioController.instance.PlaySound("archerShoot");
        bullet.transform.DOMove(monster.transform.position, time).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time);

        AudioController.instance.PlaySound("archerHit");

        //Destroy(bullet);
        bullet.SetActive(false);

        if (monster.GetComponentInParent<MonsterController>().Health > 0)
        {
            monster.GetComponentInParent<MonsterController>().TakeDamage(damage);
        }

        trackEntry = skeletonAnimation.state.SetAnimation(0, "attack_end", false);
        if (Level == 1 || Level == 2)
        {
            spineLevelParent.GetChild(Level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack_end", false);
        }

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "idle", true);
        if (Level == 1 || Level == 2)
        {
            spineLevelParent.GetChild(Level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", true);
        }
    }

}

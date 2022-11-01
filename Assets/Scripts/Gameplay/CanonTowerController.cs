using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CanonTowerController : MonoBehaviour
{
    public Transform spineLevelParent;
    SkeletonAnimation skeletonAnimation;

    public Transform bulletParent;
    public GameObject bomb;
    public GameObject missile;
    List<GameObject> monsters = new List<GameObject>();
    float countDown = 0;

    private float bulletSpeed = 1f;
    private float missileSpeed = 3f;

    SkeletonAnimation effectShoot;

    List<float> angles = new List<float>();

    IEnumerator idleAnimCoroutine;
    bool isIdle = false;

    public Transform bulletParentLevel3_1;
    public Transform bulletParentLevel3_2;
    public Transform bulletParentLevel4_1;
    public Transform bulletParentLevel4_2;
    public Transform bulletParentLevel4_3;
    public Transform bulletParentLevel4_4;

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

        angles.Add(0);
        angles.Add(45);
        angles.Add(90);
        angles.Add(135);
        angles.Add(180);
        angles.Add(225);
        angles.Add(270);
        angles.Add(315);
        countDown = fireRate;
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
        else if (!isIdle)
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
        JSONNode jsonNode = loadTextData("Data/CanonTower");
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
        isIdle = false;
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        else
        {
            
            StartCoroutine(SetShoot(monster.transform.position));
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
        isIdle = true;

        Transform spineLevel = spineLevelParent.GetChild(level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();

        if (level < 3)
        {
            spineLevelParent.GetChild(level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", true);
            idleAnimCoroutine = SetIdleLoop(skeletonAnimation, 0.1f);
            StartCoroutine(idleAnimCoroutine);
        }
        else
        {
            spineLevel = spineLevelParent.GetChild(level - 1);

            foreach (Transform spine in spineLevel)
            {
                spine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "idle", true);
            }
        }
    }

    IEnumerator SetShoot(Vector3 monsterPosition)
    {
        countDown = fireRate;
        Transform spineLevel = spineLevelParent.GetChild(Level - 1).GetChild(0);
        skeletonAnimation = spineLevel.GetComponent<SkeletonAnimation>();

        if (Level < 3)
        {
            StopCoroutine(idleAnimCoroutine);

            Vector3 direct = monsterPosition - transform.position;
            float angle = Mathf.Abs(90 - (Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg));

            float minAngle = Mathf.Abs(angles[0] - angle);
            float angleToRotate = 0;
            foreach (float angleTemp in angles)
            {
                if (Mathf.Abs(angleTemp - angle) < minAngle)
                {
                    minAngle = Mathf.Abs(angleTemp - angle);
                    angleToRotate = angleTemp;
                }
            }

            switch (angleToRotate)
            {
                case 0:
                    skeletonAnimation.state.SetAnimation(0, "attack_08", false);
                    break;
                case 45:
                    skeletonAnimation.state.SetAnimation(0, "attack_01", false);
                    break;
                case 90:
                    skeletonAnimation.state.SetAnimation(0, "attack_02", false);
                    break;
                case 135:
                    skeletonAnimation.state.SetAnimation(0, "attack_03", false);
                    break;
                case 180:
                    skeletonAnimation.state.SetAnimation(0, "attack_04", false);
                    break;
                case 225:
                    skeletonAnimation.state.SetAnimation(0, "attack_05", false);
                    break;
                case 270:
                    skeletonAnimation.state.SetAnimation(0, "attack_06", false);
                    break;
                case 315:
                    skeletonAnimation.state.SetAnimation(0, "attack_07", false);
                    break;
            }

            spineLevelParent.GetChild(level - 1).GetChild(1).GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack", true);
            effectShoot = spineLevelParent.GetChild(Level - 1).GetChild(2).GetComponent<SkeletonAnimation>();
            effectShoot.state.SetAnimation(0, "play", false);

            yield return null;

            GameObject bullet = Instantiate(bomb, bulletParent);
            bullet.transform.localPosition = new Vector3(0, 0, 0);

            AudioController.instance.PlaySound("canonShoot");

            float timeToShoot = Vector2.Distance(bullet.transform.position, monsterPosition) / bulletSpeed;

            bullet.transform.DOJump(monsterPosition, 1f, 1, 1f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);

            bullet.transform.GetChild(0).gameObject.SetActive(false);
            bullet.transform.GetChild(1).gameObject.SetActive(false);

            AudioController.instance.PlaySound("canonHit");

            Transform effectHit = bullet.transform.GetChild(2);
            effectHit.gameObject.SetActive(true);
            SkeletonAnimation effectHit1 = effectHit.GetChild(0).GetComponent<SkeletonAnimation>();
            SkeletonAnimation effectHit2 = effectHit.GetChild(1).GetComponent<SkeletonAnimation>();
            Spine.TrackEntry trackEntry = effectHit1.state.SetAnimation(0, "hit", false);
            effectHit2.state.SetAnimation(0, "hit", false);

            foreach (GameObject monsterTakeDamage in bullet.GetComponent<CanonBulletController>().monsters)
            {
                if (monsterTakeDamage.GetComponentInParent<MonsterController>().Health > 0)
                {
                    monsterTakeDamage.GetComponentInParent<MonsterController>().TakeDamage(damage);
                }
            }

            bullet.transform.GetChild(3).gameObject.SetActive(false);

            yield return new WaitForSpineAnimationComplete(trackEntry);

            Destroy(bullet);
        }
        else if (level < 4)
        {
            StartCoroutine(DelayShootLv3(0, monsterPosition));
            StartCoroutine(DelayShootLv3(1, monsterPosition));
        }
        else
        {
            StartCoroutine(DelayShootLv4(0, monsterPosition));
            StartCoroutine(DelayShootLv4(1, monsterPosition));
            StartCoroutine(DelayShootLv4(2, monsterPosition));
            StartCoroutine(DelayShootLv4(3, monsterPosition));
        }
    }

    IEnumerator DelayShootLv3(int i, Vector3 monsterPosition)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1);
        foreach (Transform spine in spineLevel)
        {
            spine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack", true);
        }

        GameObject bullet;
        if (i == 0)
        {
            bullet = Instantiate(missile, bulletParentLevel3_1);
        }
        else
        {
            bullet = Instantiate(missile, bulletParentLevel3_2);
        }

        bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;

        bullet.transform.localPosition = Vector3.zero;

        SkeletonAnimation missileAnim = bullet.transform.GetChild(0).GetComponent<SkeletonAnimation>();
        Spine.TrackEntry trackEntry = missileAnim.state.SetAnimation(0, "reload", false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        missileAnim.state.SetAnimation(0, "idle", false);

        Vector3 positionUp = new Vector3(bullet.transform.position.x, bullet.transform.position.y + 1, 0);

        Vector3 direct = positionUp - bullet.transform.position;
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        bullet.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        AudioController.instance.PlaySound("missileShoot");
        bullet.transform.DOMove(positionUp, 1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        direct = monsterPosition - positionUp;
        angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        bullet.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        float timeToShoot = Vector2.Distance(bullet.transform.position, monsterPosition) / missileSpeed;

        bullet.transform.DOMove(monsterPosition, timeToShoot).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeToShoot);

        bullet.transform.GetChild(3).gameObject.SetActive(false);

        AudioController.instance.PlaySound("missileHit");

        missileAnim.state.SetAnimation(0, "hit", false);

        Transform effectHit = bullet.transform.GetChild(2);
        effectHit.gameObject.SetActive(true);
        SkeletonAnimation effectHit1 = effectHit.GetChild(0).GetComponent<SkeletonAnimation>();
        SkeletonAnimation effectHit2 = effectHit.GetChild(1).GetComponent<SkeletonAnimation>();
        trackEntry = effectHit1.state.SetAnimation(0, "hit", false);
        effectHit2.state.SetAnimation(0, "hit", false);

        foreach (GameObject monsterTakeDamage in bullet.GetComponent<CanonBulletController>().monsters)
        {
            if (monsterTakeDamage.GetComponentInParent<MonsterController>().Health > 0)
            {
                monsterTakeDamage.GetComponentInParent<MonsterController>().TakeDamage(damage);
            }
        }

        bullet.transform.GetChild(1).gameObject.SetActive(false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        Destroy(bullet);
    }

    IEnumerator DelayShootLv4(int i, Vector3 monsterPosition)
    {
        Transform spineLevel = spineLevelParent.GetChild(level - 1);
        foreach (Transform spine in spineLevel)
        {
            spine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "attack", true);
        }

        GameObject bullet;

        switch (i)
        {
            case 0:
                bullet = Instantiate(missile, bulletParentLevel4_1);
                bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
                break;
            case 1:
                bullet = Instantiate(missile, bulletParentLevel4_2);
                bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
                break;
            case 2:
                bullet = Instantiate(missile, bulletParentLevel4_3);
                bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 3;
                break;
            case 3:
                bullet = Instantiate(missile, bulletParentLevel4_4);
                bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 3;
                break;
            default:
                bullet = Instantiate(missile, bulletParentLevel4_1);
                bullet.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
                break;
        }

        bullet.transform.localPosition = Vector3.zero;

        SkeletonAnimation missileAnim = bullet.transform.GetChild(0).GetComponent<SkeletonAnimation>();
        Spine.TrackEntry trackEntry = missileAnim.state.SetAnimation(0, "reload", false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        missileAnim.state.SetAnimation(0, "idle", false);

        Vector3 positionUp = new Vector3(bullet.transform.position.x, bullet.transform.position.y + 1, 0);

        Vector3 direct = positionUp - bullet.transform.position;
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        bullet.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        AudioController.instance.PlaySound("missileShoot");
        bullet.transform.DOMove(positionUp, 1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        direct = monsterPosition - positionUp;
        angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        bullet.transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        float timeToShoot = Vector2.Distance(bullet.transform.position, monsterPosition) / missileSpeed;

        bullet.transform.DOMove(monsterPosition, timeToShoot).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeToShoot);

        bullet.transform.GetChild(3).gameObject.SetActive(false);

        AudioController.instance.PlaySound("missileHit");

        missileAnim.state.SetAnimation(0, "hit", false);

        Transform effectHit = bullet.transform.GetChild(2);
        effectHit.gameObject.SetActive(true);
        SkeletonAnimation effectHit1 = effectHit.GetChild(0).GetComponent<SkeletonAnimation>();
        SkeletonAnimation effectHit2 = effectHit.GetChild(1).GetComponent<SkeletonAnimation>();
        trackEntry = effectHit1.state.SetAnimation(0, "hit", false);
        effectHit2.state.SetAnimation(0, "hit", false);

        foreach (GameObject monsterTakeDamage in bullet.GetComponent<CanonBulletController>().monsters)
        {
            if (monsterTakeDamage.GetComponentInParent<MonsterController>().Health > 0)
            {
                monsterTakeDamage.GetComponentInParent<MonsterController>().TakeDamage(damage);
            }
        }

        bullet.transform.GetChild(1).gameObject.SetActive(false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        Destroy(bullet);
    }

    IEnumerator SetIdleLoop(SkeletonAnimation skeletonAnimation, float delayTime)
    {
        while (true)
        {
            skeletonAnimation.state.SetAnimation(0, "idle_01", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_02", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_03", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_04", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_05", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_06", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_07", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_08", false);

            yield return new WaitForSeconds(delayTime);

            skeletonAnimation.state.SetAnimation(0, "idle_01", false);

            yield return new WaitForSeconds(2f);
        }
    }
}

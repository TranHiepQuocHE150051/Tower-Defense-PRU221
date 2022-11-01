using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.UI;
using System;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed;
    SkeletonAnimation skeletonAnimation;
    public Vector3[] wayPoints;

    public SpriteRenderer healthGreen;
    public SpriteRenderer healthRed;
    public Vector3 offset;

    public GameObject wood;
    public float priceDeath;

    public float maxHealth;

    Color defaultColor;

    public float armor;

    private float health;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            healthGreen.size = new Vector2((health * 2.41f) / maxHealth, 0.43f);
            healthRed.size = new Vector2(2.41f - healthGreen.size.x, 0.43f);

            if (health <= 0)
            {
                
                StartCoroutine(SetDeath());
            }
        }
    }

    private void Awake()
    {
        skeletonAnimation = transform.GetChild(0).GetChild(0).GetComponent<SkeletonAnimation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
        Move();
        defaultColor = skeletonAnimation.skeleton.GetColor();

        if (MonsterSpawnController.instance.WaveSpawn != 1)
        {
            armor += 0.05f * MonsterSpawnController.instance.WaveSpawn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSetting.instance.Health == 0)
        {
            transform.DOKill();
            skeletonAnimation.state.SetAnimation(0, "idle", true);
        }
    }

    public void Move()
    {
        SetWalk();
        float timeToMove = CalculateDistance(wayPoints) / moveSpeed;
        transform.DOPath(wayPoints, timeToMove, PathType.Linear, PathMode.TopDown2D, 0, Color.red).SetEase(Ease.Linear).OnWaypointChange(MyCallBack).OnComplete(() =>
        {
            AudioController.instance.PlayVibrate();
            PlayerSetting.instance.Health = Math.Max(0, PlayerSetting.instance.Health - 1);
            Destroy(gameObject);
        });
    }

    void MyCallBack(int waypointIndex)
    {
        float scaleX = Mathf.Abs(skeletonAnimation.gameObject.transform.localScale.x);
        // face right
        if (wayPoints[waypointIndex + 1].x - wayPoints[waypointIndex].x > 0)
        {
            skeletonAnimation.gameObject.transform.localScale = new Vector3(-scaleX, skeletonAnimation.gameObject.transform.localScale.y, skeletonAnimation.gameObject.transform.localScale.z);
        }
        // face left
        else
        {
            skeletonAnimation.gameObject.transform.localScale = new Vector3(scaleX, skeletonAnimation.gameObject.transform.localScale.y, skeletonAnimation.gameObject.transform.localScale.z);
        }
    }

    float CalculateDistance(Vector3[] wayPoints)
    {
        float distance = 0;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            distance += Vector3.Distance(wayPoints[i], wayPoints[i + 1]);
        }
        return distance;
    }

    void SetWalk()
    {
        skeletonAnimation.state.SetAnimation(0, "walk", true);
    }

    IEnumerator SetDeath()
    {
        transform.DOKill();

        AudioController.instance.PlaySound("gold");
        PlayerSetting.instance.Coin += priceDeath;

        wood.SetActive(true);
        wood.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "play1", false);

        Spine.TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, "death", false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        skeletonAnimation.state.SetAnimation(0, "death_idle", true);

        healthGreen.gameObject.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        trackEntry = skeletonAnimation.state.SetAnimation(0, "death_end", false);

        yield return new WaitForSpineAnimationComplete(trackEntry);

        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health = Math.Max(0, Health - (damage - (armor * damage / 100)));
        skeletonAnimation.skeleton.SetColor(Color.red);
        Invoke("SetDefaultColor", 0.1f);
    }

    void SetDefaultColor()
    {
        skeletonAnimation.skeleton.SetColor(defaultColor);
    }
}

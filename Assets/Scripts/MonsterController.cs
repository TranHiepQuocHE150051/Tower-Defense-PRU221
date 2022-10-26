using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed;
    SkeletonAnimation skeletonAnimation;
    public Vector3[] wayPoints;

    public SpriteRenderer healthGreen;
    public SpriteRenderer healthRed;
    void Start()
    {
        Move();
    }

    private void Awake()
    {
        skeletonAnimation = transform.GetChild(0).GetChild(0).GetComponent<SkeletonAnimation>();
    }
    void Update()
    {
        
    }
    public void Move()
    {
        SetWalk();
        float timeToMove = CalculateDistance(wayPoints) / moveSpeed;
        transform.DOPath(wayPoints, timeToMove, PathType.Linear, PathMode.TopDown2D, 0, Color.red).SetEase(Ease.Linear).OnWaypointChange(MyCallBack).OnComplete(() =>
        {
            //AudioController.instance.PlayVibrate();
            //PlayerSetting.instance.Health = Math.Max(0, PlayerSetting.instance.Health - 1);
            Destroy(gameObject);
        });
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
}

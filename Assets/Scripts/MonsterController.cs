using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3[] wayPoints;
    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move()
    {
        
        float timeToMove = CalculateDistance(wayPoints) / moveSpeed;
        transform.DOPath(wayPoints, timeToMove, PathType.Linear, PathMode.TopDown2D, 0, Color.red).SetEase(Ease.Linear).OnComplete(() =>
        {          
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
}

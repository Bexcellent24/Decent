using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{

    [SerializeField] private float speed = 3;
    private Vector3 targetPos;

    public GameObject ways;
    public Transform[] wayPoints;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;

    private void Awake()
    {
        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    void Start()
    {
        pointCount = wayPoints.Length;
        pointIndex = 1;
        targetPos = wayPoints[pointIndex].transform.position;
    }

    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos)
        {
            NextPoint();
        }
    }

    private void NextPoint()
    {
        if (pointIndex == pointCount -1)
        {
            direction = -1;
        }
        if (pointIndex ==  0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;
    }
}

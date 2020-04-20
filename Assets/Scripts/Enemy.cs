using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] points;

    public GameObject ballObject;
    private Ball ballComponent;

    public bool dontMove;

    private int pointIndex = 0;

    private float speed = 20;

    private void Start()
    {
        ballComponent = ballObject.GetComponent<Ball>();
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, ballObject.transform.position);

        if (distance <= 5f)
        {
            var x = transform.position.x - ballObject.transform.position.x;

            if (Mathf.Abs(x) > 0.5f)
            {
                transform.position += Vector3.left * Mathf.Sign(x) * speed * Time.deltaTime;
            }
        }
        else
        {
            var direction = Mathf.Sign(ballComponent.Direction.z);

            if (direction == -1)
            {
                var x = transform.position.x - points[pointIndex].position.x;

                if (Mathf.Abs(x) > 0.5f)
                {
                    transform.position += Vector3.left * Mathf.Sign(x) * speed * Time.deltaTime;
                }
            }
        }
    }

    public void UpdateLocation()
    {
        pointIndex++;

        if (pointIndex == points.Length)
            pointIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        var isBall = other.gameObject.TryGetComponent<Ball>(out Ball ballComponent);

        if (!isBall)
            return;

        ballComponent.Direction = transform.forward;

        Game.instance.Enemy_BallHit();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;

    private float speed = 20f;

    private Ball ball;
    private bool isBallInRange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Fire();
        }

        var h = Input.GetAxis("Horizontal");

        transform.position += new Vector3(h, 0f, 0f) * speed * Time.deltaTime;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -14f, 14f), transform.position.y, transform.position.z);
    }

    private void Fire()
    {
        if (isBallInRange)
        {
            var h = Input.GetAxis("Horizontal");
            var heading = enemyTransform.position - transform.position;
            ball.Direction = heading.normalized;

            Game.instance.Player_BallHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var isBall = other.gameObject.TryGetComponent<Ball>(out Ball ball);

        if (!isBall)
            return;

        this.ball = ball;

        isBallInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        var isBall = other.gameObject.TryGetComponent<Ball>(out Ball ball);

        if (!isBall)
            return;

        this.ball = null;

        isBallInRange = false;
    }
}

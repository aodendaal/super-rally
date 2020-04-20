using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform visibleBall;
    [HideInInspector]
    public bool IsWobbly = false;
    private bool isHeld = false;
    private Vector3 direction = Vector3.forward;
    public Vector3 Direction
    {
        get { return direction; }
        set
        {
            direction = value;
        }
    }

    private float speed = 16f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
            return;

        transform.position += direction * speed * Time.deltaTime;

        if (transform.position.x > 14f)
        {
            direction = Vector3.Reflect(direction, Vector3.left);
        }
        else if (transform.position.x < -14f)
        {
            direction = Vector3.Reflect(direction, Vector3.left);
        }

        if (transform.position.z > 15f || transform.position.z < -15f)
        {
            Game.instance.Ball_OutOfBounds(transform.position);
        }

        if (IsWobbly)
        {
            visibleBall.localPosition = new Vector3(0, Mathf.Sin(Time.time * 15), 0);
        }
    }
}

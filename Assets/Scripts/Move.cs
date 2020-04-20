using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");

        transform.position += new Vector3(h, 0f, 0f) * speed * Time.deltaTime;
    }
}

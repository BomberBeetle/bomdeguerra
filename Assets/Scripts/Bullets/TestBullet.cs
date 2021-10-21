using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    public LayerMask GroundLayer;
    public float Speed;
    SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {  
	    _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x + (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI/180)*Speed, transform.position.y + (float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI/180)*Speed);
	Vector2 posR = transform.position;

        float distance = 0.35f;
        RaycastHit2D hitR = Physics2D.Raycast(posR, transform.forward, distance, GroundLayer);
        if (hitR.collider != null) {
                if(hitR.collider.tag == "SolidPlatform") Destroy(gameObject);
        }
    }
}

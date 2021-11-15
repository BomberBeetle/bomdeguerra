using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public float Speed;
    public bool EnemyBullet;
    public float DespawnSeconds;

    SpriteRenderer _renderer;
    float despawnTimer;


    // Start is called before the first frame update
    void Start()
    {  
	    _renderer = GetComponent<SpriteRenderer>();
	    despawnTimer = DespawnSeconds;
    }

    // Update is called once per frame
    // this is sphagoot
    void Update()
    {
	despawnTimer -= Time.deltaTime;
	if(despawnTimer <= 0f){
		Destroy(gameObject);
		return;
	}
        transform.position = new Vector2(transform.position.x + (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI/180)*Speed*Time.deltaTime, transform.position.y + (float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI/180)*Speed*Time.deltaTime);
	Vector2 posR = transform.position + new Vector3(0,0, 0);

        float distance = 0.7f;
	Debug.DrawRay(posR, transform.forward*0.7f, Color.cyan);
        RaycastHit2D hitR = Physics2D.Raycast(posR, transform.forward, distance);
        if (hitR.collider != null) {
		Debug.Log(hitR.collider.tag);
		if(hitR.collider.tag == "Enemy" && !EnemyBullet ){
			hitR.collider.gameObject.GetComponent<BulletCollisionHandler>().HandleCollision(hitR.collider, this);
			Debug.Log("Hit ENEMY");
		}
		if(hitR.collider.tag == "Cleiton" && EnemyBullet){
			hitR.collider.gameObject.GetComponent<BulletCollisionHandler>().HandleCollision(hitR.collider, this);
			Debug.Log("Hit CLEITON");
		}
                if(hitR.collider.tag == "SolidPlatform"){
			Destroy(gameObject);
			Debug.Log("Hit WALL");
		}
        }
    }
}

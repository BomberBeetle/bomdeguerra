using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BulletCollisionHandler))]
public class TestEnemyBehaviour : MonoBehaviour
{
   
    void SpawnBullet(){
    	GameObject.Instantiate(BulletPrefab, transform.position, transform.rotation);	
   }

    void BulletCollision(Collider2D col, Bullet b){
	TakeDamage(b.Damage);
	Destroy(b.gameObject);
    }

    void TakeDamage(int damage){
	Health -= damage;
	if(Health <= 0){
		Destroy(gameObject);
	}
    }
     
    public int Health;
    public bool DoJump;
    public float JumpForce = 500;
    BulletCollisionHandler.HandlerFunctionDelegate collisionDelegate;
    public GameObject BulletPrefab;

    int frameTimer;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
       GetComponent<BulletCollisionHandler>().HandlerFunction = new BulletCollisionHandler.HandlerFunctionDelegate(BulletCollision);
       rb = GetComponent<Rigidbody2D>();
       frameTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer++;
	if(frameTimer == 120){
		SpawnBullet();
		frameTimer = 0;
	}
	else if(frameTimer == 55 && DoJump){
		rb.AddForce(Vector2.up*JumpForce);
	}
    }
}

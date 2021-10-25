using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{

    public delegate void HandlerFunctionDelegate(Collider2D col, Bullet b);

    public HandlerFunctionDelegate HandlerFunction;

    public void HandleCollision(Collider2D col, Bullet b){
	HandlerFunction(col, b);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

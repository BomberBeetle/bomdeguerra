using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHandler))]
public class Machinegun : MonoBehaviour
{

    public GameObject BulletPrefab;
    public float FiringInterval;
    bool fired;
    bool firedPrev;
    float firingTimer;
    Quaternion rotation;

    void Fire(Quaternion rotation){
	//do some shit
	this.rotation = rotation;
	fired = true;

	return;
    }

    // Start is called before the first frame update
    void Start()
    {
    	WeaponHandler wh = GetComponent<WeaponHandler>();
	wh.Fire = Fire;
	firedPrev = false;
	fired = false;
	firingTimer = FiringInterval;
    }

    // Update is called once per frame
    void LateUpdate()
    {
	if(fired){
	 firingTimer -= Time.deltaTime;
	}
	if(!firedPrev&&fired || fired && firingTimer <= 0){
		GameObject.Instantiate(BulletPrefab, transform.position + new Vector3(0, 0.2f, 0), rotation);
		firingTimer = FiringInterval;
	}

	firedPrev = fired;
	fired = false;

    }
}

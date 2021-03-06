using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHandler))]
public class Pistol : MonoBehaviour
{

    public GameObject BulletPrefab;
    bool fired;
    bool firedPrev;
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
	if(!firedPrev&&fired){
		GameObject.Instantiate(BulletPrefab, transform.position + new Vector3(0, 0.2f, 0), rotation);
	}
	firedPrev = fired;
	fired = false;
    }
}

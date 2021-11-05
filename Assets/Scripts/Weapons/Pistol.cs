using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHandler))]
public class Pistol : MonoBehaviour
{

    public GameObject BulletPrefab;

    void Fire(Quaternion rotation){
	//do some shit
	GameObject.Instantiate(BulletPrefab, transform.position + new Vector3(0, 0.2f, 0), rotation);	
	return;
    }

    // Start is called before the first frame update
    void Start()
    {
    	WeaponHandler wh = GetComponent<WeaponHandler>();
	wh.Fire = Fire;
    }

    // Update is called once per frame
    void Update()
    {
        //Not important as this weapon does not autofire.
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour {

    public Transform DestroyedObject;
    public double Maxhealth = 100;

    private double _currHealth;

    // Use this for initialization
    void Start ()
    {
        _currHealth = Maxhealth;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void TriggerHit(GameObject bullet)
    {
        BulletBehaviorScript bulletBehaviorScript = bullet.GetComponent<BulletBehaviorScript>();
        if (gameObject.tag.Contains("Breakable"))
        {
            _currHealth -= bulletBehaviorScript.Damage;
        }
        if(_currHealth <= 0)
        {
            Instantiate(DestroyedObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

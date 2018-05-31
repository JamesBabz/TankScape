using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour {

    public double Maxhealth = 100;

    private double _currHealth;

    // Use this for initialization
    void Start()
    {
        _currHealth = Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void TriggerHit(GameObject bullet)
    {
        BulletBehaviorScript bulletBehaviorScript = bullet.GetComponent<BulletBehaviorScript>();
        _currHealth -= bulletBehaviorScript.Damage;
        if (_currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrHealth(double health)
    {
        _currHealth = health;
    }

    public double GetCurrHealth()
    {
        return _currHealth;
    }

}

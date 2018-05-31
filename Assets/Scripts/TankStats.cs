using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStats : MonoBehaviour
{
    public Transform BustedTank;
    public double Maxhealth = 3;

    private double _currHealth;

    void Start()
    {
        _currHealth = Maxhealth;
    }

    public void triggerHit(GameObject bullet)
    {
        BulletBehaviorScript bulletBehaviorScript = bullet.GetComponent<BulletBehaviorScript>();
        _currHealth -= bulletBehaviorScript.Damage;
        if (_currHealth <= 0)
        {
            Destroy(gameObject);
            Instantiate(BustedTank, transform.position, transform.rotation);
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

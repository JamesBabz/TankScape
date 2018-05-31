using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretRotation : MonoBehaviour {
    
    public float damp = 1.0f;
    public Transform Bullet;
    
    private Quaternion rotate;
    private Transform _turret;
    private BulletBehaviorScript _bulletScript;
    private double _timeAtLastShot;
    private GameObject _target;


    // Use this for initialization
    void Start ()
    {
        if (GameObject.Find("Game Rules").GetComponent<GameRules>().LoseCondition.ToString().Equals("FailToProtect"))
        {
            _target = GameObject.Find("Game Rules").GetComponent<GameRules>().TargetToProtect;
        }
        else
        {
            _target = GameObject.Find("Player");
        }
        Transform tankBody = transform.GetChild(0).GetChild(0);
        _turret = FindTransformInChildWithTag(tankBody, "Turret");
        _bulletScript = Bullet.GetComponent<BulletBehaviorScript>();
    }

    // Update is called once per frame
    void Update ()
	{
    }

    public void OnTriggerStay(Collider objectTriggered)
    {
        if (objectTriggered.transform.root.gameObject == _target)
        {
            RotateTurreTowards(objectTriggered);

            if (Time.time - _timeAtLastShot > _bulletScript.Cooldown)
            {
                Shoot();
                _timeAtLastShot = Time.time;
            }
        }
    }

    private void RotateTurreTowards(Collider objectTriggered)
    {
        Transform target = objectTriggered.transform;
        rotate = Quaternion.LookRotation(target.position - _turret.position);
        rotate.z = 0.0f;
        rotate.x = 0.0f;
        _turret.rotation = Quaternion.Slerp(transform.rotation, rotate, 1f);
    }

    public static Transform FindTransformInChildWithTag(Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.tag == tag)
            {
                return parent.GetChild(i);
            }
        }
        return null;
    }

    private void Shoot()
    {
        Vector3 turretNuzzle = _turret.Find("BulletSpawn").position;
        GameObject bullet = Instantiate(Bullet.gameObject, turretNuzzle, _turret.rotation);
        Destroy(bullet, _bulletScript.MaxTravelTime);
    }

}

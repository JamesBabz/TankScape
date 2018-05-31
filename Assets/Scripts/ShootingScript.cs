using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public Camera Cam;
    public GameObject Bullet;

    private BulletBehaviorScript _bulletScript;
    private float _timeAtLastShot;
    private Transform _turret;

    // Use this for initialization
    void Start ()
    {
        Transform tankBody = transform.GetChild(0).GetChild(0);
        _turret = FindTransformInChildWithTag(tankBody, "Turret");

         _bulletScript = Bullet.GetComponent<BulletBehaviorScript>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    RotateTurret();

	    if (Input.GetButton("Fire1"))
	    {
	        if (Time.time - _timeAtLastShot > _bulletScript.Cooldown)
	        {
	            Shoot();
	            _timeAtLastShot = Time.time;
	        }
	    }
	    

    }

    private void Shoot()
    {
        Vector3 turretNuzzle = _turret.Find("BulletSpawn").position;
        GameObject bullet = Instantiate(Bullet, turretNuzzle, _turret.rotation);
        Destroy(bullet, _bulletScript.MaxTravelTime);
    }
    
    private void RotateTurret()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Cam.WorldToScreenPoint(_turret.position);

        Vector3 dir = mousePos - playerPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _turret.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.up);
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
}

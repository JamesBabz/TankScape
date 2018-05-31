using UnityEngine;

public class CampaignSpawner : MonoBehaviour {

    public GameObject Enemy;

	// Use this for initialization
	void Start () {
       // _enemy = GameObject.Find("Enemy");
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Enemy") == null && GameObject.Find("Enemy(Clone)") == null)
        {
            Instantiate(Enemy, transform.position, Quaternion.identity);
        }
	}
}

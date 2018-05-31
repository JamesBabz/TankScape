using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanBehavior : MonoBehaviour {

    CapsuleCollider _collider;
    Transform _forceField;

    // Use this for initialization
    void Start () {
        _collider = GetComponent<CapsuleCollider>();
        _forceField = gameObject.transform.Find("ForceField");
        CapsuleCollider FFCollider = _forceField.GetComponent<CapsuleCollider>();
        FFCollider.height = _collider.height;
        FFCollider.radius = _collider.radius;
        _forceField.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.name.Equals("Ground"))
        {
            _forceField.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.gameObject.name.Equals("Ground"))
        {
            _forceField.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 3;
    public float TurnSpeed = 150;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * TurnSpeed;
	    var z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

	    transform.Rotate(0, x, 0);
	    transform.Translate(0, 0, z);
    }
}

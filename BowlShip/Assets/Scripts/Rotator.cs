using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float rotateSpeed = 5.0f;				//How fast the object rotates

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.angularVelocity = rotateSpeed;
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}

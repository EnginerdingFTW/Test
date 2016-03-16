using UnityEngine;
using System.Collections;

public class LaserMover : MonoBehaviour {

	public float laserSpeed = 10.0f; 				//the speed at which the laser shoots out

	/// <summary>
	/// Immediately set the laser to move forward at whatever rate given. Each prefab can set this rate differently.
	/// Additionally starts a despawn timer in case it stays on screen too long?
	/// </summary>
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.up * -laserSpeed;
	}

}

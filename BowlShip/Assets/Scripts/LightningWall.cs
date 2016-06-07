using UnityEngine;
using System.Collections;

public class LightningWall : MonoBehaviour {

	private Animator anim;				//The animator this is attached to

	/// <summary>
	/// Get the animator
	/// </summary>
	void Start () {
		anim = GetComponent<Animator> ();
	}

	/// <summary>
	/// Stop things like Asteroids from going through.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D (Collision2D coll) {
		anim.SetTrigger ("Hit");
		if (!coll.gameObject.tag.Equals ("Player")) {
			Destroy (coll.gameObject);
		}
	}

	/// <summary>
	/// Destroys weapon fire trying to enter.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D (Collider2D coll) {
		anim.SetTrigger ("Hit");
		if (!coll.gameObject.tag.Equals ("Player")) {
			Destroy (coll.gameObject);
		}
	}
}

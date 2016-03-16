using UnityEngine;
using System.Collections;

public class Boundaries : MonoBehaviour {
	
	void OnCollisionEnter2D (Collision2D coll) {
		if (!coll.gameObject.CompareTag ("Player")) {
			Destroy (coll.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (!coll.gameObject.CompareTag ("Player")) {
			Destroy (coll.gameObject);
		}
	}
}

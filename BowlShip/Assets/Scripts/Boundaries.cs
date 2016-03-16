using UnityEngine;	
using System.Collections;

public class Boundaries : MonoBehaviour {

	void OnCollisionExit2D (Collision2D coll) {
		if (!coll.gameObject.CompareTag ("Player")) {
			Destroy (coll.gameObject);
		}
	}

	void OnTriggerExit2D (Collider2D coll) {
		if (!coll.gameObject.CompareTag ("Player")) {
			Destroy (coll.gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class GargoyleHealth : MonoBehaviour {

	private GargoyleAI ai;

	// Use this for initialization
	void Start () {
		ai = GetComponentInParent<GargoyleAI> ();
	}
	

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.tag == "Arrow") {
			ai.TailHit();
		}
	}
}

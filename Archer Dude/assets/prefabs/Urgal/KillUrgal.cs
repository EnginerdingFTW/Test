using UnityEngine;
using System.Collections;

public class KillUrgal : MonoBehaviour {

	private Animator urgalAnimator; 				//Urgal's Animator

	// Use this for initialization
	void Start () {
		urgalAnimator = GetComponentInParent<Animator> ();	//Retrieve the animator
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.collider.CompareTag ("Arrow")) {
			urgalAnimator.SetTrigger ("Death");		//Only die if hit by an arrow
		}
	}
}

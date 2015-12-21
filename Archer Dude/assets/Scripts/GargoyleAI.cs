using UnityEngine;
using System.Collections;

public class GargoyleAI : MonoBehaviour {

	private Animator animator;
	public int health = 3;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.V)) {
			animator.SetBool ("Fire", true);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
//		if (coll.gameObject.tag.Equals("Arrow")) {
//			animator.SetBool("Death", true);
//		}
	}

	void DestroyThis () {
		Destroy (this.gameObject);
	}

	public void TailHit () {
		health -= 1;
	}
}

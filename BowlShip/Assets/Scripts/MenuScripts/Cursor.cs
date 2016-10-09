using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	[HideInInspector] public bool selected = false;	//Lets the buttons know if they've been selected
	[HideInInspector] public bool chosen = false;	//Locks the cursor in place over a button until shot again
	private bool selecting = false;					//Only run one IEnumerator at a time
	private Animator anim;							//The attached animator
	private AudioSource audio;						//The source that plays the selection sound
	[HideInInspector] public Rigidbody2D rb;		//The attached rigidbody2d

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	// In case menu switches in the middle of selection
	void OnDisable () {
		selected = false;
		selecting = false;
		chosen = false;
	}

	/// <summary>
	/// Select whatever the cursor is hovering over.
	/// </summary>
	public void Select () {
			anim.SetTrigger ("Select");
			audio.Play ();
			StartCoroutine ("Selecting");
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "WeaponFire") {
			Select ();
		}
	}

	/// <summary>
	/// Used to show selection only temporarily
	/// </summary>
	IEnumerator Selecting () {
		selecting = true;
		yield return new WaitForSeconds (0.2f);
		selected = true;
		yield return new WaitForSeconds (0.2f);
		selected = false;
		selecting = false;
	}
}

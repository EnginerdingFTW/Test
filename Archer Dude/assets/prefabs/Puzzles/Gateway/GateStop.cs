using UnityEngine;
using System.Collections;

public class GateStop : MonoBehaviour {

	private Animator animatorGate;			//Gate animator
	private Animator animatorPlayer;		//Player animator
	public GameObject target;				//the target that triggers animation (will set this later to be it's own class for a list of targets)

	// Use this for initialization
	void Start () 
	{
			//getting components
		animatorGate = GetComponent<Animator>();
		GameObject player = GameObject.Find ("Player");
		animatorPlayer = player.GetComponent<Animator>();
	}

		//if the player collides, stop it from moving.
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && animatorGate.GetBool("Closed") == true)
		{
			animatorPlayer.SetBool("run", false);
		}
	}

		//keep checking to see if the target has been hit, when it is hit, open the gate
	void Update ()
	{
		if (target != null && target.GetComponent<TargetOnOff>().targetOnOff)
		{
			animatorGate.SetBool ("Closed", false);
			animatorGate.SetBool ("Open", true);
		}
	}

		//called by the open animation, tells the player to start running again.
	void Go()
	{
		animatorPlayer.SetBool ("run", true);
	}
}

using UnityEngine;
using System.Collections;

public class GateStop : MonoBehaviour {

	private Animator animatorGate;
	private Animator animatorPlayer;
	public GameObject target;

	// Use this for initialization
	void Start () 
	{
		animatorGate = GetComponent<Animator>();
		GameObject player = GameObject.Find ("Player");
		animatorPlayer = player.GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && animatorGate.GetBool("Closed") == true)
		{
			animatorPlayer.SetBool("run", false);
		}
	}

	void Update ()
	{
		if (target != null && target.GetComponent<TargetOnOff>().targetOnOff)
		{
			animatorGate.SetBool ("Closed", false);
			animatorGate.SetBool ("Open", true);
		}
	}

	void Go()
	{
		animatorPlayer.SetBool ("run", true);
	}
}

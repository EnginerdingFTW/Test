using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour {

	private GameObject lever;			//the "lever" part of the target system gameobject
	private GameObject target;			//the target part of this system
	private Animator leverAnimator;		//animator

	public bool LeverOnOff = false;		//is the lever on or off

	void Start () 
	{
			//getting components
		leverAnimator = this.GetComponent<Animator>();
		lever = this.gameObject.transform.GetChild(1).gameObject;
		target = lever.transform.GetChild(0).gameObject;
	}
	
		// anytime the target changes state, change the state of the lever
	void Update () 
	{
		if (target.GetComponent<TargetOnOff>().targetOnOff)
		{
			leverAnimator.SetBool("RotateOnOff", true);
		}
		else 
		{
			leverAnimator.SetBool("RotateOnOff", false);
		}
	}
}

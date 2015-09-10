using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour {

	private GameObject lever;
	private GameObject target;
	private Animator leverAnimator;

	public bool LeverOnOff = false;	

	// Use this for initialization
	void Start () 
	{
		leverAnimator = this.GetComponent<Animator>();
		lever = this.gameObject.transform.GetChild(1).gameObject;
		target = lever.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
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

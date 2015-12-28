using UnityEngine;
using System.Collections;

public class GateStop : MonoBehaviour {

	private Animator animatorGate;			//Gate animator
	private Animator animatorPlayer;		//Player animator
	private GameObject enemyStop;					//enemy stopper for this puzzle
	public GameObject target;				//the target that triggers animation (will set this later to be it's own class for a list of targets)


	// Use this for initialization
	void Start () 
	{
			//getting components
		animatorGate = GetComponent<Animator>();
		enemyStop = this.transform.FindChild("oncollisionidle").gameObject;
		enemyStop.GetComponent<OnCollisionIdle>().stop = true;
	}

		//keep checking to see if the target has been hit, when it is hit, open the gate
	void Update ()
	{
		if (target != null && target.GetComponent<TargetOnOff>().targetOnOff)
		{
			animatorGate.SetBool ("Closed", false);
			animatorGate.SetBool ("Open", true);
			enemyStop.GetComponent<OnCollisionIdle>().stop = false;	
		}
	}
}

using UnityEngine;
using System.Collections;

/// <summary>
/// The drawbridge controller script
/// </summary>
public class DrawBridge : MonoBehaviour {

	private Animator animatorDrawBridge;			//animator of bridge
	private Animator animatorPlayer;				//animator of player
	public GameObject target;						//the target gameobject that when

	void Start () 
	{
			//getting components
		animatorDrawBridge = GetComponent<Animator>();
		GameObject player = GameObject.Find ("Player");
		animatorPlayer = player.GetComponent<Animator>();
	}
	
		// when the player hits the drawbridge, stop it's movement (this will be revised to stop any enemy/ally as well).
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && animatorDrawBridge.GetBool("DrawBridgeLower") == false)
		{
			animatorPlayer.SetBool("run", false);
		}
	}
	
		//when the target is hit, trigger the lower animation
	void Update ()
	{
		if (target != null && target.GetComponent<TargetOnOff>().targetOnOff)
		{
			animatorDrawBridge.SetBool ("DrawBridgeLower", true);
		}
	}
	
		//at the end of the drawbride lower animation, set the player to run again.
	void Go()
	{
		animatorPlayer.SetBool ("run", true);
		this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
	}
}

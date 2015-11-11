using UnityEngine;
using System.Collections;

public class TargetOnOff : MonoBehaviour {

	public bool targetOnOff = false;
	
		//when the target is hit by an arrow changes the state of targetOnOff
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Arrow")
		{
			targetOnOff = !targetOnOff;
		}
	}
}

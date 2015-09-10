using UnityEngine;
using System.Collections;

public class TargetOnOff : MonoBehaviour {

	public bool targetOnOff = false;
	
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Arrow")
		{
			targetOnOff = !targetOnOff;
		}
	}
}

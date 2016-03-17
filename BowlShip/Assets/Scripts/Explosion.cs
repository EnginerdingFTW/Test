using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float destroyTime = 1.0f;
	
		//this destroys itself at the end of it's animation which starts immediately
	void Start () 
	{
		Destroy(this.gameObject, destroyTime);
	}
}

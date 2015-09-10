using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour 
{
	public float lifetime;
	
	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
}

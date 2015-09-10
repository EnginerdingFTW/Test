using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour {

	public GameObject view;
	public float factor = 2f;

	private float oldPosition;
	private float change;

	// Use this for initialization
	void Start () 
	{
		view = GameObject.Find("Main Camera");
		oldPosition = view.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(view.transform.position.x != oldPosition)
		{
			change = view.transform.position.x - oldPosition;
			oldPosition = view.transform.position.x;

			transform.position = new Vector3(transform.position.x + (change/factor),transform.position.y,transform.position.z);
		}
	}
}

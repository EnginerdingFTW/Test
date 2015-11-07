using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour {

	public GameObject view;			//the camera
	public float factor = 2f;		//how quickly the background moves compared to the foreground

	private float oldPosition;		//old position of the camera

	// Use this for initialization
	void Start () 
	{
		view = GameObject.Find("Main Camera");
		oldPosition = view.transform.position.x;
	}
	
		//updates the parallax
	void Update () 
	{
		if(view.transform.position.x != oldPosition)
		{
			float change = view.transform.position.x - oldPosition;
			oldPosition = view.transform.position.x;

			transform.position = new Vector3(transform.position.x + (change/factor),transform.position.y,transform.position.z);
		}
	}
}

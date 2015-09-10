using UnityEngine;
using System.Collections;

public class BrokenRopeSize : MonoBehaviour {

	private float oldy;
	private float scale;
	private float oldScale;

//	// Use this for initialization
//	void Start () 
//	{
//
//	}
	
	// Update is called once per frame
	void Update () 
	{
		if(RopeBreak.enact == true);
		{
			oldy = RopeBreak.tempy;
			oldScale = transform.localScale.y;
			scale = Mathf.Abs(this.transform.position.y - oldy);
			this.transform.localScale += new Vector3(0f, scale - oldScale - 0.1f, 0f);
			Destroy (this);
		}
	}
}

using UnityEngine;
using System.Collections;

public class BackgroundMaker : MonoBehaviour {

	public int length = 1;
	public float distanceBetweenObjects = 6f;
	public GameObject background;
	
	// Use this for initialization
	void Start () 
	{
		for (int k = 0; k < length; k ++)
		{
			GameObject instance = Instantiate(background, new Vector3(k * distanceBetweenObjects + this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			instance.transform.parent = this.gameObject.transform;
		}
	}
}

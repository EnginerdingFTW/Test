using UnityEngine;
using System.Collections;

public class BackgroundMaker : MonoBehaviour {

	public int length = 1;						//number of duplications of the background
	public float distanceBetweenObjects = 6f;	//distance between each duplication
	public GameObject background;				//the prefab of the background
	
		//creates every instance of the background
	void Start () 
	{
		for (int k = 0; k < length; k ++)
		{
			GameObject instance = Instantiate(background, new Vector3(k * distanceBetweenObjects + this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			instance.transform.parent = this.gameObject.transform;
		}
	}
}

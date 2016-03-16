using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
	public bool pickedUp = false;			//boolean indicating if a player has picked up this collectible
	public float despawnTime = 5.0f;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Start");
		StartCoroutine(checkDespawn(0.0f));
	}

	IEnumerator checkDespawn(float time)
	{
		if (time >= despawnTime)
		{
			Destroy(this.gameObject);
			Debug.Log("In");
		}
		Debug.Log(time.ToString());
		yield return new WaitForSeconds(0.5f);
		if (pickedUp == false)
		{
			StartCoroutine(checkDespawn(time + 0.5f));
		}
	}
}

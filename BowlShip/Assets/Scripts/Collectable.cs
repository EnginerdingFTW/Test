using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {
	
	public bool pickedUp = false;			//boolean indicating if a player has picked up this collectible
	public float despawnTime = 30.0f;		//time before item despawns off map.

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(checkDespawn());
	}

/// <summary>
/// Checks the despawn.
/// </summary>
/// <param name="time">current time on timer</param>
	IEnumerator checkDespawn()		
	{
		float time = 0;
		while (true)
		{
			if (time >= despawnTime)
			{
				Destroy(this.gameObject);
			}
			yield return new WaitForSeconds(0.5f);
			if (pickedUp == false)
			{	
				time = time + 0.5f;
			}
		}
	}
}

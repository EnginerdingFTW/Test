using UnityEngine;
using System.Collections;

public class WarpHoles : MonoBehaviour {

	public GameObject OutHole;							//The Wormhole to spit players back out at

	/// <summary>
	/// When a player enters, spit it out at the outHole, else destroy it.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.gameObject.CompareTag("Player")) {
			coll.gameObject.transform.position = OutHole.transform.position;
		} else {
			Destroy (coll.gameObject);
		}
	}
}

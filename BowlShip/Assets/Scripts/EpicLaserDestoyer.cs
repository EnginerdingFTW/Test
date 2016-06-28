using UnityEngine;
using System.Collections;

public class EpicLaserDestoyer : MonoBehaviour {

	public float timeTillDeath = 1.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine ("DestroyThis");
	}

	/// <summary>
	/// Since the Epic Laser doesn't stop for anything, it has to be destroyed via scripting otherwise the game will lag.
	/// </summary>
	/// <returns>The this.</returns>
	IEnumerator DestroyThis () {
		yield return new WaitForSeconds (timeTillDeath);
		Destroy (this.gameObject);
	}
}

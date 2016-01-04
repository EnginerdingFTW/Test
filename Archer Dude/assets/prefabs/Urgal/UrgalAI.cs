using UnityEngine;
using System.Collections;

public class UrgalAI : MonoBehaviour {

	void DestroyThis () {
		Destroy (this.gameObject);					//Used in death animation to remove dead Urgal from the game
	}
}

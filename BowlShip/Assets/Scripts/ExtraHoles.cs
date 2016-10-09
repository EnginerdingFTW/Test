using UnityEngine;
using System.Collections;

public class ExtraHoles : MonoBehaviour {

	private SceneController sc;				//the scenecontroller
	public GameObject extra;				//the extra warpholes

	// Use this for initialization
	void Start () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController>();
		if (sc.numPlayers > 4) {
			extra.SetActive (true);
		}
	}
}

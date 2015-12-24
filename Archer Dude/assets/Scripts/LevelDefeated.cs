using UnityEngine;
using System.Collections;

public class LevelDefeated : MonoBehaviour {

	private Animator gargoyle;
	public GameObject congrats;

	// Use this for initialization
	void Start () {
		gargoyle = FindObjectOfType<GargoyleAI> ().gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gargoyle.GetBool("Death")) {
			congrats.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsDefault : MonoBehaviour {

	public Slider sfx;
	public Slider music;
	private SceneController sc;

	// Use this for initialization
	void Start () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
		sfx.value = sc.SFXLevel;
		music.value = sc.musicLevel;
	}
}

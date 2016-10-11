using UnityEngine;
using System.Collections;

public class SetLaserVol : MonoBehaviour {

	public float volFactor = 0.5f;				//to be multiplied by sfx vol

	// Use this for initialization
	void Awake () {
		AudioSource aud = GetComponent<AudioSource> ();
		aud.volume = volFactor * ((float) GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ().SFXLevel / 100f);
	}
}

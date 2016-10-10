using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	/// <summary>
	/// Logo animation ends by calling the main menu up
	/// </summary>
	public void Play () {
		SceneManager.LoadScene ("Menu");
	}
}

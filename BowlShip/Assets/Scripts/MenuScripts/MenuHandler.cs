using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	void OnSubmit() {
		SceneManager.LoadScene ("Parr");
	}

	public void Test () {
		SceneManager.LoadScene ("Parr");
	}

	public void Play () {
		SceneManager.LoadScene ("Asteroids");
	}

	public void Quit () {
		Application.Quit();
	}
}

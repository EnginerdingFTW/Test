using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	void OnSubmit() {
		SceneManager.LoadScene ("Parr");
	}

	public void Play () {
		SceneManager.LoadScene ("Parr");
	}

	public void Quit () {
		Application.Quit();
	}
}

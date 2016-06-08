using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	public Slider scoreToWin;
	public Text scoreToWinText;

	private SceneController sc;

	void Start () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
	}

	void OnSubmit() {
		SceneManager.LoadScene ("Parr");
	}

	public void Test () {
		SceneManager.LoadScene ("Parr");
	}

	public void Play () {
		SceneManager.LoadScene ("Asteroids");
	}

	public void PlayWalls () {
		SceneManager.LoadScene ("Walls");
	}

	public void PlayRotation () {
		SceneManager.LoadScene ("Rotation");
	}

	public void Quit () {
		Application.Quit();
	}

	public void SetScoreToWin () {
		sc.setScore ((int) scoreToWin.value);
		scoreToWinText.text = "" + ((int)scoreToWin.value);
	}

	public void ToggleThrust () {
		Player.toggleThrust();
	}

	public void ToggleShieldRecharge () {
		Player.toggleShieldRecharge();
	}
}

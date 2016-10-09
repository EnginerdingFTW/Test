using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonMenuEnable : MonoBehaviour {

	public Button defaultButton;
	private SceneController sc;
	public GameObject main;
	public GameObject gamemode;
	public GameObject stage;
	public GameObject menuHandler;


	/// <summary>
	/// Every time the scene is loaded make sure that the correct menu is pulled up
	/// </summary>
	void Start() {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController>();
		sc.main = main;
		sc.gamemodeSelect = gamemode;
		sc.stageSelect = stage;
		sc.menuHandler = menuHandler;
		sc.SetMenu ();
	}

	/// <summary>
	/// Allow the play button to be selected by default
	/// </summary>
	void OnEnable() {
		defaultButton.Select ();
	}
}

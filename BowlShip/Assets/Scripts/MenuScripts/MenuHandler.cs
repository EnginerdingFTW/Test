using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	//This script is almost entirely small public functions used in the Menu 
	//in order to do specific lines of code at runtime, functions implementations are fairly obvious

	public Slider sfx;						//used in options to set sfx
	public Text SFXText;					//used in options to show SFX levels
	public Text musicText;					//used in options to show Music levels
	public GameObject fadeIn;				//fadein/out between scenes
	private Animator fadeAnim;				//animator to call fadeout
	private AudioSource titleMusic;			//the titleMusic in the menu
	public GameObject mainMenu;				//the Main menu
	public GameObject optionsMenu;			//the options menu
	public GameObject controlsMenu;			//the controls menu
	public Button optionsDefault;			//let resume be the default option
	public Button mainDefault;				//let play be the default option
	public GameObject characterMenu;		//the character menu
	public GameObject gameModeMenu;			//the gamemode menu
	public GameObject stageMenu;			//the stage menu
	public bool canGoBack = true;			//don't load a new stage without characters!

	public Slider music;					//used in options to set music

	//for player instantiation
	public Slider health;
	public Slider shield;
	public GameObject chargin;
	public GameObject weapon;
	public GameObject circle;

	private SceneController sc;				//used to set scene controller values in menu

	void Awake () {
		sc = GameObject.FindGameObjectWithTag ("SceneController").GetComponent<SceneController> ();
		titleMusic = GetComponent<AudioSource> ();
		fadeAnim = fadeIn.GetComponent<Animator> ();
	}

	void Update () {
		if (!mainMenu.activeSelf && !optionsMenu.activeSelf && !controlsMenu.activeSelf) {
			if (Input.GetAxis("Pause") > 0.3f) {
				optionsMenu.SetActive (true);
				optionsDefault.Select ();
			}
		}
	}

	public void OptionsBack () {
		optionsMenu.SetActive (false);
		if (!characterMenu.activeSelf && !gameModeMenu.activeSelf && !stageMenu.activeSelf && canGoBack) {
			mainMenu.SetActive (true);
			mainDefault.Select ();
		}
	}

	public void ChangeSFX() {
		sc.SFXLevel = (int) sfx.value;
		SFXText.text = sc.SFXLevel.ToString ();
	}

	public void ChangeMusic() {
		sc.musicLevel = (int) music.value;
		musicText.text = sc.musicLevel.ToString ();
		titleMusic.volume = ((float)sc.musicLevel / 100f);
	}

	public void PeoplePerController (bool two) {
		if (two) {
			Player.fireButton = "Fire";
		} else {
			Player.fireButton = "FireAlt";
		}
	}

	/// <summary>
	/// Get to the main menu from wherever you are
	/// </summary>
	public void MainMenu() {
		if (canGoBack) {
			if (optionsMenu.activeSelf) {
				optionsMenu.SetActive (false);
			}
			if (controlsMenu.activeSelf) {
				controlsMenu.SetActive (false);
			}
			if (characterMenu.activeSelf) {
				CheckForReset ();
				characterMenu.SetActive (false);
			}
			if (gameModeMenu.activeSelf) {
				CheckForReset ();
				gameModeMenu.SetActive (false);
			}
			if (stageMenu.activeSelf) {
				CheckForReset ();
				stageMenu.SetActive (false);
			}
			mainMenu.SetActive (true);
			sc.menuLevel = 0;
			mainDefault.Select ();
		}
	}

	/// <summary>
	/// Only get rid of players if possible, otherwise get rid of players that just came back from a game
	/// </summary>
	void CheckForReset() {
		if (sc.menuLevel < 2) {
			characterMenu.GetComponent<BetterCharacterSelection> ().Reset ();
		} else {
			GameObject[] destructable = GameObject.FindGameObjectsWithTag ("Player");
			for (int i = 0; i < destructable.Length; i++) {
				Destroy (destructable [i]);
			}
			destructable = GameObject.FindGameObjectsWithTag ("Nuke");
			for (int i = 0; i < destructable.Length; i++) {
				Destroy (destructable [i]);
			}
		}
	}

	void OnSubmit() {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Parr");
	}

	public void Test () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Parr");
	}

	public void Play () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Asteroids");
	}

	public void PlayWalls () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Walls");
	}

	public void PlayRotation () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Rotation");
	}

	public void PlayWarp () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "WarpHoles");
	}

	public void PlayCredits () {
		fadeAnim.SetTrigger ("FadeOut");
		StartCoroutine ("Load", "Credits");
	}

	public void Quit () {
		Application.Quit();
	}

	IEnumerator Load (string level) {
		yield return new WaitForSeconds (0.3f);
		SceneManager.LoadScene (level);
	}

	public void ToggleShieldRecharge () {
		Player.toggleShieldRecharge();
	}

	public void InstantiatePlayers() {
		for (int i = 0; i < sc.numPlayers; i++) {
			GameObject temp = (GameObject) Instantiate (sc.playerShips[i], sc.transform);
			temp.GetComponent<Player> ().playerNum = sc.playerNumArray [i];
			temp.GetComponent<Player>().AssignHUD (health, shield, chargin, weapon, circle);
			if (sc.playerShips[i].GetComponent<EnemyAI>() != null) {
				temp.tag = "Nuke";
				Destroy (temp.GetComponent<EnemyAI>());
			}
		}
	}
		
}

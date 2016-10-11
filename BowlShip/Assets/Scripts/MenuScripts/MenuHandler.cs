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

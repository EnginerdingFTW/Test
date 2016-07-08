using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Camera mainCamera;					//the main camera of the scene
	public bool tdPerspective;					//True for top down perspective, false for side scrolling perspective
	public GameObject ssPlayer;					//the side scrolling player
	public GameObject tdPlayer;					//the top down player
	public GameObject[] tdObjects;				//all top down gameObjects
	public GameObject[] ssObjects;				//all side scrolling gameObjects
	public bool shouldSwitch = true;			//is the player in a location that doesn't allow perspective switching
	public float timeBetweenSwitching = 1.0f;	//how long until the player can switch perspectives again?
	public GameObject transitionPanel;			//the panel to radially fill and defill for perspective transitions
	public float timeBetweenTI = 0.01f;			//time between each transition iteration while switching perspective
	public float amountBetweenTI = 0.02f;		//how much the radius of the panel is filled each iteration

	private Image transitionImage;				//the image attached to the transition panel
	private bool canSwitch = true;				//has there been ample time between perspective switching

	// Use this for initialization
	void Start () {
		transitionImage = transitionPanel.GetComponent<Image> ();
	}

//	// Update is called once per frame
//	void Update () {
//	
//	}

	/// <summary>
	/// Switchs the perspective from Top Down to Side Scrolling, or vice versa.
	/// </summary>
	public void SwitchPerspective() {
		if (canSwitch && shouldSwitch) {
			StartCoroutine ("PerspectiveTransition");
		}
		//else play error sound?
	}

	/// <summary>
	/// This is fun to say 10 times fast. Also actually switchs the perspective from Top Down to Side Scrolling, or vice versa.
	/// </summary>
	void ReplaceRespectivePerspectiveObjects() {
		//StartCoroutine ("WaitToSwitch");

		//Switch the perspective by deactivating all old objects and activating all new ones
		if (tdPerspective) {
			tdPerspective = false;
			for (int i = 0; i < tdObjects.Length; i++) {
				tdObjects [i].SetActive (false);
			}
			for (int i = 0; i < ssObjects.Length; i++) {
				ssObjects [i].SetActive (true);
			}
		} else {
			tdPerspective = true;
			for (int i = 0; i < ssObjects.Length; i++) {
				ssObjects [i].SetActive (false);
			}
			for (int i = 0; i < tdObjects.Length; i++) {
				tdObjects [i].SetActive (true);
			}
		}

		//Update Player position
		if (tdPerspective) {
			Animator animator = tdPlayer.GetComponent<Animator> ();
			tdPlayer.transform.position = new Vector3 (ssPlayer.transform.position.x, 0, 0);
			animator.SetBool ("Moving", false);
			animator.SetBool ("FaceRight", true);
		} else {
			Animator animator = ssPlayer.GetComponent<Animator> ();
			ssPlayer.transform.position = new Vector3 (tdPlayer.transform.position.x, 0, 0);
			animator.SetBool ("Moving", false);
			animator.SetBool ("FaceRight", true);
		}
	}

	/// <summary>
	/// Don't allow the Player to accidentally switch between perspectives too fast
	/// </summary>
	/// <returns>The to switch.</returns>
	IEnumerator WaitToSwitch() {
		canSwitch = false;
		yield return new WaitForSeconds (timeBetweenSwitching);
		canSwitch = true;
	}

	/// <summary>
	/// Fill in the transition panel radially for a set time, then once full, call Perspective switch and defill it radially for a smooth transition.
	/// </summary>
	/// <returns>The time between each fill amount.</returns>
	public IEnumerator PerspectiveTransition() {
		canSwitch = false;
		while (transitionImage.fillAmount < 1.0f) {
			transitionImage.fillAmount += amountBetweenTI;
			yield return new WaitForSeconds (timeBetweenTI);
		}
		ReplaceRespectivePerspectiveObjects ();
		transitionImage.fillClockwise = !transitionImage.fillClockwise;
		while (transitionImage.fillAmount > 0.0f) {
			transitionImage.fillAmount -= amountBetweenTI;
			yield return new WaitForSeconds (timeBetweenTI);
		}
		StartCoroutine ("WaitToSwitch");
	}
}

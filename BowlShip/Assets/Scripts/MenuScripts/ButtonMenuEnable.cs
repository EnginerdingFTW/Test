using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonMenuEnable : MonoBehaviour {

	public Button defaultButton;

	void OnEnable() {
		defaultButton.Select ();
	}
}

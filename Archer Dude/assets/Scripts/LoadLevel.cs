using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class LoadLevel : MonoBehaviour {
	
		//when the button is pressed load the scene
	public void Load()
	{
		SceneManager.LoadScene("Parr");
	}

}

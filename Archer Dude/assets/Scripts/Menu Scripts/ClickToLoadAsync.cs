using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {
	
	public Slider loadingBar;           //This is a loading Bar
	public GameObject loadingImage;		//This is a image that will prevent further user input while a level is being loaded
	
	
	private AsyncOperation async;
	private string scene;
	
	public void ClickAsync(int level) //This is the UI function called when a level is selected
	{
		loadingImage.SetActive(true);
		StartCoroutine(LoadLevelWithBar(level));
	}
	
	
	IEnumerator LoadLevelWithBar (int level)		//The point of this function is to provide a loading bar to give the user instant feedback as the selected level is rendered in the background
	{
		switch (level) {
		case 0:
			scene = "Menu";
			break;
		case 1:
			scene = "Parr";
			break;
		case 2:
			scene = "Boss";
			break;
		}
		async = SceneManager.LoadSceneAsync(scene);
		while (!async.isDone)
		{
			loadingBar.value = async.progress;
			yield return null;						//Wait every frame to update the loading bar
		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {
	
	public Slider loadingBar;           //This is a loading Bar
	public GameObject loadingImage;		//This is a image that will prevent further user input while a level is being loaded
	
	
	private AsyncOperation async;
	
	
	public void ClickAsync(int level) //This is the UI function called when a level is selected
	{
		loadingImage.SetActive(true);
		StartCoroutine(LoadLevelWithBar(level));
	}
	
	
	IEnumerator LoadLevelWithBar (int level)		//The point of this function is to provide a loading bar to give the user instant feedback as the selected level is rendered in the background
	{
		async = Application.LoadLevelAsync(level);
		while (!async.isDone)
		{
			loadingBar.value = async.progress;
			yield return null;						//Wait every frame to update the loading bar
		}
	}
}
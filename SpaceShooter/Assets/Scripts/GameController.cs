using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject hazard;  //hazard object
	public Vector3 spawnValue; //location of spawn
	public int hazardCount;    //lcv
	public float spawnWait;    //time between spawnwaves
	public float waveWait;     //time between waves
	public float startWait;    //time to wait before starting
	public GUIText restartText;
	public GUIText gameOverText;
	private bool gameOver;
	private bool restart;

	public GUIText scoreText;
	private int score;

	void Start()
	{
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i ++)
			{	
					Vector3 spawnLocation = new Vector3 (Random.Range(-spawnValue.x,spawnValue.x), 0.0f, spawnValue.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate(hazard, spawnLocation, spawnRotation);
					yield return new WaitForSeconds (spawnWait);	
			}
			yield return new WaitForSeconds (waveWait);
		
			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore()
	{
		scoreText.text = "Score " + score;
	}
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}

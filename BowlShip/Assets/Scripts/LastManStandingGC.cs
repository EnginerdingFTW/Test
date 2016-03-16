using UnityEngine;
using System.Collections;

public class LastManStandingGC : GameController {

	int playersActive;
	public int[] lives;
	public int num_lives;

	// Use this for initialization
	void Start () 
	{
		int i;
		for (i = 0; i < lives.Length; i++)
		{
			lives[i] = num_lives;
		}	
	}
	
	int CheckPlayersActive()
	{
		return 0;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class LastManStandingGC : GameController {

	public int winScore;						//

	// Use this for initialization
	void Start () 
	{
		winScore = sceneController.score;
	}

	// Update is called once per frame, hopefully don't need to do this
	void Update () {
	
	}

	public void checkEnd () {
		numPlayers--;
		if (numPlayers == 1) {
			//player that's left wins!
			//update player's score
			//reset
		}
		if (numPlayers < 1) {
			//its a draw
			//reset
		}
	}

	public void resetMatch () {
		//disable all players
		//set playerPositions at random spawnPoints
		//begin match
	}
}

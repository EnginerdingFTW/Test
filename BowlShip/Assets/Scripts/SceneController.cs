﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Scene controller. This Script should never be destroyed the entirety of the game. Serves as a core options menu with things such as 
/// Music and SFX volume, Number of Players, controls, etc.
/// </summary>
public class SceneController : MonoBehaviour {

	public int numPlayers = 8;									//The current number of players playing
	public int musicLevel = 100;								//The volume level of the music (1 - 100)
	public int SFXLevel = 100;									//The volume level of the Sound fx (1 - 100)
	public GameObject[] playerShips;							//An array of the ships that the players have selected to use

	public int score;											//A variable used to transfer a generic maxScore value to a specific gameMode

	// Use this for initialization
	void Start () {
		playerShips = new GameObject[8];
	}

	/// <summary>
	/// The setter method for music volume in pause/options menu
	/// </summary>
	/// <param name="musicLevel">Music level.</param>
	public void setMusicLevel (int musicLevel) {
		this.musicLevel = musicLevel;
	}

	/// <summary>
	/// The setter method for SFX volume in pause/options menu
	/// </summary>
	/// <param name="sfx">Sfx.</param>
	public void setSFXLevel (int sfx) {
		SFXLevel = sfx;
	}

	/// <summary>
	/// The setter method for the number of players playing
	/// </summary>
	/// <param name="num">Number.</param>
	public void setNumPlayers (int num) {
		numPlayers = num;
	}

	/// <summary>
	/// The setter method for a players selected ship after he's selected
	/// </summary>
	/// <param name="playerNum">Player number.</param>
	/// <param name="ship">Ship.</param>
	public void selectShip (int playerNum, GameObject ship) {
		playerShips [playerNum] = ship;
	}

	/// <summary>
	/// Setter method for the maxScore needed in the next gameMode.
	/// </summary>
	/// <param name="score">Score.</param>
	public void setScore (int score) {
		this.score = score;
	}
}

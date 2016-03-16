using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int playerNum;								//The player controlling this player
	int health;											//How much health the ship has left
	int shield;											//How much shields the ship has
	float speed;										//How fast the ship can accelerate
	float fireRate;										//How fast the ship can fire
	float horiz;										//The horizontal movement input
	float vert;											//The vertical movement input
	Weapon[] weapons;									//An array of collected weapons

	/** Use this for initialization
	 * 
	 * */
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		


	}
}

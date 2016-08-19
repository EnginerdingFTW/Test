using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameController gamecontroller;
	public float horizontal;
	public float vertical;
	public int drift;
	public bool fire;
	public int difficulty = 2;
	
	void Start()
	{
		gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
	}


}

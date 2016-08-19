using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	
	#region Constants for StateMachine and Difficulty
	private const int COLLECT = 0;
	private const int OFFENSIVE = 1;
	private const int DEFENSIVE = 2; 
	private const int CELEBRATE = 3;
	private const int NOOBS = 0;
	private const int STANDARD = 1;
	private const int ELIT3PR0HAX0RS = 2;
	#endregion
	
	private GameController gamecontroller;
	private int stateMachine = 1;

	public float horizontal;
	public float vertical;
	public int drift;
	public bool fire;
	public int difficulty = 2;
		

	void Start()
	{
		gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
	}

	void Update()
	{
		switch (stateMachine)
		{
			case COLLECT:
				StateCollect();
				break;
	
			case OFFENSIVE:
				StateOffensive();
				break; 

			case DEFENSIVE:
				StateDefensive();
				break;

			case CELEBRATE:
				StateCelebrate();
				break;
		}
	}
	
	void StateCollect()
	{
		switch (difficulty)
		{
			case NOOBS:
				break;
	
			case STANDARD:
				break;
	
			case ELIT3PR0HAX0RS:
				break;
		}
	}

	void StateOffensive()
	{
		switch (difficulty)
		{
			case NOOBS:
				break;
	
			case STANDARD:
				break;
	
			case ELIT3PR0HAX0RS:
				break;
		}
	}

	void StateDefensive()
	{
		switch (difficulty)
		{
			case NOOBS:
				break;
	
			case STANDARD:
				break;
	
			case ELIT3PR0HAX0RS:
				break;
		}
	}

	void StateCelebrate()
	{

	}

	void MoveTowardsObject(GameObject obj)
	{
		//if (Physics2D.Raycast(this.gameObject.transform.position
	}

	void ShootTowardsObject(GameObject obj)
	{

	}
}

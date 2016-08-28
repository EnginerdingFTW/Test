using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private List<Vector2> waypoints;
	private int stateMachine = 1;

	[HideInInspector] public float horizontal;
	[HideInInspector] public float vertical;
	[HideInInspector] public int drift;
	[HideInInspector] public bool fire;
	public int difficulty = 2;
		

	void Start()
	{
		gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
	}

	void Update()
	{
		MoveTowardsObject(GameObject.Find("TayShip"));
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
		List<GameObject> path = PathFinding.ReturnAStarPath(this.gameObject, obj);
		Debug.Log("count = " + path.Count.ToString());
		for (int k = 0; k < path.Count; k++)
		{
			this.horizontal = this.transform.position.x - path[k].transform.position.x;
			this.vertical = path[k].transform.position.y - this.transform.position.y;
			int i = 0;
			while (i < 100000)
			{
				i++;
			}
		}
	}

	void ShootTowardsObject(GameObject obj)
	{

	}

	GameObject FindClosestObject(GameObject[] list)
	{
		float distance = 1000000;
		GameObject temp = null;
		foreach (GameObject obj in list)
		{
			if (Vector3.Distance(this.transform.position, obj.transform.position) < distance)
			{
				temp = obj;
			}
		}
		return temp;
	}

	bool DetectRaycastWithinRange(Vector2 point, Vector2 direction, out RaycastHit2D hit, float limit)
	{
		hit = Physics2D.Raycast(point, direction);
		if (hit != null)
		{
			if (hit.distance > limit)
			{
				return false;
			}
		}
		return true;
	}

	
}

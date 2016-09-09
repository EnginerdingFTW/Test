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
	private int stateMachine = 1;
	private float reactionTime = 0.2f;
	private bool aiPathSetRecent = false;
	private List<GameObject> pathFindingWaypoints = null;
	private float speed = 1.0f;

	[HideInInspector] public float horizontal;
	[HideInInspector] public float vertical;
	[HideInInspector] public int drift;
	[HideInInspector] public bool fire;
	public int difficulty = 2;

		

	void Start()
	{
		gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
		StartCoroutine("MoveTowardsObjectThread");
		this.stateMachine = OFFENSIVE;
	}

	void Update()
	{
		this.fire = true;
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
		//ShootTowardsObject(GameObject.Find("TayShip"));
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
		if (aiPathSetRecent == false)
		{
			aiPathSetRecent = true;
			List<string> tagExc = new List<string> {"Boundary", "WeaponFire"};
			pathFindingWaypoints = PathFinding.ReturnAStarPath(this.gameObject, obj, tagExc);	//path is the List of Waypoints to get to the goal.
			//for (int k = 0; k < pathFindingWaypoints.Count; k++)
			//{
				//Debug.Log("Path[k] = Path[" + k.ToString() + "] = " + pathFindingWaypoints[k].name);
			//}
		}
	}

	IEnumerator MoveTowardsObjectThread()
	{	
		
		Vector2 posThere = new Vector2(0, 0);
		Vector2 posHere = new Vector2(0, 0);
		while (true)
		{
			yield return new WaitForSeconds(reactionTime);
			if (pathFindingWaypoints != null)
			{
				
				while (pathFindingWaypoints.Count > 0)
				{	
					posThere = new Vector2(pathFindingWaypoints[0].transform.position.x, pathFindingWaypoints[0].transform.position.y);
					posHere = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
					if (Vector2.Distance(posHere, posThere) < 0.5f)
					{
						pathFindingWaypoints.Remove(pathFindingWaypoints[0]);
					}
					posThere = new Vector2(pathFindingWaypoints[0].transform.position.x, pathFindingWaypoints[0].transform.position.y);
					//Debug.Log("posThere = " + posThere.ToString());
					this.horizontal = (posThere.x - posHere.x) * speed;  
					this.vertical = (posThere.y - posHere.y) * speed;
					yield return new WaitForSeconds(reactionTime);
					aiPathSetRecent = false;
				}
			}
		}
	}

	

	void ShootTowardsObject(GameObject obj)
	{	
		this.fire = false;
		List<string> tagExc = new List<string> {"Boundary", "WeaponFire"};
		if (!PathFinding.RaycastAllWithExeptions(this.gameObject, obj, tagExc))	
		{
			this.fire = true;
		}
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

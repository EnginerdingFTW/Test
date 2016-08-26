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
		waypoints.Clear();
		waypoints.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
		Vector2 movement = new Vector2(waypoints[0].x - this.transform.position.x, this.transform.position.y - waypoints[0].y) * -1;
		movement = movement.normalized;

		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		RaycastHit2D hit;
		if (DetectRaycastWithinRange(pos, movement, hit, 10.0f))
		{
			//waypoints.Insert(0, item)
			for (int angle = 5; angle < 360; angle += 5)
			{
				Vector2 temp = Quaternion.AngleAxis(angle, Vector3.forward) * movement;
				if (!DetectRaycastWithinRange(pos, temp, hit, 10.0f))
				{
					movement = temp;
					break;
				}
			}		
			
		}
		
		



/*		
		if (DetectRaycastWithingRange(leftRay, Vector2.down, out hit, 10.0f) && hit.transform.tag == "Wall")
		{
			float distance1 = 0;
			float distance2 = 0;
			if (DetectRaycastWithingRange(leftRay, (new Vector2(-0.1f, -1)).normalized, out hit, 10.0f) && hit.transform.tag == "Wall")
			{
				distance1 = hit.distance;
			}
			if (DetectRaycastWithingRange(leftRay, (new Vector2(0.1f, -1)).normalized, out hit, 10.0f) && hit.transform.tag == "Wall")
			{
				distance2 = hit.distance;
			}

			if (distance1 < distance2)
			{
				//rotate clockwise
				movement = Quaternion.Euler(0, 0, -60) * movement;
			} 
			else 
			{
				//rotate counter clockwise	
				movement = Quaternion.Euler(0, 0, 60) * movement;
			}
		}
		this.vertical = movement.y;
		this.horizontal = movement.x;
		Debug.Log("final: " + movement.ToString());

*/
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

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
		Vector3 movement = new Vector3(this.gameObject.transform.position.x - obj.transform.position.x, obj.transform.position.y - this.gameObject.transform.position.y, 0);
		movement = movement.normalized;

		RaycastHit hit;
		Transform leftRay = this.transform;
		if (Physics.Raycast(leftRay.position, Vector3.down, out hit, 5.0f))
		{
			float distance1 = 0;
			float distance2 = 0;
			if (Physics.Raycast(leftRay.position, new Vector3(-0.1f, -1, 0), out hit, 5.0f))
			{
				distance1 = hit.distance;
			}
			if (Physics.Raycast(leftRay.position, new Vector3(0.1f, -1, 0), out hit, 5.0f))
			{
				distance2 = hit.distance;
			}

			if (distance1 < distance2)
			{
				//rotate clockwise
				movement = Quaternion.Euler(0, 0, -1) * movement;
			} 
			else 
			{
				//rotate counter clockwise	
				movement = Quaternion.Euler(0, 0, 1) * movement;
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
	
}

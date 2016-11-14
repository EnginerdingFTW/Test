using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {
	
	//simple struct for sorting the different objetives
	public class WeightedObjective
	{
		public float weight;
		public GameObject obj;

		public WeightedObjective(float wt, GameObject objective)
		{
			this.weight = wt;
			this.obj = objective;
		}

		public static bool Compare(WeightedObjective a, WeightedObjective b)
		{
			if (a.weight < b.weight)
			{
				return false;
			}
			return true;
		}
	}

	#region Constants for StateMachine and Difficulty
	private const int COLLECT = 0;			//States
	private const int OFFENSIVE = 1;
	private const int DEFENSIVE = 2; 
	private const int CELEBRATE = 3;
	private const int NOOBS = 0;			//Difficulties
	private const int STANDARD = 1;
	private const int ELIT3PR0HAX0RS = 2;
	#endregion
	
	private Player player;
	private GameController gamecontroller;					//gamecontroller pointer to get lists of gameobjects for various purposes
	private int stateMachine = OFFENSIVE;					//current state of the state maching
	private float reactionTime = 0.2f;						//reaction time of the AI
	private bool aiPathSetRecent = false;					//current state of PathSet, makes calculations less frequent
	private List<GameObject> pathFindingWaypoints = null;	//list of gameobjects that make the shortest path to an objective
	private float speed = 1.0f;								//speed of AI, probably unecessary because it should be taken care of in Player

	//AI relevent, weights and settings
	private List<GameObject> watchList = null;				//list of gameobjects to watch as objects
	private bool watchListChanged = false;					//tells the ObjectiveList watcher that a new object is in the list or removed
	private List<WeightedObjective> objList = null;			//list of sorted objectives based on weights
	private bool stateChange = false;						//signal that notifies if there was a change in the state machine
	private bool celebratestate = false;
	private float playerWeight = 1;							//the weight of players based on difficulty for use in sorting
	private float itemWeight = 1;							//the weight of items based on difficulty for use in sorting
	private float areaWeight = 1;							//the weight of areas of maps based on difficulty for use in sorting
	private float currentObjWeight = 1.0f;					//weight of the objective currently being moved to
	private GameObject currentObj = null;					//current Gameobject main objectiv
	public int difficulty =	ELIT3PR0HAX0RS;					//difficulty of the AI

	//Temporary Credits fix (Ignore weapons)
	public bool isCredits = false;							//will the AI acknowledge weapons

	#region Virtual Controls to the Player
	[HideInInspector] public float horizontal;		//these controls act as if the AI is using a controller and is pressing buttons
	[HideInInspector] public float vertical;		//these are sent to the player script where it takes care of the rest.
	[HideInInspector] public int drift;
	[HideInInspector] public bool fire;
	[HideInInspector] public bool boost;
	#endregion
		

	void Start()
	{
		gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
		player = this.gameObject.GetComponent<Player>();
		this.stateMachine = DEFENSIVE;
		watchList = new List<GameObject>();
		objList = new List<WeightedObjective>();
		OnEnable();
		switch (difficulty)
		{
			case NOOBS:
				reactionTime = 0.7f;
				break;
	
			case STANDARD:
				reactionTime = 0.3f;
				break;
	
			case ELIT3PR0HAX0RS:
				reactionTime = 0.1f;
				break;
		}
	}

	void Update()
	{
		switch (stateMachine)
		{
			case COLLECT:
				if (isCredits) {
					StateOffensive ();
				} else {
					StateCollect ();
				}
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
		DetermineAIAction();	
		SetNextState();
	}

	#region Objective Watching 
	/// <summary>
	/// This thread is modifying the watchList list of gameobjects. If it detects a change in the lists of objects that can be in here, it
	/// modifies the watchList and notifies the ObjectiveSorter
	/// </summary>
	IEnumerator WatchGameObjectsAddRemoveThread()
	{	
		List<GameObject> temp = new List<GameObject>();
		while (true)
		{	
			try
			{
				temp.Clear();
				if (gamecontroller != null)
				{
					if (!isCredits) {
						foreach (GameObject o in gamecontroller.SpawnerList)
						{
							if (GameObject.Find("InnerBoundary").GetComponent<BoxCollider2D>().bounds.Contains(o.transform.position))
							{
								temp.Add(o);
							}
						}
						foreach (GameObject o in gamecontroller.CollectableList)
						{
							if (GameObject.Find("InnerBoundary").GetComponent<BoxCollider2D>().bounds.Contains(o.transform.position))
							{
								temp.Add(o);
							}
						}
					}
					foreach (GameObject o in gamecontroller.players)
					{
						if (o.activeSelf == true && o.gameObject != this.gameObject)
						{
							temp.Add(o);
						}
					}
					//if (temp.Count != watchList.Count)
					//{
						watchList = temp;
						//Debug.Log(watchList.Count);
					//}	
				}
				else
				{
					Debug.Log("gamecontroller is null.");
				}
			}
			catch 
			{
				Debug.Log("Error in watchGameObjectsAddREmoveThread");
			}
			yield return new WaitForSeconds(reactionTime);
		}
	}

	/// <summary>
	/// When notified that there has been a change in the watchlist or in state, the ObjectList is resorted
	/// </summary>
	IEnumerator WatchObjectiveSorterThread()
	{
		if (watchList.Count != 0)
		{
			List<WeightedObjective> temp = new List<WeightedObjective>();
			foreach (GameObject o in watchList)
			{
				if (o != null || o.activeSelf == false)
				{
					temp.Add(new WeightedObjective(1.0f, o));
				}	
			}
			objList = temp;
			ReOrderObjectives();
//			Debug.Log("watching objective sorter thread 1");
		}
		else if (stateChange == true && objList.Count != 0)
		{
//			Debug.Log("watching objective sorter thread 2");
			ReOrderObjectives();
		} 
		else
		{
//			Debug.Log("NEITHER ARE HAPPENING");
		}
//		Debug.Log("watching objective sorter thread 3");

		stateChange = false;
		watchListChanged = true;

		yield break;
	}

	IEnumerator ThreadWatcher()
	{
		while (true)
		{
			StartCoroutine(WatchObjectiveSorterThread());
			yield return new WaitForSeconds(reactionTime);
		}
	}


	/// <summary>
	/// Function to restart threads whenever the player/AI is re-enabled
	/// </summary>
	void OnEnable()
	{
		StartCoroutine("MoveTowardsObjectThread");
		StartCoroutine("WatchGameObjectsAddRemoveThread");
		StartCoroutine("ThreadWatcher");
	}
	#endregion
	
	#region StateMachine Logic
	/// <summary>
	/// Sets the values of the weights for objectives based on the difficulty when in COLLECT
	/// </summary>
	void StateCollect()
	{
		//Debug.Log("Collecting");
		switch (difficulty)
		{
			case NOOBS:
				playerWeight = 1.0f; 
				itemWeight = 4.0f;
				areaWeight = 0.1f;
				break;
	
			case STANDARD:
				playerWeight = 2.0f; 
				itemWeight = 10.0f;
				areaWeight = 1.0f;
				break;
	
			case ELIT3PR0HAX0RS:
				playerWeight = 0.1f; 
				itemWeight = 15.0f;
				areaWeight = 5.0f;
				break;
		}
	}

	/// <summary>
	/// Sets the values of the weights for objectives based on the difficulty when in OFFENSIVE
	/// </summary>
	void StateOffensive()
	{
		//Debug.Log("Attacking");
		switch (difficulty)
		{
			case NOOBS:
				playerWeight = 10.0f; 
				itemWeight = 1.0f;
				areaWeight = 0.1f;
				break;
	
			case STANDARD:
				playerWeight = 10.0f; 
				itemWeight = 7.0f;
				areaWeight = 0.1f;
				break;
	
			case ELIT3PR0HAX0RS:
				playerWeight = 15.0f; 
				itemWeight = 1.0f;
				areaWeight = 1.0f;
				break;
		}
	}

	/// <summary>
	/// Sets the values of the weights for objectives based on the difficulty when in DEFENSIVE
	/// </summary>
	void StateDefensive()
	{
		//Debug.Log("Defending");
		switch (difficulty)
		{
			case NOOBS:
				playerWeight = 2.0f; 
				itemWeight = 4.0f;
				areaWeight = 1.0f;
				break;
	
			case STANDARD:
				playerWeight = 0.5f; 
				itemWeight = 12.0f;
				areaWeight = 2.0f;
				break;
	
			case ELIT3PR0HAX0RS:
				playerWeight = 0.5f; 
				itemWeight = 15.0f;
				areaWeight = 4.0f;
				break;
		}
	}

	/// <summary>
	/// There's a party goin' on right here! A celebration to last throughout the years.
	/// </summary>
	void StateCelebrate()
	{
		if (!celebratestate)
		{
			StopAllCoroutines();
			StartCoroutine(Celebrating());
		}
		celebratestate = true;
	}

	/// <summary>
	/// Logic to determine the next state that the state machine goes to based on difficulty
	/// </summary>
	void SetNextState()
	{
		int tempState = stateMachine;
		switch (difficulty)
		{
			case NOOBS:
				if (gamecontroller.CollectableList.Count == 0 && player.weapons.Count == 0)
				{
					stateMachine = DEFENSIVE;
				}
				else if (gamecontroller.CollectableList.Count > 0 && player.weapons.Count == 0)
				{
					stateMachine = COLLECT;
				}
				else if (player.weapons.Count > 0)
				{
					stateMachine = OFFENSIVE;
				}
				break;
	
			case STANDARD:
				if (gamecontroller.CollectableList.Count == 0 && player.weapons.Count == 0)
				{
					stateMachine = DEFENSIVE;
				}
				else if (gamecontroller.CollectableList.Count > 0 && player.weapons.Count < 3)
				{
					stateMachine = COLLECT;
				}
				else if (player.weapons.Count >= 3)
				{
					stateMachine = OFFENSIVE;
				}
				break;
	
			case ELIT3PR0HAX0RS:
				if ((gamecontroller.CollectableList.Count == 0 && player.weapons.Count == 0) || (objList.Count > 0 && CompareWeapon(objList[0].obj) == true))
				{
					stateMachine = DEFENSIVE;
				}
				else if (gamecontroller.CollectableList.Count > 0 && (player.currentWeapon == null || player.currentWeapon.laserType.name != "LaserUpgraded"))
				{
					stateMachine = COLLECT;
				}
				else if (player.currentWeapon != null && player.currentWeapon.laserType.name == "LaserUpgraded")
				{
					stateMachine = OFFENSIVE;
				}
				break;
		}
		if (gamecontroller.numPlayers == 1)	
		{
			stateMachine = CELEBRATE;
		}
		if (tempState != stateMachine)
		{
			stateChange = true;
		}
	}
	#endregion

	#region Functions for Updating Weights
	/// <summary>
	/// Goes through the list of objects and modifies there weight based on the distance and difficulty, 
	/// then sorts the list based on the changes.
	/// </summary>
	void ReOrderObjectives()
	{
		for (int k = 0; k < objList.Count; k++)
		{	
			objList[k].weight = ModifyWeight(objList[k]);
		}
		objList = objList.OrderByDescending(p => p.weight).ToList();	//telling list to sort by weights
//		DetermineObjectToMoveTowards();
		//Debug.Log("reordinger objectives, objList.Count = " + objList.Count);
		string temp = this.gameObject.name + ": list = ";
		foreach (WeightedObjective t in objList)
		{
			temp += t.obj.name + ", " + t.weight + ", ";
		}
//		Debug.Log(temp);
		
	}

	/// <summary>
	/// takes the objective and sets it's weight based on the distance it is from the AI and on how important it is based on difficulty
	/// </summary>
	/// <param name="objective">the Objective</param>
	float ModifyWeight(WeightedObjective objective)
	{
		float newWeight = 1.0f;
		switch(objective.obj.tag)
		{
			case "Player":
				newWeight = CalculateWeight(playerWeight, objective.obj);
				break;

/*
			case "Asteroid":
				newWeight = CalculateWeight(itemWeight, objective.obj);
				break;
*/

			case "Collectable":
				newWeight = CalculateWeight(itemWeight, objective.obj);
				break;

			case "ItemSpawner":
				newWeight = CalculateWeight(areaWeight, objective.obj);
				break;
		}
		return newWeight;
	}

	/// <summary>
	/// Helper function because the distance to the objective can't be 0.
	/// </summary>
	/// <returns>The weight.</returns>
	/// <param name="objWeight">Object weight.</param>
	/// <param name="objective">Objective.</param>
	float CalculateWeight(float objWeight, GameObject objective)
	{
		if (Vector3.Distance(this.gameObject.transform.position, objective.transform.position) < 0.01)
		{
			return 100.0f;
		}
		return objWeight / Vector3.Distance(this.gameObject.transform.position, objective.transform.position);
	}
	#endregion

	#region AI Actions
	/// <summary>
	/// Compares the weapon of the AI and the other player. Returns true if the AI's weapon is better. Also returns
	/// false if the other object is not a player
	/// </summary>
	/// <returns><c>true</c>, AI has better weapon <c>false</c> otherwise.</returns>
	/// <param name="other">Other player</param>
	bool CompareWeapon(GameObject other)
	{
		if (other.GetComponent<Player>() == null)
		{
			return false;
		}	
		int myWeapon = rateWeapon(this.gameObject);
		int thereWeapon = rateWeapon(other);
		if (myWeapon > thereWeapon)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Rates the weapon attached to a player
	/// </summary>
	/// <returns>The weapon.</returns>
	/// <param name="playerObject">Player object.</param>
	int rateWeapon(GameObject playerObject)
	{
		int weaponPower = 0;
		if (playerObject.GetComponent<Player>().currentWeapon != null)
		{
			switch (playerObject.GetComponent<Player>().currentWeapon.laserType.name)
			{
				case "EMP":
					weaponPower = 6;
					break;
				
				case "EpicLaser":
					weaponPower = 2;
					break;
		
				case "LaserUpgrade":
					weaponPower = 7;
					break;
				
				case "Mine":
					weaponPower = 4;
					break;
					
				case "Missile":
					weaponPower = 3;
					break;
				
				case "Nuke":
					weaponPower = 5;
					break;
			
				case "StunBolt":
					if (gamecontroller.numPlayers == 2)
					{
						weaponPower = 8;
					} 
					else 
					{
						weaponPower = 1;
					}
					break;
			}
		}
		return weaponPower;
	}
	
	void DetermineAIAction()
	{
		if (objList.Count != 0)
		{
			DetermineObjectToMoveTowards();
//			ShootTowardsObject(currentObj);
			StartCoroutine("ShootingThread");
			switch (difficulty)
			{
				case NOOBS:
					MoveTowardsObject(currentObj);
					break;
		
				case STANDARD:
					MoveTowardsObject(currentObj);
					break;
		
				case ELIT3PR0HAX0RS:
					MoveTowardsObject(currentObj);
					break;
			}
		}
	}

	void DetermineObjectToMoveTowards()
	{
		for (int k = 0; k < objList.Count; k++)
		{
			if (GameObject.Find("InnerBoundary").GetComponent<BoxCollider2D>().bounds.Contains(objList[k].obj.transform.position))
			{
				currentObj = objList[k].obj;
//				Debug.Log("Current Obj = " + currentObj.name);
				break;
			}
		}
	}
	GameObject FindClosestPlayerObj()
	{
		for (int k = 0; k < objList.Count; k++)
		{
			if (objList[k].obj.tag == "Player")
			{
				return objList[k].obj;
			}
		}
		return null;
	}
	#endregion

	#region AI Movement
	IEnumerator Celebrating()
	{
		while (true)
		{
			drift = 1;
			this.horizontal = 2.0f;
			this.vertical = 2.0f;
			yield return new WaitForSeconds(0.2f);
			this.horizontal = -2.0f;
			this.vertical = 2.0f;
			yield return new WaitForSeconds(0.2f);
			this.horizontal = -2.0f;
			this.vertical = -2.0f;
			yield return new WaitForSeconds(0.2f);
			this.horizontal = 2.0f;
			this.vertical = -2.0f;
			yield return new WaitForSeconds(0.2f);
		}
	}
		
	/// <summary>
	/// Takes a destination and calculate all waypoints that must be taken to get there for the shortest path possible.
	/// The waypoints are pointed to by pathFindingWayPoints so the the list is viewable in other places at the same time.
	/// </summary>
	/// <param name="dest"> the destination object </param>
	void MoveTowardsObject(GameObject dest)
	{	
		if (aiPathSetRecent == false)
		{
//			Debug.Log("new path set");
			aiPathSetRecent = true;
			List<string> tagExc = new List<string> {"Boundary", "WeaponFire", "Player", "Credits"};
			pathFindingWaypoints = PathFinding.ReturnAStarPath(this.gameObject, dest, tagExc);	//path is the List of Waypoints to get to the goal.
//			Debug.Log("start = " + this.gameObject.name + "    dest = " + dest.name);
//			Debug.Log("Tags: start.tag = " + this.gameObject.tag + "   dest = " + dest.tag);
			string temp = "printing PathFindingWaypoitns after Calc: PathFindingWaypoints.count = " + pathFindingWaypoints.Count + "\nlist: ";
			foreach (GameObject o in pathFindingWaypoints)
			{
				temp += o.name + ", ";
			}
//			Debug.Log(temp);
		}
	}

	/// <summary>
	/// This function is a thread the watches the pathFindingWaypoints list. It sets the AI velocity to move the shortest path to the
	/// objective (the final node of the list). When a node has been reached it is removed from the list and the next node is moved towards.
	/// reactionTime is there to delay how often this expression is re-evaluated, not only to limit calculations but also to simulate normal
	/// delay a human would have in noticing there is a new objective.
	/// </summary>
	/// <returns>The towards object thread.</returns>
	IEnumerator MoveTowardsObjectThread()
	{		
		Vector2 posThere = new Vector2(0, 0);
		Vector2 posHere = new Vector2(0, 0);
		while (true)
		{
//			Debug.Log("still moving1");
			yield return new WaitForSeconds(reactionTime);
			if (pathFindingWaypoints != null)
			{
				while (pathFindingWaypoints.Count > 0)
				{	
					if (pathFindingWaypoints[0] != null)
					{
						posThere = new Vector2(pathFindingWaypoints[0].transform.position.x, pathFindingWaypoints[0].transform.position.y);
						posHere = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
						if (Vector2.Distance(posHere, posThere) < 0.5f)
						{
							pathFindingWaypoints.Remove(pathFindingWaypoints[0]);
						}	
					}
					if (pathFindingWaypoints.Count > 0 && pathFindingWaypoints[0] != null)
					{
						posThere = new Vector2(pathFindingWaypoints[0].transform.position.x, pathFindingWaypoints[0].transform.position.y);
						//Debug.Log("posThere = " + posThere.ToString());


						this.drift = 0;
						if (pathFindingWaypoints[0].tag != "Player" && this.difficulty == ELIT3PR0HAX0RS)
						{
							GameObject tempplay = FindClosestPlayerObj();
							StartCoroutine(HoverShoot(pathFindingWaypoints[0], tempplay));
						}
						else
						{
							this.horizontal = (posThere.x - posHere.x) * speed;  
							this.vertical = (posThere.y - posHere.y) * speed;
						}
						this.boost = false;
						if (this.difficulty != NOOBS && (pathFindingWaypoints[0].name == "Nuke_Powerup" || pathFindingWaypoints[0].name == "Rapid Fire Powerup"))
						{
							this.boost = true;
						}
					}
					yield return new WaitForSeconds(reactionTime * 2);
					aiPathSetRecent = false;
				}
				if (pathFindingWaypoints.Count == 0)
				{
					aiPathSetRecent = false;
//					Debug.Log("pathfindingwaypoints.count = 0");
				}
			}
		}
	}
	#endregion

	#region AI Fire
	/// <summary>
	/// Shoots a weaponfire towards it's objective (obj) when the target is visible or is shootable by the current weaponfire
	/// </summary>
	/// <param name="obj">the object to be shot.</param>
	void ShootTowardsObject(GameObject obj)
	{	
		if (obj == null)
		{
			Debug.Log("obj is null");
			return;
		}

		this.fire = false;
		for (float k = -0.1f; k <= 0.1f; k = k + 0.1f)
		{
			RaycastHit2D[] hitarr = Physics2D.RaycastAll(new Vector2(this.transform.position.x, this.transform.position.y), -transform.up + new Vector3(k, k, 0));
			foreach (RaycastHit2D hit in hitarr)
			{
//				Debug.Log(this.gameObject.name + ": " + hit.collider.gameObject.name);
				if (hit.collider.tag == "Wall")
				{
					break;
				}
				if (hit != null && hit.collider.tag == "Player" && hit.collider.gameObject != this.gameObject)
				{
//					Debug.Log("hitt");
					this.fire = true;
					break;
				}
			}
			if (fire == true)
			{
				break;
			}
		}
	}

	IEnumerator ShootingThread()
	{
		yield return new WaitForSeconds(Random.Range(reactionTime, reactionTime + 0.2f));
		ShootTowardsObject(currentObj);
	}

	IEnumerator HoverShoot(GameObject dest, GameObject toShoot)
	{
		this.horizontal = (dest.transform.position.x - this.gameObject.transform.position.x) * speed;  
		this.vertical = (dest.transform.position.y - this.gameObject.transform.position.y) * speed;
		yield return new WaitForSeconds(0.2f);
		this.drift = 1;
		this.horizontal = (toShoot.transform.position.x - this.gameObject.transform.position.x) * speed;  
		this.vertical = (toShoot.transform.position.y - this.gameObject.transform.position.y) * speed;
	}
	#endregion


	
}

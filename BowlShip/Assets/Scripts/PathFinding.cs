using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathFinding {

	public static List<GameObject> ReturnAStarPath(GameObject start, GameObject goal)
	{
		GameObject[] nodelist = GameObject.FindGameObjectsWithTag("Waypoint");
		return AStarPathFinding(start, nodelist, goal);
	}
		
	public static List<GameObject> AStarPathFinding(GameObject start, GameObject[] waypoints, GameObject goal)
	{
			//initiallizing for run of Astar	
		List<GameObject> closedSet = new List<GameObject>();
		List<GameObject> openSet = new List<GameObject>();
		openSet.Add(start);
		List<GameObject> cameFrom = new List<GameObject>();

		//The Gameobject (node) is the key, the float is the distance to the object
		Dictionary<GameObject, float> gScore = new Dictionary<GameObject, float>();
		foreach (GameObject node in waypoints)
		{
			gScore.Add(node, 10000000f);
		}
		gScore.Add(start, 0f);
		gScore.Add(goal, 10000000f);

		Dictionary<GameObject, float> fScore = new Dictionary<GameObject, float>();
		foreach (GameObject node in waypoints)
		{
			fScore.Add(node, 10000000f);
		}
		fScore.Add(start, heuristic_cost_estimate(start, goal));
		fScore.Add(goal, 10000000f);




		return openSet;
	}

	static float heuristic_cost_estimate(GameObject obj1, GameObject obj2)
	{
		return Vector3.Magnitude(obj1.transform.position - obj2.transform.position);
	}

		//don't know if I need this just moving it just in case for layer
	static bool DetectRaycastWithinRange(Vector2 point, Vector2 direction, out RaycastHit2D hit, float limit)
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

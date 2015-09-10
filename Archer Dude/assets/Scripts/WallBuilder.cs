using UnityEngine;
using System.Collections;

public class WallBuilder : MonoBehaviour {

	public GameObject torch;
	public GameObject[] wallTiles;
	public GameObject[] destroyedTiles;
	public int xBorder;
	public int yBorder;
	public float destroyedPercent;
	
	private Transform wallHolder;
	
	public void BoardSetup()
	{
		wallHolder = this.transform;
		
		for (int x = 0; x < xBorder; x++)
		{
			for (int y = 0; y < yBorder; y++)
			{
				GameObject toInstantiate;
				if (Random.Range(0f,1f) < destroyedPercent)
				{
					toInstantiate = destroyedTiles[Random.Range(0, destroyedTiles.Length)];
				}
				else 
				{
					toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
				}
				if (x % 4 == 0 && y == 1)
				{
					GameObject newObject = Instantiate(torch, new Vector3(x * 1.56f + wallHolder.position.x, y * 1.56f + wallHolder.position.y, 0f + wallHolder.position.z), Quaternion.identity) as GameObject;
					newObject.transform.parent = this.transform.FindChild("Torches").transform;
				}
				
				GameObject instance = Instantiate(toInstantiate, new Vector3(x * 1.56f + wallHolder.position.x, y * 1.56f + wallHolder.position.y, 0f + wallHolder.position.z), Quaternion.identity) as GameObject;
				instance.transform.parent = this.transform;
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		BoardSetup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

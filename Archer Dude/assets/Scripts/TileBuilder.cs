using UnityEngine;
using System.Collections;

public class TileBuilder : MonoBehaviour {

	public GameObject torch;				//torch prefab - only used in the case of walls.
	public GameObject[] Tiles;				//prefabs of potential tiles that will be chosen to be used (randomly chosen from array)
	public GameObject[] destroyedTiles;		//same as Tiles but destroyed/damaged tiles
	public int xBorder;						//xBorder of tiles, how many in x direction will be made
	public int yBorder;						//yBorder of tiles, how many in y direction will be made
	public float destroyedPercent;			//percent of destroyed tiles that will be placed in the array.
	public int wave = 0;					//the amount of times you've reached the end of instantiated tiles


/// <summary>
/// Builds the array of tiles element by element, choosing from either the array of regular tiles or destroyed tiles based on the 
/// destroyed percent. This function only works for tiles of the same size but otherwise works good for any tile
/// </summary>
/// <param name="tag"> tag is the tag that will be assigned to the instantiation. </param>
	public void BoardSetup(string tag)
	{
		Transform tileHolder = this.transform;			//gets the transform of this object as the starting a placement
			
		for (int x = 0; x < xBorder; x++)
		{
			for (int y = 1; y < yBorder + 1; y++)
			{
				GameObject toInstantiate;
				if (Random.Range(0f,1f) < destroyedPercent)
				{
					toInstantiate = destroyedTiles[Random.Range(0, destroyedTiles.Length)];
				}
				else 
				{
					toInstantiate = Tiles[Random.Range(0, Tiles.Length)];
				}
				if (x % 4 == 0 && y == 2 && torch != null)
				{
					GameObject newObject = Instantiate(torch, new Vector3(x * 1.56f + tileHolder.position.x + (1.56f * xBorder * wave), y * 1.56f + tileHolder.position.y, 0f + tileHolder.position.z), Quaternion.identity) as GameObject;
					newObject.transform.parent = this.transform.FindChild("Torches").transform;
				}
				
				GameObject instance = Instantiate(toInstantiate, new Vector3(x * 1.56f + tileHolder.position.x + (1.56f * xBorder * wave), y * 1.56f + tileHolder.position.y, 0f + tileHolder.position.z), Quaternion.identity) as GameObject;
				instance.transform.parent = this.transform;
				instance.tag = tag;
			}
		}
	}

	//once the player gets near the end of the instantiated walls/floor, instantiate some more
	void OnTriggerEnter2D() {
		wave += 1;
		if (this.gameObject.transform.CompareTag ("Wall")) {
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((wave + 1) * xBorder / 4, 1);
		} else {
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((wave + 1) * xBorder * 5 / 4, 10);
		}
		BoardSetup (this.gameObject.transform.tag);
	}

	//initialize the first series of walls and floors, set up the trigger zone for next instantiation
	void Start () { 
		if (this.gameObject.transform.CompareTag ("Wall")) {
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((wave + 1) * xBorder / 4, 1);
			gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (1, 5);
		} else {
			gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 ((wave + 1) * xBorder * 5 / 4, 10);
			gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (5, 25);
		}
		BoardSetup(this.gameObject.transform.tag);
	}
}

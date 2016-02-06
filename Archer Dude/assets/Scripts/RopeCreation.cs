using UnityEngine;
using System.Collections;


/// <summary>
/// To create the rope, an initial start and end piece are placed using the unity inspector. From there
/// this script fills in the rope pieces inbetween the start and end.
/// </summary>
public class RopeCreation : MonoBehaviour {

	private GameObject ropeStart;		//starting piece of rope.
	private GameObject ropeEnd;			//ending piece of rope
	private GameObject[] ropeLink;		//array of ropeLinks - this is each piece of rope
	private int ropeLength;				//number of pieces of rope (calculated)

	public GameObject ropeLinkOG;		//prefab of rope
	public bool slack;					//do you want slack? TRUE || FALSE?!?!?!?
	public int slackAmount = -1;		//slack to add/remove, negative removes slack
	public bool ropeBroke = false;

	void Update()
	{
		foreach (GameObject rope in ropeLink)
		{
			if (rope.GetComponent<HingeJoint2D>() == null)
			{
				ropeBroke = true;
			}
		}
	}

	void Start () 
	{
			//Get the start and end piece initialized
		ropeStart = this.transform.Find ("RopeStart").gameObject;  
		ropeEnd = this.transform.Find ("RopeEnd").gameObject;

			//Calculate the amount of ropeLinks needed between the start and end
		ropeLength = (int) (Vector3.Distance(ropeEnd.gameObject.transform.position, ropeStart.gameObject.transform.position) * 5f);  

			//this is for adding/removing slack from the rope. 
		if(slack)
		{
			ropeLength = ropeLength + slackAmount;	
		}

			//initializing arrays to correct size
		ropeLink = new GameObject[ropeLength];

			//creates rope to fill the gap between the start and finish, automatically connects via a chain of distance2d joints
		for(int i = 0; i < ropeLength ; i++)
		{ 
			HingeJoint2D joint;		//the joint of the rope that needs to be set so each piece of rope is connected
			ropeLink[i] = (GameObject) Instantiate(ropeLinkOG, new Vector3(ropeStart.transform.position.x, (ropeStart.transform.position.y - (0.22f* (i + 1))), ropeStart.transform.position.z), Quaternion.identity);  
			ropeLink[i].transform.parent = this.transform;
			ropeLink[i].tag = "Rope";
			joint = ropeLink[i].GetComponent<HingeJoint2D>();

				//connect the first piece of rope to the intial rope piece. otherwise connect it to the last piece of rope
			if(i == 0)
			{
				joint.connectedBody = ropeStart.GetComponent<Rigidbody2D>();
			}
			else
			{
				joint.connectedBody = ropeLink[i - 1].GetComponent<Rigidbody2D>();
			}
		}
		
		ropeEnd.GetComponent<HingeJoint2D>().connectedBody = ropeLink[ropeLength - 1].GetComponent<Rigidbody2D>();
	}

}

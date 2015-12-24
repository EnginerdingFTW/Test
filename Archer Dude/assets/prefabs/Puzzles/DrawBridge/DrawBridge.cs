using UnityEngine;
using System.Collections;

/// <summary>
/// The drawbridge controller script
/// </summary>
public class DrawBridge : MonoBehaviour {

	private GameObject bridge;						//the wood part of the bridge that raises and lowers
	private GameObject enemyStop;					//enemy stopper for this puzzle
	private bool go = false;						//true when the bridge has lowered
	

	public GameObject target;						//the target gameobject that when

	void Start () 
	{
			//getting components
		bridge = this.transform.FindChild("DrawBridgeUpright_0").gameObject;
		bridge.GetComponent<PolygonCollider2D>().enabled = false;
		enemyStop = this.transform.FindChild("oncollisionidle").gameObject;
		enemyStop.GetComponent<OnCollisionIdle>().stop = true;
	}
	
		//when the target is hit, trigger the lower animation
	void Update ()
	{
		if (target != null && target.GetComponent<TargetOnOff>().targetOnOff)
		{
			//old pos x = -1.53, y = -1.19, z = 4.9
			bridge.transform.localPosition= new Vector3(-6.64f, -6.47f, 4.9f);
			bridge.transform.localRotation = Quaternion.Euler(0, 180, 90);
			bridge.GetComponent<PolygonCollider2D>().enabled = true;
			if (!go)
			{
				go = true;
				enemyStop.GetComponent<OnCollisionIdle>().stop = false;
				this.GetComponent<EdgeCollider2D>().enabled = false;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

/// <summary>
/// The drawbridge controller script
/// </summary>
public class DrawBridge : MonoBehaviour {

	private Animator animatorDrawBridge;			//animator of bridge
	private Animator animatorPlayer;				//animator of player
	private GameObject bridge;						//the wood part of the bridge that raises and lowers
	private bool go = false;
	

	public GameObject target;						//the target gameobject that when

	void Start () 
	{
			//getting components
		animatorDrawBridge = GetComponent<Animator>();
		GameObject player = GameObject.Find ("Player");
		animatorPlayer = player.GetComponent<Animator>();
		bridge = this.transform.FindChild("DrawBridgeUpright_0").gameObject;
		bridge.GetComponent<PolygonCollider2D>().enabled = false;
	}
	
		// when the player hits the drawbridge, stop it's movement (this will be revised to stop any enemy/ally as well).
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && !go)
		{
			animatorPlayer.SetBool("run", false);
		}
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
				animatorPlayer.SetBool ("run", true);
				go = true;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class RopeBreak : MonoBehaviour {

	public GameObject brokenRope;
	public static float tempy;
	public static bool enact;

	private float contactx;
	private float contacty;
	private float scale;
	private float posy;

	// Use this for initialization
	void Start () 
	{
		scale = 2f * transform.localScale.y;
		posy = this.transform.position.y;
	}

	void OnCollisionEnter2D(Collision2D collision) 
	{
		
		if(collision.gameObject.tag == "Arrow")
		{
			contactx = collision.contacts[0].point.x;
			tempy = collision.contacts[0].point.y;
			contacty = (tempy - posy) + (scale / 2f);

			Vector2 topRope = new Vector2(contactx, posy - (scale / 2f) + (scale - ((scale - contacty)/2f)));
			Vector2 bottomRope = new Vector2(contactx, posy - (scale / 2f) + (contacty - (contacty / 2f)));

			enact = true;
			if(contactx >= 0f)
			{
				if((transform.rotation.z >= 0f))
				{
					Instantiate (brokenRope, topRope, transform.rotation);
					Instantiate (brokenRope, bottomRope, Quaternion.Inverse(transform.rotation));
				}
				else
				{
					Instantiate (brokenRope, topRope, Quaternion.Inverse(transform.rotation));
					Instantiate (brokenRope, bottomRope, transform.rotation);
				}
			}
			else
			{
				if((transform.rotation.z >= 0f))
				{
					Instantiate (brokenRope, topRope, Quaternion.Inverse(transform.rotation));
					Instantiate (brokenRope, bottomRope, transform.rotation);
				}
				else
				{
					Instantiate (brokenRope, topRope, transform.rotation);
					Instantiate (brokenRope, bottomRope, Quaternion.Inverse(transform.rotation));
				}
			}
			enact = false;
			Destroy (this.gameObject);
		}
	}
}

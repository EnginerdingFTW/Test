using UnityEngine;
using System.Collections;

public class LightningWall : MonoBehaviour {

	public int damage = 1;				//How much damage do the lightning walls do upon collision

	private Animator anim;				//The animator this is attached to

	/// <summary>
	/// Get the animator
	/// </summary>
	void Start () {
		anim = GetComponent<Animator> ();
	}

	/// <summary>
	/// Stop things like Asteroids from going through.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D (Collision2D coll) {
		anim.SetTrigger ("Hit");
		if (coll.gameObject.tag.Equals ("Player")) {
			coll.gameObject.GetComponent<Player> ().Hurt (damage);
			foreach (ContactPoint2D loc in coll.contacts)
			{
				coll.gameObject.GetComponent<Player>().CreateExplosionAnimation(new Vector3(loc.point.x, loc.point.y, 0), damage / 2);
			}
		} else {
			Destroy (coll.gameObject);
		}
	}

	/// <summary>
	/// Destroys weapon fire trying to enter.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D (Collider2D coll) {
		anim.SetTrigger ("Hit");
		if (!coll.gameObject.tag.Equals ("Player")) {
			Destroy (coll.gameObject);
		}
	}
}

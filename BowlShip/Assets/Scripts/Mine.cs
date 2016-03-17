using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public GameObject blastRadius;		//gameobject containing collider for the blast radius
	public GameObject explosion;		//prefab

		//the loops themselves are just a nice organization of the explosion, but it's simply instantiating explosions
	void Kaboom()
	{
		float i = 0.25f;
		while (i <= 1.0f)
		{
			float angle = 0;
			for (angle = 0; angle <= 2*Mathf.PI; angle = angle + Mathf.PI / 4)
			{
				Vector3 pos = blastRadius.transform.position + i * this.transform.localScale.x * blastRadius.GetComponent<CircleCollider2D>().radius * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
				GameObject temp = Instantiate(explosion, pos, Quaternion.identity) as GameObject;
				temp.transform.localScale = temp.transform.localScale / 10f;
			}
			i += 0.25f;
		}
	}

		//if the collided object it a Player, explode.
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (other.tag == "Player")
		{
			Kaboom();
			GetComponentInChildren<ExplosionHelper>().exploded = true;
			//other.GetComponent<Player>().Hurt(damage);
			Destroy(this.gameObject, 0.05f);
		}
	}
}

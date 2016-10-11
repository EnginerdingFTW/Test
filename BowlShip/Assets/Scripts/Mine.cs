using UnityEngine;
using System.Collections;

public class Mine : WeaponFire {

	public GameObject blastRadius;				//gameobject containing collider for the blast radius
	public GameObject explosion;				//prefab
	public float startingPoint = 0.1f;			//Where the explosions start
	public float endingPoint = 0.3f;			//Where the explosions end (radius outward)
	public float scale = 0.2f;					//How big are the explosions
	public float volume = 0.05f;				//How loud each of the explosions are

		//the loops themselves are just a nice organization of the explosion, but it's simply instantiating explosions
	void Kaboom()
	{
		float i = startingPoint;
		while (i <= endingPoint)
		{
			float angle = 0;
			for (angle = 0; angle <= 2*Mathf.PI; angle = angle + Mathf.PI / 4)
			{
				Vector3 pos = blastRadius.transform.position + i * this.transform.localScale.x * blastRadius.GetComponent<CircleCollider2D>().radius * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
				GameObject temp = Instantiate(explosion, pos, Quaternion.identity) as GameObject;
				temp.transform.localScale = temp.transform.localScale / (1 / scale);
				temp.GetComponent<AudioSource> ().volume = volume;
			}
			i += startingPoint;
		}
	}

		//if the collided object it a Player, explode.
	void OnTriggerEnter2D(Collider2D other)	
	{
		if (hasShot && other.gameObject != shootingPlayer && other.tag == "Player" && !other.gameObject.GetComponent<Player>().victor)
		{
			blastRadius.GetComponent<ExplosionHelper> ().shootingPlayer = this.shootingPlayer;
			Kaboom();
			GetComponentInChildren<ExplosionHelper>().exploded = true;
			//other.GetComponent<Player>().Hurt(damage);
			Destroy(this.gameObject, 0.05f);
		}
	}
}

using UnityEngine;
using System.Collections;

public class GargoyleAI : MonoBehaviour {

	private Animator animator;
	private GameObject player;
	private int fireCount = 0;
	private bool inBreak = false;
	private float attackChoice;
	private bool attacking = false;

	public int health = 3;
	public float breakTime = 4;
	public float fireAttackChance = 0.5f;
	public int distanceFromPlayer = 7;
	public bool standby = true;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
		player = GameObject.Find ("Player");
	}

	void Update () {
		if (standby) {
			transform.position = new Vector3 (player.transform.position.x + distanceFromPlayer, transform.position.y, transform.position.z);
		}
		if (!inBreak && !attacking) {
			attackChoice = Random.value;
			if (attackChoice < fireAttackChance) {
				attacking = true;
				IncreaseFireCount();
			}
			else if (attackChoice >= fireAttackChance) {
				attacking = true;
				SwoopBack();
			}
		}
	}

	IEnumerator GivePlayerABreak () {
		inBreak = true;
		attacking = false;
		yield return new WaitForSeconds (breakTime);
		inBreak = false;
	}

	void DestroyThis () {
		Destroy (this.gameObject);
	}

	void TurnHurtOff () {
		animator.SetBool ("Hurt", false);
	}

	void ResetFireCount () {
		animator.SetInteger ("Fire", 0);
		fireCount = 0;
	}

	void IncreaseFireCount () {
		fireCount++;
		animator.SetInteger ("Fire", fireCount);
	}

	void SwoopBack () {
		animator.SetBool ("Swoop", true);
	}

	void FinishedSwoop () {
		distanceFromPlayer = 20;
	}

	public void TailHit () {
		health -= 1;
		animator.SetBool ("Hurt", true);
		if (health < 1) {
			animator.SetBool ("Death", true);
		}
	}
}

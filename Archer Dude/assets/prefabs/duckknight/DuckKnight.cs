using UnityEngine;
using System.Collections;


/// <summary>
/// THIS WILL BE REVISED
/// </summary>
public class DuckKnight : MonoBehaviour {

	public float speed;
	[HideInInspector]
	public Animator animatorKnight;

	private int factor = 1;
	private Vector3 constantVelocity;
	private Animator AnimatorEnemy;
	// Use this for initialization
	void Start () 
	{
		animatorKnight = GetComponent<Animator>();
		constantVelocity = new Vector3(speed, 0, 0);
		animatorKnight.SetBool("Run", true);
	}
	
	void FixedUpdate () 
	{
		if (animatorKnight.GetBool("Run"))
		{
			this.transform.position += factor * constantVelocity * Time.deltaTime;
		}
		
	}
	
	void ChangeBaseAnimation (string str)
	{
		animatorKnight.SetBool("Run", false);
		animatorKnight.SetBool("Idle", false);
		animatorKnight.SetBool("Death", false);
		animatorKnight.SetBool("Fatality", false);
		animatorKnight.SetBool(str, true);
	}
	
	void ChangeAttackAnimation (string str)
	{
		animatorKnight.SetBool("Block", false);
		animatorKnight.SetBool("Stab", false);
		animatorKnight.SetBool("Slash", false);
		animatorKnight.SetBool("JumpAttack", false);
		animatorKnight.SetBool(str, true);
	}
	
	private GameObject FindNearestEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject nearest = null;

		foreach (GameObject enemy in enemies)
		{
			if (nearest == null)
			{
				nearest = enemy;
			}
			float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
			if (distance < Vector3.Distance(this.transform.position, nearest.transform.position))
			{
				nearest = enemy;
			}
		}
		return nearest;
	}
}

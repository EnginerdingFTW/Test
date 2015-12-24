using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnCollisionIdle : MonoBehaviour {

	public bool stop = true;
	private bool go = false;
	private List<Animator> animatorList = new List<Animator>();
	
	// Update is called once per frame
	void Update () 
	{
		if (!go && !stop)
		{
			foreach (Animator animator in animatorList)
			{
				if (animator != null)
				{
					animator.SetBool("run", true);
				}
			}
			animatorList.Clear();
			go = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (stop)
		{
			Animator animator = other.gameObject.GetComponent<Animator>();
			if (animator != null)
			{
				animator.SetBool("run", false);
				animatorList.Add(animator);
			}
		}
	}
}

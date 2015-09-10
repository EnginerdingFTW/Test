using UnityEngine;
using System.Collections;

public class RopeStretch : MonoBehaviour {

	public float correctScale = 7f;
	public float yScale;

	private MeshRenderer mesh; 

	// Use this for initialization
	void Start () 
	{
		mesh = GetComponent<MeshRenderer>();
	}

	void Update ()
	{
		yScale = transform.localScale.y;
		mesh.material.mainTextureScale = new Vector2(1f, yScale * correctScale);
	}
}







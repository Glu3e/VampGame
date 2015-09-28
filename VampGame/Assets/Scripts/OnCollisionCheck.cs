using UnityEngine;
using System.Collections;

public class OnCollisionCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onColliderEnter(Collision collision)
	{
//		foreach (ContactPoint contact in collision.contacts)
//			Debug.Log ("I COLLIDED");
		Destroy (collision.gameObject);
	}
}

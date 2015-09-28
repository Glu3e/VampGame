using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabObjects : MonoBehaviour {

	GameObject grabbedObject;
	float grabbedObjectSize;

	GameObject getMouseHoverObject(float range)
	{
		Vector3 position = gameObject.transform.position;
		RaycastHit raycastHit;
		Vector3 target = position + Camera.main.transform.forward * range; 

		if (Physics.Linecast (position, target, out raycastHit)) 
			return raycastHit.collider.gameObject;
			return null;
	}

	void TryGrabObject(GameObject grabObject) 
	{
		if (grabObject == null || !CanGrab(grabObject))
			return;

		grabbedObject = grabObject;
		grabbedObjectSize = grabbedObject.GetComponent<Renderer>().bounds.size.magnitude;
	}

	bool CanGrab(GameObject candidate)
	{
		return candidate.GetComponent<Rigidbody> () != null;
	}

	void DropObject()
	{
		if (grabbedObject == null)
			return;
		if (grabbedObject.GetComponent<Rigidbody> () != null)
			grabbedObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			grabbedObject = null;

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) 
		{
			if(grabbedObject == null)
				TryGrabObject(getMouseHoverObject(30));
			else
				DropObject();
		}
		if (grabbedObject != null) 
		{
			Vector3 newPosition = gameObject.transform.position+Camera.main.transform.forward* grabbedObjectSize;
			grabbedObject.transform.position = newPosition;
		}
	
	}
}

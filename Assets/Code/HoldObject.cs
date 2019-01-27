using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObject : MonoBehaviour
{
	#region Inspector Fields
	[Tooltip("The currently held object")]
	public Holdable currentObject = null;
	[Tooltip("Holding object")]
	public bool holdingObject = false;
	[Tooltip("Holding point transfrom")]
	public Transform holdingPoint = null;
	#endregion

	// Start is called before the first frame update
	private void Start()
	{
		if (holdingPoint == null) Debug.LogError("Must have a holding point transform", this);
		holdingObject = currentObject != null;
	}

	// Update is called once per frame
	private void Update()
	{
		Touch touch;
		// Make the current object follow this
		if (holdingObject)
		{
			currentObject.transform.position = holdingPoint.position;
			if (Input.touchCount == 0) return;
			touch = Input.GetTouch(0);
		}
		if (Input.touchCount == 0) return;
		touch = Input.GetTouch(0);
		//if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
		{
			Vector3 touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
			Ray touchRay = new Ray(new Vector3(touchWorldPoint.x, touchWorldPoint.y, -10), new Vector3(touchWorldPoint.x, touchWorldPoint.y, -10));
			RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(touchWorldPoint.x, touchWorldPoint.y), new Vector2(touchWorldPoint.x, touchWorldPoint.y));
			foreach (RaycastHit2D hit in hits)
			{
				//Debug.Log(this.name + " touching object name=" + hit.transform.name);
				Holdable holdable = hit.transform.GetComponent<Holdable>();
				if (holdable)
				{
					if (touch.phase == TouchPhase.Began && holdable != currentObject)
					{
						ReleaseAnObject();
					}
					else
					{
						HoldAnObject(holdable);
					}
				}
			}
		}
	}

	// Returns true if new object could be held
	public bool HoldAnObject(Holdable objectToHold)
	{
		//Debug.Log(this.name + " trying to hold " + objectToHold.name);
		if (holdingObject) return false;
		currentObject = objectToHold;
		objectToHold.HeldBy(this);
		holdingObject = true;
		return holdingObject;
	}

	// Returns true if new object could be held
	public bool ReleaseAnObject()
	{
		if (!holdingObject) return false;
		Debug.Log(this.name + " trying to release " + currentObject.name);
		currentObject.ReleasedBy(this);
		holdingObject = false;
		currentObject = null;
		return holdingObject;
	}
}
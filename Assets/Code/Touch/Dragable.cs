using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Dragable : MonoBehaviour
{
	public Color HighlightColor;
	private Color originalColor;
	private CircleCollider2D col;
	private SpriteRenderer spr;
	// Start is called before the first frame update
	private void Start()
	{
		col = GetComponent<CircleCollider2D>();
		spr = GetComponent<SpriteRenderer>();
		originalColor = spr.color;
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnMouseEnter()
	{
		originalColor = spr.color;
		if (HighlightColor.a > 0) spr.color = HighlightColor;
	}
	private void OnMouseExit()
	{
		spr.color = originalColor;
	}
	private void OnMouseDrag()
	{
		Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
		objPosition.z = transform.position.z;

		transform.position = objPosition;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpriteRenderer))]
public class Dragable : MonoBehaviour
{
    public Color HighlightColor;
    Color originalColor;
    CircleCollider2D col;
    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        spr= GetComponent<SpriteRenderer>();
        originalColor = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        originalColor = spr.color;
        if (HighlightColor.a>0) spr.color = HighlightColor;
    }
    void OnMouseExit()
    {
        spr.color = originalColor;
    }
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.z = transform.position.z;

        transform.position = objPosition;
    }
}

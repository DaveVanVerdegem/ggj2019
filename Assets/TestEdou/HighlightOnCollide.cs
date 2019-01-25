using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpriteRenderer))]
public class HighlightOnCollide : MonoBehaviour
{
    public Color HighlightColor;
    Color originalColor;
    Collider col;
    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
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
        spr.color = HighlightColor;
    }
    void OnMouseExit()
    {
        spr.color = originalColor;
    }
}

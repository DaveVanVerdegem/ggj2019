using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Holdable : MonoBehaviour
{
    public HoldableType holdableType;

    private Vector3 originalPosition;

    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HeldBy(HoldObject holdObject)
    {
        Debug.Log(this.name + " being held by " + holdObject.name);
    }
    public void ReleasedBy(HoldObject holdObject)
    {
        Debug.Log(this.name + " released by " + holdObject.name);
        transform.position = originalPosition;
    }
}

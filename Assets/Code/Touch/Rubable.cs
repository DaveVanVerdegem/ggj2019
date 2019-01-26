using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class Rubable : MonoBehaviour
{
    [Tooltip("Optional log output")]
    public Text log = null;
    [Tooltip("Delay to revert to originalrotation")]
    public float revertDelay = 0.5f;
    [Tooltip("Rotation speed for rubbing in degrees")]
    public float speed = 200;
    [Tooltip("Maximum rubable angle in degrees")]
    public float maxAngle = 15;

    CircleCollider2D col;
    Vector3 rotateDirection;
    Quaternion originalRotation;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        originalRotation = Quaternion.Euler(transform.eulerAngles);
        targetRotation = originalRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;
        for (int touchIndex = 0; touchIndex < touches.Length; touchIndex++)
        {
            if (touches[touchIndex].phase == TouchPhase.Ended)
            {
                targetRotation = originalRotation;
            }
            else
            {
                Vector3 touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touches[touchIndex].position.x, touches[touchIndex].position.y, 0));
                //if (log) log.text = "\n col.bounds " + col.bounds+ "\n touches[touchIndex].position " + touches[touchIndex].position + "\n touchWorldPoint " + touchWorldPoint;
                if (col.OverlapPoint(touchWorldPoint))
                {
                    if (touches[touchIndex].phase == TouchPhase.Moved)
                    {
                        float newAngle = Mathf.Clamp(Mathf.Rad2Deg * (touches[touchIndex].deltaPosition.sqrMagnitude * Mathf.Sign(-touches[touchIndex].deltaPosition.x)), -maxAngle, maxAngle);
                        targetRotation = Quaternion.Euler(new Vector3(originalRotation.x, originalRotation.y, newAngle));
                    }
                }
            }
        }
        //if (log) log.text = "\n Rotating towards " + targetRotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime *speed);
    }
}

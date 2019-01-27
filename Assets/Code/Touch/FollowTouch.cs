using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTouch : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        Vector3 touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
        transform.position = new Vector3(touchWorldPoint.x, touchWorldPoint.y, transform.position.z); 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOn : MonoBehaviour
{
    private Collider2D _collider2D = null;

    private InputTrigger _inputTrigger = null;

    private Swipeable _swipeable = null;

    private float _lastTap = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _inputTrigger = GetComponent<InputTrigger>();
    }

    // Update is called once per frame
    private void Update()
    {
        Touch[] touches = Input.touches;
        for (int touchIndex = 0; touchIndex < touches.Length; touchIndex++)
        { 
            if (touches[touchIndex].phase == TouchPhase.Ended)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touches[touchIndex].position);
                Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);

                if (_collider2D.OverlapPoint(touchPosition))
                {
                    _lastTap = Time.time;
                    _inputTrigger.TriggerInput(ActionType.DragAndDrop);
                    return;
                }
            }
        }
    }
}
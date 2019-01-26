using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipeable : MonoBehaviour
{
    public Text log=null;
    //Minimum seconds delay between 2 swipes that swipes are counted
    public float minDelayBetweenSwipes = 0.05f;
    //Maximum seconds delay between 2 swipes that swipes are counted before resetting the swipe count
    public float maxDelayBetweenSwipes = 0.5f;
    //Minimum squared distance between touch end and start to be counted as a swipe
    public int minDistanceForSwipe = 450;
    //Swipe also when not dragging finger (not lifting)
    public bool swipeDrag=false;

    private float distanceSwiped;
    private int swipeCount;
    private float sinceLastSwipe;
    private Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        distanceSwiped = 0;
        swipeCount = 0;
        sinceLastSwipe = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;
        for (int touchIndex = 0; touchIndex < touches.Length; touchIndex++)
        {
            //if (log) log.text = "\n" + GetDebugInfo(touches[touchIndex]);// + "\n" + log.text;
            if ((swipeDrag && touches[touchIndex].phase == TouchPhase.Moved)
               || (!swipeDrag && touches[touchIndex].phase == TouchPhase.Ended))
            {
                distanceSwiped+= touches[touchIndex].deltaPosition.sqrMagnitude;
                if (distanceSwiped > minDistanceForSwipe)
                {
                    distanceSwiped %= minDistanceForSwipe;
                    if ((sinceLastSwipe < Time.time + maxDelayBetweenSwipes)
                        && (sinceLastSwipe+minDelayBetweenSwipes < Time.time ))
                    {
                        touch = touches[touchIndex];
                        if (log) log.text = "\n Swiped! \n" + GetDebugInfo(touch);// + "\n"+ log.text;
                        swipeCount++;
                    }
                    sinceLastSwipe = Time.time;
                }
            }
        }
        if (sinceLastSwipe + maxDelayBetweenSwipes < Time.time )
        {
            distanceSwiped = 0;
               swipeCount = 0;
            if (log) log.text = "\n" + GetDebugInfo(touch);// + "\n"+ log.text;
        }
    }

    public double NumberOfSwipes()
    {
        return swipeCount;
    }

    public Touch LastTouch()
    {
        return touch;
    }

    public string GetDebugInfo(Touch t)
    {
        return "SwipeCount " + swipeCount.ToString() + "\nPosition" + t.position + "\nAngle" + (t.deltaPosition.x/ t.deltaPosition.y) + "\nDeltaPosition" + t.deltaPosition;
    }
}

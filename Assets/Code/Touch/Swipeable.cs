﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Swipeable : MonoBehaviour
{
	#region Inspector Fields
	[Tooltip("Optional log output")]
	public Text log = null;
	[Tooltip("Minimum seconds delay between 2 swipes that swipes are counted")]
	public float minDelayBetweenSwipes = 0.05f;
	[Tooltip("Maximum seconds delay between 2 swipes that swipes are counted before resetting the swipe count")]
	public float maxDelayBetweenSwipes = 0.5f;
	[Tooltip("Minimum squared distance between touch end and start to be counted as a swipe")]
	public int minDistanceForSwipe = 450;
	[Tooltip("Swipe when dragging finger (instead of lifting finger)")]
	public bool swipeDrag = false;
	#endregion

	#region Fields
	/// <summary>
	/// Collider attached to this object.
	/// </summary>
	private Collider2D _collider = null;

	private float distanceSwiped;
	private int swipeCount;
	private float sinceLastSwipe;
	private Touch touch;

	private List<Swipe> _previousSwipes = new List<Swipe>();

	private Swipe _lastSwipe = null;

	private Swipe _averageSwipe = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Start()
	{
		_collider = GetComponent<Collider2D>();
		distanceSwiped = 0;
		swipeCount = 0;
		sinceLastSwipe = Time.time;
	}

	// Update is called once per frame
	private void Update()
	{
		Touch[] touches = Input.touches;
		for (int touchIndex = 0; touchIndex < touches.Length; touchIndex++)
		{
			Vector3 touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touches[touchIndex].position.x, touches[touchIndex].position.y, 0));

			//if (log) log.text = "\n col.bounds " + col.bounds+ "\n touches[touchIndex].position " + touches[touchIndex].position + "\n touchWorldPoint " + touchWorldPoint;

			if (_collider.OverlapPoint(touchWorldPoint))
			{
				if ((swipeDrag && touches[touchIndex].phase == TouchPhase.Moved) || (!swipeDrag && touches[touchIndex].phase == TouchPhase.Ended))
				{
					distanceSwiped += touches[touchIndex].deltaPosition.sqrMagnitude;

					if (distanceSwiped > minDistanceForSwipe)
					{
						if ((sinceLastSwipe < Time.time + maxDelayBetweenSwipes) && (sinceLastSwipe + minDelayBetweenSwipes < Time.time))
						{
							distanceSwiped %= minDistanceForSwipe;
							touch = touches[touchIndex];

							_lastSwipe = new Swipe(distanceSwiped, Time.time - sinceLastSwipe);
							SaveSwipe(_lastSwipe);

							if (log)
								log.text = "\n Swiped! \n" + GetDebugInfo(touch);

							swipeCount++;
						}

						sinceLastSwipe = Time.time;
					}
				}
			}
		}

		if (sinceLastSwipe + maxDelayBetweenSwipes < Time.time)
		{
			distanceSwiped = 0;
			swipeCount = 0;
			if (log) log.text = "\n Reset! \n" + GetDebugInfo(touch);

			// Clear the previous swipes.
			_previousSwipes.Clear();
		}
	}
	#endregion

	#region Debug Methods
	public string GetDebugInfo(Touch t)
	{
		return "SwipeCount " + swipeCount.ToString() + "\nPosition" + t.position + "\nAngle" + (t.deltaPosition.x / t.deltaPosition.y) + "\nDeltaPosition" + t.deltaPosition;
	}
	#endregion

	#region Returns
	public double NumberOfSwipes()
	{
		return swipeCount;
	}

	public Touch LastTouch()
	{
		return touch;
	}
	#endregion

	private void SaveSwipe(Swipe swipe)
	{
		_previousSwipes.Add(swipe);

		// Update average swipe.
		float averageDistance = _previousSwipes.Sum(previousSwipe => previousSwipe.Distance);
		float averageDuration = _previousSwipes.Sum(previousSwipe => previousSwipe.Duration);

		_averageSwipe = new Swipe(averageDistance, averageDuration);
	}
}

[Serializable]
public class Swipe
{
	#region Properties
	/// <summary>
	/// Distance of the swipe.
	/// </summary>
	public float Distance = 0f;

	/// <summary>
	/// Duration of the swipe.
	/// </summary>
	public float Duration = 0f;

	/// <summary>
	/// Speed of the swipe.
	/// </summary>
	public float Speed = 0f;
	#endregion

	#region Constructors
	public Swipe(float distance, float duration)
	{
		Distance = distance;
		Duration = duration;

		Speed = distance / duration;
	}
	#endregion
}
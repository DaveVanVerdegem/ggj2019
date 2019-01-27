using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class Swipeable : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("Optional log output")]
	public Text log = null;
	[Tooltip("Maximum seconds delay between 2 swipes that swipes are counted before resetting the swipe count")]
	public float maxDelayBetweenSwipes = 0.5f;
	[Tooltip("Minimum distance between touch end and start to be counted as a swipe")]
	public int minDistanceForSwipe = 10;
	[Tooltip("Swipe when dragging finger (instead of lifting finger)")]
	public bool swipeDrag = false;
    [Tooltip("Amount of swipes required for triggering")]
    public int swipesRequiredForTrigger = 8;
	#endregion

	#region Fields
	/// <summary>
	/// Collider attached to this object.
	/// </summary>
	private PolygonCollider2D _collider = null;

	private HotSpotHelper _hotSpotHelper = null;

	private InputTrigger _inputTrigger = null;

	private float distanceSwiped;
	private bool leftToRightSwiped;
	private int swipeCount;
	private float sinceLastSwipe;
	private Touch touch;

	private Swipe _lastSwipe = null;

	private Swipe _averageSwipe = null;

	private List<Swipe> _previousSwipes = new List<Swipe>();

	private bool _theSwipeIsRight = false;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Start()
	{
		_collider = GetComponent<PolygonCollider2D>();
		_hotSpotHelper = GetComponent<HotSpotHelper>();
		_inputTrigger = GetComponent<InputTrigger>();

		distanceSwiped = 0;
		leftToRightSwiped = false;
		swipeCount = 0;
		sinceLastSwipe = Time.time;
		_lastSwipe = new Swipe(0, 0, false);
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
					distanceSwiped += touches[touchIndex].deltaPosition.magnitude;

					if (distanceSwiped > minDistanceForSwipe)
					{
						leftToRightSwiped = touches[touchIndex].deltaPosition.x > 0;
						if (leftToRightSwiped != _lastSwipe.LeftToRight)
						{
							_lastSwipe = new Swipe(distanceSwiped, Time.time - sinceLastSwipe, leftToRightSwiped);
							SaveSwipe(_lastSwipe);

							_theSwipeIsRight = IsThisSwipeRight();

							string animationToPlay = _theSwipeIsRight ? "Getting_Scratched" : "Angry_Idle";
							Monster.Instance.AnimationHelper.UpdateAnimation(animationToPlay, 1f);

							touch = touches[touchIndex];
							distanceSwiped = 0;

							if (log)
								log.text = "\n Swiped! \n" + GetDebugInfo(touch);

							swipeCount++;

							if (swipeCount == swipesRequiredForTrigger && _theSwipeIsRight)
								_inputTrigger.TriggerInput(ActionType.Swipe);
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
		float averageDistance = _previousSwipes.Sum(previousSwipe => previousSwipe.Distance) / _previousSwipes.Count;
		float averageDuration = _previousSwipes.Sum(previousSwipe => previousSwipe.Duration) / _previousSwipes.Count;

		_averageSwipe = new Swipe(averageDistance, averageDuration, false);
	}

	private bool IsThisSwipeRight()
	{
		ActionProperties actionProperties = Monster.Instance.ReturnCurrentActionProperties();

		if (actionProperties == null)
			return false;

		return actionProperties.ActionType == ActionType.Swipe && actionProperties.HotSpotLocation == _hotSpotHelper.HotSpotLocation;
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

	/// <summary>
	/// Horizontal direction of the swipe.
	/// </summary>
	public bool LeftToRight = false;
	#endregion

	#region Constructors
	public Swipe(float distance, float duration, bool leftToRight)
	{
		Distance = distance;
		Duration = duration;
		LeftToRight = leftToRight;
		Speed = distance / duration;
	}
	#endregion
}
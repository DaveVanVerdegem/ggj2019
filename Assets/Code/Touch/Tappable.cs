﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tappable : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// Threshold to prevent multiple taps.
	/// </summary>
	[Tooltip("Threshold to prevent multiple taps.")]
	[SerializeField]
	private float _tapThreshold = 1f;
	#endregion

	private Collider2D _collider2D = null;

	private InputTrigger _inputTrigger = null;

	private float _lastTap = 0f;

	//private Timer _thresholdTimer = null;

	// Start is called before the first frame update
	private void Start()
	{
		_collider2D = GetComponent<Collider2D>();
		_inputTrigger = GetComponent<InputTrigger>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.touchCount == 1)
		{
			if (Time.time < _lastTap + _tapThreshold)
				return;

			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);

			if (_collider2D == Physics2D.OverlapPoint(touchPosition))
			{
				Debug.Log(Time.frameCount + " Tippy tap.");

				_lastTap = Time.time;
				_inputTrigger.TriggerInput(ActionType.Tap);
			}
		}
	}
}
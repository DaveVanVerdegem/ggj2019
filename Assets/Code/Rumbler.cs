using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumbler : MonoBehaviour
{
	#region Fields
	/// <summary>
	/// The rumbler is active.
	/// </summary>
	private bool _rumbling = false;

	/// <summary>
	/// Delay between rumbles.
	/// </summary>
	private float _rumbleDelay = -1f;

	/// <summary>
	/// Timer for the delay.
	/// </summary>
	private Timer _timer = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if (!_rumbling)
			return;

		if (_timer.CountDown())
		{
			Handheld.Vibrate();
			_timer.Reset();
		}
	}
	#endregion

	#region Methods
	public void StartRumbling(float delay)
	{
		_rumbleDelay = delay;

		_timer = new Timer(_rumbleDelay);

		_rumbling = true;
	}

	/// <summary>
	/// Stop the rumble.
	/// </summary>
	public void StopTheRumble()
	{
		_rumbling = false;
	}
	#endregion
}
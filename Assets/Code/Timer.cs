using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
	#region Fields
	/// <summary>
	/// Countdown timer in seconds.
	/// </summary>
	private float _countDown = 0;

	/// <summary>
	/// Starting count in seconds to be used when resetting this timer.
	/// </summary>
	private float _startingCount = 0;
	#endregion

	#region Constructors
	/// <summary>
	/// Creates a new timer.
	/// </summary>
	/// <param name="countdown">Amount to count down to in seconds.</param>
	public Timer(float countdown)
	{
		// Cache countdown value.
		_startingCount = countdown;

		// Set the timer.
		Reset();
	}
	#endregion

	#region Life Cycle
	/// <summary>
	/// Resets the timer.
	/// </summary>
	public void Reset()
	{
		// Set timer.
		_countDown = _startingCount;
	}

	/// <summary>
	/// Count down the timer.
	/// </summary>
	/// <returns>Returns true if the timer has completed counting down.</returns>
	public bool CountDown()
	{
		// Timer has completed count down.
		if (_countDown <= 0)
			return true;

		// Count down.
		_countDown -= Time.deltaTime;

		return false;
	}
	#endregion
}
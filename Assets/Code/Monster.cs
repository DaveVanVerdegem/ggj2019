using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// Action queue to currently use for this monster.
	/// </summary>
	[Tooltip("Action queue to currently use for this monster.")]
	[SerializeField]
	private ActionQueueProperties _actionQueue = null;
	#endregion

	#region Fields
	/// <summary>
	/// Attached rumbler.
	/// </summary>
	private Rumbler _rumbler = null;

	/// <summary>
	/// Timer for this monster.
	/// </summary>
	private Timer _timer = null;

	/// <summary>
	/// Timer to escalate rumbles.
	/// </summary>
	private Timer _rumbleTimer = null;

	private int _rumbleEscalation = 0;

	/// <summary>
	/// Index of the current action the player needs to perform.
	/// </summary>
	private int _currentActionIndex = 0;

	private float _timeBetweenRumbles = 0f;
	#endregion

	#region Life Cycle
	private void Awake()
	{
		_rumbler = GetComponent<Rumbler>();
	}

	// Start is called before the first frame update
	private void Start()
	{
		// Set rumble thresholds.
		_timeBetweenRumbles = _actionQueue.FailTimer * .25f;

		// Start timers.
		_timer = new Timer(_actionQueue.FailTimer);
		_rumbleTimer = new Timer(_timeBetweenRumbles);
	}

	// Update is called once per frame
	private void Update()
	{
		// Countdown timers.
		if (_timer != null && _timer.CountDown())
		{
			Debug.Log("Timer ran out!", this);
			_timer.Reset();
			Escalate();
		}

		if (_rumbleTimer != null && _rumbleTimer.CountDown())
		{
			_rumbleEscalation++;

			float rumbleDelay = 0f;

			switch (_rumbleEscalation)
			{
				default:
				case 0:
					rumbleDelay = 999f;
					break;

				case 1:
					rumbleDelay = 5f;
					break;

				case 2:
					rumbleDelay = 2f;
					break;

				case 3:
					rumbleDelay = 1f;
					break;
			}

			_rumbler.StartRumbling(rumbleDelay);
			_rumbleTimer = new Timer(_timeBetweenRumbles);
		}
	}
	#endregion

	#region Methods
	/// <summary>
	/// Registers an action taken on the monster.
	/// </summary>
	/// <param name="actionType">Type of input used.</param>
	/// <param name="hotSpot">Hot spot where the input was used on.</param>
	public void RegisterAction(ActionType actionType, HotSpotLocation hotSpot)
	{
		Debug.Log(string.Format("Registering input of {0} at {1}.", actionType, hotSpot), this);

		ActionProperties actionProperties = ReturnCurrentActionProperties();

		if (actionProperties == null)
			return;

		if (actionProperties.ActionType == actionType && actionProperties.HotSpotLocation == hotSpot)
		{
			// Succes!
			_timer.Reset();
			Iterate();
		}
		else
		{
			Escalate();
		}
	}

	/// <summary>
	/// Escalate the monster to a new danger level.
	/// </summary>
	public void Escalate()
	{
		Debug.Log("Monster escalates!", this);
	}

	public void Iterate()
	{
		_currentActionIndex++;

		if (_currentActionIndex >= _actionQueue.Actions.Count)
		{
			// Finished with the queue!
			Debug.Log("Finished with the queue!", this);

			_timer = null;
			_currentActionIndex = 0;
		}
	}
	#endregion

	#region Returns
	public ActionProperties ReturnCurrentActionProperties()
	{
		if (_currentActionIndex >= _actionQueue.Actions.Count)
			return null;

		return _actionQueue.Actions[_currentActionIndex];
	}
	#endregion
}
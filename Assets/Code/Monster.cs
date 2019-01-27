using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum EscalationLevel
{
	None = 0,
	Low = 1,
	Medium = 2,
	High = 3
}

public enum GameState
{
	Menu = 0,
	Playing = 1,
	Win = 2,
	Lose = 3
}
#endregion

public class Monster : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// HoldObject, keep track of currently held object
	/// </summary>
	[Tooltip("HoldObject, keep track of currently held object")]
	[SerializeField]
	private HoldObject _holdObject = null;

	/// <summary>
	/// Action queue to currently use for this monster.
	/// </summary>
	[Tooltip("Action queue to currently use for this monster.")]
	[SerializeField]
	private ActionQueueProperties _actionQueue = null;

	/// <summary>
	/// Angry audio clips
	/// </summary>
	[Tooltip("Angry audio clips")]
	[SerializeField]
	private RandomAudioClip _angrySound = null;

	/// <summary>
	/// Happy audio clips
	/// </summary>
	[Tooltip("Happy audio clips")]
	[SerializeField]
	private RandomAudioClip _happySound = null;

	/// <summary>
	/// Hungry monster audio clips
	/// </summary>
	[Tooltip("Hungry monster audio clips")]
	[SerializeField]
	private RandomAudioClip _hungrySound = null;

	/// <summary>
	/// Satisfied monster audio clips
	/// </summary>
	[Tooltip("Satisfied monster audio clips")]
	[SerializeField]
	private RandomAudioClip _satisfiedSound = null;

	/// <summary>
	/// Eating audio clips
	/// </summary>
	[Tooltip("Eating audio clips")]
	[SerializeField]
	private RandomAudioClip _eatingSound = null;

	/// <summary>
	/// toothbrush audio clips
	/// </summary>
	[Tooltip("toothbrush audio clips")]
	[SerializeField]
	private RandomAudioClip _toothbrushSound = null;

	/// <summary>
	/// Combing audio clips
	/// </summary>
	[Tooltip("Combing audio clips")]
	[SerializeField]
	private RandomAudioClip _combingSound = null;

	/// <summary>
	/// Pincer audio clips
	/// </summary>
	[Tooltip("Pincer audio clips")]
	[SerializeField]
	private RandomAudioClip _pincerSound = null;

	/// <summary>
	/// Nail filing audio clips
	/// </summary>
	[Tooltip("Nail filing audio clips")]
	[SerializeField]
	private RandomAudioClip _nailFilingSound = null;

	[Header("Hot Spots")]
	/// <summary>
	/// Hot spot for the teeth of the monster.
	/// </summary>
	[Tooltip("Hot spot for the teeth of the monster.")]
	[SerializeField]
	private HotSpot _hotSpotTeeth = null;

	/// <summary>
	/// Hot spot for the back of the monster.
	/// </summary>
	[Tooltip("Hot spot for the back of the monster.")]
	[SerializeField]
	private HotSpot _hotSpotBack = null;

	/// <summary>
	/// Hot spot for the tummy of the monster.
	/// </summary>
	[Tooltip("Hot spot for the tummy of the monster.")]
	[SerializeField]
	private HotSpot _hotSpotTummy = null;

	/// <summary>
	/// Hot spot for the tail of the monster.
	/// </summary>
	[Tooltip("Hot spot for the tail of the monster.")]
	[SerializeField]
	private HotSpot _hotSpotTail = null;

	[Header("Animations")]
	[SerializeField]
	private string _animationEscalationNone = "";

	[SerializeField]
	private string _animationEscalationLow = "";

	[SerializeField]
	private string _animationEscalationMedium = "";

	[SerializeField]
	private string _animationEscalationHigh = "";
	#endregion

	#region Properties
	/// <summary>
	/// Static reference to the monster prefab.
	/// </summary>
	public static Monster Instance = null;

	public GameState GameState
	{
		get { return _gameState; }
	}

	[HideInInspector]
	public AnimationHelper AnimationHelper = null;

	[Header("Debug")]
	/// <summary>
	/// Escalation level where the monster is currently at.
	/// </summary>
	[HideInInspector]
	public EscalationLevel MonsterEscalationLevel = 0;
	#endregion

	#region Fields
	private GameState _gameState = GameState.Playing;

	/// <summary>
	/// Timer for this monster.
	/// </summary>
	private Timer _actionTimer = null;

	/// <summary>
	/// Index of the current action the player needs to perform.
	/// </summary>
	private int _currentActionIndex = 0;

	private HotSpot _currentlyActiveHotSpot = null;

	private GameObject _hotSpotIndicator = null;

	/// <summary>
	/// The skeleton animation of the monster.
	/// </summary>
	[Obsolete]
	private SkeletonAnimation _skeletonAnimation;

	#region Rumbler
	/// <summary>
	/// Attached rumbler.
	/// </summary>
	private Rumbler _rumbler = null;

	/// <summary>
	/// Timer to escalate rumbles.
	/// </summary>
	private Timer _rumbleTimer = null;

	/// <summary>
	/// The escalation level of the rumbling. Contained to 4 levels, where every escalation increases the amount of rumbling.
	/// </summary>
	private EscalationLevel _rumbleEscalation = 0;

	/// <summary>
	/// Time until the rumble get escalated.
	/// </summary>
	private float _timeBetweenRumbles = 0f;
	#endregion
	#endregion

	#region Life Cycle
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		_rumbler = GetComponent<Rumbler>();
		AnimationHelper = GetComponent<AnimationHelper>();

		_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
	}

	// Start is called before the first frame update
	private void Start()
	{
		if (!_holdObject) Debug.LogError("Must have a HoldObject", this);

		// Set rumble thresholds.
		_timeBetweenRumbles = _actionQueue.FailTimer * .25f;

		// Start timers.
		_actionTimer = new Timer(_actionQueue.FailTimer);
		_rumbleTimer = new Timer(_timeBetweenRumbles);

		// Initialize.
		UpdateActiveHotSpot();

		DisplayActionToTake();
	}

	// Update is called once per frame
	private void Update()
	{
		ProgressTimers();
	}
	#endregion

	#region Game Loop
	public void UpdateState(GameState gameState)
	{
		if (GameState == GameState.Win || GameState == GameState.Lose)
		{
			return;
		}

		_gameState = gameState;

		switch (GameState)
		{
			default:
				break;

			case GameState.Win:
				// Display win screen.
				Debug.Log("<color=green><b>Won the game!</b></color>");
				GameCanvas.Instance.ShowWin();

				break;

			case GameState.Lose:
				// Display lose screen.
				Debug.Log("<color=red><b>Game over!</b></color>");
				GameCanvas.Instance.ShowLose();

				break;
		}
	}

	public void WinGame()
	{
		UpdateState(GameState.Win);
	}

	public void LoseGame()
	{
		UpdateState(GameState.Lose);
	}

	/// <summary>
	/// Escalate the monster to a new danger level.
	/// </summary>
	public void Escalate()
	{
		_angrySound.PlayRandomAudioClip();

		UpdateEscalation(true);

		Debug.Log(string.Format("Monster escalates! It is now at level {0}.", MonsterEscalationLevel), this);

		if ((int)MonsterEscalationLevel >= Enum.GetNames(typeof(EscalationLevel)).Length)
		{
			LoseGame();
		}
	}

	public void Iterate()
	{
		_currentActionIndex++;
		UpdateEscalation(false);

		UpdateActiveHotSpot();

		// Reset the action and rumble timer.
		_actionTimer.Reset();
		_rumbleTimer.Reset();

		_rumbleEscalation = (EscalationLevel)Mathf.Clamp((int)_rumbleEscalation--, 0, 4);
		UpdateAnimation();

		if (_currentActionIndex >= _actionQueue.Actions.Count)
		{
			// Finished with the queue!
			Debug.Log("Finished with the queue!", this);

			// Clear the action timer.
			_actionTimer = null;
			_currentActionIndex = 0;

			// Stop the rumble timer.
			_rumbleTimer = null;
			_rumbler.StopTheRumble();

			WinGame();
			return;
		}

		DisplayActionToTake();
	}
	#endregion

	#region Input Methods
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

		//Debug.Log(actionProperties.ActionType + "==" + actionType + " && " + actionProperties.HotSpotLocation + "==" + hotSpot + " && " + actionProperties.holdableType + "==" + _holdObject.currentObject.holdableType);

		if (actionProperties.ActionType == actionType && actionProperties.HotSpotLocation == hotSpot && actionProperties.holdableType == _holdObject.currentObject.holdableType)
		{
			if (actionProperties.ActionType == ActionType.DragAndDrop)
			{
				_holdObject.DropObject();
			}

			// Succes!
			PlayAudio(actionProperties.AudioToPlayOnSucces);

			AnimationHelper.UpdateAnimation(actionProperties.AnimationTypeOnSucces, 3f);

			Iterate();
		}
		else if (actionProperties.HotSpotLocation == hotSpot)
		{
			return;
		}
		else
		{
			AnimationHelper.UpdateAnimation(AnimationType.Angry, 3f);
			Escalate();
		}
	}
	#endregion

	#region Visual Methods
	/// <summary>
	/// Activate any visuals for the hot spots here.
	/// </summary>
	public void IndicateHotSpot(HotSpot hotSpot)
	{
		if (hotSpot == null)
			return;

		GameObject indicator = ReturnCurrentActionProperties().Indicator;
		if (indicator == null)
			return;

		_hotSpotIndicator = Instantiate(indicator);

		Debug.Log(string.Format("Hot spot {0} is active now.", hotSpot.HotSpotHelper.HotSpotLocation), this);

		_hotSpotIndicator.transform.position = hotSpot.HotSpotHelper.transform.position;
		_hotSpotIndicator.SetActive(true);
	}

	public void HideHotSpotIndication(HotSpot hotSpot)
	{
		//_hotSpotIndicator.SetActive(false);
		Destroy(_hotSpotIndicator);
	}
	#endregion

	#region Methods
	/// <summary>
	/// Evaluates and progresses the timers.
	/// </summary>
	private void ProgressTimers()
	{
		// Countdown timers.
		if (_actionTimer != null && _actionTimer.CountDown())
		{
			Debug.Log("Timer ran out!", this);
			_actionTimer.Reset();
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

				case (EscalationLevel)1:
					rumbleDelay = 5f;
					break;

				case (EscalationLevel)2:
					rumbleDelay = 2f;
					break;

				case (EscalationLevel)3:
					rumbleDelay = 1f;
					break;
			}

			_rumbler.StartRumbling(rumbleDelay);
			_rumbleTimer = new Timer(_timeBetweenRumbles);

			UpdateAnimation();
		}
	}

	public void UpdateActiveHotSpot()
	{
		ActionProperties actionProperties = ReturnCurrentActionProperties();
		HotSpot newHotSpot = (actionProperties == null) ? null : ReturnMatchingHotSpot(ReturnCurrentActionProperties().HotSpotLocation);

		if (_currentlyActiveHotSpot == newHotSpot)
			return;

		if (_currentlyActiveHotSpot != null)
			HideHotSpotIndication(_currentlyActiveHotSpot);

		// Set new hot spot.
		_currentlyActiveHotSpot = newHotSpot;
		IndicateHotSpot(_currentlyActiveHotSpot);
		AnimationHelper.UpdateDefaultAnimation(actionProperties.AnimationTypeToIndicate);
	}

	public void UpdateAnimation()
	{
		string animation = "";

		switch (_rumbleEscalation)
		{
			default:
			case EscalationLevel.None:
				animation = _animationEscalationNone;
				break;

			case EscalationLevel.Low:
				animation = _animationEscalationLow;
				break;

			case EscalationLevel.Medium:
				animation = _animationEscalationMedium;
				break;

			case EscalationLevel.High:
				animation = _animationEscalationHigh;
				break;
		}

		_skeletonAnimation.AnimationName = animation;
	}

	public void UpdateEscalation(bool increment)
	{
		if (increment)
			MonsterEscalationLevel++;
		else
			MonsterEscalationLevel--;

		MonsterEscalationLevel = (EscalationLevel)Mathf.Clamp((int)MonsterEscalationLevel, 0, 4);

		//UpdateAnimation();
	}
	#endregion

	#region Debug Methods
	public void DisplayActionToTake()
	{
		ActionProperties actionProperties = ReturnCurrentActionProperties();
		Debug.Log(string.Format("You need to {0} on the monsters {1} with {2}.", actionProperties.ActionType, actionProperties.HotSpotLocation, actionProperties.holdableType), this);
	}
	#endregion

	#region Returns
	public ActionProperties ReturnCurrentActionProperties()
	{
		if (_currentActionIndex >= _actionQueue.Actions.Count)
			return null;

		return _actionQueue.Actions[_currentActionIndex];
	}

	public HotSpot ReturnMatchingHotSpot(HotSpotLocation hotSpotLocation)
	{
		switch (hotSpotLocation)
		{
			default:
				Debug.LogWarning(string.Format("A hot spot for {0} hasn't been set yet.", hotSpotLocation));
				return null;

			case HotSpotLocation.Teeth:
				return _hotSpotTeeth;

			case HotSpotLocation.Back:
				return _hotSpotBack;

			case HotSpotLocation.Tummy:
				return _hotSpotTummy;

			case HotSpotLocation.Tail:
				return _hotSpotTail;
		}
	}

	public RandomAudioClip ReturnMatchingAudio(AudioType audioType)
	{
		switch (audioType)
		{
			default:
				return null;

			case AudioType.Angry:
				return _angrySound;

			case AudioType.Happy:
				return _happySound;

			case AudioType.Hungry:
				return _hungrySound;

			case AudioType.Satisfied:
				return _satisfiedSound;

			case AudioType.Eating:
				return _eatingSound;

			case AudioType.Toothbrush:
				return _toothbrushSound;

			case AudioType.Combing:
				return _combingSound;

			case AudioType.Pincer:
				return _pincerSound;

			case AudioType.NailFiling:
				return _nailFilingSound;
		}
	}
	#endregion

	#region Audio Methods
	public void PlayAudio(AudioType audioType)
	{
		RandomAudioClip audioPlayer = ReturnMatchingAudio(audioType);

		if (audioPlayer == null)
			return;

		audioPlayer.PlayRandomAudioClip();
	}
	#endregion

	#region Setters
	public void SetCurrentActionProperties(ActionQueueProperties actionQueueProperties)
	{
		_actionQueue = actionQueueProperties;
	}
	#endregion
}
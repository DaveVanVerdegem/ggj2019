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
#endregion

public class Monster : MonoBehaviour
{
    #region Inspector Fields
    /// <summary>
    /// Prefab to indicate the current hotspot.
    /// </summary>
    [Tooltip("Prefab to indicate the current hotspot.")]
    [SerializeField]
    private GameObject _hotSpotIndicatorPrefab = null;

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
    [Header("Debug")]
    /// <summary>
    /// Escalation level where the monster is currently at.
    /// </summary>
	//[HideInInspector]
    public EscalationLevel MonsterEscalationLevel = 0;
    #endregion

    #region Fields

    /// <summary>
    /// Action queue to currently use for this monster.
    /// </summary>
    //[Tooltip("Action queue to currently use for this monster.")]
    //[SerializeField]
    private ActionQueueProperties _actionQueue = null;

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
        _rumbler = GetComponent<Rumbler>();
		_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    // Start is called before the first frame update
    private void Start()
    {
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
    public void WinGame()
    {
        Debug.Log("<color=green><b>Won the game!</b></color>");
    }

    public void LoseGame()
    {
        Debug.Log("<color=red><b>Game over!</b></color>");
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

        if (_currentActionIndex >= _actionQueue.Actions.Count)
        {
            // Finished with the queue!
            Debug.Log("Finished with the queue!", this);

            // Clear the action timer.
            _actionTimer = null;
            _currentActionIndex = 0;

            // Stop the rumble timer.
            _rumbleTimer = null;
            _rumbleEscalation = 0;
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

        if (actionProperties.ActionType == actionType && actionProperties.HotSpotLocation == hotSpot)
        {
            // Succes!
            _actionTimer.Reset();
            Iterate();
        }
        else
        {
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

        if (_hotSpotIndicator == null)
            _hotSpotIndicator = Instantiate(_hotSpotIndicatorPrefab);

        Debug.Log(string.Format("Hot spot {0} is active now.", hotSpot.HotSpotHelper.HotSpotLocation), this);

        _hotSpotIndicator.transform.position = hotSpot.HotSpotHelper.transform.position;
        _hotSpotIndicator.SetActive(true);
    }

    public void HideHotSpotIndication(HotSpot hotSpot)
    {
        _hotSpotIndicator.SetActive(false);
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
	}

	public void UpdateAnimation()
	{
		string animation = "";

		switch (MonsterEscalationLevel)
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

		UpdateAnimation();
    }
    #endregion

    #region Debug Methods
    public void DisplayActionToTake()
    {
        ActionProperties actionProperties = ReturnCurrentActionProperties();
        Debug.Log(string.Format("You need to {0} on the monsters {1}.", actionProperties.ActionType, actionProperties.HotSpotLocation), this);
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
    #endregion

    #region Setters
    public void SetCurrentActionProperties(ActionQueueProperties actionQueueProperties)
    {
        _actionQueue = actionQueueProperties;
    }
    #endregion
}
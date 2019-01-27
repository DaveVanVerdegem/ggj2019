using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum AnimationType
{
	None = 0,
	Angry = 1,
	Scratching = 2,
	Eating = 3,
	Hungry = 4,
	Idle = 5
}
#endregion

public class AnimationHelper : MonoBehaviour
{
	#region Fields
	/// <summary>
	/// The skeleton animation of the monster.
	/// </summary>
	private SkeletonAnimation _skeletonAnimation;

	private string _defaultAnimation = "Awake_Idle";

	private Timer _timer = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Awake()
	{
		_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (_timer == null)
			return;

		if (_timer.CountDown())
		{
			_skeletonAnimation.AnimationName = _defaultAnimation;
			_timer = null;
		}
	}
	#endregion

	#region Methods
	public void UpdateAnimation(string animation, float duration)
	{
		_skeletonAnimation.AnimationName = animation;

		_timer = new Timer(duration);
	}

	public void UpdateAnimation(AnimationType animationType, float duration)
	{
		_skeletonAnimation.AnimationName = ReturnAnimation(animationType);

		_timer = new Timer(duration);
	}

	public void UpdateDefaultAnimation(string animation)
	{
		_defaultAnimation = animation;

		if (_timer == null)
			_skeletonAnimation.AnimationName = _defaultAnimation;
	}

	public void UpdateDefaultAnimation(AnimationType animationType)
	{
		_defaultAnimation = ReturnAnimation(animationType);

		if (_timer == null)
			_skeletonAnimation.AnimationName = _defaultAnimation;
	}
	#endregion

	public string ReturnAnimation(AnimationType animationType)
	{
		switch (animationType)
		{
			default:
			case AnimationType.None:
				return "";

			case AnimationType.Angry:
				return "Angry_Idle";

			case AnimationType.Eating:
				return "Eating";

			case AnimationType.Scratching:
				return "Getting_Scratched";

			case AnimationType.Hungry:
				return "Awake_Hungry";

			case AnimationType.Idle:
				return "Sleep_Idle";
		}
	}
}
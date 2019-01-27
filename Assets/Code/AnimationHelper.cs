using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public void UpdateDefaultAnimation(string animation)
	{
		_defaultAnimation = animation;

		if (_timer == null)
			_skeletonAnimation.AnimationName = _defaultAnimation;
	}
	#endregion
}
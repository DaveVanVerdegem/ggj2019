using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ2019/Action")]
public class ActionProperties : ScriptableObject
{
	#region Inspector Fields
	/// <summary>
	/// Type of action to use.
	/// </summary>
	[Tooltip("Type of action to use.")]
	public ActionType ActionType = ActionType.None;

	/// <summary>
	/// Hot spot location to use for this action.
	/// </summary>
	[Tooltip("Hot spot location to use for this action.")]
	public HotSpotLocation HotSpotLocation = HotSpotLocation.None;

	//[Header("Swipe Properties")]
	//public int NumberOfSwipes = 0;

	//public float SwipeLength = 15f;

	//public float MinimumSwipeSpeed = 0f;

	//public float MaximumSwipeSpeed = 0f;
	#endregion
}
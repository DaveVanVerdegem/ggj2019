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

	/// <summary>
	/// Object used to indicate what interaction is needed here.
	/// </summary>
	public GameObject Indicator = null;

	/// <summary>
	/// Audio that is played when the action was succesful.
	/// </summary>
	[Tooltip("Audio that is played when the action was succesful.")]
	public AudioType AudioToPlayOnSucces = AudioType.None;

	[Header("Animations")]
	/// <summary>
	/// Animation to play to indicate the action.
	/// </summary>
	[Tooltip("Animation to play to indicate the action")]
	public AnimationType AnimationTypeToIndicate = AnimationType.None;

	/// <summary>
	/// Animation to play on succes.
	/// </summary>
	[Tooltip("Animation to play on succes.")]
	public AnimationType AnimationTypeOnSucces = AnimationType.None;
	#endregion
}
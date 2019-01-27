using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HotSpot
{
	#region Inspector Fields
	/// <summary>
	/// Helper to manage this hot spot.
	/// </summary>
	[Tooltip("Helper to manage this hot spot.")]
	public HotSpotHelper HotSpotHelper = null;

	///// <summary>
	///// Name of the animation to play.
	///// </summary>
	//[Tooltip("Name of the animation to play.")]
	//public string Animation = "";
	#endregion
}
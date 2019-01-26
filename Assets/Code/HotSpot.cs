using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HotSpot
{
	#region Inspector Fields

	/// <summary>
	/// Hot spot location to use for this action.
	/// </summary>
	[Tooltip("Hot spot location to use for this action.")]
	public HotSpotLocation HotSpotLocation = HotSpotLocation.None;
	#endregion
}
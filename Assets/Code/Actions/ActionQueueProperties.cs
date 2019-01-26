using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ2019/Action Queue")]
public class ActionQueueProperties : ScriptableObject
{
	#region Inspector Fields
	/// <summary>
	/// Time to start this queue.
	/// </summary>
	[Tooltip("Time to start this queue.")]
	public float FailTimer = 30f;

	/// <summary>
	/// Actions to go through.
	/// </summary>
	[Tooltip("Actions to go through.")]
	public List<ActionProperties> Actions = new List<ActionProperties>();
	#endregion
}
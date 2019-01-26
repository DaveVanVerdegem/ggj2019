using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HotSpot
{
	None = 0,
	Teeth = 1,
	Back = 2,
	Tummy = 3,
	Tail = 4
}

public enum ActionType
{
	None = 0,
	Tap = 1,
	DragAndDrop = 2,
	Swipe = 3
}

public class InputTrigger : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// Hot spot that this trigger activates.
	/// </summary>
	[Tooltip("Hot spot that this trigger activates.")]
	public HotSpot HotSpot = HotSpot.None;
	#endregion

	#region Fields
	/// <summary>
	/// Attached monster component.
	/// </summary>
	private Monster _monster = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Awake()
	{
		_monster = GetComponentInParent<Monster>();

		if (_monster == null)
			Debug.LogWarning("No monster component found.", this);
	}

	// Update is called once per frame
	private void Update()
	{
	}
	#endregion

	#region Methods
	public void TriggerInput(ActionType inputType)
	{
		_monster.RegisterAction(inputType, HotSpot);
	}
	#endregion

	#region Debug Methods
	/// <summary>
	/// Triggers the tapping input.
	/// </summary>
	[ContextMenu("Trigger Tap")]
	private void TriggerTap()
	{
		TriggerInput(ActionType.Tap);
	}

	/// <summary>
	/// Triggers the tapping input.
	/// </summary>
	[ContextMenu("Trigger Swipe")]
	private void TriggerSwipe()
	{
		TriggerInput(ActionType.Swipe);
	}

	/// <summary>
	/// Triggers the tapping input.
	/// </summary>
	[ContextMenu("Trigger Drag and Drop")]
	private void TriggerDragAndDrop()
	{
		TriggerInput(ActionType.DragAndDrop);
	}
	#endregion
}
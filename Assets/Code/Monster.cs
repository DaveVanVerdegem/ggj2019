using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// Action queue to currently use for this monster.
	/// </summary>
	[Tooltip("Action queue to currently use for this monster.")]
	[SerializeField]
	private ActionQueueProperties _actionQueue = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}
	#endregion

	#region Methods
	/// <summary>
	/// Registers an action taken on the monster.
	/// </summary>
	/// <param name="inputType">Type of input used.</param>
	/// <param name="hotSpot">Hot spot where the input was used on.</param>
	public void RegisterAction(ActionType inputType, HotSpot hotSpot)
	{
		Debug.Log(string.Format("Registering input of {0} at {1}.", inputType, hotSpot), this);
	}
	#endregion
}
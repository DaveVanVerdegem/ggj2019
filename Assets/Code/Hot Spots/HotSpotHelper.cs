using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotHelper : MonoBehaviour
{
	#region Inspector Fields
	/// <summary>
	/// Hot spot location that this helper is assigned to.
	/// </summary>
	[Tooltip("Hot spot location that this helper is assigned to.")]
	public HotSpotLocation HotSpotLocation = HotSpotLocation.None;
	#endregion

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimer : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private MouseDirectionHandler _directionHandler;
	#endregion private-field

	#region MonoBehaviour-method
	private void Update()
	{
		UpdateDierction();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void UpdateDierction()
	{
		var dir = _directionHandler.GetDirection();
		var rotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, rotation, 0);
	}
	#endregion private-method
}

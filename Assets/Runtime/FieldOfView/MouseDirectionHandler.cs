using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDirectionHandler : MonoBehaviour
{
	#region public-method
	public Vector3 GetDirection()
	{
		var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		var plane = new Plane(Vector3.up, transform.position);
		if (plane.Raycast(ray, out var dis))
		{
			var target = ray.GetPoint(dis);
			var dir = target - transform.position;
			return dir;
		}
		return Vector3.zero;
	}
	#endregion public-method
}

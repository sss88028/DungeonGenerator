using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CharacterMover : MonoBehaviour
{
	#region private-field
	private InputMaster _inputMaster;

	[SerializeField]
	private CharacterController _cc;
	[SerializeField]
	private float _speed = 5;
	#endregion private-field

	#region MonoBehaviour-method
	private void Awake()
	{
		_inputMaster = new InputMaster();
		_inputMaster.Enable();
	}

	private void Update()
	{
		UpdateCharacterController();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void UpdateCharacterController()
	{
		if (_cc == null) 
		{
			return;
		}
		var v = _inputMaster.Player.Movement.ReadValue<Vector2>();
		var newV = new Vector3(v.x, 0, v.y).normalized * _speed;
		_cc.SimpleMove(newV);
	}
	#endregion private-method
}

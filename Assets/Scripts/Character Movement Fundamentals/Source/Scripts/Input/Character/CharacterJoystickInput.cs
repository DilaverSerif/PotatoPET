using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This character movement input class is an example of how to get input from a gamepad/joystick to control the character;
	//It comes with a dead zone threshold setting to bypass any unwanted joystick "jitter";
	public class CharacterJoystickInput : CharacterInput {
		
		private Joystick joystick;
		private void Awake()
		{
			joystick = FindObjectOfType<Joystick>();
		}
		
		public override float GetHorizontalMovementInput()
		{
			float _horizontalInput;
			
			_horizontalInput = joystick.Output.x;
			
			return _horizontalInput;
		}

		public override float GetVerticalMovementInput()
		{
			float _verticalInput;

			_verticalInput = joystick.Output.y;
			
			return _verticalInput;
		}

		public override bool IsJumpKeyPressed()
		{
			return Input.GetKey(KeyCode.Space);
		}

	}
}

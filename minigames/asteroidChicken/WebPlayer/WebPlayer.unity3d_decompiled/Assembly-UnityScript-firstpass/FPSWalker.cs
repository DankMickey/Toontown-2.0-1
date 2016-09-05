using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class FPSWalker : MonoBehaviour
{
	public float speed;

	public float jumpSpeed;

	public float gravity;

	private Vector3 moveDirection;

	private bool grounded;

	public FPSWalker()
	{
		this.speed = 6f;
		this.jumpSpeed = 8f;
		this.gravity = 20f;
		this.moveDirection = Vector3.get_zero();
	}

	public override void FixedUpdate()
	{
		if (this.grounded)
		{
			this.moveDirection = new Vector3(Input.GetAxis("Horizontal"), (float)0, Input.GetAxis("Vertical"));
			this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
			this.moveDirection *= this.speed;
			if (Input.GetButton("Jump"))
			{
				this.moveDirection.y = this.jumpSpeed;
			}
		}
		this.moveDirection.y = this.moveDirection.y - this.gravity * Time.get_deltaTime();
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		CollisionFlags collisionFlags = characterController.Move(this.moveDirection * Time.get_deltaTime());
		this.grounded = ((collisionFlags & 4) != 0);
	}

	public override void Main()
	{
	}
}

using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("Third Person Player/Third Person Controller"), RequireComponent(typeof(CharacterController))]
[Serializable]
public class ThirdPersonController : MonoBehaviour
{
	public float walkSpeed;

	public float trotSpeed;

	public float runSpeed;

	public float inAirControlAcceleration;

	public float jumpHeight;

	public float extraJumpHeight;

	public float gravity;

	public float controlledDescentGravity;

	public float speedSmoothing;

	public float rotateSpeed;

	public float trotAfterSeconds;

	public bool canJump;

	public bool canControlDescent;

	public bool canWallJump;

	private float jumpRepeatTime;

	private float wallJumpTimeout;

	private float jumpTimeout;

	private float groundedTimeout;

	private float lockCameraTimer;

	private Vector3 moveDirection;

	private float verticalSpeed;

	private float moveSpeed;

	private CollisionFlags collisionFlags;

	private bool jumping;

	private bool jumpingReachedApex;

	private bool movingBack;

	private bool isMoving;

	private float walkTimeStart;

	private float lastJumpButtonTime;

	private float lastJumpTime;

	private Vector3 wallJumpContactNormal;

	private float wallJumpContactNormalHeight;

	private float lastJumpStartHeight;

	private float touchWallJumpTime;

	private Vector3 inAirVelocity;

	private float lastGroundedTime;

	private float lean;

	private bool slammed;

	private bool isControllable;

	public ThirdPersonController()
	{
		this.walkSpeed = 3f;
		this.trotSpeed = 4f;
		this.runSpeed = 6f;
		this.inAirControlAcceleration = 3f;
		this.jumpHeight = 0.5f;
		this.extraJumpHeight = 2.5f;
		this.gravity = 20f;
		this.controlledDescentGravity = 2f;
		this.speedSmoothing = 10f;
		this.rotateSpeed = 500f;
		this.trotAfterSeconds = 3f;
		this.canJump = true;
		this.canControlDescent = true;
		this.jumpRepeatTime = 0.05f;
		this.wallJumpTimeout = 0.15f;
		this.jumpTimeout = 0.15f;
		this.groundedTimeout = 0.25f;
		this.moveDirection = Vector3.get_zero();
		this.lastJumpButtonTime = -10f;
		this.lastJumpTime = -1f;
		this.touchWallJumpTime = -1f;
		this.inAirVelocity = Vector3.get_zero();
		this.isControllable = true;
	}

	public override void Awake()
	{
		this.moveDirection = this.get_transform().TransformDirection(Vector3.get_forward());
	}

	public override void HidePlayer()
	{
		RuntimeServices.SetProperty(GameObject.Find("rootJoint").GetComponent(typeof(SkinnedMeshRenderer)), "enabled", false);
		this.isControllable = false;
	}

	public override void ShowPlayer()
	{
		RuntimeServices.SetProperty(GameObject.Find("rootJoint").GetComponent(typeof(SkinnedMeshRenderer)), "enabled", true);
		this.isControllable = true;
	}

	public override void UpdateSmoothedMovementDirection()
	{
		Transform transform = Camera.get_main().get_transform();
		bool flag = this.IsGrounded();
		Vector3 vector = transform.TransformDirection(Vector3.get_forward());
		vector.y = (float)0;
		vector = vector.get_normalized();
		Vector3 vector2 = new Vector3(vector.z, (float)0, -vector.x);
		float axisRaw = Input.GetAxisRaw("Vertical");
		float axisRaw2 = Input.GetAxisRaw("Horizontal");
		if (axisRaw < -0.2f)
		{
			this.movingBack = true;
		}
		else
		{
			this.movingBack = false;
		}
		bool flag2 = this.isMoving;
		this.isMoving = ((Mathf.Abs(axisRaw2) > 0.1f) ?? (Mathf.Abs(axisRaw) > 0.1f));
		Vector3 vector3 = axisRaw2 * vector2 + axisRaw * vector;
		if (flag)
		{
			this.lockCameraTimer += Time.get_deltaTime();
			if (this.isMoving != flag2)
			{
				this.lockCameraTimer = (float)0;
			}
			if (vector3 != Vector3.get_zero())
			{
				if (this.moveSpeed < this.walkSpeed * 0.9f && flag)
				{
					this.moveDirection = vector3.get_normalized();
				}
				else
				{
					this.moveDirection = Vector3.RotateTowards(this.moveDirection, vector3, this.rotateSpeed * 0.0174532924f * Time.get_deltaTime(), (float)1000);
					this.moveDirection = this.moveDirection.get_normalized();
				}
			}
			float num = this.speedSmoothing * Time.get_deltaTime();
			float num2 = Mathf.Min(vector3.get_magnitude(), 1f);
			if (Input.GetButton("Fire3"))
			{
				num2 *= this.runSpeed;
			}
			else if (Time.get_time() - this.trotAfterSeconds > this.walkTimeStart)
			{
				num2 *= this.trotSpeed;
			}
			else
			{
				num2 *= this.walkSpeed;
			}
			this.moveSpeed = Mathf.Lerp(this.moveSpeed, num2, num);
			if (this.moveSpeed < this.walkSpeed * 0.3f)
			{
				this.walkTimeStart = Time.get_time();
			}
		}
		else
		{
			if (this.jumping)
			{
				this.lockCameraTimer = (float)0;
			}
			if (this.isMoving)
			{
				this.inAirVelocity += vector3.get_normalized() * Time.get_deltaTime() * this.inAirControlAcceleration;
			}
		}
	}

	public override void ApplyWallJump()
	{
		if (this.jumping)
		{
			if (this.collisionFlags == 1)
			{
				this.touchWallJumpTime = Time.get_time();
			}
			bool arg_58_0;
			if (arg_58_0 = (this.lastJumpButtonTime > this.touchWallJumpTime - this.wallJumpTimeout))
			{
				arg_58_0 = (this.lastJumpButtonTime < this.touchWallJumpTime + this.wallJumpTimeout);
			}
			if (arg_58_0)
			{
				if (this.lastJumpTime + this.jumpRepeatTime <= Time.get_time())
				{
					if (Mathf.Abs(this.wallJumpContactNormal.y) < 0.2f)
					{
						this.wallJumpContactNormal.y = (float)0;
						this.moveDirection = this.wallJumpContactNormal.get_normalized();
						this.moveSpeed = Mathf.Clamp(this.moveSpeed * 1.5f, this.trotSpeed, this.runSpeed);
					}
					else
					{
						this.moveSpeed = (float)0;
					}
					this.verticalSpeed = this.CalculateJumpVerticalSpeed(this.jumpHeight);
					this.DidJump();
					this.SendMessage("DidWallJump", null, 1);
				}
			}
		}
	}

	public override void ApplyJumping()
	{
		if (this.lastJumpTime + this.jumpRepeatTime <= Time.get_time())
		{
			if (this.IsGrounded() && this.canJump && Time.get_time() < this.lastJumpButtonTime + this.jumpTimeout)
			{
				this.verticalSpeed = this.CalculateJumpVerticalSpeed(this.jumpHeight);
				this.SendMessage("DidJump", 1);
			}
		}
	}

	public override void ApplyGravity()
	{
		if (this.isControllable)
		{
			bool button = Input.GetButton("Jump");
			bool arg_30_0;
			if (arg_30_0 = this.canControlDescent)
			{
				arg_30_0 = (this.verticalSpeed <= (float)0);
			}
			bool arg_38_0;
			if (arg_38_0 = arg_30_0)
			{
				arg_38_0 = button;
			}
			bool arg_45_0;
			if (arg_45_0 = arg_38_0)
			{
				arg_45_0 = this.jumping;
			}
			bool flag = arg_45_0;
			if (this.jumping && !this.jumpingReachedApex && this.verticalSpeed <= (float)0)
			{
				this.jumpingReachedApex = true;
				this.SendMessage("DidJumpReachApex", 1);
			}
			bool arg_93_0;
			if (arg_93_0 = this.IsJumping())
			{
				arg_93_0 = (this.verticalSpeed > (float)0);
			}
			bool arg_9B_0;
			if (arg_9B_0 = arg_93_0)
			{
				arg_9B_0 = button;
			}
			bool arg_C4_0;
			if (arg_C4_0 = arg_9B_0)
			{
				arg_C4_0 = (this.get_transform().get_position().y < this.lastJumpStartHeight + this.extraJumpHeight);
			}
			bool flag2 = arg_C4_0;
			if (flag)
			{
				this.verticalSpeed -= this.controlledDescentGravity * Time.get_deltaTime();
			}
			else if (!flag2)
			{
				if (this.IsGrounded())
				{
					this.verticalSpeed = (float)0;
				}
				else
				{
					this.verticalSpeed -= this.gravity * Time.get_deltaTime();
				}
			}
		}
	}

	public override float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt((float)2 * targetJumpHeight * this.gravity);
	}

	public override void DidJump()
	{
		this.jumping = true;
		this.jumpingReachedApex = false;
		this.lastJumpTime = Time.get_time();
		this.lastJumpStartHeight = this.get_transform().get_position().y;
		this.touchWallJumpTime = (float)-1;
		this.lastJumpButtonTime = (float)-10;
	}

	public override void Update()
	{
		if (!this.isControllable)
		{
			Input.ResetInputAxes();
		}
		if (Input.GetButtonDown("Jump"))
		{
			this.lastJumpButtonTime = Time.get_time();
		}
		this.UpdateSmoothedMovementDirection();
		this.ApplyGravity();
		if (this.canWallJump)
		{
			this.ApplyWallJump();
		}
		this.ApplyJumping();
		Vector3 vector = this.moveDirection * this.moveSpeed + new Vector3((float)0, this.verticalSpeed, (float)0) + this.inAirVelocity;
		vector *= Time.get_deltaTime();
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.wallJumpContactNormal = Vector3.get_zero();
		this.collisionFlags = characterController.Move(vector);
		if (this.IsGrounded())
		{
			if (this.slammed)
			{
				this.slammed = false;
				characterController.set_height((float)2);
				float y = this.get_transform().get_position().y + 0.75f;
				Vector3 position = this.get_transform().get_position();
				float num = position.y = y;
				Vector3 vector2;
				this.get_transform().set_position(vector2 = position);
			}
			this.get_transform().set_rotation(Quaternion.LookRotation(this.moveDirection));
		}
		else if (!this.slammed)
		{
			Vector3 vector3 = vector;
			vector3.y = (float)0;
			if (vector3.get_sqrMagnitude() > 0.001f)
			{
				this.get_transform().set_rotation(Quaternion.LookRotation(vector3));
			}
		}
		if (this.IsGrounded())
		{
			this.lastGroundedTime = Time.get_time();
			this.inAirVelocity = Vector3.get_zero();
			if (this.jumping)
			{
				this.jumping = false;
				this.SendMessage("DidLand", 1);
			}
		}
	}

	public override void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.get_moveDirection().y <= 0.01f)
		{
			this.wallJumpContactNormal = hit.get_normal();
		}
	}

	public override float GetSpeed()
	{
		return this.moveSpeed;
	}

	public override bool IsJumping()
	{
		bool arg_16_0;
		if (arg_16_0 = this.jumping)
		{
			arg_16_0 = !this.slammed;
		}
		return arg_16_0;
	}

	public override bool IsGrounded()
	{
		return (this.collisionFlags & 4) != 0;
	}

	public override void SuperJump(float height)
	{
		this.verticalSpeed = this.CalculateJumpVerticalSpeed(height);
		this.collisionFlags = 0;
		this.SendMessage("DidJump", 1);
	}

	public override void SuperJump(float height, Vector3 jumpVelocity)
	{
		this.verticalSpeed = this.CalculateJumpVerticalSpeed(height);
		this.inAirVelocity = jumpVelocity;
		this.collisionFlags = 0;
		this.SendMessage("DidJump", 1);
	}

	public override void Slam(Vector3 direction)
	{
		this.verticalSpeed = this.CalculateJumpVerticalSpeed((float)1);
		this.inAirVelocity = direction * (float)6;
		direction.y = 0.6f;
		Quaternion.LookRotation(-direction);
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		characterController.set_height(0.5f);
		this.slammed = true;
		this.collisionFlags = 0;
		this.SendMessage("DidJump", 1);
	}

	public override Vector3 GetDirection()
	{
		return this.moveDirection;
	}

	public override bool IsMovingBackwards()
	{
		return this.movingBack;
	}

	public override float GetLockCameraTimer()
	{
		return this.lockCameraTimer;
	}

	public override bool IsMoving()
	{
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}

	public override bool HasJumpReachedApex()
	{
		return this.jumpingReachedApex;
	}

	public override bool IsGroundedWithTimeout()
	{
		return this.lastGroundedTime + this.groundedTimeout > Time.get_time();
	}

	public override bool IsControlledDescent()
	{
		bool button = Input.GetButton("Jump");
		bool arg_25_0;
		if (arg_25_0 = this.canControlDescent)
		{
			arg_25_0 = (this.verticalSpeed <= (float)0);
		}
		bool arg_2D_0;
		if (arg_2D_0 = arg_25_0)
		{
			arg_2D_0 = button;
		}
		bool arg_3A_0;
		if (arg_3A_0 = arg_2D_0)
		{
			arg_3A_0 = this.jumping;
		}
		return arg_3A_0;
	}

	public override void Reset()
	{
		this.get_gameObject().set_tag("Player");
	}

	public override void Main()
	{
	}
}

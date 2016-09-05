using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class ThirdPersonController : MonoBehaviour
{
	public AnimationClip idleAnimation;

	public AnimationClip walkAnimation;

	public AnimationClip runAnimation;

	public AnimationClip jumpPoseAnimation;

	public float walkMaxAnimationSpeed;

	public float trotMaxAnimationSpeed;

	public float runMaxAnimationSpeed;

	public float jumpAnimationSpeed;

	public float landAnimationSpeed;

	private object _animation;

	private CharacterState _characterState;

	public float walkSpeed;

	public float trotSpeed;

	public float runSpeed;

	public float inAirControlAcceleration;

	public float jumpHeight;

	public float gravity;

	public float speedSmoothing;

	public float rotateSpeed;

	public float trotAfterSeconds;

	public bool canJump;

	private float jumpRepeatTime;

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

	private float lastJumpStartHeight;

	private Vector3 inAirVelocity;

	private float lastGroundedTime;

	private bool isControllable;

	public ThirdPersonController()
	{
		this.walkMaxAnimationSpeed = 0.75f;
		this.trotMaxAnimationSpeed = 1f;
		this.runMaxAnimationSpeed = 1f;
		this.jumpAnimationSpeed = 1.15f;
		this.landAnimationSpeed = 1f;
		this.walkSpeed = 2f;
		this.trotSpeed = 4f;
		this.runSpeed = 6f;
		this.inAirControlAcceleration = 3f;
		this.jumpHeight = 0.5f;
		this.gravity = 20f;
		this.speedSmoothing = 10f;
		this.rotateSpeed = 500f;
		this.trotAfterSeconds = 3f;
		this.canJump = true;
		this.jumpRepeatTime = 0.05f;
		this.jumpTimeout = 0.15f;
		this.groundedTimeout = 0.25f;
		this.moveDirection = Vector3.get_zero();
		this.lastJumpButtonTime = -10f;
		this.lastJumpTime = -1f;
		this.inAirVelocity = Vector3.get_zero();
		this.isControllable = true;
	}

	public override void Awake()
	{
		this.moveDirection = this.get_transform().TransformDirection(Vector3.get_forward());
		this._animation = this.GetComponent(typeof(Animation));
		if (!RuntimeServices.ToBool(this._animation))
		{
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		}
		if (!this.idleAnimation)
		{
			this._animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if (!this.walkAnimation)
		{
			this._animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if (!this.runAnimation)
		{
			this._animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if (!this.jumpPoseAnimation && this.canJump)
		{
			this._animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
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
			this._characterState = CharacterState.Idle;
			if (Input.GetKey(304) | Input.GetKey(303))
			{
				num2 *= this.runSpeed;
				this._characterState = CharacterState.Running;
			}
			else if (Time.get_time() - this.trotAfterSeconds > this.walkTimeStart)
			{
				num2 *= this.trotSpeed;
				this._characterState = CharacterState.Trotting;
			}
			else
			{
				num2 *= this.walkSpeed;
				this._characterState = CharacterState.Walking;
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
			if (this.jumping && !this.jumpingReachedApex && this.verticalSpeed <= (float)0)
			{
				this.jumpingReachedApex = true;
				this.SendMessage("DidJumpReachApex", 1);
			}
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
		this.lastJumpButtonTime = (float)-10;
		this._characterState = CharacterState.Jumping;
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
		this.ApplyJumping();
		Vector3 vector = this.moveDirection * this.moveSpeed + new Vector3((float)0, this.verticalSpeed, (float)0) + this.inAirVelocity;
		vector *= Time.get_deltaTime();
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.collisionFlags = characterController.Move(vector);
		if (RuntimeServices.ToBool(this._animation))
		{
			if (this._characterState == CharacterState.Jumping)
			{
				if (!this.jumpingReachedApex)
				{
					RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}), "speed", this.jumpAnimationSpeed);
					RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}), "wrapMode", 8);
					UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}, typeof(MonoBehaviour));
				}
				else
				{
					RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}), "speed", -this.landAnimationSpeed);
					RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}), "wrapMode", 8);
					UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
					{
						this.jumpPoseAnimation.get_name()
					}, typeof(MonoBehaviour));
				}
			}
			else if (characterController.get_velocity().get_sqrMagnitude() < 0.1f)
			{
				UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
				{
					this.idleAnimation.get_name()
				}, typeof(MonoBehaviour));
			}
			else if (this._characterState == CharacterState.Running)
			{
				RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
				{
					this.runAnimation.get_name()
				}), "speed", Mathf.Clamp(characterController.get_velocity().get_magnitude(), (float)0, this.runMaxAnimationSpeed));
				UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
				{
					this.runAnimation.get_name()
				}, typeof(MonoBehaviour));
			}
			else if (this._characterState == CharacterState.Trotting)
			{
				RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
				{
					this.walkAnimation.get_name()
				}), "speed", Mathf.Clamp(characterController.get_velocity().get_magnitude(), (float)0, this.trotMaxAnimationSpeed));
				UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
				{
					this.walkAnimation.get_name()
				}, typeof(MonoBehaviour));
			}
			else if (this._characterState == CharacterState.Walking)
			{
				RuntimeServices.SetProperty(RuntimeServices.GetSlice(this, "_animation", new object[]
				{
					this.walkAnimation.get_name()
				}), "speed", Mathf.Clamp(characterController.get_velocity().get_magnitude(), (float)0, this.walkMaxAnimationSpeed));
				UnityRuntimeServices.Invoke(this._animation, "CrossFade", new object[]
				{
					this.walkAnimation.get_name()
				}, typeof(MonoBehaviour));
			}
		}
		if (this.IsGrounded())
		{
			this.get_transform().set_rotation(Quaternion.LookRotation(this.moveDirection));
		}
		else
		{
			Vector3 vector2 = vector;
			vector2.y = (float)0;
			if (vector2.get_sqrMagnitude() > 0.001f)
			{
				this.get_transform().set_rotation(Quaternion.LookRotation(vector2));
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
		if (hit.get_moveDirection().y > 0.01f)
		{
		}
	}

	public override float GetSpeed()
	{
		return this.moveSpeed;
	}

	public override bool IsJumping()
	{
		return this.jumping;
	}

	public override bool IsGrounded()
	{
		return (this.collisionFlags & 4) != 0;
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

	public override void Reset()
	{
		this.get_gameObject().set_tag("Player");
	}

	public override void Main()
	{
	}
}

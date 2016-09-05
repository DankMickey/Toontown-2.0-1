using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Character/Character Motor"), RequireComponent(typeof(CharacterController))]
[Serializable]
public class CharacterMotor : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $SubtractNewPlatformVelocity$5 : GenericGenerator<object>
	{
		internal CharacterMotor $self_$8;

		public $SubtractNewPlatformVelocity$5(CharacterMotor self_)
		{
			this.$self_$8 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new CharacterMotor.$SubtractNewPlatformVelocity$5.$(this.$self_$8);
		}
	}

	public bool canControl;

	public bool useFixedUpdate;

	[NonSerialized]
	public Vector3 inputMoveDirection;

	[NonSerialized]
	public bool inputJump;

	public CharacterMotorMovement movement;

	public CharacterMotorJumping jumping;

	public CharacterMotorMovingPlatform movingPlatform;

	public CharacterMotorSliding sliding;

	[NonSerialized]
	public bool grounded;

	[NonSerialized]
	public Vector3 groundNormal;

	private Vector3 lastGroundNormal;

	private Transform tr;

	private CharacterController controller;

	public CharacterMotor()
	{
		this.canControl = true;
		this.useFixedUpdate = true;
		this.inputMoveDirection = Vector3.get_zero();
		this.movement = new CharacterMotorMovement();
		this.jumping = new CharacterMotorJumping();
		this.movingPlatform = new CharacterMotorMovingPlatform();
		this.sliding = new CharacterMotorSliding();
		this.grounded = true;
		this.groundNormal = Vector3.get_zero();
		this.lastGroundNormal = Vector3.get_zero();
	}

	public override void Awake()
	{
		this.controller = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.tr = this.get_transform();
	}

	private void UpdateFunction()
	{
		Vector3 vector = this.movement.velocity;
		vector = this.ApplyInputVelocityChange(vector);
		vector = this.ApplyGravityAndJumping(vector);
		Vector3 vector2 = Vector3.get_zero();
		if (this.MoveWithPlatform())
		{
			Vector3 vector3 = this.movingPlatform.activePlatform.TransformPoint(this.movingPlatform.activeLocalPoint);
			vector2 = vector3 - this.movingPlatform.activeGlobalPoint;
			if (vector2 != Vector3.get_zero())
			{
				this.controller.Move(vector2);
			}
			Quaternion quaternion = this.movingPlatform.activePlatform.get_rotation() * this.movingPlatform.activeLocalRotation;
			float y = (quaternion * Quaternion.Inverse(this.movingPlatform.activeGlobalRotation)).get_eulerAngles().y;
			if (y != (float)0)
			{
				this.tr.Rotate((float)0, y, (float)0);
			}
		}
		Vector3 position = this.tr.get_position();
		Vector3 vector4 = vector * Time.get_deltaTime();
		float num = Mathf.Max(this.controller.get_stepOffset(), new Vector3(vector4.x, (float)0, vector4.z).get_magnitude());
		if (this.grounded)
		{
			vector4 -= num * Vector3.get_up();
		}
		this.movingPlatform.hitPlatform = null;
		this.groundNormal = Vector3.get_zero();
		this.movement.collisionFlags = this.controller.Move(vector4);
		this.movement.lastHitPoint = this.movement.hitPoint;
		this.lastGroundNormal = this.groundNormal;
		if (this.movingPlatform.enabled && this.movingPlatform.activePlatform != this.movingPlatform.hitPlatform && this.movingPlatform.hitPlatform != null)
		{
			this.movingPlatform.activePlatform = this.movingPlatform.hitPlatform;
			this.movingPlatform.lastMatrix = this.movingPlatform.hitPlatform.get_localToWorldMatrix();
			this.movingPlatform.newPlatform = true;
		}
		Vector3 vector5 = new Vector3(vector.x, (float)0, vector.z);
		this.movement.velocity = (this.tr.get_position() - position) / Time.get_deltaTime();
		Vector3 vector6 = new Vector3(this.movement.velocity.x, (float)0, this.movement.velocity.z);
		if (vector5 == Vector3.get_zero())
		{
			this.movement.velocity = new Vector3((float)0, this.movement.velocity.y, (float)0);
		}
		else
		{
			float num2 = Vector3.Dot(vector6, vector5) / vector5.get_sqrMagnitude();
			this.movement.velocity = vector5 * Mathf.Clamp01(num2) + this.movement.velocity.y * Vector3.get_up();
		}
		if (this.movement.velocity.y < vector.y - 0.001f)
		{
			if (this.movement.velocity.y < (float)0)
			{
				this.movement.velocity.y = vector.y;
			}
			else
			{
				this.jumping.holdingJumpButton = false;
			}
		}
		if (this.grounded && !this.IsGroundedTest())
		{
			this.grounded = false;
			if (this.movingPlatform.enabled && (this.movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer || this.movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer))
			{
				this.movement.frameVelocity = this.movingPlatform.platformVelocity;
				this.movement.velocity = this.movement.velocity + this.movingPlatform.platformVelocity;
			}
			this.SendMessage("OnFall", 1);
			this.tr.set_position(this.tr.get_position() + num * Vector3.get_up());
		}
		else if (!this.grounded && this.IsGroundedTest())
		{
			this.grounded = true;
			this.jumping.jumping = false;
			this.StartCoroutine_Auto(this.SubtractNewPlatformVelocity());
			this.SendMessage("OnLand", 1);
		}
		if (this.MoveWithPlatform())
		{
			this.movingPlatform.activeGlobalPoint = this.tr.get_position() + Vector3.get_up() * (this.controller.get_center().y - this.controller.get_height() * 0.5f + this.controller.get_radius());
			this.movingPlatform.activeLocalPoint = this.movingPlatform.activePlatform.InverseTransformPoint(this.movingPlatform.activeGlobalPoint);
			this.movingPlatform.activeGlobalRotation = this.tr.get_rotation();
			this.movingPlatform.activeLocalRotation = Quaternion.Inverse(this.movingPlatform.activePlatform.get_rotation()) * this.movingPlatform.activeGlobalRotation;
		}
	}

	public override void FixedUpdate()
	{
		if (this.movingPlatform.enabled)
		{
			if (this.movingPlatform.activePlatform != null)
			{
				if (!this.movingPlatform.newPlatform)
				{
					Vector3 platformVelocity = this.movingPlatform.platformVelocity;
					this.movingPlatform.platformVelocity = (this.movingPlatform.activePlatform.get_localToWorldMatrix().MultiplyPoint3x4(this.movingPlatform.activeLocalPoint) - this.movingPlatform.lastMatrix.MultiplyPoint3x4(this.movingPlatform.activeLocalPoint)) / Time.get_deltaTime();
				}
				this.movingPlatform.lastMatrix = this.movingPlatform.activePlatform.get_localToWorldMatrix();
				this.movingPlatform.newPlatform = false;
			}
			else
			{
				this.movingPlatform.platformVelocity = Vector3.get_zero();
			}
		}
		if (this.useFixedUpdate)
		{
			this.UpdateFunction();
		}
	}

	public override void Update()
	{
		if (!this.useFixedUpdate)
		{
			this.UpdateFunction();
		}
	}

	private Vector3 ApplyInputVelocityChange(Vector3 velocity)
	{
		if (!this.canControl)
		{
			this.inputMoveDirection = Vector3.get_zero();
		}
		Vector3 vector = default(Vector3);
		if (this.grounded && this.TooSteep())
		{
			vector = new Vector3(this.groundNormal.x, (float)0, this.groundNormal.z).get_normalized();
			Vector3 vector2 = Vector3.Project(this.inputMoveDirection, vector);
			vector = vector + vector2 * this.sliding.speedControl + (this.inputMoveDirection - vector2) * this.sliding.sidewaysControl;
			vector *= this.sliding.slidingSpeed;
		}
		else
		{
			vector = this.GetDesiredHorizontalVelocity();
		}
		if (this.movingPlatform.enabled && this.movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
		{
			vector += this.movement.frameVelocity;
			vector.y = (float)0;
		}
		if (this.grounded)
		{
			vector = this.AdjustGroundVelocityToNormal(vector, this.groundNormal);
		}
		else
		{
			velocity.y = (float)0;
		}
		float num = this.GetMaxAcceleration(this.grounded) * Time.get_deltaTime();
		Vector3 vector3 = vector - velocity;
		if (vector3.get_sqrMagnitude() > num * num)
		{
			vector3 = vector3.get_normalized() * num;
		}
		if (this.grounded || this.canControl)
		{
			velocity += vector3;
		}
		if (this.grounded)
		{
			velocity.y = Mathf.Min(velocity.y, (float)0);
		}
		return velocity;
	}

	private Vector3 ApplyGravityAndJumping(Vector3 velocity)
	{
		if (!this.inputJump || !this.canControl)
		{
			this.jumping.holdingJumpButton = false;
			this.jumping.lastButtonDownTime = (float)-100;
		}
		if (this.inputJump && this.jumping.lastButtonDownTime < (float)0 && this.canControl)
		{
			this.jumping.lastButtonDownTime = Time.get_time();
		}
		if (this.grounded)
		{
			velocity.y = Mathf.Min((float)0, velocity.y) - this.movement.gravity * Time.get_deltaTime();
		}
		else
		{
			velocity.y = this.movement.velocity.y - this.movement.gravity * Time.get_deltaTime();
			if (this.jumping.jumping && this.jumping.holdingJumpButton && Time.get_time() < this.jumping.lastStartTime + this.jumping.extraHeight / this.CalculateJumpVerticalSpeed(this.jumping.baseHeight))
			{
				velocity += this.jumping.jumpDir * this.movement.gravity * Time.get_deltaTime();
			}
			velocity.y = Mathf.Max(velocity.y, -this.movement.maxFallSpeed);
		}
		if (this.grounded)
		{
			if (this.jumping.enabled && this.canControl && Time.get_time() - this.jumping.lastButtonDownTime < 0.2f)
			{
				this.grounded = false;
				this.jumping.jumping = true;
				this.jumping.lastStartTime = Time.get_time();
				this.jumping.lastButtonDownTime = (float)-100;
				this.jumping.holdingJumpButton = true;
				if (this.TooSteep())
				{
					this.jumping.jumpDir = Vector3.Slerp(Vector3.get_up(), this.groundNormal, this.jumping.steepPerpAmount);
				}
				else
				{
					this.jumping.jumpDir = Vector3.Slerp(Vector3.get_up(), this.groundNormal, this.jumping.perpAmount);
				}
				velocity.y = (float)0;
				velocity += this.jumping.jumpDir * this.CalculateJumpVerticalSpeed(this.jumping.baseHeight);
				if (this.movingPlatform.enabled && (this.movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer || this.movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer))
				{
					this.movement.frameVelocity = this.movingPlatform.platformVelocity;
					velocity += this.movingPlatform.platformVelocity;
				}
				this.SendMessage("OnJump", 1);
			}
			else
			{
				this.jumping.holdingJumpButton = false;
			}
		}
		return velocity;
	}

	public override void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.get_normal().y > (float)0 && hit.get_normal().y > this.groundNormal.y && hit.get_moveDirection().y < (float)0)
		{
			if ((hit.get_point() - this.movement.lastHitPoint).get_sqrMagnitude() > 0.001f || this.lastGroundNormal == Vector3.get_zero())
			{
				this.groundNormal = hit.get_normal();
			}
			else
			{
				this.groundNormal = this.lastGroundNormal;
			}
			this.movingPlatform.hitPlatform = hit.get_collider().get_transform();
			this.movement.hitPoint = hit.get_point();
			this.movement.frameVelocity = Vector3.get_zero();
		}
	}

	private IEnumerator SubtractNewPlatformVelocity()
	{
		return new CharacterMotor.$SubtractNewPlatformVelocity$5(this).GetEnumerator();
	}

	private bool MoveWithPlatform()
	{
		bool arg_2D_0;
		if (arg_2D_0 = this.movingPlatform.enabled)
		{
			arg_2D_0 = (this.grounded ?? (this.movingPlatform.movementTransfer == MovementTransferOnJump.PermaLocked));
		}
		bool arg_45_0;
		if (arg_45_0 = arg_2D_0)
		{
			arg_45_0 = (this.movingPlatform.activePlatform != null);
		}
		return arg_45_0;
	}

	private Vector3 GetDesiredHorizontalVelocity()
	{
		Vector3 vector = this.tr.InverseTransformDirection(this.inputMoveDirection);
		float num = this.MaxSpeedInDirection(vector);
		if (this.grounded)
		{
			float num2 = Mathf.Asin(this.movement.velocity.get_normalized().y) * 57.29578f;
			num *= this.movement.slopeSpeedMultiplier.Evaluate(num2);
		}
		return this.tr.TransformDirection(vector * num);
	}

	private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
	{
		Vector3 vector = Vector3.Cross(Vector3.get_up(), hVelocity);
		return Vector3.Cross(vector, groundNormal).get_normalized() * hVelocity.get_magnitude();
	}

	private bool IsGroundedTest()
	{
		return this.groundNormal.y > 0.01f;
	}

	public override float GetMaxAcceleration(bool grounded)
	{
		return (!grounded) ? this.movement.maxAirAcceleration : this.movement.maxGroundAcceleration;
	}

	public override float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt((float)2 * targetJumpHeight * this.movement.gravity);
	}

	public override bool IsJumping()
	{
		return this.jumping.jumping;
	}

	public override bool IsSliding()
	{
		bool arg_18_0;
		if (arg_18_0 = this.grounded)
		{
			arg_18_0 = this.sliding.enabled;
		}
		bool arg_25_0;
		if (arg_25_0 = arg_18_0)
		{
			arg_25_0 = this.TooSteep();
		}
		return arg_25_0;
	}

	public override bool IsTouchingCeiling()
	{
		return (this.movement.collisionFlags & 2) != 0;
	}

	public override bool IsGrounded()
	{
		return this.grounded;
	}

	public override bool TooSteep()
	{
		return this.groundNormal.y <= Mathf.Cos(this.controller.get_slopeLimit() * 0.0174532924f);
	}

	public override Vector3 GetDirection()
	{
		return this.inputMoveDirection;
	}

	public override void SetControllable(bool controllable)
	{
		this.canControl = controllable;
	}

	public override float MaxSpeedInDirection(Vector3 desiredMovementDirection)
	{
		float arg_AC_0;
		if (desiredMovementDirection == Vector3.get_zero())
		{
			arg_AC_0 = (float)0;
		}
		else
		{
			float num = ((desiredMovementDirection.z <= (float)0) ? this.movement.maxBackwardsSpeed : this.movement.maxForwardSpeed) / this.movement.maxSidewaysSpeed;
			Vector3 normalized = new Vector3(desiredMovementDirection.x, (float)0, desiredMovementDirection.z / num).get_normalized();
			float num2 = new Vector3(normalized.x, (float)0, normalized.z * num).get_magnitude() * this.movement.maxSidewaysSpeed;
			arg_AC_0 = num2;
		}
		return arg_AC_0;
	}

	public override void SetVelocity(Vector3 velocity)
	{
		this.grounded = false;
		this.movement.velocity = velocity;
		this.movement.frameVelocity = Vector3.get_zero();
		this.SendMessage("OnExternalVelocity");
	}

	public override void Main()
	{
	}
}

using System;
using UnityEngine;
using UnitySteer;

public class AutonomousVehicle : Vehicle
{
	private Vector3 _smoothedAcceleration;

	private Rigidbody _rigidbody;

	private CharacterController _characterController;

	private void Start()
	{
		this._rigidbody = base.GetComponent<Rigidbody>();
		this._characterController = base.GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		Vector3 vector = Vector3.get_zero();
		Steering[] steerings = base.Steerings;
		for (int i = 0; i < steerings.Length; i++)
		{
			Steering steering = steerings[i];
			if (steering.get_enabled())
			{
				vector += steering.WeighedForce;
			}
		}
		this.ApplySteeringForce(vector, Time.get_fixedDeltaTime());
	}

	private void ApplySteeringForce(Vector3 force, float elapsedTime)
	{
		if (base.MaxForce == 0f || base.MaxSpeed == 0f || elapsedTime == 0f)
		{
			return;
		}
		Vector3 vector = Vector3.ClampMagnitude(force, base.MaxForce);
		Vector3 newValue = vector / base.Mass;
		if (newValue.get_sqrMagnitude() == 0f && !base.HasInertia)
		{
			base.Speed = 0f;
		}
		Vector3 vector2 = base.Velocity;
		this._smoothedAcceleration = OpenSteerUtility.blendIntoAccumulator(0.4f, newValue, this._smoothedAcceleration);
		vector2 += this._smoothedAcceleration * elapsedTime;
		vector2 = Vector3.ClampMagnitude(vector2, base.MaxSpeed);
		if (base.IsPlanar)
		{
			vector2.y = base.Velocity.y;
		}
		base.Speed = vector2.get_magnitude();
		Vector3 vector3 = vector2 * elapsedTime;
		if (this._characterController != null)
		{
			this._characterController.Move(vector3);
		}
		else if (this._rigidbody == null || this._rigidbody.get_isKinematic())
		{
			Transform expr_12B = base.get_transform();
			expr_12B.set_position(expr_12B.get_position() + vector3);
		}
		else
		{
			this._rigidbody.MovePosition(this._rigidbody.get_position() + vector3);
		}
		this.RegenerateLocalSpace(vector2);
	}
}

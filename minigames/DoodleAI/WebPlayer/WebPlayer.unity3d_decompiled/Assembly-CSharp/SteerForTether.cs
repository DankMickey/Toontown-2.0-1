using System;
using UnityEngine;

public class SteerForTether : Steering
{
	[SerializeField]
	private float _maximumDistance = 30f;

	[SerializeField]
	private Vector3 _tetherPosition;

	public float MaximumDistance
	{
		get
		{
			return this._maximumDistance;
		}
		set
		{
			this._maximumDistance = Mathf.Clamp(value, 0f, 3.40282347E+38f);
		}
	}

	public Vector3 TetherPosition
	{
		get
		{
			return this._tetherPosition;
		}
		set
		{
			this._tetherPosition = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		Vector3 result = Vector3.get_zero();
		Vector3 vector = this.TetherPosition - base.Vehicle.Position;
		float magnitude = vector.get_magnitude();
		if (magnitude > this._maximumDistance)
		{
			result = vector - base.Vehicle.Velocity;
		}
		return result;
	}
}

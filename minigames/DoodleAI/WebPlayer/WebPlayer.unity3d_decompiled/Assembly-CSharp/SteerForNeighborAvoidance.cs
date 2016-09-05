using System;
using System.Collections.Generic;
using UnityEngine;

public class SteerForNeighborAvoidance : Steering
{
	[SerializeField]
	private float _avoidAngleCos = 0.707f;

	[SerializeField]
	private float _minTimeToCollision = 2f;

	public float AvoidAngleCos
	{
		get
		{
			return this._avoidAngleCos;
		}
		set
		{
			this._avoidAngleCos = value;
		}
	}

	public float MinTimeToCollision
	{
		get
		{
			return this._minTimeToCollision;
		}
		set
		{
			this._minTimeToCollision = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		float num = 0f;
		Vehicle vehicle = null;
		float num2 = this._minTimeToCollision;
		Vector3 vector = Vector3.get_zero();
		Vector3 position = Vector3.get_zero();
		using (IEnumerator<Vehicle> enumerator = base.Vehicle.Radar.Vehicles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vehicle current = enumerator.get_Current();
				if (current != this)
				{
					float num3 = base.Vehicle.ScaledRadius + current.ScaledRadius;
					float num4 = base.Vehicle.PredictNearestApproachTime(current);
					if (num4 >= 0f && num4 < num2)
					{
						Vector3 zero = Vector3.get_zero();
						Vector3 zero2 = Vector3.get_zero();
						float num5 = base.Vehicle.ComputeNearestApproachPositions(current, num4, ref zero, ref zero2);
						if (num5 < num3)
						{
							num2 = num4;
							vehicle = current;
							vector = zero2;
							position = zero;
						}
					}
				}
			}
		}
		if (vehicle != null)
		{
			float num6 = Vector3.Dot(base.get_transform().get_forward(), vehicle.get_transform().get_forward());
			if (num6 < -this._avoidAngleCos)
			{
				Vector3 vector2 = vector - base.Vehicle.Position;
				float num7 = Vector3.Dot(vector2, base.get_transform().get_right());
				num = ((num7 <= 0f) ? 1f : -1f);
			}
			else if (num6 > this._avoidAngleCos)
			{
				Vector3 vector3 = vehicle.Position - base.Vehicle.Position;
				float num8 = Vector3.Dot(vector3, base.get_transform().get_right());
				num = ((num8 <= 0f) ? 1f : -1f);
			}
			else if (base.Vehicle.Speed < vehicle.Speed || vehicle.Speed == 0f || base.get_gameObject().GetInstanceID() < vehicle.get_gameObject().GetInstanceID())
			{
				float num9 = Vector3.Dot(base.get_transform().get_right(), vehicle.Velocity);
				num = ((num9 <= 0f) ? 1f : -1f);
			}
			num *= base.Vehicle.ScaledRadius + vehicle.ScaledRadius;
			this.AnnotateAvoidNeighbor(vehicle, num, position, vector);
		}
		return base.get_transform().get_right() * num;
	}

	protected virtual void AnnotateAvoidNeighbor(Vehicle vehicle, float steer, Vector3 position, Vector3 threatPosition)
	{
		Debug.DrawLine(base.Vehicle.Position, vehicle.Position, Color.get_red());
		Debug.DrawLine(base.Vehicle.Position, position, Color.get_green());
		Debug.DrawLine(base.Vehicle.Position, threatPosition, Color.get_magenta());
	}
}

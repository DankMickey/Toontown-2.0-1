using System;
using UnityEngine;
using UnitySteer;
using UnitySteer.Helpers;

public class SteerForPursuit : Steering
{
	private bool _reportedArrival;

	private SteeringEventHandler<Vehicle> _onArrival;

	[SerializeField]
	private Vehicle _quarry;

	[SerializeField]
	private float _maxPredictionTime = 5f;

	public float MaxPredictionTime
	{
		get
		{
			return this._maxPredictionTime;
		}
		set
		{
			this._maxPredictionTime = value;
		}
	}

	public SteeringEventHandler<Vehicle> OnArrival
	{
		get
		{
			return this._onArrival;
		}
		set
		{
			this._onArrival = value;
		}
	}

	public Vehicle Quarry
	{
		get
		{
			return this._quarry;
		}
		set
		{
			if (this._quarry != value)
			{
				this._reportedArrival = false;
				this._quarry = value;
			}
		}
	}

	protected override Vector3 CalculateForce()
	{
		if (this._quarry == null)
		{
			base.set_enabled(false);
			return Vector3.get_zero();
		}
		Vector3 vector = Vector3.get_zero();
		Vector3 vector2 = this._quarry.Position - base.Vehicle.Position;
		float magnitude = vector2.get_magnitude();
		float num = base.Vehicle.ScaledRadius + this._quarry.ScaledRadius;
		if (magnitude > num)
		{
			Vector3 vector3 = vector2 / magnitude;
			float x = Vector3.Dot(base.get_transform().get_forward(), this._quarry.get_transform().get_forward());
			float x2 = Vector3.Dot(base.get_transform().get_forward(), vector3);
			float num2 = magnitude / base.Vehicle.Speed;
			int num3 = OpenSteerUtility.intervalComparison(x2, -0.707f, 0.707f);
			int num4 = OpenSteerUtility.intervalComparison(x, -0.707f, 0.707f);
			float num5 = 0f;
			int num6 = num3;
			switch (num6 + 1)
			{
			case 0:
			{
				int num7 = num4;
				switch (num7 + 1)
				{
				case 0:
					num5 = 2f;
					break;
				case 1:
					num5 = 2f;
					break;
				case 2:
					num5 = 0.5f;
					break;
				}
				break;
			}
			case 1:
			{
				int num7 = num4;
				switch (num7 + 1)
				{
				case 0:
					num5 = 4f;
					break;
				case 1:
					num5 = 0.8f;
					break;
				case 2:
					num5 = 1f;
					break;
				}
				break;
			}
			case 2:
			{
				int num7 = num4;
				switch (num7 + 1)
				{
				case 0:
					num5 = 0.85f;
					break;
				case 1:
					num5 = 1.8f;
					break;
				case 2:
					num5 = 4f;
					break;
				}
				break;
			}
			}
			float num8 = num2 * num5;
			float predictionTime = (num8 <= this._maxPredictionTime) ? num8 : this._maxPredictionTime;
			Vector3 vector4 = this._quarry.PredictFuturePosition(predictionTime);
			vector = base.Vehicle.GetSeekVector(vector4);
			Debug.DrawLine(base.Vehicle.Position, vector, Color.get_blue());
			Debug.DrawLine(this.Quarry.Position, vector4, Color.get_cyan());
			Debug.DrawRay(vector4, Vector3.get_up() * 4f, Color.get_cyan());
		}
		if (!this._reportedArrival && this._onArrival != null && vector == Vector3.get_zero())
		{
			this._reportedArrival = true;
			this._onArrival(new SteeringEvent<Vehicle>(this, "arrived", this.Quarry));
		}
		else
		{
			this._reportedArrival = (vector == Vector3.get_zero());
		}
		return vector;
	}
}

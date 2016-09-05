using System;
using UnityEngine;

public class SteerForEvasion : Steering
{
	[SerializeField]
	private Vehicle _menace;

	[SerializeField]
	private float _predictionTime;

	public float PredictionTime
	{
		get
		{
			return this._predictionTime;
		}
		set
		{
			this._predictionTime = value;
		}
	}

	public Vehicle Menace
	{
		get
		{
			return this._menace;
		}
		set
		{
			this._menace = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		float magnitude = (this._menace.Position - base.Vehicle.Position).get_magnitude();
		float num = magnitude / this._menace.Speed;
		float predictionTime = (num <= this._predictionTime) ? num : this._predictionTime;
		Vector3 vector = this._menace.PredictFuturePosition(predictionTime);
		Vector3 vector2 = base.Vehicle.Position - vector;
		return vector2 - base.Vehicle.Velocity;
	}
}

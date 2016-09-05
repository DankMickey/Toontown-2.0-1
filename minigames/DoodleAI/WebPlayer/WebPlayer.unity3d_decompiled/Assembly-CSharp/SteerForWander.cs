using System;
using UnityEngine;
using UnitySteer;

public class SteerForWander : Steering
{
	private float _wanderSide;

	private float _wanderUp;

	[SerializeField]
	private float _maxLatitudeSide = 2f;

	[SerializeField]
	private float _maxLatitudeUp = 2f;

	public float MaxLatitudeSide
	{
		get
		{
			return this._maxLatitudeSide;
		}
		set
		{
			this._maxLatitudeSide = value;
		}
	}

	public float MaxLatitudeUp
	{
		get
		{
			return this._maxLatitudeUp;
		}
		set
		{
			this._maxLatitudeUp = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		float maxSpeed = base.Vehicle.MaxSpeed;
		this._wanderSide = OpenSteerUtility.scalarRandomWalk(this._wanderSide, maxSpeed, -this._maxLatitudeSide, this._maxLatitudeSide);
		this._wanderUp = OpenSteerUtility.scalarRandomWalk(this._wanderUp, maxSpeed, -this._maxLatitudeUp, this._maxLatitudeUp);
		return base.get_transform().get_right() * this._wanderSide + base.get_transform().get_up() * this._wanderUp;
	}
}

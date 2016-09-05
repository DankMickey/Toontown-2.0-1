using System;
using UnityEngine;

public class SteerForPoint : Steering
{
	public Vector3 TargetPoint;

	private void Awake()
	{
		if (this.TargetPoint == Vector3.get_zero())
		{
			this.TargetPoint = base.get_transform().get_position();
		}
	}

	protected override Vector3 CalculateForce()
	{
		return base.Vehicle.GetSeekVector(this.TargetPoint);
	}
}

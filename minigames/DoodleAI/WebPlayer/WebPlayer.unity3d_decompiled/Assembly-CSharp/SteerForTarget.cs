using System;
using UnityEngine;

public class SteerForTarget : Steering
{
	public Transform Target;

	private void Awake()
	{
		if (this.Target == null)
		{
			Object.Destroy(this);
			throw new Exception("SteerForTarget need a target transform. Dying.");
		}
	}

	protected override Vector3 CalculateForce()
	{
		return base.Vehicle.GetSeekVector(this.Target.get_position());
	}
}

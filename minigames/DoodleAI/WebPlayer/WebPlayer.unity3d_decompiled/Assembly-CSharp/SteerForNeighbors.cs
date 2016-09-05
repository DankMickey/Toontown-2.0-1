using System;
using UnityEngine;
using UnitySteer;

public class SteerForNeighbors : Steering
{
	[SerializeField]
	private float _minRadius = 3f;

	[SerializeField]
	private float _maxRadius = 7.5f;

	[SerializeField]
	private float _angleCos = 0.7f;

	[SerializeField]
	private LayerMask _layersChecked;

	public float AngleCos
	{
		get
		{
			return this._angleCos;
		}
		set
		{
			this._angleCos = Mathf.Clamp(value, -1f, 1f);
		}
	}

	public float AngleDeg
	{
		get
		{
			return OpenSteerUtility.DegreesFromCos(this._angleCos);
		}
		set
		{
			this._angleCos = OpenSteerUtility.CosFromDegrees(value);
		}
	}

	public LayerMask LayersChecked
	{
		get
		{
			return this._layersChecked;
		}
		set
		{
			this._layersChecked = value;
		}
	}

	public float MinRadius
	{
		get
		{
			return this._minRadius;
		}
		set
		{
			this._minRadius = value;
		}
	}

	public float MaxRadius
	{
		get
		{
			return this._maxRadius;
		}
		set
		{
			this._maxRadius = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		Vector3 vector = Vector3.get_zero();
		int num = 0;
		for (int i = 0; i < base.Vehicle.Radar.Vehicles.Count; i++)
		{
			Vehicle vehicle = base.Vehicle.Radar.Vehicles[i];
			if ((1 << vehicle.get_gameObject().get_layer() & this.LayersChecked) != 0 && base.Vehicle.IsInNeighborhood(vehicle, this.MinRadius, this.MaxRadius, this.AngleCos))
			{
				Debug.DrawLine(base.Vehicle.Position, vehicle.Position, Color.get_magenta());
				vector += this.CalculateNeighborContribution(vehicle);
				num++;
			}
		}
		if (num > 0)
		{
			vector /= (float)num;
			vector.Normalize();
		}
		return vector;
	}

	protected virtual Vector3 CalculateNeighborContribution(Vehicle other)
	{
		return Vector3.get_zero();
	}
}

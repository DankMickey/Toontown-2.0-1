using System;
using UnityEngine;
using UnitySteer.Helpers;

public class Steering : MonoBehaviour, ITick
{
	private Vector3 _force = Vector3.get_zero();

	private Vehicle _vehicle;

	[SerializeField]
	private Tick _tick;

	[SerializeField]
	private float _weight = 1f;

	public Vector3 Force
	{
		get
		{
			if (this.Tick.ShouldTick())
			{
				this._force = this.CalculateForce();
			}
			return this._force;
		}
	}

	public Vector3 WeighedForce
	{
		get
		{
			return this.Force * this._weight;
		}
	}

	public Tick Tick
	{
		get
		{
			return this._tick;
		}
	}

	public Vehicle Vehicle
	{
		get
		{
			return this._vehicle;
		}
	}

	public float Weight
	{
		get
		{
			return this._weight;
		}
		set
		{
			this._weight = value;
		}
	}

	protected void Start()
	{
		this._vehicle = base.GetComponent<Vehicle>();
	}

	protected virtual Vector3 CalculateForce()
	{
		return Vector3.get_zero();
	}
}

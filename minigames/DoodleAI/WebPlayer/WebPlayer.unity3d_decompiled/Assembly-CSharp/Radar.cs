using C5;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySteer;
using UnitySteer.Helpers;

public class Radar : MonoBehaviour, ITick
{
	private SteeringEventHandler<Radar> _onDetected;

	[SerializeField]
	private Tick _tick;

	[SerializeField]
	private LayerMask _obstacleLayer;

	[SerializeField]
	private LayerMask _layersChecked;

	private IList<Collider> _detected;

	private IList<Vehicle> _vehicles = new ArrayList<Vehicle>();

	private IList<Obstacle> _obstacles = new ArrayList<Obstacle>();

	private ObstacleFactory _obstacleFactory;

	private Vehicle _vehicle;

	public IList<Collider> Detected
	{
		get
		{
			this.ExecuteRadar();
			return this._detected;
		}
	}

	public IList<Obstacle> Obstacles
	{
		get
		{
			this.ExecuteRadar();
			return new GuardedList<Obstacle>(this._obstacles);
		}
	}

	public SteeringEventHandler<Radar> OnDetected
	{
		get
		{
			return this._onDetected;
		}
		set
		{
			this._onDetected = value;
		}
	}

	public Vehicle Vehicle
	{
		get
		{
			return this._vehicle;
		}
	}

	public IList<Vehicle> Vehicles
	{
		get
		{
			this.ExecuteRadar();
			return new GuardedList<Vehicle>(this._vehicles);
		}
	}

	public LayerMask ObstacleLayer
	{
		get
		{
			return this._obstacleLayer;
		}
		set
		{
			this._obstacleLayer = value;
		}
	}

	public ObstacleFactory ObstacleFactory
	{
		get
		{
			return this._obstacleFactory;
		}
		set
		{
			this._obstacleFactory = value;
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

	public Tick Tick
	{
		get
		{
			return this._tick;
		}
	}

	protected virtual void Awake()
	{
		this._vehicle = base.GetComponent<Vehicle>();
	}

	private void ExecuteRadar()
	{
		if (this._tick.ShouldTick())
		{
			this._detected = this.Detect();
			this.FilterDetected();
			if (this._onDetected != null)
			{
				this._onDetected(new SteeringEvent<Radar>(null, "detect", this));
			}
		}
	}

	protected virtual IList<Collider> Detect()
	{
		return new ArrayList<Collider>();
	}

	protected virtual void FilterDetected()
	{
		this._vehicles.Clear();
		this._obstacles.Clear();
		using (IEnumerator<Collider> enumerator = this._detected.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Collider current = enumerator.get_Current();
				Vehicle component = current.get_gameObject().GetComponent<Vehicle>();
				if (component != null && current.get_gameObject() != base.get_gameObject())
				{
					this._vehicles.Add(component);
				}
				if (this.ObstacleFactory != null && (1 << current.get_gameObject().get_layer() & this.ObstacleLayer) > 0)
				{
					Obstacle item = this.ObstacleFactory(current.get_gameObject());
					this._obstacles.Add(item);
				}
			}
		}
	}
}

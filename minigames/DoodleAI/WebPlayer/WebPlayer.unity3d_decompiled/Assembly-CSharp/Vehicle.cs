using System;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	private static float MIN_FORCE_THRESHOLD = 0.01f;

	private Steering[] _steerings;

	[SerializeField]
	private bool _drawGizmos;

	[HideInInspector, SerializeField]
	private Vector3 _center;

	[HideInInspector, SerializeField]
	private Vector3 _scaledCenter;

	[SerializeField]
	private bool _hasInertia;

	[SerializeField]
	private float _internalMass = 1f;

	[SerializeField]
	private bool _isPlanar;

	[HideInInspector, SerializeField]
	private float _radius = 1f;

	[HideInInspector, SerializeField]
	private float _scaledRadius = 1f;

	private float _speed;

	[SerializeField]
	private float _maxSpeed = 1f;

	[SerializeField]
	private float _maxForce = 10f;

	[SerializeField]
	private bool _canMove = true;

	private Radar _radar;

	private Transform _transform;

	public bool CanMove
	{
		get
		{
			return this._canMove;
		}
		set
		{
			this._canMove = value;
		}
	}

	public Vector3 Center
	{
		get
		{
			return this._center;
		}
		set
		{
			this._center = value;
			this.RecalculateScaledValues();
		}
	}

	public bool HasInertia
	{
		get
		{
			return this._hasInertia;
		}
		set
		{
			this._hasInertia = value;
		}
	}

	public bool IsPlanar
	{
		get
		{
			return this._isPlanar;
		}
		set
		{
			this._isPlanar = value;
		}
	}

	public float Mass
	{
		get
		{
			return (!(base.get_rigidbody() != null)) ? this._internalMass : base.get_rigidbody().get_mass();
		}
		set
		{
			if (base.get_rigidbody() != null)
			{
				base.get_rigidbody().set_mass(value);
			}
			else
			{
				this._internalMass = value;
			}
		}
	}

	public float MaxForce
	{
		get
		{
			return this._maxForce;
		}
		set
		{
			this._maxForce = Mathf.Clamp(value, 0f, 3.40282347E+38f);
		}
	}

	public float MaxSpeed
	{
		get
		{
			return this._maxSpeed;
		}
		set
		{
			this._maxSpeed = Mathf.Clamp(value, 0f, 3.40282347E+38f);
		}
	}

	public Vector3 Position
	{
		get
		{
			return this._transform.get_position() + this._scaledCenter;
		}
	}

	public Radar Radar
	{
		get
		{
			if (this._radar == null)
			{
				this._radar = base.GetComponent<Radar>();
			}
			return this._radar;
		}
	}

	public float Radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = Mathf.Clamp(value, 0.01f, 3.40282347E+38f);
			this.RecalculateScaledValues();
		}
	}

	public Vector3 ScaledCenter
	{
		get
		{
			return this._scaledCenter;
		}
	}

	public float ScaledRadius
	{
		get
		{
			return this._scaledRadius;
		}
	}

	public float Speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			this._speed = Mathf.Clamp(value, 0f, this.MaxSpeed);
		}
	}

	public Steering[] Steerings
	{
		get
		{
			return this._steerings;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return this._transform.get_forward() * this._speed;
		}
	}

	protected void Awake()
	{
		this._steerings = base.GetComponents<Steering>();
		this._transform = base.GetComponent<Transform>();
		this.RecalculateScaledValues();
	}

	protected virtual void RegenerateLocalSpace(Vector3 newVelocity)
	{
		if (this.Speed > 0f && newVelocity.get_sqrMagnitude() > Vehicle.MIN_FORCE_THRESHOLD)
		{
			Vector3 forward = newVelocity / this.Speed;
			forward.y = ((!this.IsPlanar) ? forward.y : this._transform.get_forward().y);
			this._transform.set_forward(forward);
		}
	}

	protected virtual Vector3 AdjustRawSteeringForce(Vector3 force)
	{
		return force;
	}

	protected void RecalculateScaledValues()
	{
		Vector3 lossyScale = this._transform.get_lossyScale();
		this._scaledRadius = this._radius * Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z));
		this._scaledCenter = Vector3.Scale(this._center, lossyScale);
	}

	public virtual Vector3 PredictFuturePosition(float predictionTime)
	{
		return this._transform.get_position() + this.Velocity * predictionTime;
	}

	public bool IsInNeighborhood(Vehicle other, float minDistance, float maxDistance, float cosMaxAngle)
	{
		if (other == this)
		{
			return false;
		}
		Vector3 vector = other.Position - this.Position;
		float sqrMagnitude = vector.get_sqrMagnitude();
		if (sqrMagnitude < minDistance * minDistance)
		{
			return true;
		}
		if (sqrMagnitude > maxDistance * maxDistance)
		{
			return false;
		}
		Vector3 vector2 = vector / Mathf.Sqrt(sqrMagnitude);
		float num = Vector3.Dot(this._transform.get_forward(), vector2);
		return num > cosMaxAngle;
	}

	public Vector3 GetSeekVector(Vector3 target)
	{
		Vector3 result = Vector3.get_zero();
		float num = Vector3.Distance(this.Position, target);
		if (num > this.Radius)
		{
			result = target - this.Position - this.Velocity;
		}
		return result;
	}

	public Vector3 GetTargetSpeedVector(float targetSpeed)
	{
		float maxForce = this.MaxForce;
		float num = targetSpeed - this.Speed;
		return this._transform.get_forward() * Mathf.Clamp(num, -maxForce, maxForce);
	}

	public float DistanceFromPerimeter(Vehicle other)
	{
		return (this.Position - other.Position).get_magnitude() - this.ScaledRadius - other.ScaledRadius;
	}

	public void ResetOrientation()
	{
		this._transform.set_up(Vector3.get_up());
		this._transform.set_forward(Vector3.get_forward());
	}

	public float PredictNearestApproachTime(Vehicle other)
	{
		Vector3 velocity = this.Velocity;
		Vector3 velocity2 = other.Velocity;
		Vector3 vector = velocity2 - velocity;
		float magnitude = vector.get_magnitude();
		if (magnitude == 0f)
		{
			return 0f;
		}
		Vector3 vector2 = vector / magnitude;
		Vector3 vector3 = this.Position - other.Position;
		float num = Vector3.Dot(vector2, vector3);
		return num / magnitude;
	}

	public float ComputeNearestApproachPositions(Vehicle other, float time, ref Vector3 ourPosition, ref Vector3 hisPosition)
	{
		Vector3 vector = this._transform.get_forward() * this.Speed * time;
		Vector3 vector2 = other._transform.get_forward() * other.Speed * time;
		ourPosition = this.Position + vector;
		hisPosition = other.Position + vector2;
		return Vector3.Distance(ourPosition, hisPosition);
	}

	private void OnDrawGizmos()
	{
		if (this._drawGizmos)
		{
			if (this._transform == null)
			{
				this._transform = base.GetComponent<Transform>();
			}
			Gizmos.set_color(Color.get_grey());
			Gizmos.DrawWireSphere(this.Position, this._scaledRadius);
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySteer;

public class SteerForSphericalObstacleAvoidance : Steering
{
	public struct PathIntersection
	{
		public bool intersect;

		public float distance;

		public SphericalObstacle obstacle;

		public PathIntersection(SphericalObstacle obstacle)
		{
			this.obstacle = obstacle;
			this.intersect = false;
			this.distance = 3.40282347E+38f;
		}
	}

	[SerializeField]
	private float _avoidanceForceFactor = 0.75f;

	[SerializeField]
	private float _minTimeToCollision = 2f;

	public float AvoidanceForceFactor
	{
		get
		{
			return this._avoidanceForceFactor;
		}
		set
		{
			this._avoidanceForceFactor = value;
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

	protected void Start()
	{
		base.Start();
		base.Vehicle.Radar.ObstacleFactory = new ObstacleFactory(SphericalObstacle.GetObstacle);
	}

	protected override Vector3 CalculateForce()
	{
		Vector3 vector = Vector3.get_zero();
		if (base.Vehicle.Radar.Obstacles == null || base.Vehicle.Radar.Obstacles.Count == 0)
		{
			return vector;
		}
		SteerForSphericalObstacleAvoidance.PathIntersection pathIntersection = new SteerForSphericalObstacleAvoidance.PathIntersection(null);
		Vector3 vector2 = base.Vehicle.PredictFuturePosition(this._minTimeToCollision);
		Vector3 line = vector2 - base.Vehicle.Position;
		using (IEnumerator<Obstacle> enumerator = base.Vehicle.Radar.Obstacles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Obstacle current = enumerator.get_Current();
				SphericalObstacle obs = current as SphericalObstacle;
				SteerForSphericalObstacleAvoidance.PathIntersection pathIntersection2 = this.FindNextIntersectionWithSphere(obs, line);
				if (!pathIntersection.intersect || (pathIntersection2.intersect && pathIntersection2.distance < pathIntersection.distance))
				{
					pathIntersection = pathIntersection2;
				}
			}
		}
		if (pathIntersection.intersect && pathIntersection.distance < line.get_magnitude())
		{
			Debug.DrawLine(base.Vehicle.Position, pathIntersection.obstacle.center, Color.get_red());
			Vector3 source = base.Vehicle.Position - pathIntersection.obstacle.center;
			vector = OpenSteerUtility.perpendicularComponent(source, base.get_transform().get_forward());
			vector.Normalize();
			vector *= base.Vehicle.MaxForce;
			vector += base.get_transform().get_forward() * base.Vehicle.MaxForce * this._avoidanceForceFactor;
		}
		return vector;
	}

	public SteerForSphericalObstacleAvoidance.PathIntersection FindNextIntersectionWithSphere(SphericalObstacle obs, Vector3 line)
	{
		Vector3 vector = base.Vehicle.Position - obs.center;
		SteerForSphericalObstacleAvoidance.PathIntersection result = new SteerForSphericalObstacleAvoidance.PathIntersection(obs);
		obs.annotatePosition();
		Debug.DrawLine(base.Vehicle.Position, base.Vehicle.Position + line, Color.get_cyan());
		float sqrMagnitude = line.get_sqrMagnitude();
		float num = 2f * Vector3.Dot(line, vector);
		float num2 = obs.center.get_sqrMagnitude();
		num2 += base.Vehicle.Position.get_sqrMagnitude();
		num2 -= 2f * Vector3.Dot(obs.center, base.Vehicle.Position);
		num2 -= Mathf.Pow(obs.radius + base.Vehicle.ScaledRadius, 2f);
		float num3 = num * num - 4f * sqrMagnitude * num2;
		if (num3 >= 0f)
		{
			result.intersect = true;
			Vector3 vector2 = Vector3.get_zero();
			if (num3 == 0f)
			{
				float num4 = -num / (2f * sqrMagnitude);
				vector2 = num4 * line;
			}
			else
			{
				float num5 = (-num + Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
				float num6 = (-num - Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
				if (num5 < 0f && num6 < 0f)
				{
					result.intersect = false;
				}
				else
				{
					vector2 = ((Mathf.Abs(num5) >= Mathf.Abs(num6)) ? (num6 * line) : (num5 * line));
				}
			}
			Debug.DrawRay(base.Vehicle.Position, vector2, Color.get_red());
			result.distance = vector2.get_magnitude();
		}
		return result;
	}

	private void OnDrawGizmos()
	{
		if (base.Vehicle != null)
		{
			using (IEnumerator<Obstacle> enumerator = base.Vehicle.Radar.Obstacles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Obstacle current = enumerator.get_Current();
					SphericalObstacle sphericalObstacle = current as SphericalObstacle;
					Gizmos.set_color(Color.get_red());
					Gizmos.DrawWireSphere(sphericalObstacle.center, sphericalObstacle.radius);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySteer
{
	public class Obstacle
	{
		private static Dictionary<int, Obstacle> _obstacleCache;

		public static Dictionary<int, Obstacle> ObstacleCache
		{
			get
			{
				return Obstacle._obstacleCache;
			}
		}

		static Obstacle()
		{
			Obstacle._obstacleCache = new Dictionary<int, Obstacle>();
		}

		public virtual Vector3 steerToAvoid(SteeringVehicle v, float minTimeToCollision)
		{
			return Vector3.get_zero();
		}
	}
}

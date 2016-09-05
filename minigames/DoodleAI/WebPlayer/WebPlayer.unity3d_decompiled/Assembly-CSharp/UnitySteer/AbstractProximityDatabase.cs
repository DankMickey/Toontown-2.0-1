using System;
using UnityEngine;

namespace UnitySteer
{
	public class AbstractProximityDatabase
	{
		public virtual AbstractTokenForProximityDatabase allocateToken(SteeringVehicle parentObject)
		{
			return new AbstractTokenForProximityDatabase();
		}

		public virtual int getPopulation()
		{
			return 0;
		}

		public virtual SteeringVehicle getNearestVehicle(Vector3 position, float radius)
		{
			return null;
		}

		public virtual Vector3 getMostPopulatedBinCenter()
		{
			return Vector3.get_zero();
		}

		public virtual void RemoveToken(AbstractTokenForProximityDatabase token)
		{
		}
	}
}

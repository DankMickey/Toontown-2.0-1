using System;
using System.Collections;

namespace UnitySteer
{
	public class BruteForceProximityDatabase : AbstractProximityDatabase
	{
		public ArrayList group;

		public BruteForceProximityDatabase()
		{
			this.group = new ArrayList();
		}

		public override AbstractTokenForProximityDatabase allocateToken(SteeringVehicle parentObject)
		{
			return new tokenType(parentObject, this);
		}

		public override void RemoveToken(AbstractTokenForProximityDatabase token)
		{
			this.group.Remove(token);
		}

		public override int getPopulation()
		{
			return this.group.get_Count();
		}
	}
}

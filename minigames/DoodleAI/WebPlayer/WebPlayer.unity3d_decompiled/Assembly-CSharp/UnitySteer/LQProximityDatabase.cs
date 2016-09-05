using System;
using System.Collections;
using UnityEngine;

namespace UnitySteer
{
	public class LQProximityDatabase : AbstractProximityDatabase
	{
		public class tokenType : AbstractTokenForProximityDatabase
		{
			private lqClientProxy proxy;

			private locationQueryDatabase lq;

			public tokenType(object parentObject, LQProximityDatabase lqsd)
			{
				this.proxy = new lqClientProxy(parentObject);
				this.lq = lqsd.lq;
			}

			~tokenType()
			{
				this.lq.lqRemoveFromBin(this.proxy);
			}

			public override void updateForNewPosition(Vector3 p)
			{
				this.lq.lqUpdateForNewLocation(this.proxy, p.x, p.y, p.z);
			}

			public override void findNeighbors(Vector3 center, float radius, ArrayList results)
			{
				ArrayList allObjectsInLocality = this.lq.getAllObjectsInLocality(center.x, center.y, center.z, radius);
				for (int i = 0; i < allObjectsInLocality.get_Count(); i++)
				{
					lqClientProxy lqClientProxy = (lqClientProxy)allObjectsInLocality.get_Item(i);
					results.Add((SteeringVehicle)lqClientProxy.clientObject);
				}
			}
		}

		private locationQueryDatabase lq;

		public LQProximityDatabase(Vector3 center, Vector3 dimensions, Vector3 divisions)
		{
			Vector3 vector = dimensions * 0.5f;
			Vector3 vector2 = center - vector;
			this.lq = new locationQueryDatabase(vector2.x, vector2.y, vector2.z, dimensions.x, dimensions.y, dimensions.z, (int)Math.Round((double)divisions.x), (int)Math.Round((double)divisions.y), (int)Math.Round((double)divisions.z));
		}

		~LQProximityDatabase()
		{
		}

		public override AbstractTokenForProximityDatabase allocateToken(SteeringVehicle parentObject)
		{
			return new LQProximityDatabase.tokenType(parentObject, this);
		}

		public override int getPopulation()
		{
			return this.lq.getAllObjects().get_Count();
		}

		public override SteeringVehicle getNearestVehicle(Vector3 position, float radius)
		{
			lqClientProxy lqClientProxy = this.lq.lqFindNearestNeighborWithinRadius(position.x, position.y, position.z, radius, null);
			SteeringVehicle result = null;
			if (lqClientProxy != null)
			{
				result = (SteeringVehicle)lqClientProxy.clientObject;
			}
			return result;
		}

		public override Vector3 getMostPopulatedBinCenter()
		{
			return this.lq.getMostPopulatedBinCenter();
		}
	}
}

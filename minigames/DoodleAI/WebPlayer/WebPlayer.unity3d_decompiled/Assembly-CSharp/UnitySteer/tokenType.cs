using System;
using System.Collections;
using UnityEngine;

namespace UnitySteer
{
	public class tokenType : AbstractTokenForProximityDatabase
	{
		private BruteForceProximityDatabase bfpd;

		private SteeringVehicle tParentObject;

		private Vector3 position;

		public tokenType(SteeringVehicle parentObject, BruteForceProximityDatabase pd)
		{
			this.bfpd = pd;
			this.tParentObject = parentObject;
			this.bfpd.group.Add(this);
		}

		~tokenType()
		{
			this.bfpd.group.Remove(this);
		}

		public override void updateForNewPosition(Vector3 newPosition)
		{
			this.position = newPosition;
		}

		public override void findNeighbors(Vector3 center, float radius, ArrayList results)
		{
			float num = radius * radius;
			for (int i = 0; i < this.bfpd.group.get_Count(); i++)
			{
				tokenType tokenType = (tokenType)this.bfpd.group.get_Item(i);
				float sqrMagnitude = (center - tokenType.position).get_sqrMagnitude();
				if (sqrMagnitude < num)
				{
					results.Add(tokenType.tParentObject);
				}
			}
		}
	}
}

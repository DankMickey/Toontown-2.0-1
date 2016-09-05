using System;
using System.Collections;
using UnityEngine;

namespace UnitySteer
{
	public class AbstractTokenForProximityDatabase
	{
		public virtual void updateForNewPosition(Vector3 position)
		{
		}

		public virtual void findNeighbors(Vector3 center, float radius, ArrayList results)
		{
		}
	}
}

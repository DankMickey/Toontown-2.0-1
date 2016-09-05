using System;
using UnityEngine;

public class SteerForCohesion : SteerForNeighbors
{
	protected override Vector3 CalculateNeighborContribution(Vehicle other)
	{
		return other.Position;
	}
}

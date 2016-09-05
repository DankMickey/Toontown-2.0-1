using System;
using UnityEngine;

public class SteerForAlignment : SteerForNeighbors
{
	protected override Vector3 CalculateNeighborContribution(Vehicle other)
	{
		return other.get_transform().get_forward();
	}
}

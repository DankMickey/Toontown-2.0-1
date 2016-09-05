using System;
using UnityEngine;

public class SteerForSeparation : SteerForNeighbors
{
	protected override Vector3 CalculateNeighborContribution(Vehicle other)
	{
		Vector3 vector = other.Position - base.Vehicle.Position;
		float num = Vector3.Dot(vector, vector);
		return vector / -num;
	}
}

using System;
using UnityEngine;

[Serializable]
public class ShipCollderHit : MonoBehaviour
{
	public string which;

	public override void OnCollisionEnter(Collision collisionInfo)
	{
		MonoBehaviour.print("ShipCollider hit: collider name = " + collisionInfo.get_transform().get_name());
	}

	public override void Main()
	{
	}
}

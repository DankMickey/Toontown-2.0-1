using System;
using UnityEngine;

[Serializable]
public class WorldEdgeAddForceRight : MonoBehaviour
{
	public float pushBackForce;

	public override void OnCollisionEnter(Collision collisionInfo)
	{
		Rigidbody rigidbody = collisionInfo.get_rigidbody();
		if (rigidbody)
		{
			Vector3 vector = default(Vector3);
			vector = new Vector3((float)-1, (float)0, (float)0);
			rigidbody.AddForce(vector * this.pushBackForce, 1);
		}
	}

	public override void Main()
	{
	}
}

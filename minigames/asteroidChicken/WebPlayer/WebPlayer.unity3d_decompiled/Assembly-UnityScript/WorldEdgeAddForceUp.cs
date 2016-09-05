using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class WorldEdgeAddForceUp : MonoBehaviour
{
	public float pushBackForce;

	public override void OnCollisionEnter(Collision collisionInfo)
	{
		object component = collisionInfo.get_transform().GetComponent("Asteroid");
		if (RuntimeServices.EqualityOperator(component, null))
		{
			component = collisionInfo.get_transform().GetComponent("Asterpinata");
		}
		if (!RuntimeServices.EqualityOperator(component, null))
		{
			Object.Destroy((Object)RuntimeServices.Coerce(collisionInfo.get_gameObject(), typeof(Object)));
		}
		else
		{
			Rigidbody rigidbody = collisionInfo.get_rigidbody();
			if (rigidbody)
			{
				Vector3 vector = default(Vector3);
				vector = new Vector3((float)0, (float)1, 0.75f);
				rigidbody.AddForce(vector * this.pushBackForce, 1);
			}
		}
	}

	public override void Main()
	{
	}
}

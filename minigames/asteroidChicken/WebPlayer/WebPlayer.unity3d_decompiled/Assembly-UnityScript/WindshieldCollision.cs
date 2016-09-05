using System;
using UnityEngine;

[Serializable]
public class WindshieldCollision : MonoBehaviour
{
	public override void OnTriggerEnter(Collider other)
	{
		Asteroid asteroid = (Asteroid)other.get_transform().GetComponent(typeof(Asteroid));
	}

	public override void Main()
	{
	}
}

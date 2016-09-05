using System;
using UnityEngine;

[Serializable]
public class DaiserBarrel : MonoBehaviour
{
	public GameObject target;

	public override void Update()
	{
		(this.target.get_transform().get_position() - this.get_transform().get_position()).Normalize();
		this.get_transform().LookAt(this.target.get_transform());
	}

	public override void Main()
	{
	}
}

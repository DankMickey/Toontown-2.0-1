using System;
using UnityEngine;

[Serializable]
public class LookAt : MonoBehaviour
{
	public Transform target;

	public override void Update()
	{
		this.get_transform().LookAt(this.target);
	}

	public override void Main()
	{
	}
}

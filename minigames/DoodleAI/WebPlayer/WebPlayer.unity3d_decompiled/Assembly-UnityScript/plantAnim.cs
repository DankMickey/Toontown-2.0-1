using System;
using UnityEngine;

[Serializable]
public class plantAnim : MonoBehaviour
{
	public override void Update()
	{
		this.get_animation().Play("idle");
	}

	public override void Main()
	{
	}
}

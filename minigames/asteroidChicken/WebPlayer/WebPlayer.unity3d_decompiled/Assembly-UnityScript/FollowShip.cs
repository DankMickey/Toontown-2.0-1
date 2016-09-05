using System;
using UnityEngine;

[Serializable]
public class FollowShip : MonoBehaviour
{
	public Ship follow;

	public override void LateUpdate()
	{
		if (!this.follow)
		{
			this.follow = (Ship)Object.FindObjectOfType(typeof(Ship));
		}
		this.get_transform().set_position(new Vector3(this.follow.get_transform().get_position().x + 0.1f, this.follow.get_transform().get_position().x + 0.1f, this.follow.get_transform().get_position().z - 0.667f));
	}

	public override void Main()
	{
	}
}

using System;
using UnityEngine;

[Serializable]
public class PenWallControl : MonoBehaviour
{
	public override void Update()
	{
	}

	public override void SetMiddleRing()
	{
		this.get_audio().Play();
		this.get_animation().get_Item("middleRing").set_speed((float)1);
		this.get_animation().Play("middleRing");
	}

	public override void SetInnerRing()
	{
		this.get_audio().Play();
		this.get_animation().get_Item("innerRing").set_speed(0.25f);
		this.get_animation().Play("innerRing");
	}

	public override void Main()
	{
	}
}

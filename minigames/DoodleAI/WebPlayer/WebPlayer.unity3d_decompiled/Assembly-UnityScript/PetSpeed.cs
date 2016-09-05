using System;
using UnityEngine;

[Serializable]
public class PetSpeed : MonoBehaviour
{
	public AnimationClip anim;

	public override void Start()
	{
		this.get_animation().set_wrapMode(2);
		this.get_animation().Play(this.anim.get_name());
	}

	public override void Main()
	{
	}
}

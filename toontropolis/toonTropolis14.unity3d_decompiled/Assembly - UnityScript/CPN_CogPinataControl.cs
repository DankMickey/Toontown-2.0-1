using System;
using UnityEngine;

[Serializable]
public class CPN_CogPinataControl : MonoBehaviour
{
	private bool Awake;

	public void Start()
	{
		this.get_animation().Play("idle0");
		this.Awake = true;
	}

	public void ToggleSleepWake()
	{
		if (this.Awake)
		{
			this.Awake = false;
			this.get_animation().CrossFade("layFlat");
		}
		else
		{
			this.Awake = true;
			this.get_animation().Stop("layFlat");
			this.get_animation().Play("idle0");
		}
	}

	public void AnvilDrop(object delay)
	{
		this.get_animation().get_Item("anvilDrop3").set_layer(11);
		this.get_animation().get_Item("anvilDrop3").set_blendMode(0);
		this.get_animation().CrossFade("anvilDrop3");
	}

	public void HonkHorn(object delay)
	{
		this.get_animation().get_Item("honkHorn").set_layer(11);
		this.get_animation().get_Item("honkHorn").set_blendMode(0);
		this.get_animation().CrossFade("honkHorn");
	}

	public void Main()
	{
	}
}

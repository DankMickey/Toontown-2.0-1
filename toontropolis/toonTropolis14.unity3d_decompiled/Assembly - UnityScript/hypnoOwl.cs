using System;
using UnityEngine;

[Serializable]
public class hypnoOwl : MonoBehaviour
{
	public void Start()
	{
		this.get_animation().set_wrapMode(2);
		this.get_animation().get_Item("hypnoEyes").set_layer(9);
		this.get_animation().get_Item("hypnoEyes").set_blendMode(0);
		this.get_animation().CrossFade("hypnoEyes");
	}

	public void Main()
	{
	}
}

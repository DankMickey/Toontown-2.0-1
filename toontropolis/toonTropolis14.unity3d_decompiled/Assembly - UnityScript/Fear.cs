using System;
using UnityEngine;

[Serializable]
public class Fear : MonoBehaviour
{
	public AnimationClip idle;

	public AnimationClip afraid;

	public AnimationClip drop;

	public void Update()
	{
		if (!triggerCogBuilding.fear)
		{
			this.get_animation().Play(this.idle.get_name());
		}
		if (triggerCogBuilding.fear)
		{
			this.get_animation().Stop(this.idle.get_name());
			this.get_animation().set_wrapMode(2);
			this.get_animation().Play(this.afraid.get_name());
		}
	}

	public void Main()
	{
	}
}

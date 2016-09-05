using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class triggerCogBuilding : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class OnTriggerEnter$40 : GenericGenerator<WaitForSeconds>
	{
		internal Collider $other197;

		internal triggerCogBuilding $self_198;

		public OnTriggerEnter$40(Collider other, triggerCogBuilding self_)
		{
			this.$other197 = other;
			this.$self_198 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new triggerCogBuilding.OnTriggerEnter$40.$(this.$other197, this.$self_198);
		}
	}

	public GameObject playerChar;

	public Transform launchPos;

	public GameObject cogBuilding;

	public AnimationClip buildingIdle;

	public AnimationClip animBuildingDamage;

	public GameObject cogA;

	public GameObject cogB;

	public GameObject cogC;

	public GameObject cogD;

	public GameObject cogE;

	public GameObject bombA;

	public GameObject bombB;

	public GameObject haze;

	public AudioClip bombSound;

	public AudioClip villianMusic;

	public static bool fear = false;

	public bool cogTakerover;

	public triggerCogBuilding()
	{
		this.cogTakerover = false;
	}

	public void Start()
	{
		this.get_animation().set_wrapMode(2);
		this.get_animation().Play(this.buildingIdle.get_name());
		if (!this.playerChar)
		{
			this.playerChar = GameObject.FindWithTag("Player");
		}
	}

	public IEnumerator OnTriggerEnter(Collider other)
	{
		return new triggerCogBuilding.OnTriggerEnter$40(other, this).GetEnumerator();
	}

	public void Main()
	{
	}
}

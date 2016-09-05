using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[Serializable]
public class Fire : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Start$69 : GenericGenerator<WaitForSeconds>
	{
		internal Fire $self_$71;

		public $Start$69(Fire self_)
		{
			this.$self_$71 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new Fire.$Start$69.$(this.$self_$71);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class $OnTriggerEnter$72 : GenericGenerator<WaitForSeconds>
	{
		internal Collider $col$76;

		internal Fire $self_$77;

		public $OnTriggerEnter$72(Collider col, Fire self_)
		{
			this.$col$76 = col;
			this.$self_$77 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new Fire.$OnTriggerEnter$72.$(this.$col$76, this.$self_$77);
		}
	}

	public AudioClip blastOff;

	public Rigidbody projectile;

	public GameObject launcher;

	public float speed;

	public float stagger;

	public Fire()
	{
		this.speed = 100f;
	}

	public override IEnumerator Start()
	{
		return new Fire.$Start$69(this).GetEnumerator();
	}

	public override IEnumerator OnTriggerEnter(Collider col)
	{
		return new Fire.$OnTriggerEnter$72(col, this).GetEnumerator();
	}

	public override void Main()
	{
	}
}

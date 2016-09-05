using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GS_PlayerTriggerActions : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class OnTriggerEnter$63 : GenericGenerator<WaitForSeconds>
	{
		internal Collider $other221;

		internal GS_PlayerTriggerActions $self_222;

		public OnTriggerEnter$63(Collider other, GS_PlayerTriggerActions self_)
		{
			this.$other221 = other;
			this.$self_222 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_PlayerTriggerActions.OnTriggerEnter$63.$(this.$other221, this.$self_222);
		}
	}

	public IEnumerator OnTriggerEnter(Collider other)
	{
		return new GS_PlayerTriggerActions.OnTriggerEnter$63(other, this).GetEnumerator();
	}

	public void Update()
	{
	}

	public void Main()
	{
	}
}

using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[Serializable]
public class JetPackParticleController : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Start$47 : GenericGenerator<object>
	{
		internal JetPackParticleController $self_$61;

		public $Start$47(JetPackParticleController self_)
		{
			this.$self_$61 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new JetPackParticleController.$Start$47.$(this.$self_$61);
		}
	}

	private float litAmount;

	public override IEnumerator Start()
	{
		return new JetPackParticleController.$Start$47(this).GetEnumerator();
	}

	public override void Main()
	{
	}
}

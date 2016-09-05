using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class cogdoAanim : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class Start$50 : GenericGenerator<WaitForSeconds>
	{
		internal cogdoAanim $self_194;

		public Start$50(cogdoAanim self_)
		{
			this.$self_194 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogdoAanim.Start$50.$(this.$self_194);
		}
	}

	public AnimationClip idleAnim;

	public AnimationClip dropAnim;

	public IEnumerator Start()
	{
		return new cogdoAanim.Start$50(this).GetEnumerator();
	}

	public void Main()
	{
	}
}

using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MUS_PlayCymbal : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class PlayCymbal$66 : GenericGenerator<WaitForSeconds>
	{
		internal object $which245;

		internal MUS_PlayCymbal $self_246;

		public PlayCymbal$66(object which, MUS_PlayCymbal self_)
		{
			this.$which245 = which;
			this.$self_246 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new MUS_PlayCymbal.PlayCymbal$66.$(this.$which245, this.$self_246);
		}
	}

	private int[] playNTimes;

	public IEnumerator PlayCymbal(object which)
	{
		return new MUS_PlayCymbal.PlayCymbal$66(which, this).GetEnumerator();
	}

	public void Main()
	{
	}
}

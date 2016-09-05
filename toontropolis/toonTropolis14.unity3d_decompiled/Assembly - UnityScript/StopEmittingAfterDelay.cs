using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class StopEmittingAfterDelay : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class Main$57 : GenericGenerator<WaitForSeconds>
	{
		internal StopEmittingAfterDelay $self_210;

		public Main$57(StopEmittingAfterDelay self_)
		{
			this.$self_210 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new StopEmittingAfterDelay.Main$57.$(this.$self_210);
		}
	}

	public float delay;

	public StopEmittingAfterDelay()
	{
		this.delay = 0.1f;
	}

	public IEnumerator Main()
	{
		return new StopEmittingAfterDelay.Main$57(this).GetEnumerator();
	}
}

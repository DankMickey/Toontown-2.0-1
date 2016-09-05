using System;
using UnityEngine;

namespace UnitySteer.Helpers
{
	[Serializable]
	public class Tick
	{
		private float _nextTick;

		[SerializeField]
		private float _randomRangeMin;

		[SerializeField]
		private float _randomRangeMax;

		[SerializeField]
		private float _tickLapse = 0.1f;

		public float Priority;

		public float NextTick
		{
			get
			{
				return this._nextTick;
			}
		}

		public float RandomRangeMax
		{
			get
			{
				return this._randomRangeMax;
			}
			set
			{
				this._randomRangeMax = Mathf.Max(value, this._randomRangeMin);
			}
		}

		public float RandomRangeMin
		{
			get
			{
				return this._randomRangeMin;
			}
			set
			{
				this._randomRangeMin = Mathf.Min(value, this._randomRangeMax);
			}
		}

		public float TickLapse
		{
			get
			{
				return this._tickLapse;
			}
			set
			{
				this._tickLapse = Mathf.Max(value, 0f);
			}
		}

		public Tick() : this(0.1f)
		{
		}

		public Tick(float tickLapse)
		{
			this.TickLapse = tickLapse;
		}

		public bool ShouldTick()
		{
			return this.ShouldTick(this.TickLapse);
		}

		public bool ShouldTick(float nextTickLapseOverride)
		{
			float time = Time.get_time();
			bool flag = this._nextTick < time;
			if (flag)
			{
				this._nextTick = time + nextTickLapseOverride + Random.Range(this._randomRangeMin, this._randomRangeMax);
			}
			return flag;
		}
	}
}

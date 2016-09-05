using System;

namespace UnitySteer
{
	public class lqClientProxy
	{
		public lqBin bin;

		public object clientObject;

		public float x;

		public float y;

		public float z;

		public lqClientProxy(object tClientObject)
		{
			this.clientObject = tClientObject;
			this.x = 0f;
			this.y = 0f;
			this.z = 0f;
		}
	}
}

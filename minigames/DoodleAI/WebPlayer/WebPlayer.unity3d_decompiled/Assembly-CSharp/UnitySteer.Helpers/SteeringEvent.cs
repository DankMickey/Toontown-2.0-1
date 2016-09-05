using System;

namespace UnitySteer.Helpers
{
	public class SteeringEvent<T>
	{
		private Steering _sender;

		private string _action;

		private T _parameter;

		public string Action
		{
			get
			{
				return this._action;
			}
			set
			{
				this._action = value;
			}
		}

		public T Parameter
		{
			get
			{
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		public Steering Sender
		{
			get
			{
				return this._sender;
			}
			set
			{
				this._sender = value;
			}
		}

		public SteeringEvent(Steering sender, string action, T parameter)
		{
			this._sender = sender;
			this._action = action;
			this._parameter = parameter;
		}
	}
}

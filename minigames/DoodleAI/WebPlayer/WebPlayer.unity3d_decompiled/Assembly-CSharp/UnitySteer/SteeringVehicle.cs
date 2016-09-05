using System;
using UnityEngine;

namespace UnitySteer
{
	public class SteeringVehicle
	{
		private Transform transform;

		private Rigidbody rigidbody;

		private float internalMass;

		private float radius;

		private float speed;

		private float maxSpeed;

		private float maxForce;

		private bool movesVertically = true;

		private Vector3 internalPosition;

		private Vector3 internalSide;

		private Vector3 internalForward;

		private Vector3 internalUp;

		public Vector3 Position
		{
			get
			{
				if (this.rigidbody != null)
				{
					return this.rigidbody.get_position();
				}
				if (this.transform != null)
				{
					return this.transform.get_position();
				}
				return this.internalPosition;
			}
			set
			{
				if (!this.MovesVertically)
				{
					value.y = this.Position.y;
				}
				if (this.rigidbody != null)
				{
					this.rigidbody.MovePosition(value);
					return;
				}
				if (this.transform != null)
				{
					this.transform.set_position(value);
				}
				else
				{
					this.internalPosition = value;
				}
			}
		}

		public Vector3 Forward
		{
			get
			{
				if (this.rigidbody != null)
				{
					return this.rigidbody.get_transform().get_forward();
				}
				if (this.transform != null)
				{
					return this.transform.get_forward();
				}
				return this.internalForward;
			}
			set
			{
				if (!this.MovesVertically)
				{
					value = new Vector3(value.x, this.Forward.y, value.z);
				}
				if (this.rigidbody != null)
				{
					this.rigidbody.get_transform().set_forward(value);
				}
				else if (this.transform != null)
				{
					this.transform.set_forward(value);
				}
				else
				{
					this.internalForward = value;
					this.RecalculateSide();
				}
			}
		}

		public Vector3 Side
		{
			get
			{
				if (this.rigidbody != null)
				{
					return this.rigidbody.get_transform().get_right();
				}
				if (this.transform != null)
				{
					return this.transform.get_right();
				}
				return this.internalSide;
			}
		}

		public Vector3 Up
		{
			get
			{
				if (this.rigidbody != null)
				{
					return this.rigidbody.get_transform().get_up();
				}
				if (this.transform != null)
				{
					return this.transform.get_up();
				}
				return this.internalUp;
			}
			set
			{
				if (this.rigidbody != null)
				{
					this.rigidbody.get_transform().set_up(value);
				}
				else if (this.transform != null)
				{
					this.transform.set_up(value);
				}
				else
				{
					this.internalUp = value;
					this.RecalculateSide();
				}
			}
		}

		public float Mass
		{
			get
			{
				if (this.rigidbody != null)
				{
					return this.rigidbody.get_mass();
				}
				return this.internalMass;
			}
			set
			{
				if (this.rigidbody != null)
				{
					this.rigidbody.set_mass(value);
				}
				else
				{
					this.internalMass = value;
				}
			}
		}

		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		public float MaxSpeed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
			}
		}

		public float MaxForce
		{
			get
			{
				return this.maxForce;
			}
			set
			{
				this.maxForce = value;
			}
		}

		public bool MovesVertically
		{
			get
			{
				return this.movesVertically;
			}
			set
			{
				this.movesVertically = value;
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return this.Forward * this.speed;
			}
		}

		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		protected Transform Transform
		{
			get
			{
				return (!(this.rigidbody != null)) ? this.transform : this.rigidbody.get_transform();
			}
		}

		protected GameObject GameObject
		{
			get
			{
				return (!(this.rigidbody != null)) ? this.transform.get_gameObject() : this.rigidbody.get_gameObject();
			}
		}

		public SteeringVehicle(Vector3 position, float mass)
		{
			this.Position = position;
			this.internalMass = mass;
		}

		public SteeringVehicle(Transform transform, float mass)
		{
			this.transform = transform;
			this.internalMass = mass;
		}

		public SteeringVehicle(Rigidbody rigidbody)
		{
			this.rigidbody = rigidbody;
		}

		public virtual Vector3 predictFuturePosition(float predictionTime)
		{
			return Vector3.get_zero();
		}

		private void RecalculateSide()
		{
			this.internalSide = Vector3.Cross(this.internalForward, this.internalUp);
			this.internalSide.Normalize();
		}
	}
}

using System;
using System.Collections;
using UnityEngine;

namespace UnitySteer
{
	public class SteerLibrary : SteeringVehicle
	{
		public SteerLibrary(Vector3 position, float mass) : base(position, mass)
		{
		}

		public SteerLibrary(Transform transform, float mass) : base(transform, mass)
		{
		}

		public SteerLibrary(Rigidbody rigidbody) : base(rigidbody)
		{
		}

		public void resetSteering()
		{
		}

		private bool isAhead(Vector3 target)
		{
			return this.isAhead(target, 0.707f);
		}

		private bool isAside(Vector3 target)
		{
			return this.isAside(target, 0.707f);
		}

		private bool isBehind(Vector3 target)
		{
			return this.isBehind(target, -0.707f);
		}

		private bool isAhead(Vector3 target, float cosThreshold)
		{
			Vector3 vector = target - base.Position;
			vector.Normalize();
			return Vector3.Dot(base.Forward, vector) > cosThreshold;
		}

		private bool isAside(Vector3 target, float cosThreshold)
		{
			Vector3 vector = target - base.Position;
			vector.Normalize();
			float num = Vector3.Dot(base.Forward, vector);
			return num < cosThreshold && num > -cosThreshold;
		}

		private bool isBehind(Vector3 target, float cosThreshold)
		{
			Vector3 vector = target - base.Position;
			vector.Normalize();
			return Vector3.Dot(base.Forward, vector) < cosThreshold;
		}

		public virtual void annotatePathFollowing(Vector3 future, Vector3 onPath, Vector3 target, float outside)
		{
			Debug.DrawLine(base.Position, future, Color.get_white());
			Debug.DrawLine(base.Position, onPath, Color.get_yellow());
			Debug.DrawLine(base.Position, target, Color.get_magenta());
		}

		public virtual void annotateAvoidCloseNeighbor(SteeringVehicle otherVehicle, Vector3 component)
		{
			Debug.DrawLine(base.Position, otherVehicle.Position, Color.get_red());
			Debug.DrawRay(base.Position, component * 3f, Color.get_yellow());
		}

		public Vector3 steerForSeek(Vector3 target)
		{
			Vector3 vector = target - base.Position;
			return vector - base.Velocity;
		}

		public Vector3 steerToStayOnPath(float predictionTime, Pathway path)
		{
			Vector3 vector = this.predictFuturePosition(predictionTime);
			mapReturnStruct mapReturnStruct = default(mapReturnStruct);
			Vector3 vector2 = path.mapPointToPath(vector, ref mapReturnStruct);
			if (mapReturnStruct.outside < 0f)
			{
				return Vector3.get_zero();
			}
			this.annotatePathFollowing(vector, vector2, vector2, mapReturnStruct.outside);
			return this.steerForSeek(vector2);
		}

		public Vector3 steerToAvoidObstacle(float minTimeToCollision, Obstacle obstacle)
		{
			return obstacle.steerToAvoid(this, minTimeToCollision);
		}

		public Vector3 steerToAvoidCloseNeighbors(float minSeparationDistance, ArrayList others)
		{
			Vector3 vector = Vector3.get_zero();
			for (int i = 0; i < others.get_Count(); i++)
			{
				SteeringVehicle steeringVehicle = (SteeringVehicle)others.get_Item(i);
				if (steeringVehicle != this)
				{
					float num = base.Radius + steeringVehicle.Radius;
					float num2 = minSeparationDistance + num;
					Vector3 vector2 = steeringVehicle.Position - base.Position;
					float magnitude = vector2.get_magnitude();
					if (magnitude < num2)
					{
						vector = OpenSteerUtility.perpendicularComponent(-vector2, base.Forward);
						this.annotateAvoidCloseNeighbor(steeringVehicle, vector);
					}
				}
			}
			return vector;
		}
	}
}

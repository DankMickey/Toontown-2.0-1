using System;
using UnityEngine;

namespace UnitySteer
{
	public class SimpleVehicle : SteerLibrary
	{
		private float _curvature;

		private Vector3 _lastForward;

		private Vector3 _lastPosition;

		private Vector3 _smoothedPosition;

		private float _smoothedCurvature;

		private Vector3 _smoothedAcceleration;

		public int SerialNumber
		{
			get
			{
				return base.GameObject.GetInstanceID();
			}
		}

		public SimpleVehicle(Vector3 position, float mass) : base(position, mass)
		{
			this.reset();
		}

		public SimpleVehicle(Transform transform, float mass) : base(transform, mass)
		{
			this.reset();
		}

		public SimpleVehicle(Rigidbody rigidbody) : base(rigidbody)
		{
			this.reset();
		}

		public virtual void reset()
		{
			base.resetSteering();
			base.Speed = 0f;
			this.resetSmoothedPosition(Vector3.get_zero());
			this.resetSmoothedCurvature(0f);
			this.resetSmoothedAcceleration(Vector3.get_zero());
		}

		private float curvature()
		{
			return this._curvature;
		}

		private float smoothedCurvature()
		{
			return this._smoothedCurvature;
		}

		private float resetSmoothedCurvature(float value)
		{
			this._lastForward = Vector3.get_zero();
			this._lastPosition = Vector3.get_zero();
			this._curvature = value;
			this._smoothedCurvature = value;
			return value;
		}

		private Vector3 smoothedAcceleration()
		{
			return this._smoothedAcceleration;
		}

		private Vector3 resetSmoothedAcceleration(Vector3 value)
		{
			this._smoothedAcceleration = value;
			return value;
		}

		private Vector3 smoothedPosition()
		{
			return this._smoothedPosition;
		}

		private Vector3 resetSmoothedPosition(Vector3 value)
		{
			this._smoothedPosition = value;
			return value;
		}

		public virtual Vector3 AdjustRawSteeringForce(Vector3 force)
		{
			float num = 0.2f * base.MaxSpeed;
			if (base.Speed > num || force == Vector3.get_zero())
			{
				return force;
			}
			float num2 = base.Speed / num;
			float cosineOfConeAngle = Mathf.Lerp(1f, -1f, Mathf.Pow(num2, 20f));
			return OpenSteerUtility.limitMaxDeviationAngle(force, cosineOfConeAngle, base.Forward);
		}

		private void applyBrakingForce(float rate, float elapsedTime)
		{
			float num = base.Speed * rate;
			float num2 = (num >= base.MaxForce) ? base.MaxForce : num;
			base.Speed -= num2 * elapsedTime;
		}

		public void regenerateLocalSpaceForBanking(Vector3 newVelocity, float elapsedTime)
		{
			Vector3 vector = new Vector3(0f, 0.2f, 0f);
			Vector3 vector2 = this._smoothedAcceleration * 0.05f;
			Vector3 newValue = vector2 + vector;
			float smoothRate = elapsedTime * 3f;
			Vector3 vector3 = base.Up;
			vector3 = OpenSteerUtility.blendIntoAccumulator(smoothRate, newValue, vector3);
			vector3.Normalize();
			base.Up = vector3;
			if (base.Speed > 0f)
			{
				base.Forward = newVelocity / base.Speed;
			}
		}

		private void measurePathCurvature(float elapsedTime)
		{
			if (elapsedTime > 0f)
			{
				Vector3 position = base.Position;
				Vector3 forward = base.Forward;
				Vector3 vector = this._lastPosition - position;
				Vector3 source = (this._lastForward - forward) / vector.get_magnitude();
				Vector3 vector2 = OpenSteerUtility.perpendicularComponent(source, forward);
				float num = (Vector3.Dot(vector2, base.Side) >= 0f) ? -1f : 1f;
				this._curvature = vector2.get_magnitude() * num;
				this._smoothedCurvature = OpenSteerUtility.blendIntoAccumulator(elapsedTime * 4f, this._curvature, this._smoothedCurvature);
				this._lastForward = forward;
				this._lastPosition = position;
			}
		}

		public override Vector3 predictFuturePosition(float predictionTime)
		{
			return base.Position + base.Velocity * predictionTime;
		}
	}
}

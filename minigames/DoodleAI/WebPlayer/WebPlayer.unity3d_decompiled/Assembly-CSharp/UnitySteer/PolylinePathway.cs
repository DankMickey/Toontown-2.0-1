using System;
using UnityEngine;

namespace UnitySteer
{
	public class PolylinePathway : Pathway
	{
		private int pointCount;

		private Vector3[] points;

		private float radius;

		private float segmentLength;

		private float segmentProjection;

		private Vector3 chosen;

		private Vector3 segmentNormal;

		private float[] lengths;

		private Vector3[] normals;

		private float totalPathLength;

		private PolylinePathway()
		{
		}

		private PolylinePathway(Vector3[] _points, float _radius, bool _cyclic)
		{
			this.initialize(_points, _radius, _cyclic);
		}

		private void initialize(Vector3[] _points, float _radius, bool _cyclic)
		{
			this.radius = _radius;
			this.isCyclic = _cyclic;
			this.pointCount = _points.Length;
			this.totalPathLength = 0f;
			if (this.isCyclic)
			{
				this.pointCount++;
			}
			this.lengths = new float[this.pointCount];
			this.points = new Vector3[this.pointCount];
			this.normals = new Vector3[this.pointCount];
			for (int i = 0; i < _points.Length; i++)
			{
				bool flag = this.isCyclic && i == this.pointCount - 1;
				int num = (!flag) ? i : 0;
				this.points[i] = _points[num];
				if (i > 0)
				{
					this.normals[i] = this.points[i] - this.points[i - 1];
					this.lengths[i] = this.normals[i].get_magnitude();
					this.normals[i] *= 1f / this.lengths[i];
					this.totalPathLength += this.lengths[i];
				}
			}
		}

		protected override float GetTotalPathLength()
		{
			return this.totalPathLength;
		}

		public override Vector3 mapPointToPath(Vector3 point, ref mapReturnStruct tStruct)
		{
			float num = 3.40282347E+38f;
			Vector3 zero = Vector3.get_zero();
			for (int i = 1; i < this.pointCount; i++)
			{
				this.segmentLength = this.lengths[i];
				this.segmentNormal = this.normals[i];
				float num2 = this.pointToSegmentDistance(point, this.points[i - 1], this.points[i]);
				if (num2 < num)
				{
					num = num2;
					zero = this.chosen;
					tStruct.tangent = this.segmentNormal;
				}
			}
			tStruct.outside = (zero - point).get_magnitude() - this.radius;
			return zero;
		}

		public override float mapPointToPathDistance(Vector3 point)
		{
			float num = 3.40282347E+38f;
			float num2 = 0f;
			float result = 0f;
			for (int i = 1; i < this.pointCount; i++)
			{
				this.segmentLength = this.lengths[i];
				this.segmentNormal = this.normals[i];
				float num3 = this.pointToSegmentDistance(point, this.points[i - 1], this.points[i]);
				if (num3 < num)
				{
					num = num3;
					result = num2 + this.segmentProjection;
				}
				num2 += this.segmentLength;
			}
			return result;
		}

		public override Vector3 mapPathDistanceToPoint(float pathDistance)
		{
			float num = pathDistance;
			if (this.isCyclic)
			{
				num = (float)Math.IEEERemainder((double)pathDistance, (double)this.totalPathLength);
			}
			else
			{
				if (pathDistance < 0f)
				{
					return this.points[0];
				}
				if (pathDistance >= this.totalPathLength)
				{
					return this.points[this.pointCount - 1];
				}
			}
			Vector3 result = Vector3.get_zero();
			for (int i = 1; i < this.pointCount; i++)
			{
				this.segmentLength = this.lengths[i];
				if (this.segmentLength >= num)
				{
					float num2 = num / this.segmentLength;
					result = Vector3.Lerp(this.points[i - 1], this.points[i], num2);
					break;
				}
				num -= this.segmentLength;
			}
			return result;
		}

		private float pointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1)
		{
			Vector3 vector = point - ep0;
			this.segmentProjection = Vector3.Dot(this.segmentNormal, vector);
			if (this.segmentProjection < 0f)
			{
				this.chosen = ep0;
				this.segmentProjection = 0f;
				return (point - ep0).get_magnitude();
			}
			if (this.segmentProjection > this.segmentLength)
			{
				this.chosen = ep1;
				this.segmentProjection = this.segmentLength;
				return (point - ep1).get_magnitude();
			}
			this.chosen = this.segmentNormal * this.segmentProjection;
			this.chosen += ep0;
			return Vector3.Distance(point, this.chosen);
		}
	}
}

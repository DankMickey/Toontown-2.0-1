using System;
using UnityEngine;

namespace UnitySteer
{
	public class OpenSteerUtility
	{
		public static Vector3 RandomUnitVectorOnXZPlane()
		{
			Vector3 insideUnitSphere = Random.get_insideUnitSphere();
			insideUnitSphere.y = 0f;
			insideUnitSphere.Normalize();
			return insideUnitSphere;
		}

		public static Vector3 limitMaxDeviationAngle(Vector3 source, float cosineOfConeAngle, Vector3 basis)
		{
			return OpenSteerUtility.vecLimitDeviationAngleUtility(true, source, cosineOfConeAngle, basis);
		}

		public static Vector3 vecLimitDeviationAngleUtility(bool insideOrOutside, Vector3 source, float cosineOfConeAngle, Vector3 basis)
		{
			float magnitude = source.get_magnitude();
			if (magnitude == 0f)
			{
				return source;
			}
			Vector3 vector = source / magnitude;
			float num = Vector3.Dot(vector, basis);
			if (insideOrOutside)
			{
				if (num >= cosineOfConeAngle)
				{
					return source;
				}
			}
			else if (num <= cosineOfConeAngle)
			{
				return source;
			}
			Vector3 vector2 = OpenSteerUtility.perpendicularComponent(source, basis);
			float num2 = (float)Math.Sqrt((double)(1f - cosineOfConeAngle * cosineOfConeAngle));
			Vector3 vector3 = basis * cosineOfConeAngle;
			Vector3 vector4 = vector2.get_normalized() * num2;
			return (vector3 + vector4) * magnitude;
		}

		public static Vector3 parallelComponent(Vector3 source, Vector3 unitBasis)
		{
			float num = Vector3.Dot(source, unitBasis);
			return unitBasis * num;
		}

		public static Vector3 perpendicularComponent(Vector3 source, Vector3 unitBasis)
		{
			return source - OpenSteerUtility.parallelComponent(source, unitBasis);
		}

		public static Vector3 blendIntoAccumulator(float smoothRate, Vector3 newValue, Vector3 smoothedAccumulator)
		{
			return Vector3.Lerp(smoothedAccumulator, newValue, Mathf.Clamp(smoothRate, 0f, 1f));
		}

		public static float blendIntoAccumulator(float smoothRate, float newValue, float smoothedAccumulator)
		{
			return Mathf.Lerp(smoothedAccumulator, newValue, Mathf.Clamp(smoothRate, 0f, 1f));
		}

		public static Vector3 sphericalWrapAround(Vector3 source, Vector3 center, float radius)
		{
			Vector3 vector = source - center;
			float magnitude = vector.get_magnitude();
			if (magnitude > radius)
			{
				return source + vector / magnitude * radius * -2f;
			}
			return source;
		}

		public static float scalarRandomWalk(float initial, float walkspeed, float min, float max)
		{
			float num = initial + (Random.get_value() * 2f - 1f) * walkspeed;
			return Mathf.Clamp(num, min, max);
		}

		public static int intervalComparison(float x, float lowerBound, float upperBound)
		{
			if (x < lowerBound)
			{
				return -1;
			}
			if (x > upperBound)
			{
				return 1;
			}
			return 0;
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, ref float segmentProjection)
		{
			Vector3 zero = Vector3.get_zero();
			return OpenSteerUtility.PointToSegmentDistance(point, ep0, ep1, ref zero, ref segmentProjection);
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, ref Vector3 chosenPoint)
		{
			float num = 0f;
			return OpenSteerUtility.PointToSegmentDistance(point, ep0, ep1, ref chosenPoint, ref num);
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, ref Vector3 chosenPoint, ref float segmentProjection)
		{
			Vector3 vector = ep1 - ep0;
			float magnitude = vector.get_magnitude();
			vector *= 1f / magnitude;
			return OpenSteerUtility.PointToSegmentDistance(point, ep0, ep1, vector, magnitude, ref chosenPoint, ref segmentProjection);
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, Vector3 segmentNormal, float segmentLength, ref float segmentProjection)
		{
			Vector3 zero = Vector3.get_zero();
			return OpenSteerUtility.PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength, ref zero, ref segmentProjection);
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, Vector3 segmentNormal, float segmentLength, ref Vector3 chosenPoint)
		{
			float num = 0f;
			return OpenSteerUtility.PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength, ref chosenPoint, ref num);
		}

		public static float PointToSegmentDistance(Vector3 point, Vector3 ep0, Vector3 ep1, Vector3 segmentNormal, float segmentLength, ref Vector3 chosenPoint, ref float segmentProjection)
		{
			Vector3 vector = point - ep0;
			segmentProjection = Vector3.Dot(segmentNormal, vector);
			if (segmentProjection < 0f)
			{
				chosenPoint = ep0;
				segmentProjection = 0f;
				return (point - ep0).get_magnitude();
			}
			if (segmentProjection > segmentLength)
			{
				chosenPoint = ep1;
				segmentProjection = segmentLength;
				return (point - ep1).get_magnitude();
			}
			chosenPoint = segmentNormal * segmentProjection;
			chosenPoint += ep0;
			return Vector3.Distance(point, chosenPoint);
		}

		public static float CosFromDegrees(float angle)
		{
			return Mathf.Cos(angle * 0.0174532924f);
		}

		public static float DegreesFromCos(float cos)
		{
			return 57.29578f * Mathf.Acos(cos);
		}
	}
}

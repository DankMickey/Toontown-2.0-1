using System;
using UnityEngine;

namespace UnitySteer
{
	public class Pathway
	{
		protected bool isCyclic;

		public bool IsCyclic
		{
			get
			{
				return this.isCyclic;
			}
		}

		public float TotalPathLength
		{
			get
			{
				return this.GetTotalPathLength();
			}
		}

		public Vector3 FirstPoint
		{
			get
			{
				return this.GetFirstPoint();
			}
		}

		public Vector3 LastPoint
		{
			get
			{
				return this.GetLastPoint();
			}
		}

		protected virtual float GetTotalPathLength()
		{
			return 0f;
		}

		protected virtual Vector3 GetFirstPoint()
		{
			return Vector3.get_zero();
		}

		protected virtual Vector3 GetLastPoint()
		{
			return Vector3.get_zero();
		}

		public virtual Vector3 mapPointToPath(Vector3 point, ref mapReturnStruct tStruct)
		{
			return Vector3.get_zero();
		}

		public virtual Vector3 mapPathDistanceToPoint(float pathDistance)
		{
			return Vector3.get_zero();
		}

		public virtual float mapPointToPathDistance(Vector3 point)
		{
			return 0f;
		}

		public bool isInsidePath(Vector3 point)
		{
			mapReturnStruct mapReturnStruct = default(mapReturnStruct);
			this.mapPointToPath(point, ref mapReturnStruct);
			return mapReturnStruct.outside < 0f;
		}

		public float howFarOutsidePath(Vector3 point)
		{
			mapReturnStruct mapReturnStruct = default(mapReturnStruct);
			this.mapPointToPath(point, ref mapReturnStruct);
			return mapReturnStruct.outside;
		}
	}
}

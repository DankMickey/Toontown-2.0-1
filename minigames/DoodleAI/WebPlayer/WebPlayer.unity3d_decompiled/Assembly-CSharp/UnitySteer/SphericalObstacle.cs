using System;
using UnityEngine;

namespace UnitySteer
{
	public class SphericalObstacle : Obstacle
	{
		public float radius;

		public Vector3 center;

		public SphericalObstacle(float r, Vector3 c)
		{
			this.radius = r;
			this.center = c;
		}

		public SphericalObstacle()
		{
			this.radius = 1f;
			this.center = Vector3.get_zero();
		}

		public override string ToString()
		{
			return string.Format("[SphericalObstacle {0} {1}]", this.center, this.radius);
		}

		public static Obstacle GetObstacle(GameObject gameObject)
		{
			int instanceID = gameObject.GetInstanceID();
			float num = 0f;
			if (!Obstacle.ObstacleCache.ContainsKey(instanceID))
			{
				SphericalObstacleData component = gameObject.GetComponent<SphericalObstacleData>();
				if (component != null)
				{
					Obstacle.ObstacleCache.set_Item(instanceID, new SphericalObstacle(component.Radius, gameObject.get_transform().get_position() + component.Center));
				}
				else
				{
					Component[] componentsInChildren = gameObject.GetComponentsInChildren<Collider>();
					if (componentsInChildren == null)
					{
						Debug.LogError("Obstacle '" + gameObject.get_name() + "' has no colliders");
						return null;
					}
					Component[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						Collider collider = (Collider)array[i];
						if (!collider.get_isTrigger())
						{
							float num2 = Mathf.Max(Mathf.Max(collider.get_bounds().get_extents().x, collider.get_bounds().get_extents().y), collider.get_bounds().get_extents().z);
							float num3 = Vector3.Distance(gameObject.get_transform().get_position(), collider.get_bounds().get_center());
							float num4 = num3 + num2;
							if (num4 > num)
							{
								num = num4;
							}
						}
					}
					Obstacle.ObstacleCache.set_Item(instanceID, new SphericalObstacle(num, gameObject.get_transform().get_position()));
				}
			}
			return Obstacle.ObstacleCache.get_Item(instanceID) as SphericalObstacle;
		}

		public void annotatePosition()
		{
			this.annotatePosition(Color.get_grey());
		}

		public void annotatePosition(Color color)
		{
			Debug.DrawRay(this.center, Vector3.get_up() * this.radius, color);
			Debug.DrawRay(this.center, Vector3.get_forward() * this.radius, color);
			Debug.DrawRay(this.center, Vector3.get_right() * this.radius, color);
		}
	}
}

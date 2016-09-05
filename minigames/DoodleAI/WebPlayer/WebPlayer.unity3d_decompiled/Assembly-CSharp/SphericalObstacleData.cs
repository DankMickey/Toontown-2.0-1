using System;
using UnityEngine;

public class SphericalObstacleData : MonoBehaviour
{
	[SerializeField]
	private Vector3 _center = Vector3.get_zero();

	[SerializeField]
	private float _radius = 1f;

	public Vector3 Center
	{
		get
		{
			return this._center;
		}
	}

	public float Radius
	{
		get
		{
			return this._radius;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.get_transform().get_position() + this.Center, this.Radius);
	}
}

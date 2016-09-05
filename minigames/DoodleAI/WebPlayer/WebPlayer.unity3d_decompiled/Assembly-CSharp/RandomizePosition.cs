using System;
using UnityEngine;

public class RandomizePosition : MonoBehaviour
{
	public float Radius = 3f;

	public bool IsPlanar;

	private void Start()
	{
		Vector3 vector = Random.get_insideUnitSphere() * this.Radius;
		Vector3 insideUnitSphere = Random.get_insideUnitSphere();
		if (this.IsPlanar)
		{
			vector.y = 0f;
			insideUnitSphere.x = 0f;
			insideUnitSphere.z = 0f;
		}
		Transform expr_4C = base.get_transform();
		expr_4C.set_position(expr_4C.get_position() + vector * this.Radius);
		base.get_transform().set_rotation(Quaternion.Euler(insideUnitSphere * 360f));
		Object.Destroy(this);
	}
}

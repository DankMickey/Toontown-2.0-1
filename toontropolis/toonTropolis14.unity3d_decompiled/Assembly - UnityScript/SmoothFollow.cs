using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Follow")]
[Serializable]
public class SmoothFollow : MonoBehaviour
{
	public Transform target;

	public float distance;

	public float height;

	public float heightDamping;

	public float rotationDamping;

	public SmoothFollow()
	{
		this.distance = 10f;
		this.height = 5f;
		this.heightDamping = 2f;
		this.rotationDamping = 3f;
	}

	public void LateUpdate()
	{
		if (this.target)
		{
			float y = this.target.get_eulerAngles().y;
			float num = this.target.get_position().y + this.height;
			float num2 = this.get_transform().get_eulerAngles().y;
			float num3 = this.get_transform().get_position().y;
			num2 = Mathf.LerpAngle(num2, y, this.rotationDamping * Time.get_deltaTime());
			num3 = Mathf.Lerp(num3, num, this.heightDamping * Time.get_deltaTime());
			Quaternion quaternion = Quaternion.Euler((float)0, num2, (float)0);
			this.get_transform().set_position(this.target.get_position());
			this.get_transform().set_position(this.get_transform().get_position() - quaternion * Vector3.get_forward() * this.distance);
			float y2 = num3;
			Vector3 position = this.get_transform().get_position();
			float num4 = position.y = y2;
			Vector3 vector;
			this.get_transform().set_position(vector = position);
			this.get_transform().LookAt(this.target);
		}
	}

	public void Main()
	{
	}
}

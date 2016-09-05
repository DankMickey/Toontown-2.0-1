using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit")]
[Serializable]
public class MouseOrbit : MonoBehaviour
{
	public Transform target;

	public float distance;

	public float xSpeed;

	public float ySpeed;

	public int yMinLimit;

	public int yMaxLimit;

	private float x;

	private float y;

	public MouseOrbit()
	{
		this.distance = 10f;
		this.xSpeed = 250f;
		this.ySpeed = 120f;
		this.yMinLimit = -20;
		this.yMaxLimit = 80;
	}

	public override void Start()
	{
		Vector3 eulerAngles = this.get_transform().get_eulerAngles();
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		if (this.get_rigidbody())
		{
			this.get_rigidbody().set_freezeRotation(true);
		}
	}

	public override void LateUpdate()
	{
		if (this.target)
		{
			this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.y = MouseOrbit.ClampAngle(this.y, (float)this.yMinLimit, (float)this.yMaxLimit);
			Quaternion quaternion = Quaternion.Euler(this.y, this.x, (float)0);
			Vector3 position = quaternion * new Vector3((float)0, (float)0, -this.distance) + this.target.get_position();
			this.get_transform().set_rotation(quaternion);
			this.get_transform().set_position(position);
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < (float)-360)
		{
			angle += (float)360;
		}
		if (angle > (float)360)
		{
			angle -= (float)360;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public override void Main()
	{
	}
}

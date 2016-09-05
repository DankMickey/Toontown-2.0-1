using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public MouseLook.RotationAxes axes;

	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	private float rotationX;

	private float rotationY;

	private Quaternion originalRotation;

	private void Update()
	{
		if (this.axes == MouseLook.RotationAxes.MouseXAndY)
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			Quaternion quaternion = Quaternion.AngleAxis(this.rotationX, Vector3.get_up());
			Quaternion quaternion2 = Quaternion.AngleAxis(this.rotationY, Vector3.get_left());
			base.get_transform().set_localRotation(this.originalRotation * quaternion * quaternion2);
		}
		else if (this.axes == MouseLook.RotationAxes.MouseX)
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			Quaternion quaternion3 = Quaternion.AngleAxis(this.rotationX, Vector3.get_up());
			base.get_transform().set_localRotation(this.originalRotation * quaternion3);
		}
		else
		{
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			Quaternion quaternion4 = Quaternion.AngleAxis(this.rotationY, Vector3.get_left());
			base.get_transform().set_localRotation(this.originalRotation * quaternion4);
		}
	}

	private void Start()
	{
		if (base.get_rigidbody())
		{
			base.get_rigidbody().set_freezeRotation(true);
		}
		this.originalRotation = base.get_transform().get_localRotation();
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}

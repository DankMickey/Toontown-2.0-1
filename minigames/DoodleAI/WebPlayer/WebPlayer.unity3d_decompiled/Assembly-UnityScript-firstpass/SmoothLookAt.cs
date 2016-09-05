using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Look At")]
[Serializable]
public class SmoothLookAt : MonoBehaviour
{
	public Transform target;

	public float damping;

	public bool smooth;

	public SmoothLookAt()
	{
		this.damping = 6f;
		this.smooth = true;
	}

	public override void LateUpdate()
	{
		if (this.target)
		{
			if (this.smooth)
			{
				Quaternion quaternion = Quaternion.LookRotation(this.target.get_position() - this.get_transform().get_position());
				this.get_transform().set_rotation(Quaternion.Slerp(this.get_transform().get_rotation(), quaternion, Time.get_deltaTime() * this.damping));
			}
			else
			{
				this.get_transform().LookAt(this.target);
			}
		}
	}

	public override void Start()
	{
		if (this.get_rigidbody())
		{
			this.get_rigidbody().set_freezeRotation(true);
		}
	}

	public override void Main()
	{
	}
}

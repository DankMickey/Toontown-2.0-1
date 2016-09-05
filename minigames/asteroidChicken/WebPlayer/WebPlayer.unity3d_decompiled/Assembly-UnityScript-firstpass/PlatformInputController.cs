using System;
using UnityEngine;

[AddComponentMenu("Character/Platform Input Controller"), RequireComponent(typeof(CharacterMotor))]
[Serializable]
public class PlatformInputController : MonoBehaviour
{
	public bool autoRotate;

	public float maxRotationSpeed;

	private CharacterMotor motor;

	public PlatformInputController()
	{
		this.autoRotate = true;
		this.maxRotationSpeed = (float)360;
	}

	public override void Awake()
	{
		this.motor = (CharacterMotor)this.GetComponent(typeof(CharacterMotor));
	}

	public override void Update()
	{
		Vector3 vector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), (float)0);
		if (vector != Vector3.get_zero())
		{
			float num = vector.get_magnitude();
			vector /= num;
			num = Mathf.Min((float)1, num);
			num *= num;
			vector *= num;
		}
		vector = Camera.get_main().get_transform().get_rotation() * vector;
		Quaternion quaternion = Quaternion.FromToRotation(-Camera.get_main().get_transform().get_forward(), this.get_transform().get_up());
		vector = quaternion * vector;
		this.motor.inputMoveDirection = vector;
		this.motor.inputJump = Input.GetButton("Jump");
		if (this.autoRotate && vector.get_sqrMagnitude() > 0.01f)
		{
			Vector3 vector2 = this.ConstantSlerp(this.get_transform().get_forward(), vector, this.maxRotationSpeed * Time.get_deltaTime());
			vector2 = this.ProjectOntoPlane(vector2, this.get_transform().get_up());
			this.get_transform().set_rotation(Quaternion.LookRotation(vector2, this.get_transform().get_up()));
		}
	}

	public override Vector3 ProjectOntoPlane(Vector3 v, Vector3 normal)
	{
		return v - Vector3.Project(v, normal);
	}

	public override Vector3 ConstantSlerp(Vector3 from, Vector3 to, float angle)
	{
		float num = Mathf.Min((float)1, angle / Vector3.Angle(from, to));
		return Vector3.Slerp(from, to, num);
	}

	public override void Main()
	{
	}
}

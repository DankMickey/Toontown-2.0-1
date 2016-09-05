using System;
using UnityEngine;

[Serializable]
public class FaceCamera : MonoBehaviour
{
	public Transform lookAtObject;

	public float starSpeedMin;

	public float starSpeedMax;

	private float starSpeed;

	private Vector3 myStartPos;

	private GameObject shipPos;

	public FaceCamera()
	{
		this.starSpeedMin = 20f;
		this.starSpeedMax = 120f;
	}

	public override void Start()
	{
		this.shipPos = GameObject.Find("Ship");
		this.myStartPos.x = this.get_transform().get_position().x;
		this.myStartPos.y = this.get_transform().get_position().y;
		this.myStartPos.z = this.get_transform().get_position().z;
	}

	public override void Update()
	{
		if (this.get_transform().get_position().z <= this.shipPos.get_transform().get_position().z - (float)200)
		{
			this.Recycle();
		}
		this.starSpeed = Random.Range(this.starSpeedMin, this.starSpeedMax);
		this.get_transform().Translate(-this.starSpeed * Vector3.get_forward() * Time.get_deltaTime(), 0);
	}

	public override void Recycle()
	{
		float x = this.myStartPos.x + this.shipPos.get_transform().get_position().x;
		Vector3 position = this.get_transform().get_position();
		float num = position.x = x;
		Vector3 vector;
		this.get_transform().set_position(vector = position);
		float y = this.myStartPos.y + this.shipPos.get_transform().get_position().y;
		Vector3 position2 = this.get_transform().get_position();
		float num2 = position2.y = y;
		Vector3 vector2;
		this.get_transform().set_position(vector2 = position2);
		float z = this.myStartPos.z + this.shipPos.get_transform().get_position().z;
		Vector3 position3 = this.get_transform().get_position();
		float num3 = position3.z = z;
		Vector3 vector3;
		this.get_transform().set_position(vector3 = position3);
	}

	public override void Main()
	{
	}
}

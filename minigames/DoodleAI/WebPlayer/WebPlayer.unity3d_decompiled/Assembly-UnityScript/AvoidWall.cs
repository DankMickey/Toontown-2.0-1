using System;
using UnityEngine;

[Serializable]
public class AvoidWall : MonoBehaviour
{
	public float wallAwareDistance;

	public float wallAvoidDistance;

	public float avoidRunSpeed;

	public float turn;

	public bool avoidWall;

	private Vector3 moveDirection;

	public AvoidWall()
	{
		this.wallAwareDistance = 20f;
		this.wallAvoidDistance = 15f;
		this.avoidRunSpeed = 8f;
		this.turn = 1f;
		this.moveDirection = Vector3.get_zero();
	}

	public override void Awake()
	{
	}

	public override void FixedUpdate()
	{
		float magnitude = (this.FindClosestWall().get_transform().get_position() - this.get_transform().get_position()).get_magnitude();
		if (magnitude < this.wallAvoidDistance)
		{
			this.avoidWall = true;
		}
		else if (magnitude > this.wallAwareDistance)
		{
			this.avoidWall = false;
		}
		if (this.avoidWall)
		{
			this.WallAvoid();
		}
	}

	public override GameObject FindClosestWall()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("notTray");
		GameObject result = null;
		float num = float.PositiveInfinity;
		Vector3 position = this.get_transform().get_position();
		int i = 0;
		GameObject[] array2 = array;
		int length = array2.get_Length();
		checked
		{
			while (i < length)
			{
				float sqrMagnitude = (array2[i].get_transform().get_position() - position).get_sqrMagnitude();
				if (sqrMagnitude < num)
				{
					result = array2[i];
					num = sqrMagnitude;
				}
				i++;
			}
			return result;
		}
	}

	public override void WallAvoid()
	{
		this.get_transform().LookAt(this.FindClosestWall().get_transform());
		float y = this.get_transform().get_eulerAngles().y + (float)-180 * this.turn;
		Vector3 eulerAngles = this.get_transform().get_eulerAngles();
		float num = eulerAngles.y = y;
		Vector3 vector;
		this.get_transform().set_eulerAngles(vector = eulerAngles);
		this.moveDirection = new Vector3((float)0, (float)0, (float)40);
		this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
		this.moveDirection *= this.avoidRunSpeed;
		this.get_rigidbody().AddForce(this.moveDirection * Time.get_deltaTime());
		this.get_animation().Play("run");
	}

	public override void Main()
	{
	}
}

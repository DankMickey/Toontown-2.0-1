using System;
using UnityEngine;

[Serializable]
public class ShipMoveForward : MonoBehaviour
{
	public float speed;

	public float delayMove;

	private float _speed;

	private float startTime;

	public ShipMoveForward()
	{
		this.speed = (float)10;
	}

	public override void Start()
	{
		this.startTime = Time.get_time();
		this._speed = (float)0;
	}

	public override void FixedUpdate()
	{
		if (Time.get_time() - this.startTime > this.delayMove && this._speed < this.speed)
		{
			this._speed += this.speed / (float)10;
		}
		this.get_transform().Translate((float)1 * this._speed * Vector3.get_forward() * Time.get_deltaTime(), 0);
	}

	public override void Main()
	{
	}
}

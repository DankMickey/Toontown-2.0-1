using System;
using UnityEngine;
using UnitySteer;

public class SteerToFollowPath : Steering
{
	private FollowDirection _direction = FollowDirection.Forward;

	private float _predictionTime = 2f;

	private Pathway _path;

	public FollowDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
		}
	}

	public float PredictionTime
	{
		get
		{
			return this._predictionTime;
		}
		set
		{
			this._predictionTime = value;
		}
	}

	public Pathway Path
	{
		get
		{
			return this._path;
		}
		set
		{
			this._path = value;
		}
	}

	protected override Vector3 CalculateForce()
	{
		if (this._path == null)
		{
			return Vector3.get_zero();
		}
		float num = (float)this._direction * this._predictionTime * base.Vehicle.Speed;
		Vector3 point = base.Vehicle.PredictFuturePosition(this._predictionTime);
		float num2 = this._path.mapPointToPathDistance(base.Vehicle.Position);
		float num3 = this._path.mapPointToPathDistance(point);
		bool flag = (num <= 0f) ? (num2 > num3) : (num2 < num3);
		mapReturnStruct mapReturnStruct = default(mapReturnStruct);
		this._path.mapPointToPath(point, ref mapReturnStruct);
		if (mapReturnStruct.outside < 0f && flag)
		{
			return Vector3.get_zero();
		}
		float pathDistance = num2 + num;
		Vector3 target = this._path.mapPathDistanceToPoint(pathDistance);
		return base.Vehicle.GetSeekVector(target);
	}
}

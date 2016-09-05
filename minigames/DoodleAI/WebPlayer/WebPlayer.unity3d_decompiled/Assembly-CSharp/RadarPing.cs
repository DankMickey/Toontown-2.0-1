using C5;
using System;
using UnityEngine;

public class RadarPing : Radar
{
	[SerializeField]
	private float _detectionRadius = 10f;

	[SerializeField]
	private bool _drawGizmos;

	public float DetectionRadius
	{
		get
		{
			return this._detectionRadius;
		}
		set
		{
			this._detectionRadius = value;
		}
	}

	private void OnDrawGizmos()
	{
		if (this._drawGizmos)
		{
			Vector3 vector = (!(base.Vehicle == null)) ? base.Vehicle.Position : base.get_transform().get_position();
			Gizmos.set_color(Color.get_cyan());
			Gizmos.DrawWireSphere(vector, this._detectionRadius);
		}
	}

	protected override IList<Collider> Detect()
	{
		Collider[] items = Physics.OverlapSphere(base.Vehicle.Position, this._detectionRadius, base.LayersChecked);
		ArrayList<Collider> arrayList = new ArrayList<Collider>();
		arrayList.AddAll<Collider>(items);
		return arrayList;
	}
}

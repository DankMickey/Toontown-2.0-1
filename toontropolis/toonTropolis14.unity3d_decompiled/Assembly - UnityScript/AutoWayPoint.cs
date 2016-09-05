using Boo.Lang.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class AutoWayPoint : MonoBehaviour
{
	public static Array waypoints = new Array();

	public Array connected;

	public static float kLineOfSightCapsuleRadius = 0.25f;

	public AutoWayPoint()
	{
		this.connected = new Array();
	}

	public static AutoWayPoint FindClosest(Vector3 pos)
	{
		AutoWayPoint result = null;
		float num = 100000f;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(AutoWayPoint.waypoints);
		while (enumerator.MoveNext())
		{
			AutoWayPoint autoWayPoint = (AutoWayPoint)RuntimeServices.Coerce(enumerator.Current, typeof(AutoWayPoint));
			float num2 = Vector3.Distance(autoWayPoint.get_transform().get_position(), pos);
			UnityRuntimeServices.Update(enumerator, autoWayPoint);
			if (num2 < num)
			{
				num = num2;
				result = autoWayPoint;
				UnityRuntimeServices.Update(enumerator, autoWayPoint);
			}
		}
		return result;
	}

	[ContextMenu("Update Waypoints")]
	public void UpdateWaypoints()
	{
		this.RebuildWaypointList();
	}

	public void Awake()
	{
		this.RebuildWaypointList();
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawIcon(this.get_transform().get_position(), "Waypoint.tif");
	}

	public void OnDrawGizmosSelected()
	{
		if (AutoWayPoint.waypoints.get_length() == 0)
		{
			this.RebuildWaypointList();
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.connected);
		while (enumerator.MoveNext())
		{
			AutoWayPoint autoWayPoint = (AutoWayPoint)RuntimeServices.Coerce(enumerator.Current, typeof(AutoWayPoint));
			if (Physics.Linecast(this.get_transform().get_position(), autoWayPoint.get_transform().get_position()))
			{
				Gizmos.set_color(Color.get_red());
				Gizmos.DrawLine(this.get_transform().get_position(), autoWayPoint.get_transform().get_position());
				UnityRuntimeServices.Update(enumerator, autoWayPoint);
			}
			else
			{
				Gizmos.set_color(Color.get_green());
				Gizmos.DrawLine(this.get_transform().get_position(), autoWayPoint.get_transform().get_position());
				UnityRuntimeServices.Update(enumerator, autoWayPoint);
			}
		}
	}

	public void RebuildWaypointList()
	{
		object[] array = Object.FindObjectsOfType(typeof(AutoWayPoint));
		AutoWayPoint.waypoints = new Array(array);
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(AutoWayPoint.waypoints);
		while (enumerator.MoveNext())
		{
			AutoWayPoint autoWayPoint = (AutoWayPoint)RuntimeServices.Coerce(enumerator.Current, typeof(AutoWayPoint));
			autoWayPoint.RecalculateConnectedWaypoints();
			UnityRuntimeServices.Update(enumerator, autoWayPoint);
		}
	}

	public void RecalculateConnectedWaypoints()
	{
		this.connected = new Array();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(AutoWayPoint.waypoints);
		while (enumerator.MoveNext())
		{
			AutoWayPoint autoWayPoint = (AutoWayPoint)RuntimeServices.Coerce(enumerator.Current, typeof(AutoWayPoint));
			if (!(autoWayPoint == this))
			{
				if (!Physics.CheckCapsule(this.get_transform().get_position(), autoWayPoint.get_transform().get_position(), AutoWayPoint.kLineOfSightCapsuleRadius))
				{
					this.connected.Add(autoWayPoint);
					UnityRuntimeServices.Update(enumerator, autoWayPoint);
				}
			}
		}
	}

	public void Main()
	{
	}
}

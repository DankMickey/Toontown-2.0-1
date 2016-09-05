using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class DOFGui : MonoBehaviour
{
	public void OnGUI()
	{
		if (RuntimeServices.EqualityOperator(Status.menuMode, "on"))
		{
			DepthOfFieldEffect depthOfFieldEffect = (DepthOfFieldEffect)this.GetComponent(typeof(DepthOfFieldEffect));
			GUILayout.BeginArea(new Rect((float)10, (float)280, (float)250, (float)100), GUI.get_skin().get_window());
			GUILayout.Label("[focal distance]: 1,2:\n Focal distance = " + depthOfFieldEffect.focalDistance.ToString("f1"), new GUILayoutOption[0]);
			GUILayout.Label("[focal range]: 3,4:\n Focal range = " + depthOfFieldEffect.focalRange.ToString("f1"), new GUILayoutOption[0]);
			GUILayout.EndArea();
		}
	}

	public void Update()
	{
		if (RuntimeServices.EqualityOperator(Status.menuMode, "on"))
		{
			DepthOfFieldEffect depthOfFieldEffect = (DepthOfFieldEffect)this.GetComponent(typeof(DepthOfFieldEffect));
			float deltaTime = Time.get_deltaTime();
			if (Input.GetKey("1"))
			{
				depthOfFieldEffect.focalDistance -= (float)5 * deltaTime;
			}
			if (Input.GetKey("2"))
			{
				depthOfFieldEffect.focalDistance += (float)5 * deltaTime;
			}
			if (Input.GetKey("3"))
			{
				depthOfFieldEffect.focalRange -= (float)50 * deltaTime;
			}
			if (Input.GetKey("4"))
			{
				depthOfFieldEffect.focalRange += (float)50 * deltaTime;
			}
			depthOfFieldEffect.focalDistance = Mathf.Clamp(depthOfFieldEffect.focalDistance, 1f, 50f);
			depthOfFieldEffect.focalRange = Mathf.Clamp(depthOfFieldEffect.focalRange, 1f, 1000f);
		}
	}

	public void Main()
	{
	}
}

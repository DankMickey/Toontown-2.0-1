using Boo.Lang.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class toggleVisibility : MonoBehaviour
{
	public Material night_material;

	public Material day_material;

	public GameObject lights_prefab;

	private GameObject night_lights;

	private int temp;

	public static string dayMode = "day";

	public toggleVisibility()
	{
		this.temp = 1;
	}

	public override void Update()
	{
		checked
		{
			if (Input.GetKeyDown(118))
			{
				this.temp++;
				if (this.temp % 2 == 0)
				{
					RenderSettings.set_fog(true);
					RenderSettings.set_fogColor(Color.get_white());
					RenderSettings.set_fogDensity(0.005f);
					RenderSettings.set_ambientLight(Color.get_gray());
					RenderSettings.set_skybox(this.day_material);
					toggleVisibility.dayMode = "day";
				}
				else
				{
					RenderSettings.set_fog(true);
					RenderSettings.set_fogColor(Color.get_blue());
					RenderSettings.set_fogDensity(0.005f);
					RenderSettings.set_ambientLight(Color.get_blue());
					RenderSettings.set_skybox(this.night_material);
					toggleVisibility.dayMode = "night";
				}
			}
		}
	}

	public override void ToggleVisibility()
	{
		object componentsInChildren = this.get_gameObject().GetComponentsInChildren(typeof(Renderer));
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(componentsInChildren);
		while (enumerator.MoveNext())
		{
			Renderer renderer = (Renderer)RuntimeServices.Coerce(enumerator.get_Current(), typeof(Renderer));
			renderer.set_enabled(!renderer.get_enabled());
			UnityRuntimeServices.Update(enumerator, renderer);
		}
	}

	public override void Main()
	{
	}
}

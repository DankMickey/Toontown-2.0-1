using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class toggleVisibility : MonoBehaviour
{
	public Material night_material;

	public Material day_material;

	public static string dayMode = "day";

	public void Update()
	{
		if (Input.GetKeyDown(118))
		{
			this.BuildingAnimStart();
			if (toggleVisibility.dayMode == "night")
			{
				RenderSettings.set_fog(true);
				RenderSettings.set_fogColor(Color.get_white());
				RenderSettings.set_fogDensity(0.005f);
				RenderSettings.set_ambientLight(Color.get_gray());
				RenderSettings.set_skybox(this.day_material);
				toggleVisibility.dayMode = "day";
				PlayerCollisions.dayMode = "day";
				this.ToggleVisibility(false);
			}
			else
			{
				RenderSettings.set_fog(true);
				RenderSettings.set_fogColor(Color.get_blue());
				RenderSettings.set_fogDensity(0.005f);
				RenderSettings.set_ambientLight(Color.get_blue());
				RenderSettings.set_skybox(this.night_material);
				toggleVisibility.dayMode = "night";
				PlayerCollisions.dayMode = "night";
				this.ToggleVisibility(true);
			}
		}
	}

	public void ToggleVisibility(object mode)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("nights");
		GameObject gameObject = GameObject.Find("t2_m_ara_ttp_powerBulding_switch");
		int num = 180;
		int i = 0;
		GameObject[] array2 = array;
		int length = array2.Length;
		checked
		{
			while (i < length)
			{
				array2[i].get_renderer().set_enabled(RuntimeServices.UnboxBoolean(mode));
				i++;
			}
			gameObject.get_transform().Rotate((float)0, (float)(0 + num), (float)0);
		}
	}

	public void BuildingAnimStart()
	{
		GameObject gameObject = GameObject.Find("sceneLight");
		GameObject gameObject2 = GameObject.Find("PowerBuilding");
		GameObject gameObject3 = GameObject.Find("PetShop");
		GameObject gameObject4 = GameObject.Find("ClothesShop");
		GameObject gameObject5 = GameObject.Find("CogPinata");
		GameObject gameObject6 = GameObject.Find("GagShop");
		GameObject gameObject7 = GameObject.Find("MusicShop");
		if (gameObject2)
		{
			PWR_PowerBuildingControl pWR_PowerBuildingControl = (PWR_PowerBuildingControl)gameObject2.GetComponentInChildren(typeof(PWR_PowerBuildingControl));
			bool flag = RuntimeServices.UnboxBoolean(RuntimeServices.GetProperty(gameObject2.GetComponentInChildren(typeof(PWR_PowerBuildingControl)), "Awake"));
			if (!pWR_PowerBuildingControl)
			{
				MonoBehaviour.print("no randomAnimScript");
				flag = pWR_PowerBuildingControl.IsAwake();
			}
			gameObject2.BroadcastMessage("ToggleSleepWake");
			gameObject3.BroadcastMessage("ToggleSleepWake");
			gameObject4.BroadcastMessage("ToggleSleepWake");
			gameObject6.BroadcastMessage("ToggleSleepWake");
			gameObject5.BroadcastMessage("ToggleSleepWake");
			gameObject7.BroadcastMessage("ToggleSleepWake");
		}
	}

	public void Main()
	{
	}
}

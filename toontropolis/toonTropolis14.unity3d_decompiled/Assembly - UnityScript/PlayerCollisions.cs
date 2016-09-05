using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("ControlPrototype/Player Collisions")]
[Serializable]
public class PlayerCollisions : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class OnControllerColliderHit$51 : GenericGenerator<WaitForSeconds>
	{
		internal ControllerColliderHit $hit268;

		internal PlayerCollisions $self_269;

		public OnControllerColliderHit$51(ControllerColliderHit hit, PlayerCollisions self_)
		{
			this.$hit268 = hit;
			this.$self_269 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PlayerCollisions.OnControllerColliderHit$51.$(this.$hit268, this.$self_269);
		}
	}

	private bool pianoEvent;

	private bool powerEvent;

	private float pianoEventTimer;

	private GameObject currentKey;

	public Material night_material;

	public Material day_material;

	public static string dayMode = "day";

	public PlayerCollisions()
	{
		this.pianoEvent = false;
		this.powerEvent = false;
		this.pianoEventTimer = 0f;
	}

	public void Update()
	{
	}

	public IEnumerator OnControllerColliderHit(ControllerColliderHit hit)
	{
		return new PlayerCollisions.OnControllerColliderHit$51(hit, this).GetEnumerator();
	}

	public void PianoPlay()
	{
		CameraController.distance = 8f;
		CameraController.y = 30f;
		CameraController.cameraFollow = false;
		PlayerController.tdcWalkerMode = true;
		MonoBehaviour.print("start piano event");
	}

	public void PianoStop()
	{
		CameraController.distance = 4f;
		CameraController.y = 15f * (float)-1;
		CameraController.cameraFollow = true;
		PlayerController.tdcWalkerMode = false;
		MonoBehaviour.print("stop piano event");
	}

	public void GagShopEventStart()
	{
	}

	public void PowerEventStart()
	{
		MonoBehaviour.print("start power event");
		this.BuildingAnimStart();
		if (PlayerCollisions.dayMode == "night")
		{
			RenderSettings.set_fog(true);
			RenderSettings.set_fogColor(Color.get_white());
			RenderSettings.set_fogDensity(0.005f);
			RenderSettings.set_ambientLight(Color.get_gray());
			RenderSettings.set_skybox(this.day_material);
			PlayerCollisions.dayMode = "day";
			toggleVisibility.dayMode = "day";
			this.ToggleVisibility(false);
		}
		else
		{
			RenderSettings.set_fog(true);
			RenderSettings.set_fogColor(Color.get_blue());
			RenderSettings.set_fogDensity(0.005f);
			RenderSettings.set_ambientLight(Color.get_blue());
			RenderSettings.set_skybox(this.night_material);
			PlayerCollisions.dayMode = "night";
			toggleVisibility.dayMode = "night";
			this.ToggleVisibility(true);
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

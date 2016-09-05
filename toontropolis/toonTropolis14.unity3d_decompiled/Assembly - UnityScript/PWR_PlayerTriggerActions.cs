using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class PWR_PlayerTriggerActions : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		MonoBehaviour.print("PLAYER TRIGGER: entered");
		GameObject gameObject = GameObject.Find("sceneLight");
		GameObject gameObject2 = GameObject.Find("PowerBuilding");
		GameObject gameObject3 = GameObject.Find("PetShop");
		GameObject gameObject4 = GameObject.Find("ClothesShop");
		if (gameObject2)
		{
			MonoBehaviour.print("the collider name is " + other.get_name());
			PWR_PowerBuildingControl pWR_PowerBuildingControl = (PWR_PowerBuildingControl)gameObject2.GetComponentInChildren(typeof(PWR_PowerBuildingControl));
			bool flag = RuntimeServices.UnboxBoolean(RuntimeServices.GetProperty(gameObject2.GetComponentInChildren(typeof(PWR_PowerBuildingControl)), "Awake"));
			if (!pWR_PowerBuildingControl)
			{
				MonoBehaviour.print("no randomAnimScript");
			}
			flag = pWR_PowerBuildingControl.IsAwake();
			if (other.get_name() == "BarrelCube" && flag)
			{
				MonoBehaviour.print("BarrelCube picked");
				Transform transform = gameObject2.get_transform().Find(string.Empty);
				if (!transform)
				{
					MonoBehaviour.print("no target ");
				}
			}
			else if (other.get_name() == "BarrelCube1" && flag)
			{
				MonoBehaviour.print("BarrelCube1 picked");
			}
			else if (other.get_name() == "BarrelCube2")
			{
				gameObject2.BroadcastMessage("ToggleSleepWake");
				gameObject3.BroadcastMessage("ToggleSleepWake");
				gameObject4.BroadcastMessage("ToggleSleepWake");
			}
		}
		else
		{
			MonoBehaviour.print("pwrBuilding not found");
		}
	}

	public void Main()
	{
	}
}

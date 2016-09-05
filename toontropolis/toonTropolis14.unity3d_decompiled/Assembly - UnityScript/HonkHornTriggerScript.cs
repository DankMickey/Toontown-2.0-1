using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class HonkHornTriggerScript : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = GameObject.Find("GagShop");
		GameObject gameObject2 = GameObject.Find("CogPinata");
		MonoBehaviour.print("HONK: honkHornTriggerEnter");
		Animation animation = (Animation)gameObject.GetComponentInChildren(typeof(Animation));
		if (!animation.get_Item("anvilDrop").get_enabled())
		{
			if (!animation.get_Item("honkHorn1").get_enabled())
			{
				MonoBehaviour.print("HONK: anims not enabled yet");
				GS_RandomAnim gS_RandomAnim = (GS_RandomAnim)gameObject.GetComponentInChildren(typeof(GS_RandomAnim));
				object property = RuntimeServices.GetProperty(gameObject.GetComponentInChildren(typeof(GS_RandomAnim)), "Awake");
				if (RuntimeServices.ToBool(property))
				{
					MonoBehaviour.print("HONK: Awake");
					gameObject.BroadcastMessage("HonkHorn", 1);
					gameObject2.BroadcastMessage("HonkHorn", 1);
				}
			}
		}
	}

	public void Main()
	{
	}
}

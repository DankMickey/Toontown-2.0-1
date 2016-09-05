using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class AnvilDropTriggerScript : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = GameObject.Find("GagShop");
		GameObject gameObject2 = GameObject.Find("CogPinata");
		MonoBehaviour.print("ANVIL DROP: enter");
		object componentInChildren = gameObject.GetComponentInChildren(typeof(Animation));
		if (!RuntimeServices.ToBool(RuntimeServices.GetProperty(RuntimeServices.GetSlice(componentInChildren, string.Empty, new object[]
		{
			"anvilDrop"
		}), "enabled")))
		{
			if (!RuntimeServices.ToBool(RuntimeServices.GetProperty(RuntimeServices.GetSlice(componentInChildren, string.Empty, new object[]
			{
				"honkHorn1"
			}), "enabled")))
			{
				MonoBehaviour.print("ANVIL DROP: anims not enabled yet");
				GS_RandomAnim gS_RandomAnim = (GS_RandomAnim)gameObject.GetComponentInChildren(typeof(GS_RandomAnim));
				object property = RuntimeServices.GetProperty(gameObject.GetComponentInChildren(typeof(GS_RandomAnim)), "Awake");
				if (RuntimeServices.ToBool(property))
				{
					MonoBehaviour.print("ANVIL DROP: awake");
					object componentInChildren2 = gameObject2.GetComponentInChildren(typeof(Animation));
					if (!RuntimeServices.ToBool(RuntimeServices.GetProperty(RuntimeServices.GetSlice(componentInChildren2, string.Empty, new object[]
					{
						"anvilDrop3"
					}), "enabled")))
					{
						if (!RuntimeServices.ToBool(RuntimeServices.GetProperty(RuntimeServices.GetSlice(componentInChildren2, string.Empty, new object[]
						{
							"honkHorn"
						}), "enabled")))
						{
							MonoBehaviour.print("ANVIL DROP: cogAnims enabled");
							gameObject.BroadcastMessage("AnvilDrop", 1);
							gameObject2.BroadcastMessage("AnvilDrop", 1);
							MonoBehaviour.print("ANVIL DROP: should be boradcasting");
						}
					}
				}
			}
		}
	}

	public void Main()
	{
	}
}

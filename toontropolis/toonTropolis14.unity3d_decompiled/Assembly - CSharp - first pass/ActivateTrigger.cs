using System;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
	public enum Mode
	{
		Trigger,
		Replace,
		Activate,
		Enable,
		Animate,
		Deactivate
	}

	public ActivateTrigger.Mode action = ActivateTrigger.Mode.Activate;

	public Object target;

	public GameObject source;

	public int triggerCount = 1;

	public bool repeatTrigger;

	private void DoActivateTrigger()
	{
		this.triggerCount--;
		if (this.triggerCount == 0 || this.repeatTrigger)
		{
			Object @object = (!(this.target != null)) ? base.get_gameObject() : this.target;
			Behaviour behaviour = @object as Behaviour;
			GameObject gameObject = @object as GameObject;
			if (behaviour != null)
			{
				gameObject = behaviour.get_gameObject();
			}
			switch (this.action)
			{
			case ActivateTrigger.Mode.Trigger:
				gameObject.BroadcastMessage("DoActivateTrigger");
				break;
			case ActivateTrigger.Mode.Replace:
				if (this.source != null)
				{
					Object.Instantiate(this.source, gameObject.get_transform().get_position(), gameObject.get_transform().get_rotation());
					Object.DestroyObject(gameObject);
				}
				break;
			case ActivateTrigger.Mode.Activate:
				gameObject.set_active(true);
				break;
			case ActivateTrigger.Mode.Enable:
				if (behaviour != null)
				{
					behaviour.set_enabled(true);
				}
				break;
			case ActivateTrigger.Mode.Animate:
				gameObject.get_animation().Play();
				break;
			case ActivateTrigger.Mode.Deactivate:
				gameObject.set_active(false);
				break;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		this.DoActivateTrigger();
	}
}

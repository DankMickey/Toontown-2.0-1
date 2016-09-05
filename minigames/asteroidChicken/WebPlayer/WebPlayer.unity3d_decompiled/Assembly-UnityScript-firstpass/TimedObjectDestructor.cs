using System;
using UnityEngine;

[Serializable]
public class TimedObjectDestructor : MonoBehaviour
{
	public float timeOut;

	public bool detachChildren;

	public TimedObjectDestructor()
	{
		this.timeOut = 1f;
	}

	public override void Awake()
	{
		this.Invoke("DestroyNow", this.timeOut);
	}

	public override void DestroyNow()
	{
		if (this.detachChildren)
		{
			this.get_transform().DetachChildren();
		}
		Object.DestroyObject(this.get_gameObject());
	}

	public override void Main()
	{
	}
}

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
		this.detachChildren = false;
	}

	public void Awake()
	{
		this.Invoke("DestroyNow", this.timeOut);
	}

	public void DestroyNow()
	{
		if (this.detachChildren)
		{
			this.get_transform().DetachChildren();
		}
		Object.DestroyObject(this.get_gameObject());
	}

	public void Main()
	{
	}
}

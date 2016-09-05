using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("Third Person Props/Jump pad"), RequireComponent(typeof(BoxCollider))]
[Serializable]
public class Jumppad : MonoBehaviour
{
	public float jumpHeight;

	public Jumppad()
	{
		this.jumpHeight = 5f;
	}

	public override void OnTriggerEnter(Collider col)
	{
		ThirdPersonController thirdPersonController = (ThirdPersonController)col.GetComponent(typeof(ThirdPersonController));
		if (thirdPersonController != null)
		{
			if (this.get_audio())
			{
				this.get_audio().Play();
			}
			thirdPersonController.SuperJump(this.jumpHeight);
		}
	}

	public override void Reset()
	{
		if (RuntimeServices.EqualityOperator(this.get_collider(), null))
		{
			this.get_gameObject().AddComponent(typeof(BoxCollider));
		}
		RuntimeServices.SetProperty(this.get_collider(), "isTrigger", true);
	}

	public override void Main()
	{
	}
}

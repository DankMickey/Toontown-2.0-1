using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DragRigidbody : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class DragObject$1 : GenericGenerator<object>
	{
		internal float $distance10;

		internal DragRigidbody $self_11;

		public DragObject$1(float distance, DragRigidbody self_)
		{
			this.$distance10 = distance;
			this.$self_11 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new DragRigidbody.DragObject$1.$(this.$distance10, this.$self_11);
		}
	}

	public float spring;

	public float damper;

	public float drag;

	public float angularDrag;

	public float distance;

	public bool attachToCenterOfMass;

	private SpringJoint springJoint;

	public DragRigidbody()
	{
		this.spring = 50f;
		this.damper = 5f;
		this.drag = 10f;
		this.angularDrag = 5f;
		this.distance = 0.2f;
		this.attachToCenterOfMass = false;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Camera camera = this.FindCamera();
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(camera.ScreenPointToRay(Input.get_mousePosition()), ref raycastHit, (float)100))
			{
				if (raycastHit.get_rigidbody() && !raycastHit.get_rigidbody().get_isKinematic())
				{
					if (!this.springJoint)
					{
						GameObject gameObject = new GameObject("Rigidbody dragger");
						object obj = gameObject.AddComponent("Rigidbody");
						this.springJoint = (SpringJoint)gameObject.AddComponent("SpringJoint");
						RuntimeServices.SetProperty(obj, "isKinematic", true);
					}
					this.springJoint.get_transform().set_position(raycastHit.get_point());
					if (this.attachToCenterOfMass)
					{
						Vector3 vector = this.get_transform().TransformDirection(raycastHit.get_rigidbody().get_centerOfMass()) + raycastHit.get_rigidbody().get_transform().get_position();
						vector = this.springJoint.get_transform().InverseTransformPoint(vector);
						this.springJoint.set_anchor(vector);
					}
					else
					{
						this.springJoint.set_anchor(Vector3.get_zero());
					}
					this.springJoint.set_spring(this.spring);
					this.springJoint.set_damper(this.damper);
					this.springJoint.set_maxDistance(this.distance);
					this.springJoint.set_connectedBody(raycastHit.get_rigidbody());
					this.StartCoroutine("DragObject", raycastHit.get_distance());
				}
			}
		}
	}

	public IEnumerator DragObject(float distance)
	{
		return new DragRigidbody.DragObject$1(distance, this).GetEnumerator();
	}

	public Camera FindCamera()
	{
		Camera result;
		if (this.get_camera())
		{
			result = this.get_camera();
		}
		else
		{
			result = Camera.get_main();
		}
		return result;
	}

	public void Main()
	{
	}
}

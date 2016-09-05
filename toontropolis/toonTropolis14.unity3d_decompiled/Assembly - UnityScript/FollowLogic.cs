using Boo.Lang.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class FollowLogic : MonoBehaviour
{
	public string[] FollowActions;

	public int indexAction;

	public float t;

	public float moveSpeed;

	public float rotationSpeed;

	public AutoWayPoint currWayPoint;

	private float jumpSpeed;

	private bool flagIdle;

	private Vector3 moveDirection;

	private int boostIncr;

	private bool canAnimate;

	private int gravity;

	private float nextLoad;

	private float rate;

	private bool run;

	private float pickNextWaypointDistance;

	public PetState petStateScript;

	public GameObject owner;

	public FollowLogic()
	{
		this.t = (float)0;
		this.moveSpeed = (float)7;
		this.rotationSpeed = (float)6;
		this.jumpSpeed = 10f;
		this.flagIdle = false;
		this.moveDirection = Vector3.get_zero();
		this.boostIncr = 1;
		this.gravity = 40;
		this.nextLoad = (float)0;
		this.rate = (float)5;
		this.run = false;
		this.pickNextWaypointDistance = (float)3;
	}

	public void Start()
	{
	}

	public void Update()
	{
	}

	public void Into()
	{
		RaycastHit raycastHit = default(RaycastHit);
		if (!Physics.Linecast(this.get_transform().get_position(), this.owner.get_transform().get_position(), ref raycastHit))
		{
			this.currWayPoint = null;
		}
	}

	public void SetFollowState(object fstate)
	{
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		petState.calledByPlayer = RuntimeServices.UnboxInt32(fstate);
	}

	public void HeelForce()
	{
		float x = this.owner.get_transform().get_position().x + (float)0;
		Vector3 position = this.get_transform().get_position();
		float num = position.x = x;
		Vector3 vector;
		this.get_transform().set_position(vector = position);
		float y = this.owner.get_transform().get_position().y;
		Vector3 position2 = this.get_transform().get_position();
		float num2 = position2.y = y;
		Vector3 vector2;
		this.get_transform().set_position(vector2 = position2);
		float z = this.owner.get_transform().get_position().z + (float)2;
		Vector3 position3 = this.get_transform().get_position();
		float num3 = position3.z = z;
		Vector3 vector3;
		this.get_transform().set_position(vector3 = position3);
	}

	public void FollowAnimation()
	{
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		if (!this.get_animation().IsPlaying("FollowActions[0]"))
		{
			this.get_animation().CrossFade(this.FollowActions[0]);
		}
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 vector = this.get_transform().TransformDirection(Vector3.get_forward());
		Vector3 vector2 = this.get_transform().TransformDirection(Vector3.get_forward());
		Vector3 vector3 = vector2 * this.moveSpeed;
		UnityRuntimeServices.Invoke(this.GetComponent(typeof(CharacterController)), "SimpleMove", new object[]
		{
			vector3
		}, typeof(MonoBehaviour));
		this.RotateTowards(this.owner.get_transform().get_position());
	}

	public void PrintTag(object tagTransform)
	{
		if (RuntimeServices.ToBool(RuntimeServices.Invoke(tagTransform, "CompareTag", new object[]
		{
			"WayPointA"
		})))
		{
			MonoBehaviour.print("FOLLOW: picked WayPointA");
		}
		else if (RuntimeServices.ToBool(RuntimeServices.Invoke(tagTransform, "CompareTag", new object[]
		{
			"WayPointB"
		})))
		{
			MonoBehaviour.print("FOLLOW: picked WayPointB");
		}
		else if (RuntimeServices.ToBool(RuntimeServices.Invoke(tagTransform, "CompareTag", new object[]
		{
			"WayPointC"
		})))
		{
			MonoBehaviour.print("FOLLOW: picked WayPointC");
		}
		else
		{
			MonoBehaviour.print("FOLLOW: compare tag didn't return anything ");
		}
	}

	public void RotateTowards(Vector3 position)
	{
		Vector3 vector = position - this.get_transform().get_position();
		vector.y = (float)0;
		if (vector.get_magnitude() >= 0.1f)
		{
			this.get_transform().set_rotation(Quaternion.Slerp(this.get_transform().get_rotation(), Quaternion.LookRotation(vector), this.rotationSpeed * Time.get_deltaTime()));
			this.get_transform().set_eulerAngles(new Vector3((float)0, this.get_transform().get_eulerAngles().y, (float)0));
		}
	}

	public AutoWayPoint PickNextWaypoint(AutoWayPoint currentWaypoint)
	{
		Vector3 vector = this.get_transform().TransformDirection(Vector3.get_forward());
		AutoWayPoint result = currentWaypoint;
		float num = 10f * (float)-1;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(currentWaypoint.connected);
		while (enumerator.MoveNext())
		{
			AutoWayPoint autoWayPoint = (AutoWayPoint)RuntimeServices.Coerce(enumerator.Current, typeof(AutoWayPoint));
			Vector3 vector2 = Vector3.Normalize(autoWayPoint.get_transform().get_position() - this.get_transform().get_position());
			UnityRuntimeServices.Update(enumerator, autoWayPoint);
			float num2 = Vector3.Dot(vector2, vector);
			if (num2 > num && autoWayPoint != currentWaypoint)
			{
				num = num2;
				result = autoWayPoint;
				UnityRuntimeServices.Update(enumerator, autoWayPoint);
			}
		}
		return result;
	}

	public void SetSpeed(float speed)
	{
		float num = (float)0;
		if (speed > num)
		{
			this.get_animation().CrossFade("run");
		}
		else
		{
			this.get_animation().CrossFade("idle");
		}
	}

	public void Main()
	{
	}
}

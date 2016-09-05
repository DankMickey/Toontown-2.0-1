using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ChickenBeam : MonoBehaviour
{
	public int beamVelocity;

	public GameObject miniExplosion;

	public AudioClip miniExplosionSound;

	public float startTime;

	public bool animStarted;

	public ChickenBeam()
	{
		this.beamVelocity = 100;
	}

	public override void Start()
	{
		this.animStarted = false;
		this.startTime = Time.get_time();
	}

	public override void Go(Vector3 shipVelocity)
	{
		this.get_rigidbody().AddForce(this.get_transform().get_up() * (float)this.beamVelocity * 0.3f, 5);
		this.get_rigidbody().AddForce(this.get_transform().get_forward() * (float)this.beamVelocity, 2);
	}

	public override void FixedUpdate()
	{
		if (Time.get_time() - this.startTime > (float)2)
		{
			if (!this.animStarted)
			{
				object componentInChildren = this.GetComponentInChildren(typeof(Animation));
				UnityRuntimeServices.Invoke(componentInChildren, "Play", new object[]
				{
					"fall"
				}, typeof(MonoBehaviour));
				this.animStarted = true;
			}
			this.get_rigidbody().set_useGravity(true);
			this.get_rigidbody().AddForce((float)-3 * this.get_transform().get_up(), 1);
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		Asteroid asteroid = (Asteroid)other.get_transform().GetComponent(typeof(Asteroid));
		if (asteroid != null)
		{
			MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
			asteroid.Hit();
			GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
			object component = gameObject.GetComponent("Detonator");
			RuntimeServices.SetProperty(component, "size", 1);
			AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
			Object.Destroy(this.get_gameObject());
		}
		if (asteroid == null)
		{
			Asterpinata asterpinata = (Asterpinata)other.get_transform().GetComponent(typeof(Asterpinata));
			if (asterpinata != null)
			{
				MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
				asterpinata.Hit();
				GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
				object component = gameObject.GetComponent("Detonator");
				RuntimeServices.SetProperty(component, "size", 1);
				AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
				Object.Destroy(this.get_gameObject());
			}
		}
		StarPinata starPinata = (StarPinata)other.get_transform().GetComponent(typeof(StarPinata));
		if (starPinata != null)
		{
			MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
			starPinata.Hit();
			GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
			object component = gameObject.GetComponent("Detonator");
			RuntimeServices.SetProperty(component, "size", 1);
			AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
			Object.Destroy(this.get_gameObject());
		}
		YellowHardCandy yellowHardCandy = (YellowHardCandy)other.get_transform().GetComponent(typeof(YellowHardCandy));
		if (yellowHardCandy != null)
		{
			MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
			if (yellowHardCandy.Hit() != 0)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
				object component = gameObject.GetComponent("Detonator");
				RuntimeServices.SetProperty(component, "size", 1);
				AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
				Object.Destroy(this.get_gameObject());
			}
		}
		Lolly lolly = (Lolly)other.get_transform().GetComponent(typeof(Lolly));
		if (lolly != null)
		{
			MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
			if (lolly.Hit() != 0)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
				object component = gameObject.GetComponent("Detonator");
				RuntimeServices.SetProperty(component, "size", 1);
				AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
				Object.Destroy(this.get_gameObject());
			}
		}
		BlueWhistle blueWhistle = (BlueWhistle)other.get_transform().GetComponent(typeof(BlueWhistle));
		if (blueWhistle != null)
		{
			MonoBehaviour.print("BEAM TRIGGER ENTER name = " + other.get_name());
			if (blueWhistle.Hit() != 0)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
				object component = gameObject.GetComponent("Detonator");
				RuntimeServices.SetProperty(component, "size", 1);
				AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), 0.5f);
				Object.Destroy(this.get_gameObject());
			}
		}
	}

	public override void Main()
	{
	}
}

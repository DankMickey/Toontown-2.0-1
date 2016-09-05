using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class EggBeam : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Go$139 : GenericGenerator<WaitForSeconds>
	{
		internal EggBeam $self_$141;

		public $Go$139(EggBeam self_)
		{
			this.$self_$141 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new EggBeam.$Go$139.$(this.$self_$141);
		}
	}

	public int beamVelocity;

	public GameObject miniExplosion;

	public AudioClip miniExplosionSound;

	public EggBeam()
	{
		this.beamVelocity = 100;
	}

	public override IEnumerator Go(Vector3 shipVelocity)
	{
		return new EggBeam.$Go$139(this).GetEnumerator();
	}

	public override void FixedUpdate()
	{
		this.get_rigidbody().AddForce(this.get_transform().get_forward() * (float)this.beamVelocity, 5);
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
			AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
			Object.Destroy(this.get_gameObject());
		}
		else
		{
			if (asteroid == null)
			{
				Asterpinata asterpinata = (Asterpinata)other.get_transform().GetComponent(typeof(Asterpinata));
				if (asterpinata != null)
				{
					asterpinata.Hit();
					GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
					object component = gameObject.GetComponent("Detonator");
					RuntimeServices.SetProperty(component, "size", 1);
					AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
					Object.Destroy(this.get_gameObject());
					return;
				}
			}
			StarPinata starPinata = (StarPinata)other.get_transform().GetComponent(typeof(StarPinata));
			if (starPinata != null)
			{
				starPinata.Hit();
				GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
				object component = gameObject.GetComponent("Detonator");
				RuntimeServices.SetProperty(component, "size", 1);
				AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
				Object.Destroy(this.get_gameObject());
			}
			else
			{
				YellowHardCandy yellowHardCandy = (YellowHardCandy)other.get_transform().GetComponent(typeof(YellowHardCandy));
				if (yellowHardCandy != null && yellowHardCandy.Hit() != 0)
				{
					GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
					object component = gameObject.GetComponent("Detonator");
					RuntimeServices.SetProperty(component, "size", 0.1f);
					AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
					Object.Destroy(this.get_gameObject());
				}
				else
				{
					Lolly lolly = (Lolly)other.get_transform().GetComponent(typeof(Lolly));
					if (lolly != null && lolly.Hit() != 0)
					{
						GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
						object component = gameObject.GetComponent("Detonator");
						RuntimeServices.SetProperty(component, "size", 1);
						AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
						Object.Destroy(this.get_gameObject());
					}
					else
					{
						BlueWhistle blueWhistle = (BlueWhistle)other.get_transform().GetComponent(typeof(BlueWhistle));
						if (blueWhistle != null && blueWhistle.Hit() != 0)
						{
							GameObject gameObject = (GameObject)Object.Instantiate(this.miniExplosion, this.get_transform().get_position(), this.get_transform().get_rotation());
							object component = gameObject.GetComponent("Detonator");
							RuntimeServices.SetProperty(component, "size", 1);
							AudioSource.PlayClipAtPoint(this.miniExplosionSound, this.get_transform().get_position(), (float)1);
							Object.Destroy(this.get_gameObject());
						}
					}
				}
			}
		}
	}

	public override void Main()
	{
	}
}

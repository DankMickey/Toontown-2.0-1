using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Ship : MonoBehaviour
{
	public string keyTurnLeft;

	public string keyTurnRight;

	public string keyThrust;

	public string keyReverseThrust;

	public string keyFire;

	public string manualRestart;

	public Camera daiserCam;

	public float thrustTurnPower;

	public float thrustPower;

	public GameObject boostersPrefab;

	public GameObject boostersPos;

	public GameObject beamPrefab;

	public GameObject chickenBeamPrefab;

	public GameObject missilePrefab;

	private object instanBoosters;

	private bool instanBoostersExists;

	public GameObject asteroidExplosion;

	public AudioClip asteroidExplosionSound;

	public AudioClip asteroidHitSound;

	public GameObject beamOrient;

	public GameObject launchPos;

	public Transform dc;

	public AudioClip shootSound;

	public AudioClip missileSound;

	public AudioClip chickenSound;

	public int upgradeLevel;

	public float prevFireTime;

	public float rapidFireRate;

	public float prevLaunchTime;

	public float launchRate;

	public float daiserRotSpeed;

	private GameObject GUICam;

	private Ray ray;

	private float lastFireTime;

	private int nHits;

	public Transform windshield1;

	public Transform windshield2;

	public AudioClip gameOverSound;

	public float minChickenDelay;

	public float maxChickenDelay;

	private float chickenDelay;

	public float crackDelay;

	private float lastTimeCracked;

	public object astroScript;

	public Ship()
	{
		this.rapidFireRate = 0.1f;
		this.launchRate = 0.5f;
		this.daiserRotSpeed = 1f;
		this.chickenDelay = (float)10;
		this.crackDelay = (float)3;
	}

	public override void Start()
	{
		this.lastTimeCracked = Time.get_time();
		this.nHits = 0;
		float time = Time.get_time();
		this.GUICam = GameObject.Find("GUICamera");
		this.astroScript = this.GetComponentInChildren(typeof(AstronautBehavior));
	}

	public override void ExplodeShip()
	{
		GameObject gameObject = GameObject.Find("Astronaut");
		if (!gameObject)
		{
			MonoBehaviour.print("NO AST");
		}
		object target = Object.Instantiate(this.asteroidExplosion, gameObject.get_transform().get_position(), gameObject.get_transform().get_rotation());
		object target2 = UnityRuntimeServices.Invoke(target, "GetComponent", new object[]
		{
			"Detonator"
		}, typeof(MonoBehaviour));
		RuntimeServices.SetProperty(target2, "size", 22);
		AudioSource.PlayClipAtPoint(this.asteroidExplosionSound, gameObject.get_transform().get_position());
		GameObject gameObject2 = GameObject.Find("CockpitLowerPlane");
		GameObject gameObject3 = GameObject.Find("Windshield1");
		GameObject gameObject4 = GameObject.Find("Windshield2");
		GameObject gameObject5 = GameObject.Find("daiserCylinder");
		Object.Destroy(gameObject2);
		Object.Destroy(gameObject);
		Object.Destroy(gameObject3);
		Object.Destroy(gameObject4);
		Object.Destroy(gameObject5);
	}

	public override void ManualRestart()
	{
		object component = this.GUICam.GetComponent("GameController");
		UnityRuntimeServices.Invoke(component, "SetGameOver", new object[]
		{
			true
		}, typeof(MonoBehaviour));
		UnityRuntimeServices.Invoke(component, "TimedGameRestart", new object[]
		{
			3,
			this.get_transform().get_position()
		}, typeof(MonoBehaviour));
	}

	public override void Update()
	{
		if (Input.GetKey(this.keyTurnLeft))
		{
			this.launchPos.get_transform().Rotate((float)-1 * this.daiserRotSpeed * Vector3.get_up() * Time.get_deltaTime(), 1);
		}
		else if (Input.GetKey(this.keyTurnRight))
		{
			this.launchPos.get_transform().Rotate(this.daiserRotSpeed * Vector3.get_up() * Time.get_deltaTime(), 1);
		}
		else if (Input.GetKey(this.keyThrust))
		{
			this.launchPos.get_transform().Rotate((float)-1 * this.daiserRotSpeed * Vector3.get_right() * Time.get_deltaTime(), 1);
		}
		else if (Input.GetKey(this.keyReverseThrust))
		{
			this.launchPos.get_transform().Rotate(this.daiserRotSpeed * Vector3.get_right() * Time.get_deltaTime(), 1);
		}
		else if (Input.GetKey(this.manualRestart))
		{
			this.ManualRestart();
		}
		if (this.dc)
		{
			float num = (float)(Screen.get_width() / 2);
			float num2 = (float)(Screen.get_height() / 2);
			this.ray = this.daiserCam.get_camera().ScreenPointToRay(new Vector3(Input.get_mousePosition().x, Input.get_mousePosition().y, (float)0));
			Debug.DrawRay(this.ray.get_origin(), this.ray.get_direction() * (float)20, Color.get_yellow());
			float x = this.ray.get_origin().x + this.ray.get_direction().x * (float)20;
			Vector3 position = this.dc.get_transform().get_position();
			float num3 = position.x = x;
			Vector3 vector;
			this.dc.get_transform().set_position(vector = position);
			float y = this.ray.get_origin().y + this.ray.get_direction().y * (float)20;
			Vector3 position2 = this.dc.get_transform().get_position();
			float num4 = position2.y = y;
			Vector3 vector2;
			this.dc.get_transform().set_position(vector2 = position2);
			float z = this.ray.get_origin().z + this.ray.get_direction().z * (float)20;
			Vector3 position3 = this.dc.get_transform().get_position();
			float num5 = position3.z = z;
			Vector3 vector3;
			this.dc.get_transform().set_position(vector3 = position3);
		}
		else
		{
			MonoBehaviour.print("CANNOT FIND DC !!!!!!! ");
		}
		int num6 = this.upgradeLevel;
		if (num6 == 0)
		{
			if (Input.GetKeyDown(this.keyFire))
			{
				this.Fire();
			}
		}
		else if (num6 == 1)
		{
			if (Input.GetKey(this.keyFire))
			{
				this.RapidFire();
			}
		}
		else if (num6 == 2)
		{
			if (Input.GetKey(this.keyFire))
			{
				this.RapidFire();
				this.MissileFire();
			}
		}
		else if (num6 == 3)
		{
			if (Input.GetKey(this.keyFire))
			{
				this.RapidFire();
				this.MissileFire();
				Rocket.Speed = (float)25;
			}
		}
	}

	public override void StartBoosters()
	{
		if (!this.instanBoostersExists)
		{
			this.instanBoosters = Object.Instantiate(this.boostersPrefab, this.boostersPos.get_transform().get_position(), this.boostersPos.get_transform().get_rotation());
			Transform transform = this.boostersPos.get_transform();
			object property = UnityRuntimeServices.GetProperty(this.instanBoosters, "transform");
			RuntimeServices.SetProperty(property, "parent", transform);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(this.instanBoosters, "transform", property)
			});
			this.instanBoostersExists = true;
		}
	}

	public override void Upgrade()
	{
		checked
		{
			this.upgradeLevel++;
			if (this.upgradeLevel > 3)
			{
				this.upgradeLevel = 3;
			}
		}
	}

	public override void StopBoosters()
	{
		if (this.instanBoostersExists)
		{
			ParticleEmitter particleEmitter = (ParticleEmitter)RuntimeServices.Coerce(UnityRuntimeServices.Invoke(this.instanBoosters, "GetComponent", new object[]
			{
				"ParticleEmitter"
			}, typeof(MonoBehaviour)), typeof(ParticleEmitter));
			particleEmitter.set_emit(false);
			Object.Destroy((Object)RuntimeServices.Coerce(this.instanBoosters, typeof(Object)), (float)2);
			this.instanBoostersExists = false;
		}
	}

	public override void Fire()
	{
		this.beamOrient.get_transform().set_rotation(Quaternion.FromToRotation(Vector3.get_forward(), this.ray.get_direction()));
		if (Time.get_time() - this.lastFireTime > this.chickenDelay)
		{
			AudioSource.PlayClipAtPoint(this.chickenSound, this.get_transform().get_position(), (float)1);
			GameObject gameObject = (GameObject)Object.Instantiate(this.chickenBeamPrefab, this.launchPos.get_transform().get_position(), this.beamOrient.get_transform().get_rotation());
			object component = gameObject.GetComponent("ChickenBeam");
			UnityRuntimeServices.Invoke(component, "Go", new object[]
			{
				this.get_rigidbody().get_velocity()
			}, typeof(MonoBehaviour));
			this.lastFireTime = Time.get_time();
			this.chickenDelay = Random.Range(this.minChickenDelay, this.maxChickenDelay);
		}
		else
		{
			AudioSource.PlayClipAtPoint(this.shootSound, this.get_transform().get_position(), (float)1);
			GameObject gameObject = (GameObject)Object.Instantiate(this.beamPrefab, this.launchPos.get_transform().get_position(), this.beamOrient.get_transform().get_rotation());
			object component2 = gameObject.GetComponent("EggBeam");
			UnityRuntimeServices.Invoke(component2, "Go", new object[]
			{
				this.get_rigidbody().get_velocity()
			}, typeof(MonoBehaviour));
		}
	}

	public override void RapidFire()
	{
		float time = Time.get_time();
		if (time - this.prevFireTime > this.rapidFireRate)
		{
			this.Fire();
			this.prevFireTime = time;
		}
	}

	public override void MissileFire()
	{
		float time = Time.get_time();
		if (time - this.prevLaunchTime > this.launchRate)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(this.missilePrefab, this.get_transform().get_position(), this.get_transform().get_rotation());
			this.get_audio().PlayOneShot(this.missileSound);
			this.prevLaunchTime = time;
		}
	}

	public override void OnCollisionEnter(Collision collisionInfo)
	{
		object component = this.GUICam.GetComponent("GameController");
		if (!RuntimeServices.ToBool(RuntimeServices.Invoke(component, "GetGameOver", new object[0])))
		{
			if (RuntimeServices.ToBool(RuntimeServices.Invoke(component, "GetReadyToPlay", new object[0])))
			{
				object component2 = collisionInfo.get_transform().GetComponent("Asterpinata");
				if (!RuntimeServices.ToBool(component2))
				{
					component2 = collisionInfo.get_transform().GetComponent("Asteroid");
				}
				if (!RuntimeServices.ToBool(component2))
				{
					component2 = collisionInfo.get_transform().GetComponent("StarPinata");
				}
				if (!RuntimeServices.EqualityOperator(component2, null))
				{
					MonoBehaviour.print(string.Empty);
					MonoBehaviour.print(string.Empty);
					MonoBehaviour.print(string.Empty);
					MonoBehaviour.print(string.Empty);
					MonoBehaviour.print(RuntimeServices.InvokeBinaryOperator("op_Addition", RuntimeServices.InvokeBinaryOperator("op_Addition", RuntimeServices.InvokeBinaryOperator("op_Addition", "ast collision = ", UnityRuntimeServices.GetProperty(component2, "name")), ", ast.transform.position = "), UnityRuntimeServices.GetProperty(UnityRuntimeServices.GetProperty(component2, "transform"), "position")));
					Vector3 vector = default(Vector3);
					vector.x = this.get_transform().get_position().x + 1.39f;
					vector.y = this.get_transform().get_position().y - 0.244f;
					vector.z = this.get_transform().get_position().z + 19.81f;
					LayerMask layerMask = LayerMask.NameToLayer("ShipLayer");
					Collider[] array = Physics.OverlapSphere(vector, 8.5f);
					MonoBehaviour.print("clist length = " + Extensions.get_length(array));
					int i = default(int);
					bool flag = false;
					checked
					{
						for (i = 0; i < Extensions.get_length(array); i++)
						{
							MonoBehaviour.print(RuntimeServices.InvokeBinaryOperator("op_Addition", "colliders in sphere = " + array[i].get_name() + ", ast.name = ", UnityRuntimeServices.GetProperty(component2, "name")));
							if (RuntimeServices.EqualityOperator(array[i].get_name(), UnityRuntimeServices.GetProperty(component2, "name")))
							{
								flag = true;
							}
						}
					}
					MonoBehaviour.print("time elapsed since last crack = " + (Time.get_time() - this.lastTimeCracked));
					if (Time.get_time() - this.lastTimeCracked >= this.crackDelay && flag)
					{
						flag = true;
						this.lastTimeCracked = Time.get_time();
					}
					else
					{
						flag = false;
					}
					MonoBehaviour.print("okToCrack = " + flag);
					UnityRuntimeServices.Invoke(this.astroScript, "PlayDuck", new object[0], typeof(MonoBehaviour));
					MonoBehaviour.print("Points colliding: " + collisionInfo.get_contacts().get_Length());
					MonoBehaviour.print("SHIP: GOT hit by an asteroid named " + collisionInfo.get_transform().get_name());
					object target = Object.Instantiate(this.asteroidExplosion, collisionInfo.get_transform().get_position(), collisionInfo.get_transform().get_rotation());
					object target2 = UnityRuntimeServices.Invoke(target, "GetComponent", new object[]
					{
						"Detonator"
					}, typeof(MonoBehaviour));
					RuntimeServices.SetProperty(target2, "size", 22);
					AudioSource.PlayClipAtPoint(this.asteroidExplosionSound, collisionInfo.get_transform().get_position());
					Object.Destroy((Object)RuntimeServices.Coerce(UnityRuntimeServices.GetProperty(component2, "gameObject"), typeof(Object)));
					if (flag)
					{
						if (this.nHits <= 1)
						{
							AudioSource.PlayClipAtPoint(this.asteroidHitSound, collisionInfo.get_transform().get_position());
						}
						Vector3 vector2 = default(Vector3);
						vector2.x = collisionInfo.get_contacts()[0].get_point().x - this.get_transform().get_position().x;
						vector2.y = collisionInfo.get_contacts()[0].get_point().y - this.get_transform().get_position().y;
						MonoBehaviour.print("locPos = " + vector2);
						if (vector2.x < -1.3f)
						{
							vector2.x = -1.3f;
						}
						else if (vector2.x > 1.3f)
						{
							vector2.x = 1.3f;
						}
						if (vector2.y < (float)-2)
						{
							vector2.y = (float)-2;
						}
						else if (vector2.y > 1.8f)
						{
							vector2.y = 1.8f;
						}
						checked
						{
							if (this.nHits == 0)
							{
								float x = vector2.x;
								Vector3 localPosition = this.windshield1.get_localPosition();
								float num = localPosition.x = x;
								Vector3 vector3;
								this.windshield1.set_localPosition(vector3 = localPosition);
								float y = vector2.y;
								Vector3 localPosition2 = this.windshield1.get_localPosition();
								float num2 = localPosition2.y = y;
								Vector3 vector4;
								this.windshield1.set_localPosition(vector4 = localPosition2);
								this.windshield1.get_renderer().set_enabled(true);
								MonoBehaviour.print("windshield1.localPosition.x = " + this.windshield1.get_localPosition().x + ", y= " + this.windshield1.get_localPosition().y);
								this.nHits++;
							}
							else if (this.nHits == 1)
							{
								float x2 = vector2.x;
								Vector3 localPosition3 = this.windshield2.get_localPosition();
								float num3 = localPosition3.x = x2;
								Vector3 vector5;
								this.windshield2.set_localPosition(vector5 = localPosition3);
								float y2 = vector2.y;
								Vector3 localPosition4 = this.windshield2.get_localPosition();
								float num4 = localPosition4.y = y2;
								Vector3 vector6;
								this.windshield2.set_localPosition(vector6 = localPosition4);
								this.windshield2.get_renderer().set_enabled(true);
								MonoBehaviour.print("windshield2.localPosition.x = " + this.windshield2.get_localPosition().x + ", y= " + this.windshield2.get_localPosition().y);
								this.nHits++;
							}
							else
							{
								UnityRuntimeServices.Invoke(component, "SetGameOver", new object[]
								{
									true
								}, typeof(MonoBehaviour));
								UnityRuntimeServices.Invoke(component, "TimedGameRestart", new object[]
								{
									5,
									this.get_transform().get_position()
								}, typeof(MonoBehaviour));
								GameController.instance.gameLives = GameController.instance.gameLives - 1;
								GameController.instance.gameEnemies = GameController.instance.gameEnemies - 1;
							}
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

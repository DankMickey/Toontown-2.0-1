using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class AsteroidBandGenerator : MonoBehaviour
{
	public Asteroid asteroidPrefab;

	public Asterpinata asterpinataPrefab;

	public StarPinata starPinataPrefab;

	public float worldSize;

	public int maxAsteroidSize;

	public int minAsteroidSize;

	public int asteroidCount;

	public PowerUp powerUpPrefab;

	public int nLayers;

	public float xDim;

	public float yDim;

	public float zDim;

	public int maxAsteroidCount;

	private float xDensity;

	private float yDensity;

	private float zDensity;

	public Vector3 offsetXYZ;

	public int xDivisions;

	public int yDivisions;

	public int zDivisions;

	public bool manualVelocityScale;

	public float velocityScale;

	public bool astCreated;

	private float genStart;

	public float genDelay;

	private int bandCount;

	public AsteroidBandGenerator()
	{
		this.nLayers = 3;
		this.xDim = 50f;
		this.yDim = 50f;
		this.zDim = 50f;
		this.maxAsteroidCount = 300;
		this.xDivisions = 5;
		this.yDivisions = 5;
		this.zDivisions = 5;
		this.manualVelocityScale = true;
		this.velocityScale = 1f;
		this.genDelay = 2000f;
	}

	public override void Start()
	{
		this.genStart = Time.get_time();
	}

	public override void FixedUpdate()
	{
		float num = Time.get_time() - this.genStart;
		checked
		{
			if (num > this.genDelay)
			{
				MonoBehaviour.print("Gen time elapsed = " + num);
				this.GenerateLevel();
				this.genStart = Time.get_time();
				this.bandCount++;
			}
		}
	}

	public override void GenerateLevel()
	{
		int num = 0;
		float num2 = (float)20;
		float num3 = default(float);
		float num4 = default(float);
		float num5 = default(float);
		float num6 = default(float);
		float num7 = default(float);
		float num8 = default(float);
		float num9 = default(float);
		float num10 = (float)150;
		float num11 = this.worldSize * 0.5f;
		float num12 = default(float);
		num12 = num10 / this.xDim;
		this.xDensity = this.xDim / (float)this.xDivisions;
		this.yDensity = this.yDim / (float)this.yDivisions;
		this.zDensity = this.zDim / (float)this.zDivisions;
		num3 = (float)0;
		Vector3 vector = default(Vector3);
		num4 = (float)0;
		MonoBehaviour.print("\tyDivisions = " + this.yDivisions);
		while (num4 < (float)this.yDivisions)
		{
			for (int i = 0; i < this.xDivisions; i = checked(i + 1))
			{
				float num13 = (float)Random.Range(this.minAsteroidSize, this.maxAsteroidSize);
				if (!this.manualVelocityScale)
				{
					this.velocityScale = (float)GameController.instance.gameLevel * 1.25f;
				}
				vector.x = this.get_transform().get_position().x - this.xDim / (float)2 + (float)i * this.xDensity + Random.Range(-0.125f * num11, 0.125f * num11);
				vector.y = this.get_transform().get_position().y - this.yDim / (float)2 + this.offsetXYZ.get_Item(1) + num4 * this.yDensity + Random.Range(-0.125f * num11, 0.125f * num11);
				vector.z = this.get_transform().get_position().z + this.offsetXYZ.get_Item(2) + Random.Range(-0.5f * num11, 0.5f * num11);
				Renderer renderer = (Renderer)this.asterpinataPrefab.GetComponent(typeof(Renderer));
				float num14 = (renderer.get_bounds().get_max() - renderer.get_bounds().get_min()).get_magnitude() * 0.5f;
				num14 *= num13;
				float num15 = default(float);
				bool flag = true;
				if (!Physics.CheckSphere(vector, num14 * 2f) && flag)
				{
					int num16 = Random.Range(0, 3);
					if (num16 == 0)
					{
						Asteroid asteroid = (Asteroid)Object.Instantiate(this.asteroidPrefab);
						asteroid.get_transform().set_position(vector);
						asteroid.Setup(num14, this.velocityScale * (float)2);
						checked
						{
							num++;
							asteroid.set_name("donkeypinata" + num);
							if (vector.x > (float)0)
							{
								asteroid.get_rigidbody().AddForce((float)500 * new Vector3(-0.1f, (float)0, (float)-1), 1);
							}
							else
							{
								asteroid.get_rigidbody().AddForce((float)500 * new Vector3(0.1f, (float)0, (float)-1), 1);
							}
						}
					}
					else if (num16 == 1)
					{
						Asterpinata asterpinata = (Asterpinata)Object.Instantiate(this.asterpinataPrefab);
						asterpinata.get_transform().set_position(vector);
						asterpinata.Setup(num14, this.velocityScale * (float)4);
						checked
						{
							num++;
							asterpinata.set_name("asterpinata" + num);
							if (vector.x > (float)0)
							{
								asterpinata.get_rigidbody().AddForce((float)500 * new Vector3(-0.1f, (float)0, (float)-1), 1);
							}
							else
							{
								asterpinata.get_rigidbody().AddForce((float)500 * new Vector3(0.1f, (float)0, (float)-1), 1);
							}
						}
					}
					else if (num16 == 2)
					{
						StarPinata starPinata = (StarPinata)Object.Instantiate(this.starPinataPrefab);
						starPinata.get_transform().set_position(vector);
						starPinata.Setup(num14, this.velocityScale * (float)3);
						checked
						{
							num++;
							starPinata.set_name("starpinata" + num);
							if (vector.x > (float)0)
							{
								starPinata.get_rigidbody().AddForce((float)500 * new Vector3(-0.1f, (float)0, (float)-1), 1);
							}
							else
							{
								starPinata.get_rigidbody().AddForce((float)500 * new Vector3(0.1f, (float)0, (float)-1), 1);
							}
						}
					}
					if (num > this.maxAsteroidCount)
					{
						MonoBehaviour.print("too many spawned asteroids  = " + num);
						return;
					}
				}
			}
			num4 += (float)1;
		}
		MonoBehaviour.print("ASTEROID band generated asteroids  = " + num);
	}

	public override void SpawnPowerUp()
	{
		bool flag = false;
		int num = 0;
		int num2 = 10000;
		while (!flag)
		{
			object obj = this.worldSize * 0.5f;
			object value = Mathf.Lerp(RuntimeServices.UnboxSingle(RuntimeServices.InvokeUnaryOperator("op_UnaryNegation", obj)), RuntimeServices.UnboxSingle(obj), Random.get_value());
			object value2 = Mathf.Lerp(RuntimeServices.UnboxSingle(RuntimeServices.InvokeUnaryOperator("op_UnaryNegation", obj)), RuntimeServices.UnboxSingle(obj), Random.get_value());
			Renderer renderer = (Renderer)this.powerUpPrefab.GetComponent(typeof(Renderer));
			float num3 = (renderer.get_bounds().get_max() - renderer.get_bounds().get_min()).get_magnitude() * 0.5f;
			if (!Physics.CheckSphere(new Vector3(RuntimeServices.UnboxSingle(value), (float)0, RuntimeServices.UnboxSingle(value2)), num3 * (float)2))
			{
				PowerUp powerUp = (PowerUp)Object.Instantiate(this.powerUpPrefab);
				powerUp.get_transform().set_position(new Vector3(RuntimeServices.UnboxSingle(value), (float)0, RuntimeServices.UnboxSingle(value2)));
				flag = true;
			}
			checked
			{
				num++;
				if (num > num2)
				{
					break;
				}
			}
		}
	}

	public override void DestroyAllAsteroids()
	{
	}

	public override void Main()
	{
	}
}

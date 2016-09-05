using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class AsteroidField : MonoBehaviour
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

	public AsteroidField()
	{
		this.nLayers = 3;
		this.xDim = 50f;
		this.yDim = 50f;
		this.zDim = 50f;
		this.maxAsteroidCount = 50;
		this.xDivisions = 5;
		this.yDivisions = 5;
		this.zDivisions = 5;
		this.manualVelocityScale = true;
		this.velocityScale = 1f;
	}

	public override void Start()
	{
		this.GenerateLevel();
	}

	public override void GenerateLevel()
	{
		int num = 0;
		this.asteroidCount = checked(5 + GameController.instance.gameLevel * 5);
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
		MonoBehaviour.print("MAKE A BLOODY ASTEROID FIELD ALREADY!!!!");
		num12 = num10 / this.xDim;
		this.xDensity = this.xDim / (float)this.xDivisions;
		this.yDensity = this.yDim / (float)this.yDivisions;
		this.zDensity = this.zDim / (float)this.zDivisions;
		num3 = (float)0;
		Vector3 vector = default(Vector3);
		while (num3 < (float)this.zDivisions)
		{
			MonoBehaviour.print("zCount = " + num3);
			float num13 = (float)this.yDivisions + num3 * num12;
			num7 = num13 * this.yDensity;
			num4 = (float)0;
			if (num3 == (float)0)
			{
				MonoBehaviour.print("\tyDivisions = " + this.yDivisions);
			}
			if (num3 == (float)0)
			{
				MonoBehaviour.print("\tyDimInc = " + num7);
			}
			if (num3 == (float)0)
			{
				MonoBehaviour.print("\tyDivisionsInc = " + num13);
			}
			if (num3 == (float)0)
			{
				MonoBehaviour.print("\tyDim = " + this.yDim);
			}
			while (num4 < num13)
			{
				int num14 = 0;
				float num15 = (float)this.xDivisions + num3 * num12;
				num9 = num15 * this.xDensity;
				if (num3 == (float)0)
				{
					MonoBehaviour.print("\t\tyCount = " + num4);
				}
				if (num3 == (float)0)
				{
					MonoBehaviour.print("\t\txDimInc = " + num9);
				}
				if (num3 == (float)0)
				{
					MonoBehaviour.print("\t\txDivisionsInc = " + num15);
				}
				while ((float)num14 < num15)
				{
					if (num3 == (float)0)
					{
						MonoBehaviour.print("\t\txCount = " + num14);
					}
					float num16 = (float)Random.Range(this.minAsteroidSize, this.maxAsteroidSize);
					if (num3 == (float)0)
					{
						num16 = (float)this.minAsteroidSize;
					}
					if (!this.manualVelocityScale)
					{
						this.velocityScale = (float)GameController.instance.gameLevel * 1.25f;
					}
					vector.x = this.get_transform().get_position().x - num9 / (float)2 + (float)num14 * this.xDensity + Random.Range(-0.125f * num11, 0.125f * num11);
					vector.y = this.get_transform().get_position().y - num7 / (float)2 + this.offsetXYZ.get_Item(1) + num4 * this.yDensity + Random.Range(-0.125f * num11, 0.125f * num11);
					vector.z = this.get_transform().get_position().z + this.offsetXYZ.get_Item(2) + num3 * this.zDensity + Random.Range(-num11, num11);
					if (num3 == (float)0)
					{
						MonoBehaviour.print("\t\tzCount = " + num3 + ", xPos = " + vector.x + ", yPos = " + vector.y + ", zPos = " + vector.z);
					}
					Renderer renderer = (Renderer)this.asterpinataPrefab.GetComponent(typeof(Renderer));
					float num17 = (renderer.get_bounds().get_max() - renderer.get_bounds().get_min()).get_magnitude() * 0.5f;
					if (num3 == (float)0)
					{
						MonoBehaviour.print("renderer radius = " + num17 + ", rand = " + num16);
					}
					num17 *= num16;
					float num18 = default(float);
					bool flag = true;
					float num19 = 6f;
					if (num3 == (float)0)
					{
						num19 = 1f;
						MonoBehaviour.print("XXXXXX final radius = " + num17);
					}
					checked
					{
						if (!Physics.CheckSphere(vector, unchecked(num17 * num19)) && flag)
						{
							int num20 = Random.Range(0, 3);
							if (num20 == 0)
							{
								Asteroid asteroid = (Asteroid)Object.Instantiate(this.asteroidPrefab);
								asteroid.get_transform().set_position(vector);
								asteroid.Setup(num17, this.velocityScale);
								num++;
								asteroid.set_name("donkeypinata" + num);
							}
							else if (num20 == 1)
							{
								Asterpinata asterpinata = (Asterpinata)Object.Instantiate(this.asterpinataPrefab);
								asterpinata.get_transform().set_position(vector);
								asterpinata.Setup(num17, this.velocityScale);
								num++;
								asterpinata.set_name("asterpinata" + num);
							}
							else if (num20 == 2)
							{
								StarPinata starPinata = (StarPinata)Object.Instantiate(this.starPinataPrefab);
								starPinata.get_transform().set_position(vector);
								starPinata.Setup(num17, this.velocityScale);
								num++;
								starPinata.set_name("starpinata" + num);
							}
							if (num > this.maxAsteroidCount)
							{
								MonoBehaviour.print("too many spawned asteroids  = " + num);
								return;
							}
							if (num3 == (float)0)
							{
								MonoBehaviour.print("spawned asteroids  = " + num);
							}
						}
						else if (num3 == (float)0)
						{
							MonoBehaviour.print("CHECK SPHERE : did not spawn asteroid");
						}
						num14++;
					}
				}
				num4 += (float)1;
			}
			num3 += (float)1;
		}
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

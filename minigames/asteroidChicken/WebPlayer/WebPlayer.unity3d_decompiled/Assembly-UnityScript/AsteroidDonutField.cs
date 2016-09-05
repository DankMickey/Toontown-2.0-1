using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class AsteroidDonutField : MonoBehaviour
{
	public Asteroid asteroidPrefab;

	public Asterpinata asterpinataPrefab;

	public float worldSize;

	public int maxAsteroidSize;

	public int minAsteroidSize;

	public int asteroidCount;

	public PowerUp powerUpPrefab;

	public int nLayers;

	public float innerRadius;

	public float outerRadius;

	private float radialDensity;

	private float arcDensity;

	public Vector3 offsetXYZ;

	public int radialDivisions;

	public int arcDivisions;

	public bool manualVelocityScale;

	public float velocityScale;

	public AsteroidDonutField()
	{
		this.nLayers = 3;
		this.innerRadius = 10f;
		this.outerRadius = 20f;
		this.radialDivisions = 5;
		this.arcDivisions = 5;
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
		int i = 0;
		float num6 = default(float);
		if (num4 < (float)5)
		{
			num4 = (float)5;
		}
		if (num5 < (float)4)
		{
			num5 = (float)5;
		}
		num4 = (float)(360 / this.radialDivisions);
		num5 = (this.outerRadius - this.innerRadius) / (float)this.arcDivisions;
		while (i < this.nLayers)
		{
			for (num2 = (float)0; num2 < (float)360; num2 += num4)
			{
				float num7 = this.worldSize * 0.5f;
				for (num3 = this.innerRadius; num3 < this.outerRadius; num3 += num5)
				{
					int num8 = Random.Range(this.maxAsteroidSize, this.minAsteroidSize);
					if (!this.manualVelocityScale)
					{
						this.velocityScale = (float)GameController.instance.gameLevel * 1.25f;
					}
					float num9 = num3 * Mathf.Cos(num2);
					float num10 = num3 * Mathf.Sin(num2);
					num6 = (float)(checked(i * 10));
					num9 += Mathf.Lerp(-num7, num7, Random.get_value());
					num6 = Mathf.Lerp(-this.worldSize, this.worldSize, Random.get_value());
					num10 += this.offsetXYZ.get_Item(2) + Mathf.Lerp(-num7, num7, Random.get_value());
					Renderer renderer = (Renderer)this.asteroidPrefab.GetComponent(typeof(Renderer));
					float num11 = (renderer.get_bounds().get_max() - renderer.get_bounds().get_min()).get_magnitude() * (float)num8 * 0.5f;
					float num12 = default(float);
					bool flag = true;
					if (num9 >= (float)0 && num10 >= (float)0)
					{
						num12 = Mathf.Atan2(Mathf.Abs(num10), Mathf.Abs(num9)) * 57.29578f;
						if (num12 < (float)10)
						{
							flag = false;
						}
					}
					else if (num9 >= (float)0 && num10 < (float)0)
					{
						num12 = Mathf.Atan2(Mathf.Abs(num10), Mathf.Abs(num9)) * 57.29578f;
						if (num12 < (float)5)
						{
							flag = false;
						}
					}
					if (!Physics.CheckSphere(new Vector3(num9, num6, num10), num11 * (float)2) && flag)
					{
						if (Random.Range(0, 2) == 0)
						{
							Asteroid asteroid = (Asteroid)Object.Instantiate(this.asteroidPrefab);
							asteroid.get_transform().set_position(new Vector3(num9, num6, num10));
							asteroid.get_transform().set_parent(this.get_transform());
							asteroid.Setup((float)num8, this.velocityScale);
						}
						else
						{
							Asterpinata asterpinata = (Asterpinata)Object.Instantiate(this.asterpinataPrefab);
							asterpinata.get_transform().set_position(new Vector3(num9, num6, num10));
							asterpinata.get_transform().set_parent(this.get_transform());
							asterpinata.Setup((float)num8, this.velocityScale / (float)5);
						}
						checked
						{
							num++;
						}
					}
				}
			}
			checked
			{
				i++;
			}
		}
		this.SpawnPowerUp();
		GameObject gameObject = GameObject.Find("Radar");
		UnityRuntimeServices.Invoke(gameObject.GetComponent(typeof(Radar)), "SetAsteroidDonutField", new object[]
		{
			this
		}, typeof(MonoBehaviour));
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

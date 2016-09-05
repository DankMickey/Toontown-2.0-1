using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[ExecuteInEditMode]
[Serializable]
public class Radar : MonoBehaviour
{
	public Texture radarTexture;

	public Texture playerTexture;

	public Texture enemyTexture;

	public float mapSize;

	public Vector2 mapCenter;

	public float worldSize;

	public Transform playerPosition;

	public Transform powerUpPosition;

	private AsteroidDonutField asteroidDonutField;

	public static bool lookForPowerUp;

	public override void Start()
	{
		this.mapSize = (float)(checked(Screen.get_width() * 15)) / 100f;
		this.mapCenter = new Vector2(this.mapSize / (float)2, (float)Screen.get_height() - this.mapSize / (float)2);
		GameObject gameObject = GameObject.Find("AsteroidDonutField");
		if (this.asteroidDonutField)
		{
			this.worldSize = this.asteroidDonutField.outerRadius;
		}
	}

	public override void SetAsteroidDonutField(AsteroidDonutField adf)
	{
		this.asteroidDonutField = adf;
		this.worldSize = this.asteroidDonutField.outerRadius;
	}

	public override void SetWorldSize()
	{
	}

	public override void OnGUI()
	{
		if (this.playerPosition == null)
		{
			object obj = Object.FindObjectOfType(typeof(Ship));
			if (RuntimeServices.EqualityOperator(obj, null))
			{
				return;
			}
			this.playerPosition = (Transform)RuntimeServices.Coerce(UnityRuntimeServices.GetProperty(obj, "transform"), typeof(Transform));
		}
		if (this.powerUpPosition == null && Radar.lookForPowerUp)
		{
			object obj2 = Object.FindObjectOfType(typeof(PowerUp));
			if (!RuntimeServices.EqualityOperator(obj2, null))
			{
				this.powerUpPosition = (Transform)RuntimeServices.Coerce(UnityRuntimeServices.GetProperty(obj2, "transform"), typeof(Transform));
			}
		}
		GUI.DrawTexture(new Rect(this.mapCenter.x - this.mapSize / (float)2, this.mapCenter.y - this.mapSize / (float)2, this.mapSize, this.mapSize), this.radarTexture);
		this.DrawPlayer();
		this.DrawEnemies();
		this.DrawPowerUp();
	}

	public override void DrawPlayer()
	{
		this.DrawBlip(this.playerPosition.get_position(), this.playerTexture, 6);
	}

	public override void DrawPowerUp()
	{
		if (this.powerUpPosition != null)
		{
			this.DrawBlip(this.powerUpPosition.get_position(), this.playerTexture, 10);
		}
	}

	public override void DrawEnemies()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		int i = 0;
		GameObject[] array2 = array;
		int length = array2.get_Length();
		checked
		{
			while (i < length)
			{
				this.DrawBlip(array2[i].get_transform().get_position(), this.enemyTexture, 3);
				i++;
			}
		}
	}

	public override void DrawBlip(Vector3 pos, Texture tex, int size)
	{
		float num = pos.x / this.worldSize;
		float num2 = pos.z / this.worldSize;
		num2 = -num2;
		if (num > -0.5f && num < 0.5f && num2 > -0.5f && num2 < 0.5f)
		{
			num *= this.mapSize * 0.9f;
			num2 *= this.mapSize * 0.72f;
			GUI.DrawTexture(new Rect(this.mapCenter.x + num, this.mapCenter.y + num2, (float)size, (float)size), tex);
		}
	}

	public override void Main()
	{
	}
}

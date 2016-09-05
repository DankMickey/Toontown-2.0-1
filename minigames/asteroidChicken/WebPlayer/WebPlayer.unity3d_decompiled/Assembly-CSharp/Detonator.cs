using System;
using UnityEngine;

[AddComponentMenu("Detonator/Detonator")]
public class Detonator : MonoBehaviour
{
	private static float _baseSize = 30f;

	private static Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	private static float _baseDuration = 3f;

	public float size = 10f;

	public Color color = Detonator._baseColor;

	public bool explodeOnStart = true;

	public float duration = Detonator._baseDuration;

	public float detail = 1f;

	public float destroyTime = 7f;

	public Material fireballAMaterial;

	public Material fireballBMaterial;

	public Material smokeAMaterial;

	public Material smokeBMaterial;

	public Material shockwaveMaterial;

	public Material sparksMaterial;

	public Material glowMaterial;

	public Material heatwaveMaterial;

	private Component[] components;

	private DetonatorFireball _fireball;

	private DetonatorSparks _sparks;

	private DetonatorShockwave _shockwave;

	private DetonatorSmoke _smoke;

	private DetonatorGlow _glow;

	private DetonatorLight _light;

	private DetonatorForce _force;

	private DetonatorHeatwave _heatwave;

	public bool autoCreateFireball = true;

	public bool autoCreateSparks = true;

	public bool autoCreateShockwave = true;

	public bool autoCreateSmoke = true;

	public bool autoCreateGlow = true;

	public bool autoCreateLight = true;

	public bool autoCreateForce = true;

	public bool autoCreateHeatwave;

	private float _lastExplosionTime = 1000f;

	private bool _firstComponentUpdate = true;

	private Component[] _subDetonators;

	public static Material defaultFireballAMaterial;

	public static Material defaultFireballBMaterial;

	public static Material defaultSmokeAMaterial;

	public static Material defaultSmokeBMaterial;

	public static Material defaultShockwaveMaterial;

	public static Material defaultSparksMaterial;

	public static Material defaultGlowMaterial;

	public static Material defaultHeatwaveMaterial;

	private void Awake()
	{
		this.FillDefaultMaterials();
		this.components = base.GetComponents(typeof(DetonatorComponent));
		Component[] array = this.components;
		for (int i = 0; i < array.Length; i++)
		{
			DetonatorComponent detonatorComponent = (DetonatorComponent)array[i];
			if (detonatorComponent is DetonatorFireball)
			{
				this._fireball = (detonatorComponent as DetonatorFireball);
			}
			if (detonatorComponent is DetonatorSparks)
			{
				this._sparks = (detonatorComponent as DetonatorSparks);
			}
			if (detonatorComponent is DetonatorShockwave)
			{
				this._shockwave = (detonatorComponent as DetonatorShockwave);
			}
			if (detonatorComponent is DetonatorSmoke)
			{
				this._smoke = (detonatorComponent as DetonatorSmoke);
			}
			if (detonatorComponent is DetonatorGlow)
			{
				this._glow = (detonatorComponent as DetonatorGlow);
			}
			if (detonatorComponent is DetonatorLight)
			{
				this._light = (detonatorComponent as DetonatorLight);
			}
			if (detonatorComponent is DetonatorForce)
			{
				this._force = (detonatorComponent as DetonatorForce);
			}
			if (detonatorComponent is DetonatorHeatwave)
			{
				this._heatwave = (detonatorComponent as DetonatorHeatwave);
			}
		}
		if (!this._fireball && this.autoCreateFireball)
		{
			this._fireball = (base.get_gameObject().AddComponent("DetonatorFireball") as DetonatorFireball);
			this._fireball.Reset();
		}
		if (!this._smoke && this.autoCreateSmoke)
		{
			this._smoke = (base.get_gameObject().AddComponent("DetonatorSmoke") as DetonatorSmoke);
			this._smoke.Reset();
		}
		if (!this._sparks && this.autoCreateSparks)
		{
			this._sparks = (base.get_gameObject().AddComponent("DetonatorSparks") as DetonatorSparks);
			this._sparks.Reset();
		}
		if (!this._shockwave && this.autoCreateShockwave)
		{
			this._shockwave = (base.get_gameObject().AddComponent("DetonatorShockwave") as DetonatorShockwave);
			this._shockwave.Reset();
		}
		if (!this._glow && this.autoCreateGlow)
		{
			this._glow = (base.get_gameObject().AddComponent("DetonatorGlow") as DetonatorGlow);
			this._glow.Reset();
		}
		if (!this._light && this.autoCreateLight)
		{
			this._light = (base.get_gameObject().AddComponent("DetonatorLight") as DetonatorLight);
			this._light.Reset();
		}
		if (!this._force && this.autoCreateForce)
		{
			this._force = (base.get_gameObject().AddComponent("DetonatorForce") as DetonatorForce);
			this._force.Reset();
		}
		if (!this._heatwave && this.autoCreateHeatwave && SystemInfo.get_supportsImageEffects())
		{
			this._heatwave = (base.get_gameObject().AddComponent("DetonatorHeatwave") as DetonatorHeatwave);
			this._heatwave.Reset();
		}
		this.components = base.GetComponents(typeof(DetonatorComponent));
	}

	private void FillDefaultMaterials()
	{
		if (!this.fireballAMaterial)
		{
			this.fireballAMaterial = Detonator.DefaultFireballAMaterial();
		}
		if (!this.fireballBMaterial)
		{
			this.fireballBMaterial = Detonator.DefaultFireballBMaterial();
		}
		if (!this.smokeAMaterial)
		{
			this.smokeAMaterial = Detonator.DefaultSmokeAMaterial();
		}
		if (!this.smokeBMaterial)
		{
			this.smokeBMaterial = Detonator.DefaultSmokeBMaterial();
		}
		if (!this.shockwaveMaterial)
		{
			this.shockwaveMaterial = Detonator.DefaultShockwaveMaterial();
		}
		if (!this.sparksMaterial)
		{
			this.sparksMaterial = Detonator.DefaultSparksMaterial();
		}
		if (!this.glowMaterial)
		{
			this.glowMaterial = Detonator.DefaultGlowMaterial();
		}
		if (!this.heatwaveMaterial)
		{
			this.heatwaveMaterial = Detonator.DefaultHeatwaveMaterial();
		}
	}

	private void Start()
	{
		if (this.explodeOnStart)
		{
			this.UpdateComponents();
			this.Explode();
		}
	}

	private void Update()
	{
		if (this.destroyTime > 0f && this._lastExplosionTime + this.destroyTime <= Time.get_time())
		{
			Object.Destroy(base.get_gameObject());
		}
	}

	private void UpdateComponents()
	{
		if (this._firstComponentUpdate)
		{
			Component[] array = this.components;
			for (int i = 0; i < array.Length; i++)
			{
				DetonatorComponent detonatorComponent = (DetonatorComponent)array[i];
				detonatorComponent.Init();
				detonatorComponent.SetStartValues();
			}
			this._firstComponentUpdate = false;
		}
		if (!this._firstComponentUpdate)
		{
			Component[] array2 = this.components;
			for (int j = 0; j < array2.Length; j++)
			{
				DetonatorComponent detonatorComponent2 = (DetonatorComponent)array2[j];
				if (detonatorComponent2.detonatorControlled)
				{
					detonatorComponent2.size = detonatorComponent2.startSize * (this.size / Detonator._baseSize);
					detonatorComponent2.timeScale = this.duration / Detonator._baseDuration;
					detonatorComponent2.detail = detonatorComponent2.startDetail * this.detail;
					detonatorComponent2.color = Color.Lerp(detonatorComponent2.startColor, this.color, this.color.a);
				}
			}
		}
	}

	public void Explode()
	{
		this._lastExplosionTime = Time.get_time();
		Component[] array = this.components;
		for (int i = 0; i < array.Length; i++)
		{
			DetonatorComponent detonatorComponent = (DetonatorComponent)array[i];
			this.UpdateComponents();
			detonatorComponent.Explode();
		}
	}

	public void Reset()
	{
		this.size = Detonator._baseSize;
		this.color = Detonator._baseColor;
		this.duration = Detonator._baseDuration;
		this.FillDefaultMaterials();
	}

	public static Material DefaultFireballAMaterial()
	{
		if (Detonator.defaultFireballAMaterial != null)
		{
			return Detonator.defaultFireballAMaterial;
		}
		Detonator.defaultFireballAMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultFireballAMaterial.set_name("FireballA-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		Detonator.defaultFireballAMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultFireballAMaterial.set_mainTexture(mainTexture);
		Detonator.defaultFireballAMaterial.set_mainTextureScale(new Vector2(0.5f, 1f));
		return Detonator.defaultFireballAMaterial;
	}

	public static Material DefaultFireballBMaterial()
	{
		if (Detonator.defaultFireballBMaterial != null)
		{
			return Detonator.defaultFireballBMaterial;
		}
		Detonator.defaultFireballBMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultFireballBMaterial.set_name("FireballB-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		Detonator.defaultFireballBMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultFireballBMaterial.set_mainTexture(mainTexture);
		Detonator.defaultFireballBMaterial.set_mainTextureScale(new Vector2(0.5f, 1f));
		Detonator.defaultFireballBMaterial.set_mainTextureOffset(new Vector2(0.5f, 0f));
		return Detonator.defaultFireballBMaterial;
	}

	public static Material DefaultSmokeAMaterial()
	{
		if (Detonator.defaultSmokeAMaterial != null)
		{
			return Detonator.defaultSmokeAMaterial;
		}
		Detonator.defaultSmokeAMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
		Detonator.defaultSmokeAMaterial.set_name("SmokeA-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		Detonator.defaultSmokeAMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultSmokeAMaterial.set_mainTexture(mainTexture);
		Detonator.defaultSmokeAMaterial.set_mainTextureScale(new Vector2(0.5f, 1f));
		return Detonator.defaultSmokeAMaterial;
	}

	public static Material DefaultSmokeBMaterial()
	{
		if (Detonator.defaultSmokeBMaterial != null)
		{
			return Detonator.defaultSmokeBMaterial;
		}
		Detonator.defaultSmokeBMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
		Detonator.defaultSmokeBMaterial.set_name("SmokeB-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		Detonator.defaultSmokeBMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultSmokeBMaterial.set_mainTexture(mainTexture);
		Detonator.defaultSmokeBMaterial.set_mainTextureScale(new Vector2(0.5f, 1f));
		Detonator.defaultSmokeBMaterial.set_mainTextureOffset(new Vector2(0.5f, 0f));
		return Detonator.defaultSmokeBMaterial;
	}

	public static Material DefaultSparksMaterial()
	{
		if (Detonator.defaultSparksMaterial != null)
		{
			return Detonator.defaultSparksMaterial;
		}
		Detonator.defaultSparksMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultSparksMaterial.set_name("Sparks-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/GlowDot") as Texture2D;
		Detonator.defaultSparksMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultSparksMaterial.set_mainTexture(mainTexture);
		return Detonator.defaultSparksMaterial;
	}

	public static Material DefaultShockwaveMaterial()
	{
		if (Detonator.defaultShockwaveMaterial != null)
		{
			return Detonator.defaultShockwaveMaterial;
		}
		Detonator.defaultShockwaveMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultShockwaveMaterial.set_name("Shockwave-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Shockwave") as Texture2D;
		Detonator.defaultShockwaveMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultShockwaveMaterial.set_mainTexture(mainTexture);
		return Detonator.defaultShockwaveMaterial;
	}

	public static Material DefaultGlowMaterial()
	{
		if (Detonator.defaultGlowMaterial != null)
		{
			return Detonator.defaultGlowMaterial;
		}
		Detonator.defaultGlowMaterial = new Material(Shader.Find("Particles/Additive"));
		Detonator.defaultGlowMaterial.set_name("Glow-Default");
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Glow") as Texture2D;
		Detonator.defaultGlowMaterial.SetColor("_TintColor", Color.get_white());
		Detonator.defaultGlowMaterial.set_mainTexture(mainTexture);
		return Detonator.defaultGlowMaterial;
	}

	public static Material DefaultHeatwaveMaterial()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			return null;
		}
		if (Detonator.defaultHeatwaveMaterial != null)
		{
			return Detonator.defaultHeatwaveMaterial;
		}
		Detonator.defaultHeatwaveMaterial = new Material(Shader.Find("HeatDistort"));
		Detonator.defaultHeatwaveMaterial.set_name("Heatwave-Default");
		Texture2D texture2D = Resources.Load("Detonator/Textures/Heatwave") as Texture2D;
		Detonator.defaultHeatwaveMaterial.SetTexture("_BumpMap", texture2D);
		return Detonator.defaultHeatwaveMaterial;
	}
}

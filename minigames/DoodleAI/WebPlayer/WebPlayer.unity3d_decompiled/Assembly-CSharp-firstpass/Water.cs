using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Water : MonoBehaviour
{
	public enum WaterMode
	{
		Simple,
		Reflective,
		Refractive
	}

	public Water.WaterMode m_WaterMode = Water.WaterMode.Refractive;

	public bool m_DisablePixelLights = true;

	public int m_TextureSize = 256;

	public float m_ClipPlaneOffset = 0.07f;

	public LayerMask m_ReflectLayers = -1;

	public LayerMask m_RefractLayers = -1;

	private Hashtable m_ReflectionCameras = new Hashtable();

	private Hashtable m_RefractionCameras = new Hashtable();

	private RenderTexture m_ReflectionTexture;

	private RenderTexture m_RefractionTexture;

	private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

	private int m_OldReflectionTextureSize;

	private int m_OldRefractionTextureSize;

	private static bool s_InsideWater;

	public void OnWillRenderObject()
	{
		if (!base.get_enabled() || !base.get_renderer() || !base.get_renderer().get_sharedMaterial() || !base.get_renderer().get_enabled())
		{
			return;
		}
		Camera current = Camera.get_current();
		if (!current)
		{
			return;
		}
		if (Water.s_InsideWater)
		{
			return;
		}
		Water.s_InsideWater = true;
		this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
		Water.WaterMode waterMode = this.GetWaterMode();
		Camera camera;
		Camera camera2;
		this.CreateWaterObjects(current, out camera, out camera2);
		Vector3 position = base.get_transform().get_position();
		Vector3 up = base.get_transform().get_up();
		int pixelLightCount = QualitySettings.get_pixelLightCount();
		if (this.m_DisablePixelLights)
		{
			QualitySettings.set_pixelLightCount(0);
		}
		this.UpdateCameraModes(current, camera);
		this.UpdateCameraModes(current, camera2);
		if (waterMode >= Water.WaterMode.Reflective)
		{
			float num = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, num);
			Matrix4x4 zero = Matrix4x4.get_zero();
			Water.CalculateReflectionMatrix(ref zero, plane);
			Vector3 position2 = current.get_transform().get_position();
			Vector3 position3 = zero.MultiplyPoint(position2);
			camera.set_worldToCameraMatrix(current.get_worldToCameraMatrix() * zero);
			Vector4 clipPlane = this.CameraSpacePlane(camera, position, up, 1f);
			Matrix4x4 projectionMatrix = current.get_projectionMatrix();
			Water.CalculateObliqueMatrix(ref projectionMatrix, clipPlane);
			camera.set_projectionMatrix(projectionMatrix);
			camera.set_cullingMask(-17 & this.m_ReflectLayers.get_value());
			camera.set_targetTexture(this.m_ReflectionTexture);
			GL.SetRevertBackfacing(true);
			camera.get_transform().set_position(position3);
			Vector3 eulerAngles = current.get_transform().get_eulerAngles();
			camera.get_transform().set_eulerAngles(new Vector3(0f, eulerAngles.y, eulerAngles.z));
			camera.Render();
			camera.get_transform().set_position(position2);
			GL.SetRevertBackfacing(false);
			base.get_renderer().get_sharedMaterial().SetTexture("_ReflectionTex", this.m_ReflectionTexture);
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			camera2.set_worldToCameraMatrix(current.get_worldToCameraMatrix());
			Vector4 clipPlane2 = this.CameraSpacePlane(camera2, position, up, -1f);
			Matrix4x4 projectionMatrix2 = current.get_projectionMatrix();
			Water.CalculateObliqueMatrix(ref projectionMatrix2, clipPlane2);
			camera2.set_projectionMatrix(projectionMatrix2);
			camera2.set_cullingMask(-17 & this.m_RefractLayers.get_value());
			camera2.set_targetTexture(this.m_RefractionTexture);
			camera2.get_transform().set_position(current.get_transform().get_position());
			camera2.get_transform().set_rotation(current.get_transform().get_rotation());
			camera2.Render();
			base.get_renderer().get_sharedMaterial().SetTexture("_RefractionTex", this.m_RefractionTexture);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.set_pixelLightCount(pixelLightCount);
		}
		switch (waterMode)
		{
		case Water.WaterMode.Simple:
			Shader.EnableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_REFRACTIVE");
			break;
		case Water.WaterMode.Reflective:
			Shader.DisableKeyword("WATER_SIMPLE");
			Shader.EnableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_REFRACTIVE");
			break;
		case Water.WaterMode.Refractive:
			Shader.DisableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
			Shader.EnableKeyword("WATER_REFRACTIVE");
			break;
		}
		Water.s_InsideWater = false;
	}

	private void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		if (this.m_RefractionTexture)
		{
			Object.DestroyImmediate(this.m_RefractionTexture);
			this.m_RefractionTexture = null;
		}
		using (IDictionaryEnumerator enumerator = this.m_ReflectionCameras.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.get_Current();
				Object.DestroyImmediate(((Camera)dictionaryEntry.get_Value()).get_gameObject());
			}
		}
		this.m_ReflectionCameras.Clear();
		using (IDictionaryEnumerator enumerator2 = this.m_RefractionCameras.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)enumerator2.get_Current();
				Object.DestroyImmediate(((Camera)dictionaryEntry2.get_Value()).get_gameObject());
			}
		}
		this.m_RefractionCameras.Clear();
	}

	private void Update()
	{
		if (!base.get_renderer())
		{
			return;
		}
		Material sharedMaterial = base.get_renderer().get_sharedMaterial();
		if (!sharedMaterial)
		{
			return;
		}
		Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
		float @float = sharedMaterial.GetFloat("_WaveScale");
		Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
		double num = (double)Time.get_timeSinceLevelLoad() / 20.0;
		Vector4 vector3 = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
		sharedMaterial.SetVector("_WaveOffset", vector3);
		sharedMaterial.SetVector("_WaveScale4", vector2);
		Vector3 size = base.get_renderer().get_bounds().get_size();
		Vector3 vector4 = new Vector3(size.x * vector2.x, size.z * vector2.y, 1f);
		Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(vector3.x, vector3.y, 0f), Quaternion.get_identity(), vector4);
		sharedMaterial.SetMatrix("_WaveMatrix", matrix4x);
		vector4 = new Vector3(size.x * vector2.z, size.z * vector2.w, 1f);
		matrix4x = Matrix4x4.TRS(new Vector3(vector3.z, vector3.w, 0f), Quaternion.get_identity(), vector4);
		sharedMaterial.SetMatrix("_WaveMatrix2", matrix4x);
	}

	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.set_clearFlags(src.get_clearFlags());
		dest.set_backgroundColor(src.get_backgroundColor());
		if (src.get_clearFlags() == 1)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.get_material())
			{
				skybox2.set_enabled(false);
			}
			else
			{
				skybox2.set_enabled(true);
				skybox2.set_material(skybox.get_material());
			}
		}
		dest.set_farClipPlane(src.get_farClipPlane());
		dest.set_nearClipPlane(src.get_nearClipPlane());
		dest.set_orthographic(src.get_orthographic());
		dest.set_fieldOfView(src.get_fieldOfView());
		dest.set_aspect(src.get_aspect());
		dest.set_orthographicSize(src.get_orthographicSize());
	}

	private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
	{
		Water.WaterMode waterMode = this.GetWaterMode();
		reflectionCamera = null;
		refractionCamera = null;
		if (waterMode >= Water.WaterMode.Reflective)
		{
			if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
			{
				if (this.m_ReflectionTexture)
				{
					Object.DestroyImmediate(this.m_ReflectionTexture);
				}
				this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_ReflectionTexture.set_name("__WaterReflection" + base.GetInstanceID());
				this.m_ReflectionTexture.set_isPowerOfTwo(true);
				this.m_ReflectionTexture.set_hideFlags(4);
				this.m_OldReflectionTextureSize = this.m_TextureSize;
			}
			reflectionCamera = (this.m_ReflectionCameras.get_Item(currentCamera) as Camera);
			if (!reflectionCamera)
			{
				GameObject gameObject = new GameObject(string.Concat(new object[]
				{
					"Water Refl Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				reflectionCamera = gameObject.get_camera();
				reflectionCamera.set_enabled(false);
				reflectionCamera.get_transform().set_position(base.get_transform().get_position());
				reflectionCamera.get_transform().set_rotation(base.get_transform().get_rotation());
				reflectionCamera.get_gameObject().AddComponent("FlareLayer");
				gameObject.set_hideFlags(13);
				this.m_ReflectionCameras.set_Item(currentCamera, reflectionCamera);
			}
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.m_TextureSize)
			{
				if (this.m_RefractionTexture)
				{
					Object.DestroyImmediate(this.m_RefractionTexture);
				}
				this.m_RefractionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_RefractionTexture.set_name("__WaterRefraction" + base.GetInstanceID());
				this.m_RefractionTexture.set_isPowerOfTwo(true);
				this.m_RefractionTexture.set_hideFlags(4);
				this.m_OldRefractionTextureSize = this.m_TextureSize;
			}
			refractionCamera = (this.m_RefractionCameras.get_Item(currentCamera) as Camera);
			if (!refractionCamera)
			{
				GameObject gameObject2 = new GameObject(string.Concat(new object[]
				{
					"Water Refr Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				refractionCamera = gameObject2.get_camera();
				refractionCamera.set_enabled(false);
				refractionCamera.get_transform().set_position(base.get_transform().get_position());
				refractionCamera.get_transform().set_rotation(base.get_transform().get_rotation());
				refractionCamera.get_gameObject().AddComponent("FlareLayer");
				gameObject2.set_hideFlags(13);
				this.m_RefractionCameras.set_Item(currentCamera, refractionCamera);
			}
		}
	}

	private Water.WaterMode GetWaterMode()
	{
		if (this.m_HardwareWaterSupport < this.m_WaterMode)
		{
			return this.m_HardwareWaterSupport;
		}
		return this.m_WaterMode;
	}

	private Water.WaterMode FindHardwareWaterSupport()
	{
		if (!SystemInfo.get_supportsRenderTextures() || !base.get_renderer())
		{
			return Water.WaterMode.Simple;
		}
		Material sharedMaterial = base.get_renderer().get_sharedMaterial();
		if (!sharedMaterial)
		{
			return Water.WaterMode.Simple;
		}
		string tag = sharedMaterial.GetTag("WATERMODE", false);
		if (tag == "Refractive")
		{
			return Water.WaterMode.Refractive;
		}
		if (tag == "Reflective")
		{
			return Water.WaterMode.Reflective;
		}
		return Water.WaterMode.Simple;
	}

	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 vector = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.get_worldToCameraMatrix();
		Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).get_normalized() * sideSign;
		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vector = projection.get_inverse() * new Vector4(Water.sgn(clipPlane.x), Water.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, vector));
		projection.set_Item(2, vector2.x - projection.get_Item(3));
		projection.set_Item(6, vector2.y - projection.get_Item(7));
		projection.set_Item(10, vector2.z - projection.get_Item(11));
		projection.set_Item(14, vector2.w - projection.get_Item(15));
	}

	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane.get_Item(0) * plane.get_Item(0);
		reflectionMat.m01 = -2f * plane.get_Item(0) * plane.get_Item(1);
		reflectionMat.m02 = -2f * plane.get_Item(0) * plane.get_Item(2);
		reflectionMat.m03 = -2f * plane.get_Item(3) * plane.get_Item(0);
		reflectionMat.m10 = -2f * plane.get_Item(1) * plane.get_Item(0);
		reflectionMat.m11 = 1f - 2f * plane.get_Item(1) * plane.get_Item(1);
		reflectionMat.m12 = -2f * plane.get_Item(1) * plane.get_Item(2);
		reflectionMat.m13 = -2f * plane.get_Item(3) * plane.get_Item(1);
		reflectionMat.m20 = -2f * plane.get_Item(2) * plane.get_Item(0);
		reflectionMat.m21 = -2f * plane.get_Item(2) * plane.get_Item(1);
		reflectionMat.m22 = 1f - 2f * plane.get_Item(2) * plane.get_Item(2);
		reflectionMat.m23 = -2f * plane.get_Item(3) * plane.get_Item(2);
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}
}

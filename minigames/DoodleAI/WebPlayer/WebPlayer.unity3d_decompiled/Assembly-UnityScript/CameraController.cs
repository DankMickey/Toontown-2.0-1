using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("ControlPrototype/Camera Controller")]
[Serializable]
public class CameraController : MonoBehaviour
{
	public Transform target;

	public Texture reticle;

	public static float distance = 4f;

	private float xSpeed;

	private float ySpeed;

	private int yMinLimit;

	private int yMaxLimit;

	private int zoomMinLimit;

	private int zoomMaxLimit;

	private float x;

	public static float y;

	private Status status;

	public static string cameraView = "third";

	public static bool mouseMode;

	public static bool dofMode;

	public CameraController()
	{
		this.xSpeed = 250f;
		this.ySpeed = 120f;
		this.yMinLimit = -20;
		this.yMaxLimit = 80;
		this.zoomMinLimit = 2;
		this.zoomMaxLimit = 6;
	}

	public override void Start()
	{
		Vector3 eulerAngles = this.get_transform().get_eulerAngles();
		this.x = eulerAngles.y;
		CameraController.y = eulerAngles.x;
		this.status = (Status)this.GetComponent(typeof(Status));
		CameraController.mouseMode = true;
	}

	public override void LateUpdate()
	{
		this.CheckStatus();
		if (CameraController.cameraView == "third")
		{
			float num = this.target.get_transform().get_eulerAngles().y;
			float num2 = this.get_transform().get_eulerAngles().y;
			if (Input.GetButton("CameraZoomIn") && CameraController.distance > (float)this.zoomMinLimit)
			{
				CameraController.distance -= (float)1 / (this.xSpeed * 0.02f);
			}
			if (Input.GetButton("CameraZoomOut") && CameraController.distance < (float)this.zoomMaxLimit)
			{
				CameraController.distance += (float)1 / (this.xSpeed * 0.02f);
			}
			if (CameraController.mouseMode)
			{
				CameraController.distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.get_deltaTime()) * this.xSpeed * 0.02f * Mathf.Abs(CameraController.distance);
				this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
				CameraController.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			}
			this.x += Input.GetAxis("CameraX") * this.xSpeed * 0.02f;
			CameraController.y -= Input.GetAxis("CameraY") * this.ySpeed * 0.02f;
			CameraController.y = CameraController.ClampAngle(CameraController.y, (float)this.yMinLimit, (float)this.yMaxLimit);
			Quaternion quaternion;
			Vector3 position;
			if (Input.GetAxis("Vertical") != (float)0 || Input.GetAxis("Horizontal") != (float)0)
			{
				quaternion = Quaternion.Slerp(this.get_transform().get_rotation(), Quaternion.Euler(CameraController.y, num, (float)0), Time.get_deltaTime() * (float)3);
				position = quaternion * new Vector3((float)0, (float)0, -CameraController.distance) + this.target.get_position();
				this.x = num;
			}
			else
			{
				quaternion = Quaternion.Slerp(this.get_transform().get_rotation(), Quaternion.Euler(CameraController.y, this.x, (float)0), Time.get_deltaTime() * (float)3);
				position = quaternion * new Vector3((float)0, (float)0, -CameraController.distance) + this.target.get_position();
			}
			this.get_transform().set_rotation(quaternion);
			this.get_transform().set_position(position);
		}
		else
		{
			float num = this.target.get_transform().get_eulerAngles().y;
			this.x = num;
			if (CameraController.mouseMode)
			{
				this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
				CameraController.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			}
			Quaternion quaternion = Quaternion.Slerp(this.get_transform().get_rotation(), Quaternion.Euler(CameraController.y, this.x, (float)0), Time.get_deltaTime() * (float)3);
			Vector3 position = quaternion * new Vector3((float)0, (float)0, -CameraController.distance) + this.target.get_position();
			this.get_transform().set_rotation(quaternion);
			this.get_transform().set_position(position);
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < (float)-360)
		{
			angle += (float)360;
		}
		if (angle > (float)360)
		{
			angle -= (float)360;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public override void CheckStatus()
	{
		if (Input.GetKeyDown(9) || Input.GetKeyDown(122))
		{
			if (CameraController.cameraView == "third")
			{
				CameraController.cameraView = "first";
				CameraController.distance = (float)0;
			}
			else
			{
				CameraController.cameraView = "third";
				CameraController.distance = 4f;
			}
		}
		if (Input.GetKeyDown(308) || Input.GetKeyDown(99))
		{
			if (CameraController.mouseMode)
			{
				CameraController.mouseMode = false;
			}
			else
			{
				CameraController.mouseMode = true;
			}
		}
		if (Input.GetKeyDown(98))
		{
			if (CameraController.dofMode)
			{
				CameraController.dofMode = false;
			}
			else
			{
				CameraController.dofMode = true;
			}
		}
		if (CameraController.dofMode && RuntimeServices.ToBool(this.GetComponent(typeof(DepthOfFieldEffect))))
		{
			RuntimeServices.SetProperty(this.GetComponent(typeof(DepthOfFieldEffect)), "enabled", true);
			RuntimeServices.SetProperty(this.GetComponent(typeof(DOFGui)), "enabled", true);
		}
		else
		{
			RuntimeServices.SetProperty(this.GetComponent(typeof(DepthOfFieldEffect)), "enabled", false);
			RuntimeServices.SetProperty(this.GetComponent(typeof(DOFGui)), "enabled", false);
		}
	}

	public override void OnGUI()
	{
		if (CameraController.mouseMode)
		{
			GUI.DrawTexture(new Rect((float)(Screen.get_width() / 2) - (float)this.reticle.get_width() * 0.5f, (float)(Screen.get_height() / 2) - (float)this.reticle.get_height() * 0.5f, (float)(this.reticle.get_width() / 4), (float)(this.reticle.get_height() / 4)), this.reticle);
		}
	}

	public override void Main()
	{
	}
}

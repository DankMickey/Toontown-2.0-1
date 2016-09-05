using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class DetonatorTest : MonoBehaviour
{
	public GameObject currentDetonator;

	private int _currentExpIdx;

	private bool buttonClicked;

	public GameObject[] detonatorPrefabs;

	public float explosionLife;

	public float timeScale;

	public float detailLevel;

	public GameObject wall;

	private GameObject _currentWall;

	private int _spawnWallTime;

	private object _guiRect;

	private bool toggleBool;

	private Rect checkRect;

	public DetonatorTest()
	{
		this._currentExpIdx = -1;
		this.explosionLife = (float)10;
		this.timeScale = 1f;
		this.detailLevel = 1f;
		this._spawnWallTime = -1000;
		this.checkRect = new Rect((float)0, (float)0, (float)260, (float)180);
	}

	public override void Start()
	{
		this.SpawnWall();
		if (!this.currentDetonator)
		{
			this.NextExplosion();
		}
		else
		{
			this._currentExpIdx = 0;
		}
	}

	public override void OnGUI()
	{
		this._guiRect = new Rect((float)7, (float)(checked(Screen.get_height() - 180)), (float)250, (float)200);
		GUILayout.BeginArea((Rect)this._guiRect);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		string name = this.currentDetonator.get_name();
		if (GUILayout.Button(name + " (Click For Next)", new GUILayoutOption[0]))
		{
			this.NextExplosion();
		}
		if (GUILayout.Button("Rebuild Wall", new GUILayoutOption[0]))
		{
			this.SpawnWall();
		}
		if (GUILayout.Button("Camera Far", new GUILayoutOption[0]))
		{
			Camera.get_main().get_transform().set_position(new Vector3((float)0, (float)0, (float)-7));
			Camera.get_main().get_transform().set_eulerAngles(new Vector3(13.5f, (float)0, (float)0));
		}
		if (GUILayout.Button("Camera Near", new GUILayoutOption[0]))
		{
			Camera.get_main().get_transform().set_position(new Vector3((float)0, -8.664466f, 31.38269f));
			Camera.get_main().get_transform().set_eulerAngles(new Vector3(1.213462f, (float)0, (float)0));
		}
		GUILayout.Label("Time Scale", new GUILayoutOption[0]);
		this.timeScale = GUILayout.HorizontalSlider(this.timeScale, (float)0, 1f, new GUILayoutOption[0]);
		GUILayout.Label("Detail Level (re-explode after change)", new GUILayoutOption[0]);
		this.detailLevel = GUILayout.HorizontalSlider(this.detailLevel, (float)0, 1f, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public override void NextExplosion()
	{
		checked
		{
			if (this._currentExpIdx >= this.detonatorPrefabs.get_Length() - 1)
			{
				this._currentExpIdx = 0;
			}
			else
			{
				this._currentExpIdx++;
			}
			this.currentDetonator = this.detonatorPrefabs[this._currentExpIdx];
		}
	}

	public override void SpawnWall()
	{
		if (this._currentWall)
		{
			Object.Destroy(this._currentWall);
		}
		this._currentWall = (GameObject)Object.Instantiate(this.wall, new Vector3((float)-7, (float)-12, (float)48), Quaternion.get_identity());
		this._spawnWallTime = checked((int)Time.get_time());
	}

	public override void Update()
	{
		this._guiRect = new Rect((float)7, (float)(checked(Screen.get_height() - 150)), (float)250, (float)200);
		if (Time.get_time() + (float)this._spawnWallTime > 0.5f)
		{
			if (!this.checkRect.Contains(Input.get_mousePosition()) && Input.GetMouseButtonDown(0))
			{
				this.SpawnExplosion();
			}
			Time.set_timeScale(this.timeScale);
		}
	}

	public override void SpawnExplosion()
	{
		Ray ray = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
		RaycastHit raycastHit = default(RaycastHit);
		GameObject gameObject;
		if (Physics.Raycast(ray, ref raycastHit, (float)1000))
		{
			object value = RuntimeServices.InvokeBinaryOperator("op_Division", UnityRuntimeServices.GetProperty(this.currentDetonator.GetComponent("Detonator"), "size"), 3);
			Vector3 vector = raycastHit.get_point() + Vector3.Scale(raycastHit.get_normal(), new Vector3(RuntimeServices.UnboxSingle(value), RuntimeServices.UnboxSingle(value), RuntimeServices.UnboxSingle(value)));
			gameObject = (GameObject)Object.Instantiate(this.currentDetonator, vector, Quaternion.get_identity());
			RuntimeServices.SetProperty(gameObject.GetComponent("Detonator"), "detail", this.detailLevel);
		}
		Object.Destroy(gameObject, this.explosionLife);
	}

	public override void Main()
	{
	}
}

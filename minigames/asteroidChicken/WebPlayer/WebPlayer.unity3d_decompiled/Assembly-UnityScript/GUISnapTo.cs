using System;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class GUISnapTo : MonoBehaviour
{
	public Camera guiCamera;

	public BorderType borderType;

	public Vector2 offset;

	public GUISnapTo()
	{
		this.offset = Vector2.get_zero();
	}

	public override void Update()
	{
		checked
		{
			int num = (int)this.guiCamera.get_pixelHeight();
			int num2 = (int)this.guiCamera.get_pixelWidth();
			BorderType borderType = this.borderType;
			if (borderType == BorderType.Left)
			{
				int num3 = num2 / 2 * -1;
				Vector3 position = this.get_transform().get_position();
				float num4 = position.x = (float)num3;
				Vector3 vector;
				this.get_transform().set_position(vector = position);
			}
			else if (borderType == BorderType.Right)
			{
				int num5 = num2 / 2;
				Vector3 position2 = this.get_transform().get_position();
				float num6 = position2.x = (float)num5;
				Vector3 vector2;
				this.get_transform().set_position(vector2 = position2);
			}
			else if (borderType == BorderType.Up)
			{
				int num7 = num / 2;
				Vector3 position3 = this.get_transform().get_position();
				float num8 = position3.y = (float)num7;
				Vector3 vector3;
				this.get_transform().set_position(vector3 = position3);
			}
			else if (borderType == BorderType.Down)
			{
				int num9 = num / 2 * -1;
				Vector3 position4 = this.get_transform().get_position();
				float num10 = position4.y = (float)num9;
				Vector3 vector4;
				this.get_transform().set_position(vector4 = position4);
			}
		}
		float y = this.get_transform().get_position().y + this.offset.y;
		Vector3 position5 = this.get_transform().get_position();
		float num11 = position5.y = y;
		Vector3 vector5;
		this.get_transform().set_position(vector5 = position5);
		float x = this.get_transform().get_position().x + this.offset.x;
		Vector3 position6 = this.get_transform().get_position();
		float num12 = position6.x = x;
		Vector3 vector6;
		this.get_transform().set_position(vector6 = position6);
	}

	public override void Main()
	{
	}
}

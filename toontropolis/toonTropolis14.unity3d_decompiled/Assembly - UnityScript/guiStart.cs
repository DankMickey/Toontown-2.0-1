using System;
using UnityEngine;

[Serializable]
public class guiStart : MonoBehaviour
{
	public void Main()
	{
		int num = checked(Screen.get_width() * 0);
		Vector2 pixelOffset = this.get_gameObject().get_guiText().get_pixelOffset();
		float num2 = pixelOffset.x = (float)num;
		Vector2 vector;
		this.get_gameObject().get_guiText().set_pixelOffset(vector = pixelOffset);
		float y = (float)Screen.get_height() * (0.44f * (float)-1);
		Vector2 pixelOffset2 = this.get_gameObject().get_guiText().get_pixelOffset();
		float num3 = pixelOffset2.y = y;
		Vector2 vector2;
		this.get_gameObject().get_guiText().set_pixelOffset(vector2 = pixelOffset2);
	}
}

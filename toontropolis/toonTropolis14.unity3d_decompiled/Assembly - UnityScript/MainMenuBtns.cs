using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[Serializable]
public class MainMenuBtns : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class OnMouseUp$75 : GenericGenerator<WaitForSeconds>
	{
		internal MainMenuBtns $self_271;

		public OnMouseUp$75(MainMenuBtns self_)
		{
			this.$self_271 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new MainMenuBtns.OnMouseUp$75.$(this.$self_271);
		}
	}

	public string levelToLoad;

	public Texture2D normalTexture;

	public Texture2D rolloverTexture;

	public AudioClip beep;

	public void Update()
	{
	}

	public void OnMouseEnter()
	{
		this.get_guiTexture().set_texture(this.rolloverTexture);
	}

	public void OnMouseExit()
	{
		this.get_guiTexture().set_texture(this.normalTexture);
	}

	public IEnumerator OnMouseUp()
	{
		return new MainMenuBtns.OnMouseUp$75(this).GetEnumerator();
	}

	public void Main()
	{
	}
}

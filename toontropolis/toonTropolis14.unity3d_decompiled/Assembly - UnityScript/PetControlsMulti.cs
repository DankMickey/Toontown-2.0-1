using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PetControlsMulti : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class MainLoop$56 : GenericGenerator<object>
	{
		internal PetControlsMulti $self_208;

		public MainLoop$56(PetControlsMulti self_)
		{
			this.$self_208 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new PetControlsMulti.MainLoop$56.$(this.$self_208);
		}
	}

	public GUIText statusGUI;

	public Transform owner;

	private Vector3 moveDirection;

	private bool grounded;

	public float speed;

	public float jumpSpeed;

	public float gravity;

	public PetControlsMulti()
	{
		this.moveDirection = Vector3.get_zero();
		this.grounded = false;
		this.speed = 6f;
		this.jumpSpeed = 8f;
		this.gravity = 20f;
	}

	public void Start()
	{
		this.StartCoroutine_Auto(this.MainLoop());
	}

	public void SetPetState(object newPetState)
	{
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		petState.indexAction = RuntimeServices.UnboxInt32(newPetState);
	}

	public IEnumerator MainLoop()
	{
		return new PetControlsMulti.MainLoop$56(this).GetEnumerator();
	}

	public void Update()
	{
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		if (!characterController.get_isGrounded())
		{
			this.moveDirection = Vector3.get_zero();
			this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
			this.moveDirection.y = this.moveDirection.y - this.gravity * Time.get_deltaTime();
			object obj = characterController.Move(this.moveDirection * Time.get_deltaTime());
		}
	}

	public void Main()
	{
	}
}

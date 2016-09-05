using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class StarField : MonoBehaviour
{
	public GameObject starPrefab;

	public float fieldSize;

	public int starCount;

	public Vector3 offsetXYZ;

	public override void Start()
	{
		for (int i = 0; i < this.starCount; i = checked(i + 1))
		{
			float num = this.fieldSize * 0.5f;
			float num2 = this.offsetXYZ.get_Item(0) + Mathf.Lerp(-0.2f * num, 0.2f * num, Random.get_value());
			float num3 = this.offsetXYZ.get_Item(1) + Mathf.Lerp(-0.2f * num, 0.2f * num, Random.get_value());
			float num4 = this.offsetXYZ.get_Item(2) + Mathf.Lerp((float)0, this.fieldSize, Random.get_value());
			object target = Object.Instantiate(this.starPrefab, new Vector3(num2, num3, num4), this.get_transform().get_rotation());
			Transform transform = this.get_transform();
			object property = UnityRuntimeServices.GetProperty(target, "transform");
			RuntimeServices.SetProperty(property, "parent", transform);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property)
			});
			float num5 = num2;
			object property2 = UnityRuntimeServices.GetProperty(target, "transform");
			object property3 = UnityRuntimeServices.GetProperty(property2, "position");
			RuntimeServices.SetProperty(property3, "x", num5);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(property2, "position", property3),
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property2)
			});
			float num6 = num3;
			object property4 = UnityRuntimeServices.GetProperty(target, "transform");
			object property5 = UnityRuntimeServices.GetProperty(property4, "position");
			RuntimeServices.SetProperty(property5, "y", num6);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(property4, "position", property5),
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property4)
			});
			float num7 = num4;
			object property6 = UnityRuntimeServices.GetProperty(target, "transform");
			object property7 = UnityRuntimeServices.GetProperty(property6, "position");
			RuntimeServices.SetProperty(property7, "z", num7);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(property6, "position", property7),
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property6)
			});
			Vector3 vector = new Vector3(Random.Range(0.5f, (float)3), Random.Range(1f, 2f), 1f);
			object value = RuntimeServices.InvokeBinaryOperator("op_Multiply", UnityRuntimeServices.GetProperty(UnityRuntimeServices.GetProperty(UnityRuntimeServices.GetProperty(target, "transform"), "localScale"), "x"), vector.x);
			object property8 = UnityRuntimeServices.GetProperty(target, "transform");
			object property9 = UnityRuntimeServices.GetProperty(property8, "localScale");
			RuntimeServices.SetProperty(property9, "x", value);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(property8, "localScale", property9),
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property8)
			});
			object value2 = RuntimeServices.InvokeBinaryOperator("op_Multiply", UnityRuntimeServices.GetProperty(UnityRuntimeServices.GetProperty(UnityRuntimeServices.GetProperty(target, "transform"), "localScale"), "y"), vector.y * vector.x);
			object property10 = UnityRuntimeServices.GetProperty(target, "transform");
			object property11 = UnityRuntimeServices.GetProperty(property10, "localScale");
			RuntimeServices.SetProperty(property11, "y", value2);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(property10, "localScale", property11),
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property10)
			});
		}
	}

	public override void Main()
	{
	}
}

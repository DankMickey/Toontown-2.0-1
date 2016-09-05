using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildren : MonoBehaviour
{
	public bool generateTriangleStrips = true;

	private void Start()
	{
		Component[] componentsInChildren = base.GetComponentsInChildren(typeof(MeshFilter));
		Matrix4x4 worldToLocalMatrix = base.get_transform().get_worldToLocalMatrix();
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshFilter meshFilter = (MeshFilter)componentsInChildren[i];
			Renderer renderer = componentsInChildren[i].get_renderer();
			MeshCombineUtility.MeshInstance meshInstance = default(MeshCombineUtility.MeshInstance);
			meshInstance.mesh = meshFilter.get_sharedMesh();
			if (renderer != null && renderer.get_enabled() && meshInstance.mesh != null)
			{
				meshInstance.transform = worldToLocalMatrix * meshFilter.get_transform().get_localToWorldMatrix();
				Material[] sharedMaterials = renderer.get_sharedMaterials();
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					meshInstance.subMeshIndex = Math.Min(j, meshInstance.mesh.get_subMeshCount() - 1);
					ArrayList arrayList = (ArrayList)hashtable[sharedMaterials[j]];
					if (arrayList != null)
					{
						arrayList.Add(meshInstance);
					}
					else
					{
						arrayList = new ArrayList();
						arrayList.Add(meshInstance);
						hashtable.Add(sharedMaterials[j], arrayList);
					}
				}
				renderer.set_enabled(false);
			}
		}
		foreach (DictionaryEntry dictionaryEntry in hashtable)
		{
			ArrayList arrayList2 = (ArrayList)dictionaryEntry.Value;
			MeshCombineUtility.MeshInstance[] combines = (MeshCombineUtility.MeshInstance[])arrayList2.ToArray(typeof(MeshCombineUtility.MeshInstance));
			if (hashtable.Count == 1)
			{
				if (base.GetComponent(typeof(MeshFilter)) == null)
				{
					base.get_gameObject().AddComponent(typeof(MeshFilter));
				}
				if (!base.GetComponent("MeshRenderer"))
				{
					base.get_gameObject().AddComponent("MeshRenderer");
				}
				MeshFilter meshFilter2 = (MeshFilter)base.GetComponent(typeof(MeshFilter));
				meshFilter2.set_mesh(MeshCombineUtility.Combine(combines, this.generateTriangleStrips));
				base.get_renderer().set_material((Material)dictionaryEntry.Key);
				base.get_renderer().set_enabled(true);
			}
			else
			{
				GameObject gameObject = new GameObject("Combined mesh");
				gameObject.get_transform().set_parent(base.get_transform());
				gameObject.get_transform().set_localScale(Vector3.get_one());
				gameObject.get_transform().set_localRotation(Quaternion.get_identity());
				gameObject.get_transform().set_localPosition(Vector3.get_zero());
				gameObject.AddComponent(typeof(MeshFilter));
				gameObject.AddComponent("MeshRenderer");
				gameObject.get_renderer().set_material((Material)dictionaryEntry.Key);
				MeshFilter meshFilter3 = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
				meshFilter3.set_mesh(MeshCombineUtility.Combine(combines, this.generateTriangleStrips));
			}
		}
	}
}

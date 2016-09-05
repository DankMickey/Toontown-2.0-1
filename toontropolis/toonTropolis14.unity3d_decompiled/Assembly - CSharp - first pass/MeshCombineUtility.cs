using System;
using UnityEngine;

public class MeshCombineUtility
{
	public struct MeshInstance
	{
		public Mesh mesh;

		public int subMeshIndex;

		public Matrix4x4 transform;
	}

	public static Mesh Combine(MeshCombineUtility.MeshInstance[] combines, bool generateStrips)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = combines.Length;
		for (int i = 0; i < num4; i++)
		{
			MeshCombineUtility.MeshInstance meshInstance = combines[i];
			if (meshInstance.mesh)
			{
				num += meshInstance.mesh.get_vertexCount();
				if (generateStrips)
				{
					int num5 = meshInstance.mesh.GetTriangleStrip(meshInstance.subMeshIndex).Length;
					if (num5 != 0)
					{
						if (num3 != 0)
						{
							if ((num3 & 1) == 1)
							{
								num3 += 3;
							}
							else
							{
								num3 += 2;
							}
						}
						num3 += num5;
					}
					else
					{
						generateStrips = false;
					}
				}
			}
		}
		if (!generateStrips)
		{
			int num6 = combines.Length;
			for (int j = 0; j < num6; j++)
			{
				MeshCombineUtility.MeshInstance meshInstance2 = combines[j];
				if (meshInstance2.mesh)
				{
					num2 += meshInstance2.mesh.GetTriangles(meshInstance2.subMeshIndex).Length;
				}
			}
		}
		Vector3[] array = new Vector3[num];
		Vector3[] array2 = new Vector3[num];
		Vector4[] array3 = new Vector4[num];
		Vector2[] array4 = new Vector2[num];
		Vector2[] array5 = new Vector2[num];
		Color[] array6 = new Color[num];
		int[] array7 = new int[num2];
		int[] array8 = new int[num3];
		int num7 = 0;
		int num8 = combines.Length;
		for (int k = 0; k < num8; k++)
		{
			MeshCombineUtility.MeshInstance meshInstance3 = combines[k];
			if (meshInstance3.mesh)
			{
				MeshCombineUtility.Copy(meshInstance3.mesh.get_vertexCount(), meshInstance3.mesh.get_vertices(), array, ref num7, meshInstance3.transform);
			}
		}
		num7 = 0;
		int num9 = combines.Length;
		for (int l = 0; l < num9; l++)
		{
			MeshCombineUtility.MeshInstance meshInstance4 = combines[l];
			if (meshInstance4.mesh)
			{
				Matrix4x4 transform = meshInstance4.transform;
				transform = transform.get_inverse().get_transpose();
				MeshCombineUtility.CopyNormal(meshInstance4.mesh.get_vertexCount(), meshInstance4.mesh.get_normals(), array2, ref num7, transform);
			}
		}
		num7 = 0;
		int num10 = combines.Length;
		for (int m = 0; m < num10; m++)
		{
			MeshCombineUtility.MeshInstance meshInstance5 = combines[m];
			if (meshInstance5.mesh)
			{
				Matrix4x4 transform2 = meshInstance5.transform;
				transform2 = transform2.get_inverse().get_transpose();
				MeshCombineUtility.CopyTangents(meshInstance5.mesh.get_vertexCount(), meshInstance5.mesh.get_tangents(), array3, ref num7, transform2);
			}
		}
		num7 = 0;
		int num11 = combines.Length;
		for (int n = 0; n < num11; n++)
		{
			MeshCombineUtility.MeshInstance meshInstance6 = combines[n];
			if (meshInstance6.mesh)
			{
				MeshCombineUtility.Copy(meshInstance6.mesh.get_vertexCount(), meshInstance6.mesh.get_uv(), array4, ref num7);
			}
		}
		num7 = 0;
		int num12 = combines.Length;
		for (int num13 = 0; num13 < num12; num13++)
		{
			MeshCombineUtility.MeshInstance meshInstance7 = combines[num13];
			if (meshInstance7.mesh)
			{
				MeshCombineUtility.Copy(meshInstance7.mesh.get_vertexCount(), meshInstance7.mesh.get_uv1(), array5, ref num7);
			}
		}
		num7 = 0;
		int num14 = combines.Length;
		for (int num15 = 0; num15 < num14; num15++)
		{
			MeshCombineUtility.MeshInstance meshInstance8 = combines[num15];
			if (meshInstance8.mesh)
			{
				MeshCombineUtility.CopyColors(meshInstance8.mesh.get_vertexCount(), meshInstance8.mesh.get_colors(), array6, ref num7);
			}
		}
		int num16 = 0;
		int num17 = 0;
		int num18 = 0;
		int num19 = combines.Length;
		for (int num20 = 0; num20 < num19; num20++)
		{
			MeshCombineUtility.MeshInstance meshInstance9 = combines[num20];
			if (meshInstance9.mesh)
			{
				if (generateStrips)
				{
					int[] triangleStrip = meshInstance9.mesh.GetTriangleStrip(meshInstance9.subMeshIndex);
					if (num17 != 0)
					{
						if ((num17 & 1) == 1)
						{
							array8[num17 + 0] = array8[num17 - 1];
							array8[num17 + 1] = triangleStrip[0] + num18;
							array8[num17 + 2] = triangleStrip[0] + num18;
							num17 += 3;
						}
						else
						{
							array8[num17 + 0] = array8[num17 - 1];
							array8[num17 + 1] = triangleStrip[0] + num18;
							num17 += 2;
						}
					}
					for (int num21 = 0; num21 < triangleStrip.Length; num21++)
					{
						array8[num21 + num17] = triangleStrip[num21] + num18;
					}
					num17 += triangleStrip.Length;
				}
				else
				{
					int[] triangles = meshInstance9.mesh.GetTriangles(meshInstance9.subMeshIndex);
					for (int num22 = 0; num22 < triangles.Length; num22++)
					{
						array7[num22 + num16] = triangles[num22] + num18;
					}
					num16 += triangles.Length;
				}
				num18 += meshInstance9.mesh.get_vertexCount();
			}
		}
		Mesh mesh = new Mesh();
		mesh.set_name("Combined Mesh");
		mesh.set_vertices(array);
		mesh.set_normals(array2);
		mesh.set_colors(array6);
		mesh.set_uv(array4);
		mesh.set_uv1(array5);
		mesh.set_tangents(array3);
		if (generateStrips)
		{
			mesh.SetTriangleStrip(array8, 0);
		}
		else
		{
			mesh.set_triangles(array7);
		}
		return mesh;
	}

	private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyPoint(src[i]);
		}
		offset += vertexcount;
	}

	private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyVector(src[i]).get_normalized();
		}
		offset += vertexcount;
	}

	private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			Vector4 vector = src[i];
			Vector3 normalized = new Vector3(vector.x, vector.y, vector.z);
			normalized = transform.MultiplyVector(normalized).get_normalized();
			dst[i + offset] = new Vector4(normalized.x, normalized.y, normalized.z, vector.w);
		}
		offset += vertexcount;
	}
}

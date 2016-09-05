using System;
using System.Collections;
using UnityEngine;

namespace UnitySteer
{
	internal class locationQueryDatabase
	{
		public float originx;

		public float originy;

		public float originz;

		public float sizex;

		public float sizey;

		public float sizez;

		public int divx;

		public int divy;

		public int divz;

		public lqBin[] bins;

		public lqBin other;

		private int bincount;

		public locationQueryDatabase(float _originx, float _originy, float _originz, float _sizex, float _sizey, float _sizez, int _divx, int _divy, int _divz)
		{
			this.originx = _originx;
			this.originy = _originy;
			this.originz = _originz;
			this.sizex = _sizex;
			this.sizey = _sizey;
			this.sizez = _sizez;
			this.divx = _divx;
			this.divy = _divy;
			this.divz = _divz;
			this.bincount = this.divx * this.divy * this.divz;
			this.bins = new lqBin[this.bincount];
			for (int i = 0; i < this.divx; i++)
			{
				for (int j = 0; j < this.divy; j++)
				{
					for (int k = 0; k < this.divz; k++)
					{
						int num = i * this.divy * this.divz + j * this.divz + k;
						float num2 = this.originx + (float)i * (this.sizex / (float)this.divx);
						float num3 = this.originy + (float)j * (this.sizey / (float)this.divy);
						float num4 = this.originz + (float)k * (this.sizez / (float)this.divz);
						Vector3 binCenter = new Vector3(num2, num3, num4);
						this.bins[num] = new lqBin(binCenter);
					}
				}
			}
			this.other = new lqBin(Vector3.get_zero());
		}

		public int lqBinCoordsToBinIndex(float ix, float iy, float iz)
		{
			return (int)(ix * (float)this.divy * (float)this.divz + iy * (float)this.divz + iz);
		}

		public lqBin lqBinForLocation(float x, float y, float z)
		{
			if (x < this.originx)
			{
				return this.other;
			}
			if (y < this.originy)
			{
				return this.other;
			}
			if (z < this.originz)
			{
				return this.other;
			}
			if (x >= this.originx + this.sizex)
			{
				return this.other;
			}
			if (y >= this.originy + this.sizey)
			{
				return this.other;
			}
			if (z >= this.originz + this.sizez)
			{
				return this.other;
			}
			int num = (int)((x - this.originx) / this.sizex * (float)this.divx);
			int num2 = (int)((y - this.originy) / this.sizey * (float)this.divy);
			int num3 = (int)((z - this.originz) / this.sizez * (float)this.divz);
			int num4 = this.lqBinCoordsToBinIndex((float)num, (float)num2, (float)num3);
			if (num4 < 0 || num4 >= this.bincount)
			{
				return this.other;
			}
			return this.bins[num4];
		}

		public void lqAddToBin(lqClientProxy clientObject, lqBin bin)
		{
			bin.clientList.Add(clientObject);
			clientObject.bin = bin;
		}

		public void lqRemoveFromBin(lqClientProxy clientObject)
		{
			if (clientObject.bin != null)
			{
				clientObject.bin.clientList.Remove(clientObject);
			}
		}

		public void lqUpdateForNewLocation(lqClientProxy clientObject, float x, float y, float z)
		{
			lqBin lqBin = this.lqBinForLocation(x, y, z);
			clientObject.x = x;
			clientObject.y = y;
			clientObject.z = z;
			if (lqBin != clientObject.bin)
			{
				this.lqRemoveFromBin(clientObject);
				this.lqAddToBin(clientObject, lqBin);
			}
		}

		public ArrayList getBinClientObjectList(lqBin bin, float x, float y, float z, float radiusSquared)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < bin.clientList.get_Count(); i++)
			{
				lqClientProxy lqClientProxy = (lqClientProxy)bin.clientList.get_Item(i);
				float num = x - lqClientProxy.x;
				float num2 = y - lqClientProxy.y;
				float num3 = z - lqClientProxy.z;
				float num4 = num * num + num2 * num2 + num3 * num3;
				if (num4 < radiusSquared)
				{
					arrayList.Add(lqClientProxy);
				}
			}
			return arrayList;
		}

		public ArrayList getAllClientObjectsInLocalityClipped(float x, float y, float z, float radius, int minBinX, int minBinY, int minBinZ, int maxBinX, int maxBinY, int maxBinZ)
		{
			int num = this.divy * this.divz;
			int num2 = this.divz;
			int num3 = minBinX * num;
			int num4 = minBinY * num2;
			float radiusSquared = radius * radius;
			ArrayList arrayList = new ArrayList();
			int num5 = num3;
			for (int i = minBinX; i <= maxBinX; i++)
			{
				int num6 = num4;
				for (int j = minBinY; j <= maxBinY; j++)
				{
					int num7 = minBinZ;
					for (int k = minBinZ; k <= maxBinZ; k++)
					{
						lqBin bin = this.bins[num5 + num6 + num7];
						ArrayList binClientObjectList = this.getBinClientObjectList(bin, x, y, z, radiusSquared);
						arrayList.AddRange(binClientObjectList);
						num7++;
					}
					num6 += num2;
				}
				num5 += num;
			}
			return arrayList;
		}

		public ArrayList getAllOutsideObjects(float x, float y, float z, float radius)
		{
			float radiusSquared = radius * radius;
			return this.getBinClientObjectList(this.other, x, y, z, radiusSquared);
		}

		public ArrayList getAllObjectsInLocality(float x, float y, float z, float radius)
		{
			bool flag = false;
			bool flag2 = x + radius < this.originx || y + radius < this.originy || z + radius < this.originz || x - radius >= this.originx + this.sizex || y - radius >= this.originy + this.sizey || z - radius >= this.originz + this.sizez;
			if (flag2)
			{
				return this.getAllOutsideObjects(x, y, z, radius);
			}
			ArrayList arrayList = new ArrayList();
			int num = (int)((x - radius - this.originx) / this.sizex * (float)this.divx);
			int num2 = (int)((y - radius - this.originy) / this.sizey * (float)this.divy);
			int num3 = (int)((z - radius - this.originz) / this.sizez * (float)this.divz);
			int num4 = (int)((x + radius - this.originx) / this.sizex * (float)this.divx);
			int num5 = (int)((y + radius - this.originy) / this.sizey * (float)this.divy);
			int num6 = (int)((z + radius - this.originz) / this.sizez * (float)this.divz);
			if (num < 0)
			{
				flag = true;
				num = 0;
			}
			if (num2 < 0)
			{
				flag = true;
				num2 = 0;
			}
			if (num3 < 0)
			{
				flag = true;
				num3 = 0;
			}
			if (num4 >= this.divx)
			{
				flag = true;
				num4 = this.divx - 1;
			}
			if (num5 >= this.divy)
			{
				flag = true;
				num5 = this.divy - 1;
			}
			if (num6 >= this.divz)
			{
				flag = true;
				num6 = this.divz - 1;
			}
			if (flag)
			{
				arrayList.AddRange(this.getAllOutsideObjects(x, y, z, radius));
			}
			arrayList.AddRange(this.getAllClientObjectsInLocalityClipped(x, y, z, radius, num, num2, num3, num4, num5, num6));
			return arrayList;
		}

		public lqClientProxy lqFindNearestNeighborWithinRadius(float x, float y, float z, float radius, object ignoreObject)
		{
			float num = 3.40282347E+38f;
			ArrayList allObjectsInLocality = this.getAllObjectsInLocality(x, y, z, radius);
			lqClientProxy result = null;
			for (int i = 0; i < allObjectsInLocality.get_Count(); i++)
			{
				lqClientProxy lqClientProxy = (lqClientProxy)allObjectsInLocality.get_Item(i);
				if (lqClientProxy != ignoreObject)
				{
					float num2 = lqClientProxy.x - x;
					float num3 = lqClientProxy.y - y;
					float num4 = lqClientProxy.z - z;
					float num5 = num2 * num2 + num3 * num3 + num4 * num4;
					if (num5 < num)
					{
						result = lqClientProxy;
						num = num5;
					}
				}
			}
			return result;
		}

		public ArrayList getAllObjects()
		{
			int num = this.divx * this.divy * this.divz;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < num; i++)
			{
				arrayList.AddRange(this.bins[i].clientList);
			}
			arrayList.AddRange(this.other.clientList);
			return arrayList;
		}

		public void lqRemoveAllObjectsInBin(lqBin bin)
		{
			bin.clientList.Clear();
		}

		public void lqRemoveAllObjects()
		{
			int num = this.divx * this.divy * this.divz;
			for (int i = 0; i < num; i++)
			{
				this.lqRemoveAllObjectsInBin(this.bins[i]);
			}
			this.lqRemoveAllObjectsInBin(this.other);
		}

		public Vector3 getMostPopulatedBinCenter()
		{
			lqBin mostPopulatedBin = this.getMostPopulatedBin();
			if (mostPopulatedBin != null)
			{
				return mostPopulatedBin.center;
			}
			return Vector3.get_zero();
		}

		public lqBin getMostPopulatedBin()
		{
			int num = this.divx * this.divy * this.divz;
			int num2 = 0;
			lqBin result = null;
			for (int i = 0; i < num; i++)
			{
				if (this.bins[i].clientList.get_Count() > num2)
				{
					num2 = this.bins[i].clientList.get_Count();
					result = this.bins[i];
				}
			}
			return result;
		}
	}
}

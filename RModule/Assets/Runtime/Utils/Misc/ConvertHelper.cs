using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ConvertHelper {
	public static byte[] Color32ArrayToByteArray(Color32[] colors) {
		if (colors == null || colors.Length == 0)
			return null;

		int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
		int length = lengthOfColor32 * colors.Length;
		byte[] bytes = new byte[length];

		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(ptr, bytes, 0, length);
		} finally {
			if (handle != default(GCHandle))
				handle.Free();
		}

		return bytes;
	}

	public static byte Color32ToByte(Color32 color) {

		int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
		int length = lengthOfColor32;
		byte[] bytes = new byte[length];

		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(color, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(ptr, bytes, 0, length);
		} finally {
			if (handle != default(GCHandle))
				handle.Free();
		}

		return bytes[0];
	}
	public static object RawDeserializeEx(byte[] rawdatas, Type anytype) {
		int rawsize = Marshal.SizeOf(anytype);
		if (rawsize > rawdatas.Length)
			return null;
		GCHandle handle = GCHandle.Alloc(rawdatas, GCHandleType.Pinned);
		IntPtr buffer = handle.AddrOfPinnedObject();
		object retobj = Marshal.PtrToStructure(buffer, anytype);
		handle.Free();
		return retobj;
	}

	public static byte[] RawSerializeEx(object anything) {
		int rawsize = Marshal.SizeOf(anything);
		byte[] rawdatas = new byte[rawsize];
		GCHandle handle = GCHandle.Alloc(rawdatas, GCHandleType.Pinned);
		IntPtr buffer = handle.AddrOfPinnedObject();
		Marshal.StructureToPtr(anything, buffer, false);
		handle.Free();
		return rawdatas;
	}

	public static byte[] ObjectToByteArray(object obj) {
		if (obj == null)
			return null;
		BinaryFormatter bf = new BinaryFormatter();
		using (MemoryStream ms = new MemoryStream()) {
			bf.Serialize(ms, obj);
			return ms.ToArray();
		}
	}

	public static List<int> ConvertColor32ToListInt(Color32 color32) {
		List<int> rgbaList = new List<int>();
		rgbaList.Add(color32.r);
		rgbaList.Add(color32.g);
		rgbaList.Add(color32.b);
		rgbaList.Add(color32.a);

		return rgbaList;
	}

	public static Color32 ConvertListIntToColor32(List<int> rgbaList) {
		return new Color32((byte)rgbaList[0], (byte)rgbaList[1], (byte)rgbaList[2], (byte)rgbaList[3]);
	}

	public static byte[] ConvertToByteArray(string myCustomString) {//myCustomString is in "[1,2,3]" format
		return myCustomString.Substring(1, myCustomString.Length - 2)
							 .Split(',')
							 .Select(s => byte.Parse(s))
							 .ToArray();
	}
}

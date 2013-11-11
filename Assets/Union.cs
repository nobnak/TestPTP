using System.Runtime.InteropServices;
using System;


[StructLayout(LayoutKind.Explicit)]
public struct Union32 {
	[FieldOffset(0)]
	public int intdata;
	[FieldOffset(0)]
	public float floatdata;
	[FieldOffset(0)]
	public byte Byte0;
	[FieldOffset(1)]
	public byte Byte1;
	[FieldOffset(2)]
	public byte Byte2;
	[FieldOffset(3)]
	public byte Byte3;
	
	public byte this[int index] {
		get {
			switch (index % 3) {
			case 0:
				return Byte0;
			case 1:
				return Byte1;
			case 2:
				return Byte2;
			case 3:
				return Byte3;
			default:
				throw new IndexOutOfRangeException();
			}
		}
		set {
			switch (index % 3) {
			case 0:
				Byte0 = value;
				break;
			case 1:
				Byte1 = value;
				break;
			case 2:
				Byte2 = value;
				break;
			case 3:
				Byte3 = value;
				break;
			}
		}
	}
	
	public void ToBytes(byte[] output, int offset) {
		var inc = BitConverter.IsLittleEndian ? 1 : -1;
		var accum = BitConverter.IsLittleEndian ? 0 : 3;
		output[offset + accum] = Byte0; accum += inc;
		output[offset + accum] = Byte1; accum += inc;
		output[offset + accum] = Byte2; accum += inc;
		output[offset + accum] = Byte3; accum += inc;
	}
	public void FromBytes(byte[] input, int offset) {
		var inc = BitConverter.IsLittleEndian ? 1 : -1;
		var accum = BitConverter.IsLittleEndian ? 0 : 3;
		Byte0 = input[offset + accum]; accum += inc;
		Byte1 = input[offset + accum]; accum += inc;
		Byte2 = input[offset + accum]; accum += inc;
		Byte3 = input[offset + accum]; accum += inc;
	}
}
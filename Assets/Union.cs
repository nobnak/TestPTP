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
	
	public void Assign(byte[] output, int offset) {
		var inc = BitConverter.IsLittleEndian ? 1 : -1;
		var accum = BitConverter.IsLittleEndian ? 0 : 3;
		output[offset + accum] = Byte0; accum += inc;
		output[offset + accum] = Byte1; accum += inc;
		output[offset + accum] = Byte2; accum += inc;
		output[offset + accum] = Byte3; accum += inc;
	}
}
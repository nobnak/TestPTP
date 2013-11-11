using System;
using System.Text;

public static class PTP {
	public const int PACKET_TYPE_INIT_COMMAND_REQUEST	= 1;
	public const int PACKET_TYPE_INIT_COMMAND_ACK		= 2;
	public const int PACKET_TYPE_INIT_EVENT_REQUEST		= 3;
	public const int PACKET_TYPE_INIT_EVENT_ACK			= 4;
	
	public static byte[] InitCommandRequest(Guid guid, string name) {
		var data = new byte[16 + Encoding.Unicode.GetByteCount(name) + 1];
		Array.Copy(guid.ToByteArray(), data, 16);
		Assign (name, data, 16);
		return Encode(PACKET_TYPE_INIT_COMMAND_REQUEST, data);
	}
	
	public static byte[] Encode(int type, byte[] data) {
		var length = 8 + data.Length;
		var packet = new byte[length];
		
		var uniLength = new Union32() { intdata = length };
		var uniType = new Union32() { intdata = type };
		
		uniLength.ToBytes(packet, 0);
		uniType.ToBytes(packet, 4);
		Array.Copy(data, 0, packet, 8, data.Length);
		
		return packet;
	}
	
	public static void Assign(string data, byte[] output, int offset) {
		var encodedLength = Encoding.Unicode.GetBytes(data, 0, data.Length, output, offset);
		output[offset + encodedLength] = 0;
	}
}
using System.IO;

public class PTPIPStream {
	
	private NetworkStream _stream;
	
	public PTPIPStream(NetworkStream stream) {
		this._stream = stream;
	}
	
	public struct Packet {
		public PacketType type;
		public byte[] payload;
	}

	public enum PacketType { 
		Init_Command_Request	= 1,
		Init_Command_Ack		= 2,
		Init_Event_Request		= 3,
		Init_Event_Ack			= 4,
		Init_Fail				= 5,
		Cmd_Request				= 6,
		Cmd_Response			= 7,
		Event					= 8,
		Start_Data_Packet		= 9,
		Data_Packet				= 10,
		Cancel_Transaction		= 11,
		End_Data_Packet			= 12,
		Ping_Pong				= 13,
	};
}


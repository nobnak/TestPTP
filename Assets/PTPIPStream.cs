using System.IO;
using System;
using System.Net.Sockets;

public class PTPIPStream {
	public event Action<Packet> OnResponse;
	
	private NetworkStream _stream;
	private ReadState _readState;
	private byte[] _bufInt32 = new byte[4];
	private int _bufInt32Offset = 0;
	private byte[] _payload;
	private int _payloadOffset = 0;
	private Union32 _intConverter = new Union32();
	private PacketType _type;
	
	public PTPIPStream(NetworkStream stream) {
		this._stream = stream;
	}
	
	public void Request(Packet packet) {
		var type = new Union32(){ intdata = (int) packet.type };
		var typeBytes = new byte[4];
		type.ToBytes(typeBytes, 0);
		_stream.Write(typeBytes, 0, typeBytes.Length);
		_stream.Write(packet.payload, 0, packet.payload.Length);
		_stream.Flush();
	}
	
	public void Poll() {
		while (_stream.DataAvailable) {
			switch (_readState) {
			case ReadState.ReadLength:
				_bufInt32Offset += _stream.Read(_bufInt32, _bufInt32Offset, 4 - _bufInt32Offset);
				if (_bufInt32Offset == 4) {
					_readState = ReadState.ReadType;
					_bufInt32Offset = 0;
					_intConverter.FromBytes(_bufInt32, 0);
					_payload = new byte[_intConverter.intdata];
				}
				break;
			case ReadState.ReadType:
				_bufInt32Offset += _stream.Read (_bufInt32, _bufInt32Offset, 4 - _bufInt32Offset);
				if (_bufInt32Offset == 4) {
					_readState = ReadState.ReadPayload;
					_bufInt32Offset = 0;
					_intConverter.FromBytes(_bufInt32, 0);
					_type = (PacketType)_intConverter.intdata;
				}
				break;
			case ReadState.ReadPayload:
				_payloadOffset += _stream.Read(_payload, _payloadOffset, _payload.Length - _payloadOffset);
				if (_payloadOffset == _payload.Length) {
					_readState = ReadState.ReadLength;
					_payloadOffset = 0;
					if (OnResponse != null) {
						OnResponse(new Packet(){ type = _type, payload = _payload });
					}
				}
				break;
			}
		}
	}
	
	public enum ReadState { ReadLength = 0, ReadType, ReadPayload };
	
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


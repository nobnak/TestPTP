using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class PTPController : MonoBehaviour {
	public string serverAddress;
	public int serverPort;

	// Use this for initialization
	void Start () {
		var client = new TcpClient(serverAddress, serverPort);
		var stream = client.GetStream();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class Networking : MonoBehaviour {
	// Use this for initialization
	SocketIOClient.Client socket;
	
	void Start () {
		
		socket = new SocketIOClient.Client("http://54.149.110.214:1357");
		socket.On("connect", (fn) => {
			Debug.Log ("connect - socket");
//			Dictionary<string, string> args = new Dictionary<string, string>();
			socket.Emit("GameSetup", "");
		});
		socket.On("Pregame", (data) => {
			Debug.Log (data.Json.ToJsonString());
		});
		socket.On("GameStart", (data) => {
			Debug.Log (data.Json.ToJsonString());

		});
		socket.Error += (sender, e) => {
			Debug.Log ("socket Error: " + e.Message.ToString() + sender);
		};
		socket.Connect();
	}

	
//	private void SocketOpened (object sender, EventArgs e){
//		Debug.Log("socket opened");
//		this.socket.Emit("GameSetup", "");
//	}
//	
//	private void SocketMessage (object sender, MessageEventArgs e) {
//		if ( e!= null && e.Message.Event == "Pregame") {
//			string msg = e.Message.MessageText;
//			Debug.Log("socket message: " + msg);
//		}
//	}
//	
//	private void SocketConnectionClosed (object sender, EventArgs e) {
//		Debug.Log("socket closed");
//	}
//	
//	private void SocketError (object sender, MessageEventArgs e) {
//		Debug.Log("socket error: " + e.Message);
//	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;


public class MyScript : Networking {
	// Use this for initialization
	public static int IntParseFast(string value)
	{
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}
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
			var N = JSON.Parse(data.Json.ToJsonString());
			string val = N["args"][0];
			Debug.Log("data");
			Debug.Log(val);
		});
		socket.On("GameStart", (data) => {
			var N = JSON.Parse(data.Json.ToJsonString());
			string temp = N["args"][0]["player"];
			int val = IntParseFast(temp);
			Debug.Log (val);

		});
		socket.On("Move", (data) => {
			var N = JSON.Parse(data.Json.ToJsonString());
			float x = float.Parse(N["args"][0]["x"]);
			float y = float.Parse(N["args"][0]["y"]);
			float dir = float.Parse(N["args"][0]["dir"]);
			string timestamp = float.Parse (N["args"][0]["timestamp"]);
			Debug.Log ("Move");
		});
		socket.On("Shoot", (data) => {
			var N = JSON.Parse(data.Json.ToJsonString());
			float x = float.Parse(N["args"][0]["x"]);
			float y = float.Parse(N["args"][0]["y"]);
			float dir = float.Parse(N["args"][0]["dir"]);
			string timestamp = float.Parse (N["args"][0]["timestamp"]);
			Debug.Log ("Move");
		});

		socket.Error += (sender, e) => {
			Debug.Log ("socket Error: " + e.Message.ToString() + sender);
		};
		socket.Connect();
	}

	// Update is called once per frame
	void Update () {
		
	}
}

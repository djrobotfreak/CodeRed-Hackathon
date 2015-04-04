using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class Networking {
	SocketIOClient.Client socket;
	long MoveTimestamp = 0;
	long ShootTimestamp = 0;
	// Use this for initialization
	public static string GetTimestamp(DateTime value)
	{
		return (value.ToString("yyyyMMddHHmmssfff"));
	}
	public void Move(float x, float y, float dir){
		string timestamp = GetTimestamp(DateTime.UtcNow);
		JSONClass cl = new JSONClass();
		cl ["timestamp"] = timestamp;
		cl ["x"].AsFloat = x;
		cl ["y"].AsFloat = y;
		cl ["dir"].AsFloat = dir;
		socket.Emit("Move", cl.ToString());
		Debug.Log ("Sending Move");
	}
	public void Shoot(float x, float y, float dir){
		string timestamp = GetTimestamp(DateTime.UtcNow);
		JSONClass cl = new JSONClass();
		cl ["timestamp"] = timestamp;
		cl ["x"].AsFloat = x;
		cl ["y"].AsFloat = y;
		cl ["dir"].AsFloat = dir;
		socket.Emit("Shoot", cl.ToString());
		Debug.Log (timestamp);
	}
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
			Debug.Log ("Recieving Move");
			var N = JSONNode.Parse(data.Json.ToJsonString()); Debug.Log (data.Json.ToJsonString());
			float x = (N["args"][0]["x"].AsFloat);
			Debug.Log (x);
			float y = (N["args"][0]["y"]).AsFloat; 
			Debug.Log (y);
			float dir = (N["args"][0]["dir"]).AsFloat; 
			Debug.Log (dir);
			long timestamp = long.Parse(N["args"][0]["timestamp"]); 
			if (MoveTimestamp > timestamp){
				return;
			}
			MoveTimestamp = timestamp;
			Debug.Log (MoveTimestamp);
			//moveFunction Here.
			
		});
		socket.On("Shoot", (data) => {
			var N = JSONNode.Parse(data.Json.ToJsonString());
			float x = (N["args"][0]["x"].AsFloat);
			float y = (N["args"][0]["y"].AsFloat);
			float dir = (N["args"][0]["dir"].AsFloat);
			long timestamp = long.Parse(N["args"][0]["timestamp"]);
			Debug.Log ("Shoot");
			if (ShootTimestamp > timestamp){
				return;
			}
			ShootTimestamp = timestamp;
			Debug.Log (ShootTimestamp);
			//shootFunction Here.
		});
		
		socket.Error += (sender, e) => {
			Debug.Log ("socket Error: " + e.Message.ToString() + sender);
		};
		socket.Connect();
	}
	void Update () {
		
	}
}

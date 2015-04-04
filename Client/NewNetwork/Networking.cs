using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class Networking {
	SocketIOClient.Client socket;
	long MoveTimestamp = 0;
	long ShootTimestamp = 0;
	bool isPregame = true;
	public bool isGame = false;
	// Use this for initialization
	public static string GetTimestamp(DateTime value)
	{
		return (value.ToString("yyyyMMddHHmmssfff"));
	}
	public void Move(Vector3 main_pos, Quaternion main_dir, Vector3 enemy_pos, Quaternion enemy_dir){
		string timestamp = GetTimestamp(DateTime.UtcNow);
		JSONClass cl = new JSONClass();
		cl ["timestamp"] = timestamp;
		cl ["main_x"].AsFloat = main_pos.x;
		cl ["main_y"].AsFloat = main_pos.y;
		cl ["main_z"].AsFloat = main_pos.z;
		cl ["main_dirx"].AsFloat = main_dir.x;
		cl ["main_diry"].AsFloat = main_dir.y;
		cl ["main_dirz"].AsFloat = main_dir.z;
		cl ["enemy_x"].AsFloat = enemy_pos.x;
		cl ["enemy_y"].AsFloat = enemy_pos.y;
		cl ["enemy_z"].AsFloat = enemy_pos.z;
		cl ["enemy_dirx"].AsFloat = enemy_dir.x;
		cl ["enemy_diry"].AsFloat = enemy_dir.y;
		cl ["enemy_dirz"].AsFloat = enemy_dir.z;
		socket.Emit("Move", cl.ToString());
		Debug.Log ("Sending Move");
	}
	public void Shoot(Vector3 main_pos, Quaternion main_dir, Vector3 enemy_pos, Quaternion enemy_dir){
		string timestamp = GetTimestamp(DateTime.UtcNow);
		JSONClass cl = new JSONClass();
		cl ["timestamp"] = timestamp;
		cl ["main_x"].AsFloat = main_pos.x;
		cl ["main_y"].AsFloat = main_pos.y;
		cl ["main_z"].AsFloat = main_pos.z;
		cl ["main_dirx"].AsFloat = main_dir.x;
		cl ["main_diry"].AsFloat = main_dir.y;
		cl ["main_dirz"].AsFloat = main_dir.z;
		if (isGame) {
			cl ["enemy_x"].AsFloat = enemy_pos.x;
			cl ["enemy_y"].AsFloat = enemy_pos.y;
			cl ["enemy_z"].AsFloat = enemy_pos.z;
			cl ["enemy_dirx"].AsFloat = enemy_dir.x;
			cl ["enemy_diry"].AsFloat = enemy_dir.y;
			cl ["enemy_dirz"].AsFloat = enemy_dir.z;
			Debug.Log("2nd Player Detected");
		}
		socket.Emit("Shoot", cl.ToString());
		Debug.Log ("shoot");
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
	
	
	public void Start () {
		socket = new SocketIOClient.Client("http://54.149.110.214:1357");
		socket.On("connect", (fn) => {
			Debug.Log ("connect - socket");
			//			Dictionary<string, string> args = new Dictionary<string, string>();
			socket.Emit("GameSetup", "");
		});
		socket.On("Pregame", (data) => {
			if (isPregame) {
				Debug.Log (data.Json.ToJsonString());
				var N = JSON.Parse(data.Json.ToJsonString());
				string val = N["args"][0];
				Debug.Log("data");
				Debug.Log(val);
				isPregame = false;
				isGame = true;
			}
		});
		socket.On("GameStart", (data) => {
			var N = JSON.Parse(data.Json.ToJsonString());
			string temp = N["args"][0]["player"];
			int val = IntParseFast(temp);
			Debug.Log (val);
			isPregame = false;

		});
		socket.On("Move", (data) => {
			Debug.Log ("Recieving Move");
			var N = JSONNode.Parse(data.Json.ToJsonString()); Debug.Log (data.Json.ToJsonString());
			Quaternion main_dir, enemy_dir; 
			Vector3 main_pos, enemy_pos;
			main_pos.x = N["args"][0]["x"].AsFloat;
			main_pos.y = N["args"][0]["y"].AsFloat; 
			main_pos.z = N["args"][0]["z"].AsFloat;
			main_dir.x = N["args"][0]["main_dirx"].AsFloat;
			main_dir.y = N["args"][0]["main_diry"].AsFloat; 
			main_dir.z = N["args"][0]["main_dirz"].AsFloat;
			enemy_pos.x = N["args"][0]["enemy_x"].AsFloat;
			enemy_pos.y = N["args"][0]["enemy_y"].AsFloat; 
			enemy_pos.z = N["args"][0]["enemy_z"].AsFloat;
			enemy_dir.x = N["args"][0]["enemy_dirx"].AsFloat;
			enemy_dir.y = N["args"][0]["enemy_diry"].AsFloat; 
			enemy_dir.z = N["args"][0]["enemy_dirz"].AsFloat;
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
			Vector3 main_pos, enemy_pos;
			Quaternion main_dir, enemy_dir;
			main_pos.x = N["args"][0]["main_x"].AsFloat;
			main_pos.y = N["args"][0]["main_y"].AsFloat; 
			main_pos.z = N["args"][0]["main_z"].AsFloat;
			main_dir.x = N["args"][0]["main_dirx"].AsFloat;
			main_dir.y = N["args"][0]["main_diry"].AsFloat; 
			main_dir.z = N["args"][0]["main_dirz"].AsFloat;
			enemy_pos.x = N["args"][0]["enemy_x"].AsFloat;
			enemy_pos.y = N["args"][0]["enemy_y"].AsFloat; 
			enemy_pos.z = N["args"][0]["enemy_z"].AsFloat;
			enemy_dir.x = N["args"][0]["enemy_dirx"].AsFloat;
			enemy_dir.y = N["args"][0]["enemy_diry"].AsFloat; 
			enemy_dir.z = N["args"][0]["enemy_dirz"].AsFloat;
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

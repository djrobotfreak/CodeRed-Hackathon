/*
 * Copyright 2013. Amazon Web Services, Inc. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
**/


console.log('Im starting');
// Load the SDK and UUID
var AWS = require('aws-sdk');
var uuid = require('node-uuid');
var io = require('socket.io');
var express = require('express');
var app = express()
  , server = require('http').createServer(app)
  , io = io.listen(server);
// The node.js HTTP server.
var First = true;
var player1;
var player2;
// The socket.io WebSocket server, running with the node.js server.

function Player( First, Socket )
{
    this.first = First;
	this.socket = Socket;
}

server.listen(1357);
io.on('connection', function(socket){
	console.log('new connection');
	socket.on('GameSetup', function (data) 
	{
		console.log('howdy');
		if (First){
			player1 = new Player(true, socket);
			First = false;
			player1.socket.emit('Pregame', "howdy");
		}
		else{
			player2 = new Player(false, socket);
			First = true;
			player2.socket.emit('GameStart', JSON.stringify({'player': 2}));
			player1.socket.emit('GameStart', JSON.stringify({'player': 1}));
		}
	});
	socket.on('Move', function(data){
		console.log('howdy');
		if (player1){
			player1.socket.emit('Move', data);
		}
		if (player2){
			player2.socket.emit('Move', data);
		}
		// if (player1.socket == socket){
		// 	player2.socket.emit('move', data);
		// }
		// else{
		// 	player1.socket.emit('move', data);
		// }
	});
	socket.on('Shoot', function(data){
		console.log('howdy');
		// player2.socket.emit('Shoot',data);
		player1.socket.emit('Shoot',data);
		// if (player1.socket == socket){
		// 	player2.socket.emit('shoot',data);
		// }
		// else{
		// 	player1.socket.emit('shoot',data);
		// }
	});
});

// Create an S3 client
// var s3 = new AWS.S3();



// Create a bucket and upload something into it
// var bucketName = 'node-sdk-sample-' + uuid.v4();
// var keyName = 'hello_world.txt';

// s3.createBucket({Bucket: bucketName}, function() {
//   var params = {Bucket: bucketName, Key: keyName, Body: 'Hello World!'};
//   s3.putObject(params, function(err, data) {
//     if (err)
//       console.log(err)
//     else
//       console.log("Successfully uploaded data to " + bucketName + "/" + keyName);
//   });
// });
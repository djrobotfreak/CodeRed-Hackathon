var AWS = require('aws-sdk');
var uuid = require('node-uuid');
var io1 = require('socket.io').listen(8321);

io1.on('connection', function(socket1) {
  socket1.on('bar', function(msg1) {
    console.log(msg1);
  });
});

// Mirror
var ioIn = require('socket.io').listen(8123);
var ioOut = require('socket.io-client');
var socketOut = ioOut.connect('http://localhost:8321');


ioIn.on('connection', function(socketIn) {
  socketIn.on('foo', function(msg) {
    socketOut.emit('bar', msg);
  });
});

// Client
var io2 = require('socket.io-client');
var socket2 = io2.connect('54.149.110.214:1357');

var msg2 = "hello";
socket2.emit('bar', msg2);
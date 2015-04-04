var AWS = require('aws-sdk');
var uuid = require('node-uuid');
var io = require('socket.io');
var express = require('express');
var app = express()
  , server = require('http').createServer(app)
  , io = io.listen(server);

server.listen(1357);

io.sockets.on('connection', function (socket) {
  // socket.emit('news', { hello: 'world' });
  socket.on('GameSetup', function (data) {
    console.log(data);
  });
});
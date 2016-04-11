var assert = require("chai").assert,
    Client = require("../app/client.js"),
    listener = require("../app/listener.js");

var client1, client2;
var clients = [];
var messages = [];

listener.on("connection", function (socket) {
    socket.write("Pong");
    clients.push(socket);
});
listener.on("data", function (data) {
    messages.push(data);
});

describe("Step 1", function () {
    before(function (done) {
        listener.on("listening", function (port) {
            client1 = new Client("127.0.0.1", port);
            console.log("port",port);
            client2 = new Client("127.0.0.1", port);
            console.log("port",port);
            
            setTimeout(function () {
                done();
            }, 1000);
        });
        listener.listen();
    });
    it("clients should be connected", function () {
        assert.equal(clients.length, 2, "Unexpected client count");
    });
});

describe("Step 2", function () {    
    it("messages should be received", function () {
        assert.equal(messages.length, 2, "Unexpected message count");
        for (var i = 0; i < messages.length; i++)
            assert.equal(messages[i], "Hello World!", "Unexpected message content");
    });
});

describe("Step 3", function () {
    before(function (done) {
        messages = [];
        for (var i = 0; i < clients.length; i++)
            clients[i].write("Marco");

        setTimeout(function () {
            done();
        }, 1000);
    });
    it("clients should be connected", function () {
        assert.equal(clients.length, 2, "Unexpected client count");
    });
    it("messages should be received", function () {
        assert.equal(messages.length, 2, "Unexpected message count");
        for (var i = 0; i < messages.length; i++)
            assert.equal(messages[i], "Polo", "Unexpected message content");
    });
});
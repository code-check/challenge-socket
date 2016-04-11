var 
    net = require("net"),
    util = require("util"),
    EventEmitter = require('events').EventEmitter,
    server = net.createServer();

function Listener() {
    EventEmitter.call(this);
    var _this = this;

    _this.listen = function () {
        server.listen(0, "127.0.0.1", 10, function () {
            _this.emit("listening", server.address().port);
        });
        
        server.on("connection", function (socket) {
            socket.on("data", function (data) {
                _this.emit("data", data.toString("utf8"));
            });
            _this.emit("connection", socket);
        });
    }
};

util.inherits(Listener, EventEmitter);
module.exports = new Listener();
//BEGIN_CHALLENGE
var net = require("net");
//END_CHALLENGE

var Client = function (ip, port) {
    //BEGIN_CHALLENGE
    var socket = new net.Socket();
    socket.on("data", function (data) {
        switch (data.toString("utf8")) {
            case "Marco":
                socket.write("Polo");
                break;
        }
    });
    socket.connect(port, ip, function () {
        socket.write("Hello World!");
    });
    //END_CHALLENGE
}

module.exports = Client;

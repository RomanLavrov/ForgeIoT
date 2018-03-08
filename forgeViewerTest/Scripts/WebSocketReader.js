alert("WebSocketReaderStarted");
var ws = new WebSocket('wss://' + location.host + '/WebSocketServer.ashx');

console.log('Websocket URL: ' + ws.url);

ws.onopen = function () {
    console.log('Connected to WebSocket');
    ws.send("Hello from forge");
};

ws.onmessage = function (message) {
    alert(message.data);
    console.log('receive message' + message.data);
};

ws.onerror = function (error) {
    console.log("WebSocket error: " + error.message);
};

ws.onclose = function () {
    console.log('connection closed')
}

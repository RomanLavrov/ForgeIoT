alert('Server started');

const express = require('express');




const app = express();


var iotHubReader = new iotHubReader(process.env['ForgeIoT.IoTHub.ConnectionString'], process.env['ForgeIoT.IoTHub.ConsumerGroup']);
iotHubReader.startReadMessage(function (obj, date) {
    try {
        console.log(date);
        date = date || Date.now()
       
    }


});
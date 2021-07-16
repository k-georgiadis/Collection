/*
	MIT License
	Copyright (c) 2021 Kosmas Georgiadis
	
	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

( function($) {
	
	 $(document).ready( function(){

		getTemp();

	});
	
	var getTemp = function(){	
		//Check if we can find the ESP locally.
		localRequest();
	};
	
	var localRequest = function(){
		
		var ip = $("#ip").val(); //Get local IP of ESP.
		var url = "http://" + ip; //Build url.
		
		//Make local GET request.
		$.ajax({
			type: "GET",
			url: url,
			data: {TEMP: "1"},
			//headers: {},
			contentType: "text/plain",
			dataType: "text",
			timeout: 5000,
			error: function (request, error) {
				failGET(error, "ESP not found in LAN, checking remote network...");
				remoteRequest(); //Try remote request.
			},
			success: function(data){ //Success.
				successGET(data);
				setTimeout(localRequest, 1000); //Loop every 1 sec.
			}
		});
		
	};
		
	var remoteRequest = function(){
		
		//Get remote IP of ESP from current URL.
		//We use a specific port to avoid entering the router (192.168.1.1:80).
		var ip = window.location.hostname + ":1088"; //Make sure to LISTEN to this port (httpd.conf).
		var url = "http://" + ip; //Build url.
		
		//Make remote GET request.
		$.ajax({
			type: "GET",
			url: url,
			data: {TEMP: "1"},
			//headers: {},
			contentType: "text/plain",
			dataType: "text",
			timeout: 10000,
			error: function (request, error) {
				failGET(error);
			},
			success: function(data){ //Success.
				successGET(data);
				setTimeout(remoteRequest, 1000);
			}
		});
		
	};
	
	var successGET = function(data){

		var color = "transparent"; //Default background color of temperature value.
		
		$("#status").removeClass("error");
		$("#status").addClass("success"); //Add success class.
		$("#status").text("Successfully connected to ESP Server.");
		$("#tempLabel").text("Temperature:").append("&nbsp;");
		
		var temp = parseFloat(data); //Get float value.
		
		if(temp) //If valid.
		{
			$("#tempVal").text(data);
			$("#sp").text("o");
			$("#celcius").text("C");
			
			//Set color of background depending on temperature.
			if(temp <= -10.0)
				color = "darkblue";
			else if(temp <= 0.0)
				color = "blue";
			else if(temp <= 9.9)
				color = "lightblue";
			else if(temp <= 19.9)
				color = "forestgreen";
			else if(temp <= 29.9)
				color = "green";
			else if(temp <= 39.9)
				color = "orange";
			else if(temp <= 49.9)
				color = "darkorange";
			else if(temp <= 110.0)
				color = "red";
		}
		else
		{
			$("#tempVal").text("Invalid data");
			$("#sp").text("");
			$("#celcius").text("");
		}
		
		$("#tempVal").css("color", color); //Set color.
		$("#sp").css("color", color); //Set color.
		$("#celcius").css("color", color); //Set color.
	};
	
	var failGET = function(error, msg = ""){
		$("#status").removeClass("success");
		$("#status").addClass("error"); //Add error class.
		
		if(msg)
			$("#status").text(msg);
		else
		{
			$("#tempLabel").text("");
			$("#tempVal").text("");
			$("#sp").text("");
			$("#celcius").text("");
			$("#tempVal").css("background-color", "transparent");
			$("#status").text("Failed to connect to ESP server: " + error);
		}
	}
	
} ) ( jQuery );
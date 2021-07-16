<?php

/*
	MIT License
	Copyright (c) 2021 Kosmas Georgiadis
	
	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

?>

<html>
	<body bgcolor="LightGray">

		<head>
			<hr>
			<font face="Segoe UI" size="+2" color="firebrick"><b>NTC Temperature Control using ESP8266</b></font>
			<font face="Segoe UI" size="+2" color="darkviolet"><b>  -  G&K Electronics</b></font>
			<meta http-equiv="refresh" content="2" > 
			<hr>
		</head>
		
</html>
<?php
	SESSION_START();
	
	//Make GET request using CURL, for Temperature value.
	$url = 'http://' . $_SESSION['IP'] . '/?TEMP=1';

	$ch = curl_init();
	$timeout = 5;
	
	curl_setopt($ch, CURLOPT_URL, $url);
	curl_setopt($ch, CURLOPT_MAXCONNECTS, 1);
	curl_setopt($ch, CURLOPT_CRLF, 1);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
	curl_setopt($ch, CURLOPT_TIMEOUT, $timeout);
	curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, $timeout);

	$response = curl_exec($ch);
	curl_close($ch);
	
	if($response == FALSE) //If GET failed.
	{
		echo '<br><hr><font face="Segoe UI" size="+1" color="red">STATUS: Failed to connect to ESP server.</font><hr>';
		exit(1);
	}
	else
		echo '<br><hr><font face="Segoe UI" size="+1" color="green">STATUS: Successfully connected to ESP Server.</font><hr>';
?>
<html>
	<br>
	<br>
	
	<?php
		$response = floatval($response); //Convert Temperature value string to float.
		$response = number_format($response, 1); //Format number. This is only needed when the value is integer i.e: 25.0;
		$color = 'black';
		
		if($response <= -10.0)
			$color = 'darkblue';
		else if($response <= 0.0)
			$color = 'blue';
		else if($response <= 9.9)
			$color = 'lightblue';
		else if($response <= 19.9)
			$color = 'forestgreen';
		else if($response <= 29.9)
			$color = 'grass';
		else if($response <= 39.9)
			$color = 'orange';
		else if($response <= 49.9)
			$color = 'darkorange';
		else if($response <= 110.0)
			$color = 'red';
		
		echo '<font face="Segoe UI" size="+1" color="black">Temperature: </font>';
		echo '<font face="Segoe UI" size="+1" color="' . $color . '"> ' . $response . '</font>';
	
	?>
	
	</body>
</html>
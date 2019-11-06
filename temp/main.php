<?php SESSION_START(); ?>
<html>
	<script type="text/javascript" src="jquery-3.2.1.min.js"></script>
	<script type="text/javascript" src="custom.js"></script>
	
	<head>
		<link rel="stylesheet" type="text/css" href="style.css">
	</head>
	
	<input type="hidden" id="ip" value=<?= $_SESSION['IP'] ?>>
	<body bgcolor="LightGray">
		<hr>
		<font face="Segoe UI" size="+2" color="firebrick"><b>NTC Temperature Control using ESP8266</b></font>
		<font face="Segoe UI" size="+2" color="darkviolet"><b>  -  Κοσμάς Γεωργιάδης</b></font>
		<!-- <meta http-equiv="refresh" content="2" > -->
		<hr>
		<hr>
		<div id="status">
</html>

<?php
	//Make GET request using CURL, for Temperature value.
	// $url = 'http://' . $_SESSION['IP'] . '/?TEMP=1';

	// $ch = curl_init();
	// $timeout = 10;
	
	// curl_setopt($ch, CURLOPT_URL, $url);
	// curl_setopt($ch, CURLOPT_MAXCONNECTS, 1);
	// curl_setopt($ch, CURLOPT_CRLF, 1);
	// curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
	// curl_setopt($ch, CURLOPT_TIMEOUT, $timeout);
	// curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, $timeout);

	// $response = curl_exec($ch);
	// curl_close($ch);
	
	// if($response == FALSE) //If GET failed.
	// {
		// echo 'Failed to connect to ESP server.';
		// exit(1);
	// }
	// else
		// echo 'Successfully connected to ESP Server.';
?>
<html>
	</div>
	<hr>
	
	<br>
	<br>
	
	<div id="tempLabel"></div>
	<div id="tempVal"></div><sup id="sp"></sup><p id="celcius"></p>
	
	<?php
		//$response = floatval($response); //Convert Temperature value string to float.
		//$response = number_format($response, 1); //Format number. This is only needed when the value is integer i.e: 25.0;
	?>
	
	</body>
</html>
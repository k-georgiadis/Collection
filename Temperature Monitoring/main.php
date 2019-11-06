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
		<div id="status"></div>
	<hr>
	
	<br>
	<br>
	
	<div id="tempLabel"></div>
	<div id="tempVal"></div><sup id="sp"></sup><p id="celcius"></p>

	</body>
</html>

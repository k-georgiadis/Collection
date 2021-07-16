<?php 

/*
	MIT License
	Copyright (c) 2021 Kosmas Georgiadis
	
	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

SESSION_START();

?>
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

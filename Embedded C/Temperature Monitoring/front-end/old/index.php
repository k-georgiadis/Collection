<?php
	/*
		MIT License
		Copyright (c) 2021 Kosmas Georgiadis
		
		Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
		The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
	*/

	SESSION_START();
	set_time_limit(0); 

	//If ip address has already been found.
	if(isset($_SESSION['IP']))
		header('Location: main.php');
	
	$localIP = getHostByName(getHostName());//Get local ip address  of server.

	$arr = explode(".", $localIP); //Break it.
	$ip = $arr[0] . '.' . $arr[1] . '.' . $arr[2] . '.0/24'; //Create base address assuming mask is 255.255.255.0

	$handle = popen("nmap -sP $ip", "r"); //Open handle to run nmap on the given ip.

	if(ob_get_level() == 0)          
		ob_start();

	$found = FALSE;
	$isUp = FALSE;
	$ESP_IP = '';
	
	while(!feof($handle))
	{
		//If ESP module found and is up, break loop.
		if($found != FALSE && $isUp != FALSE)
			break;
		
		$buffer = fgets($handle);
		$buffer = trim(htmlspecialchars($buffer)); //Convert special characters to html. 
	
		echo $buffer . "<br />"; //Print line.
		
		if($found)
		{
			if(preg_match('/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/', $buffer, $ip_match)) //Get ESP module ip address.
				$_SESSION['IP'] = $ip_match[0]; //Save ip.
		}
		
		if(stripos($buffer, 'esp') !== FALSE)
			$found = 1;
		if($found && stripos($buffer, 'is up') !== FALSE)
			$isUp = 1;
	}

	pclose($handle);
?>
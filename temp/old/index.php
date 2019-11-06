<?php
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
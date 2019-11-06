<?php
	SESSION_START();
	set_time_limit(0); 

	//If IP address is already found, move to main page.
	// if(isset($_SESSION['IP']))
		// header("Location: main.php");
	
	// $localIP = getHostByName(getHostName());//Get local ip address  of server.

	// $arr = explode(".", $localIP); //Break it.
	// $ip = $arr[0] . '.' . $arr[1] . '.' . $arr[2] . '.0/24'; //Create base address assuming mask is 255.255.255.0

	//Parse settings file without sections.
	$settings = parse_ini_file("settings.ini");

	// Function to get the client ip address
	function get_client_ip_server() {
		$ipaddress = '';
		if ($_SERVER['HTTP_CLIENT_IP'])
			$ipaddress = $_SERVER['HTTP_CLIENT_IP'];
		else if($_SERVER['HTTP_X_FORWARDED_FOR'])
			$ipaddress = $_SERVER['HTTP_X_FORWARDED_FOR'];
		else if($_SERVER['HTTP_X_FORWARDED'])
			$ipaddress = $_SERVER['HTTP_X_FORWARDED'];
		else if($_SERVER['HTTP_FORWARDED_FOR'])
			$ipaddress = $_SERVER['HTTP_FORWARDED_FOR'];
		else if($_SERVER['HTTP_FORWARDED'])
			$ipaddress = $_SERVER['HTTP_FORWARDED'];
		else if($_SERVER['REMOTE_ADDR'])
			$ipaddress = $_SERVER['REMOTE_ADDR'];
		else
			$ipaddress = 'UNKNOWN';
	 
		return $ipaddress;
	}
	//$_SESSION['client_IP'] = get_client_ip_server(); //Get client IP.
	
	// //Execute nmap command.
	// exec($settings['Path'] . " -sP " . $ip . " 2>&1", $cmdOutput, $ret);

	// //If command was executed successfully.
	// if(count($cmdOutput))
	// {
		// if(ob_get_level() == 0)          
			// ob_start();
		
		// $index = -1;
		
		// //Search for MAC address.
		// foreach($cmdOutput as $i => $line)
		// {
			// //echo $line;

			// //If MAC address found, exit loop.
			// if(stripos($line, $settings['MAC']) !== FALSE)
			// {
				// $index = $i;
				// break;
			// }
		// }
		
		// //Search for IP of MAC address.
		// for($index; $index >= 0; $index--)
		// {
			// //If IP is found, save it.
			// if(preg_match('/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/', $cmdOutput[$index], $ip_match))
			// {
				// $_SESSION['IP'] = $ip_match[0]; //Save ip.
				// break; //Exit loop.
			// }
		// }
		
	// }
	// else
		// echo "Nmap command failed."; 
	
	//Check if a valid IP is given and save it.
	if(preg_match('/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/', $settings['IP']))
		$_SESSION['IP'] = $settings['IP']; //Save valid IP.
	
	//If IP address is set, move to main page.
	if(isset($_SESSION['IP']))
		header("Location: main.php");
	else
		echo "Invalid IP address. Please check your settings INI file."
?>
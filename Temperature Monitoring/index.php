<?php
	SESSION_START();
	set_time_limit(0); 

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
	
	//Check if a valid IP is given and save it.
	if(preg_match('/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/', $settings['IP']))
		$_SESSION['IP'] = $settings['IP']; //Save valid IP.
	
	//If IP address is set, move to main page.
	if(isset($_SESSION['IP']))
		header("Location: main.php");
	else
		echo "Invalid IP address. Please check your settings INI file."
?>

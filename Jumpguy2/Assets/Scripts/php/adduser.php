<?php 
        $db = mysql_connect('localhost', 'u572217271_dcantrick', 'Pizzataco2!') or die('Could not connect: ' . mysql_error()); 
        mysql_select_db('u572217271_jumpguy2') or die('Could not select database');
 
        // Strings must be escaped to prevent SQL injection attack. 
        $name = mysql_real_escape_string($_GET['name'], $db); 
        $deviceid = mysql_real_escape_string($_GET['deviceid'], $db); 
        $hash = $_GET['hash']; 
 
        $secretKey="0fHtewtgfSV35W"; # Change this value to match the value stored in the client javascript below 

        $real_hash = md5($deviceid . $name . $secretKey); 
        if($real_hash == $hash) { 
            // Send variables for the MySQL database class. 
            $query = "insert into users values ('$deviceid','$name', '$score');"; 
            $result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
        } 
?>
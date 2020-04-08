<?php 
        $db = mysqli_connect("localhost", "u572217271_dcantrick", "Pizzataco2!","u572217271_jumpguy2");

        if (mysqli_connect_errno())
        {
            echo "1: connection failed"; // error code 1, connection failed
            exit();
        }
 
        // Strings must be escaped to prevent SQL injection attack. 
        $deviceid = $_POST['deviceid'];
        $name = $_POST['name'];

        $namecheckquery = "SELECT name from users where name='" . $name . "';";
        $namecheck = mysqli_query($db, $namecheckquery) or die("3: Name check query failed");

        if (mysqli_num_rows($namecheck) > 0)
        {
            echo "4: Name already exists";
            exit();
        }

        $query = "insert into users (device_id,name) values ('" . $deviceid . "','" . $name . "');";


        mysqli_query($db, $query) or die("2: Insert query failed");

        echo "0";

?>
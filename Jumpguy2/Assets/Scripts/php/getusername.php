<?php 
        $db = mysqli_connect("localhost", "u572217271_dcantrick", "Pizzataco2!","u572217271_jumpguy2");

        if (mysqli_connect_errno())
        {
            echo "!!ERROR 1: connection failed"; // error code 1, connection failed
            exit();
        }
 
        // Strings must be escaped to prevent SQL injection attack. 
        $deviceid = $_POST['deviceid'];

 
        $query = "SELECT name from users where device_id='" . $deviceid . "';";

        $namecheck = mysqli_query($db, $query) or die("!!ERROR 2: UserID retrieve query failed for query: " . $query);

        if (mysqli_num_rows($namecheck) < 1)
        {
            echo "!!ERROR 3: Name Doesn't Exist";
            exit();
        }

        while($row = mysqli_fetch_assoc($namecheck)) {
            echo $row['name']; 
        }

?>
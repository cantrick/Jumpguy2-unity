<?php 
        $db = mysqli_connect("localhost", "u572217271_dcantrick", "Pizzataco2!","u572217271_jumpguy2");

        if (mysqli_connect_errno())
        {
            echo "ERROR: connection failed"; // error code 1, connection failed
            exit();
        }
 
        // Strings must be escaped to prevent SQL injection attack. 

        $userId = (int)$_POST['userId'];

        $scorecheckquery = "SELECT score FROM scores where user_id = " . $userId . ";";
        $scorecheck = mysqli_query($db, $scorecheckquery) or die("ERROR: My Score grab query failed");

        if (mysqli_num_rows($scorecheck) < 1)
        {
            echo "ERROR: No scores to display";
            exit();
        }

        while($row = mysqli_fetch_assoc($scorecheck)) {
            echo $row['score']; 
        }
?>
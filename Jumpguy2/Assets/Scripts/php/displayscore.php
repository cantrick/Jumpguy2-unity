<?php 
        $db = mysqli_connect("localhost", "u572217271_dcantrick", "Pizzataco2!","u572217271_jumpguy2");

        if (mysqli_connect_errno())
        {
            echo "ERROR: connection failed"; // error code 1, connection failed
            exit();
        }
 
        // Strings must be escaped to prevent SQL injection attack. 

        $scorecheckquery = "SELECT users.name,scores.score FROM scores join users on scores.user_id = users.user_id order by 2 desc;";
        $scorecheck = mysqli_query($db, $scorecheckquery) or die("ERROR: Score grab query failed");

        if (mysqli_num_rows($scorecheck) < 1)
        {
            echo "No scores to display";
            exit();
        }

        if (mysqli_num_rows($scorecheck) == 1)
        {
            while($row = mysqli_fetch_assoc($scorecheck)) {
                echo $row['score'] . "\t\t" . $row['name'];
            }
        } 
        else 
        {
            while($row = mysqli_fetch_assoc($scorecheck)) {
                echo $row['score'] . "\t\t" . $row['name'] . "#";
            }
        }
        

        

?>
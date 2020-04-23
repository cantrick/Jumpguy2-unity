<?php 
        $db = mysqli_connect("localhost", "u572217271_dcantrick", "Pizzataco2!","u572217271_jumpguy2");

        if (mysqli_connect_errno())
        {
            echo "1: connection failed"; // error code 1, connection failed
            exit();
        }
 
        // Strings must be escaped to prevent SQL injection attack. 
        $userId = (int)$_POST['userId'];
        $score = (int)$_POST['score'];


        $scorecheckquery = "SELECT score from scores where user_id=" . $userId . ";";
        $scorecheck = mysqli_query($db, $scorecheckquery) or die("3: Score check query failed");
        $addscorequery = "insert into scores (user_id,score) values (" . $userId . "," . $score . ");";
        $updatescorequery = "update scores set score = " . $score . " where user_id = " . $userId . ";";

        if (mysqli_num_rows($scorecheck) > 0)
        {
            mysqli_query($db, $updatescorequery) or die("2: Update score query failed: " . $updatescorequery);
        }
        else
        {
            mysqli_query($db, $addscorequery) or die("2.5: Insert score query failed: " . $addscorequery);
        }

        echo "score successfully updated or inserted";

?>
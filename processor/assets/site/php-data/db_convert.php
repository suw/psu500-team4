<?php

/**
 * Process data from MySQL data to be ready for JSON reads
 *
 * @author Su Wang <sxw323@psu.edu>
 */

#$link = mysqli_connect("host320.hostmonster.com","fivninni_apikids","makeitw0rk","fivninni_sweng500") or die("Error " . mysqli_error($link));

/*
 * Logic:
 *  For each symbol, generate one file for each column
 */
$symbols = [
    'JPM',
    'MS',
    'C',
    'BCS',
    'BAC',
    'UBS',
    'AAPL',
    'DB',
    'FB',
    'GOOG',
    'GM',
    'GS',
    'HSBC'
];


$symbolOutArray = [];

foreach ($symbols as $symbol) {
    $symbolOutArray[] = array(
        'file' => strtolower($symbol),
        'name' => $symbol . " - "
    );
}

$symbols = array_map('strtolower', $symbols);
$columns = ['price', 'forecast', 'F1', 'F2', 'F3'];

foreach ($symbols as $symbol) {

    // Initialize (VAROOOOMMMMM)
    $data = [];

    // Get data
    $query = "
        SELECT
            unix_timestamp(date) as unix_timestamp,
            price, 
            forecast,
            F1,
            F2,
            F3
        FROM
            data_gui_graph 
        WHERE
            symbol = '$symbol'
        ORDER BY
            unix_timestamp
    " or die("ERROR" . mysqli_error($link));

    $result = $link->query($query);
    while($row = mysqli_fetch_array($result)) {
        $data[] = $row;
    }

    foreach ($columns as $column) {

        // Initiate stuff for a new column
        $outString = '';
        ob_start();
        $rowCount = 0;

        // Set up json array dataset
        echo '[';

        // Process data rows
        foreach ($data as $row) {
            $output = [];
            $rowCount++;
            echo '[';

            $output[] = $row['unix_timestamp'] . '000';
            $output[] = $row[$column];
            echo implode(',', $output);

            echo "]";

            if ($rowCount < count($data)) {
                echo ",\r\n";
            }
        }

        // Close off the json array dataset
        echo ']';

        // Shove stuff out into json file
        $outString = ob_get_contents();
        ob_clean();
        file_put_contents('json/' . $symbol . '-' . strtolower($column) . '.json', $outString);
    }
}

?>

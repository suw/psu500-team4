<?php

/**
 * Display live JSON data from MySQL database
 *
 * @author Su Wang <sxw323@psu.edu>
 */

$link = mysqli_connect("host320.hostmonster.com","fivninni_apikids","makeitw0rk","fivninni_sweng500") or die("Error " . mysqli_error($link));

/*
 * Logic:
 *  For each symbol, generate one file for each column
 */
$symbols = array(
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
);


$symbolOutArray = array();

foreach ($symbols as $symbol) {
    $symbolOutArray[] = array(
        'file' => strtolower($symbol),
        'name' => $symbol . " - "
    );
}

$columns = ['price', 'forecast', 'f1', 'f2', 'f3'];

$symbol = $_GET['symbol'];
$column = $_GET['column'];

if (!in_array($symbol, $symbols)) {
    http_response_code(400);
    echo "Symbol not found";
    exit;
}

if (!in_array($column, $columns)) {
    http_response_code(400);
    echo "Column not found";
    exit;
}

$symbols = array_map('strtolower', $symbols);


// Initialize (VAROOOOMMMMM)
$data = array();

// Get data
$query = "
    SELECT
    unix_timestamp(date) as unix_timestamp,
        $column
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


// Initiate stuff for a new column
$outString = '';
ob_start();
$rowCount = 0;

// Set up json array dataset
echo '[';

// Process data rows
foreach ($data as $row) {
    $output = array();
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

echo $outString;

?>

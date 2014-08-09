<?php

date_default_timezone_set('America/Los_Angeles');

require_once('lib_convert.php');

$data = csv_to_array('./raw-data.csv');

$types = ['actual', 'predicted', 'jpm-corr'];

$data = array_reverse($data);

foreach ($types as $type) {

    ob_start();

    // Set up json array dataset
    echo '[';

    // Process data rows
    $rowCount = 0;
    foreach ($data as $row) {

        $rowCount++;
        echo '[';

        $output = array();
        $unixTime = strftime('%s', strtotime($row['date'])) . '000';

        $output[] = $unixTime;
        switch ($type) {

        case 'actual':
            $output[] = $row['actual'];
            break;

        case 'predicted':
            $output[] = $row['predicted'];
            break;

        case 'jpm-corr':
            $output[] = $row['jpm-corr'];
            break;

        default:
            break;
        }

        echo implode(',', $output);

        echo "]";

        if ($rowCount < count($data)) {
            echo ",";
        }
    }

    // Close off the json array dataset
    echo ']';

    $outString = ob_get_contents();
    ob_clean();

    file_put_contents('json/' . $type . '.json', $outString);
}

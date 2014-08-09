<?php

/**
 * Quick helper function to convert CSV to JSON
 *
 * @author Su Wang <suw@suwdo.com>
 */

/**
 * Convert CSV to Array with given file name
 *
 * Source: http://www.php.net/manual/en/function.str-getcsv.php#99323
 *
 * @param String $filename Filename
 * @param String $delimiter CSV Delimiter (default = ,)
 *
 * @return Array
 */
function csv_to_array($filename, $delimiter = ',')
{
    if(!file_exists($filename) || !is_readable($filename))
        return FALSE;

    $header = NULL;
    $data = array();
    if (($handle = fopen($filename, 'r')) !== FALSE)
    {
        while (($row = fgetcsv($handle, 1000, $delimiter)) !== FALSE)
        {
            if(!$header)
                $header = $row;
            else
                $data[] = array_combine($header, $row);
        }
        fclose($handle);
    }
    return $data;
}

'use strict';

/**
 * Filters
 *
 * @author Su Wang <sxw323@psu.edu>
 */

var DisplayFilters = angular.module('DisplayFilters', []);

DisplayFilters.filter('textLimiter', function() {
    return function(input) {
        if (input.length > 30) {
            input.substring(0, 27) + '...';
        } else {
            return input;
        }
    };
});

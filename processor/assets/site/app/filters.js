'use strict';

/**
 * Filters
 *
 * @author Su Wang <sxw323@psu.edu>
 */

var DisplayFilters = angular.module('DisplayFilters', []);

DisplayFilters.filter('textLimiter', function() {
    return function(input) {
        if (input.length > 25) {
            return input.substring(0, 23) + '...';
        } else {
            return input;
        }
    };
});

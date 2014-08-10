/**
* Controllers for the front page
*
* @author Su Wang <sxw323@psu.edu>
*/

'use strict';

var FrontPageControllers = angular.module('FrontPageControllers', [
'SentimentServices',
'DisplayFilters'
]);

FrontPageControllers.controller('RealTimeAnalysisController',[
'$scope',
'$rootScope',
'$parse',
'$location',
'$routeParams',
'SentimentRequest',
function(
  $scope,
  $rootScope,
  $parse,
  $location,
  $routeParams,
  SentimentRequest
) {

  /**
  * Call the SentimentRequest service to grab past API requests
  */
  var updatePastRequests = function() {
    $scope.pastRequests = SentimentRequest.query();
  }

  /*
  * Update the table with past requests before doing other stuff
  */
  updatePastRequests();

  /*
  * Trigger an article processing request
  */
  $scope.processArticle = function() {

    // Freshen up the page to be ready for new request
    $scope.sendingRequest = true;
    $scope.requestSuccess = false;
    $scope.responseType = null;
    $scope.responseScore = null;

    // Set up the object to be ready for the request
    var articleRequested = new SentimentRequest({
      apikey: $rootScope.alchemyApiKey,
      flavor: "url",
      url: $scope.requestUrl,
      target: "to",
      jsonp: null
    });

    // Send off the request and handle the response data
    articleRequested.$save(
      function(response) {
        if (response.sentiment.results.type == 'negative' || response.sentiment.results.type == 'positive') {
          $scope.sendingRequest = false;
          $scope.requestSuccess = true;
          $scope.requestUrl = '';
          $scope.responseData = response.sentiment.results;
          $scope.responseType = response.sentiment.results.type;
          $scope.responseScore = response.sentiment.results.score;
        } else {
          $scope.apiError = true;
        }
      }
    );
  }
}
]);

/**
* Controller for /dashboard
*
*/
FrontPageControllers.controller('DashboardController',[
'$scope',
'$parse',
'$location',
'$routeParams',
function(
  $scope,
  $parse,
  $location,
  $routeParams
) {

  // Get symbol from the route. Default to JPM
  var symbol = ('symbol' in $routeParams) ? $routeParams.symbol : 'jpm';
  $scope.symbol = symbol;
  $scope.forecastedChange;
  $scope.lastUpdatedDate = new Date();

  // Symbol : name
  $scope.allSymbols = [
      {"symbol":"jpm","name":"JPM - JPMorgan Chase"},
      {"symbol":"ms","name":"MS - Morgan Stanley"},
      {"symbol":"c","name":"C - Citigroup"},
      {"symbol":"bcs","name":"BCS - Barclays"},
      {"symbol":"bac","name":"BAC - Bank of America"},
      {"symbol":"ubs","name":"UBS - UBS"},
      {"symbol":"aapl","name":"AAPL - Apple"},
      {"symbol":"db","name":"DB - Deutsche Bank"},
      {"symbol":"fb","name":"FB - Facebook"},
      {"symbol":"goog","name":"GOOG - Google"},
      {"symbol":"gm","name":"GM - General Motors"},
      {"symbol":"gs","name":"GS - Goldman Sachs"},
      {"symbol":"hsbc","name":"HSBC - HSBC"}
    ];

  /**
  * Display data
  */
  var dataSeriesOptions = [],
  yAxisOptions = [],
  seriesCounter = 0,
  names = ['Price', 'Forecast', "F1", "F2", "F3"],
  colors = Highcharts.getOptions().colors;
  $scope.isLoading = true;

  var dayForecast = 0;
  var dayPrice = 0;

  $.each(names, function(i, name) {
    $.getJSON('/site/php-data/db_live.php?symbol=' + symbol.toUpperCase() + '&column='+ name.toLowerCase(), function(data) {

      if (name != 'F3') {
        dataSeriesOptions[i] = {
          name: name,
          data: data,
          yAxis: 0
        };
      } else {
        // F3 data should be display in separate yAxis
        dataSeriesOptions[i] = {
          name: 'F3',
          data: data,
          yAxis: 1
        };
      }

      // Get the latest day's price vs forecast
      if (name == 'Price') {
        dayPrice = data[data.length-1][1];
      }

      if (name == 'Forecast') {
        dayForecast = data[0][1];
      }


      // As we're loading the data asynchronously, we don't know what order it will arrive. So
      // we keep a counter and create the chart when all the data is loaded.
      seriesCounter++;

      if (seriesCounter == names.length) {
        createDataChart();
      }

    }).error( function(jqXHR, textStatus, errorThrown) {
      // Uh oh, errors. Print out stuff for now.
      console.log("error " + textStatus);
      console.log("incoming Text " + jqXHR.responseText);
    });
  });

  // Have the chart generation in separate call so we can wait for data to load
  var createDataChart = function() {

    $scope.forecastedChange = (dayForecast - dayPrice).toFixed(2);
    $scope.isLoading = false;
    $scope.$apply();

    angular.element('#data').highcharts('StockChart', {
      rangeSelector: {
        inputEnabled: $('#container').width() > 480,
        selected: 4
      },

      yAxis: [
      {
        // yAxis settings for the comparison data
        title: {
          text: 'Actual vs Forecast'
        },
        labels: {
          formatter: function() {
            return (this.value > 0 ? '+' : '') + this.value + '%';
          }
        },
        height: 300,
        lineWidth: 2,
        plotLines: [{
          value: 0,
          width: 2,
          color: 'silver'
        }]
      },
      {
        // yAxis settings for the correlation data
        title: {
          text: 'F3'
        },
        top: 350,
        height: 100,
        offset: -30,
        lineWidth: 0
      }
      ],

      ploOptions: {
        series: {
          compare: 'percent'
        }
      },

      tooltip: {
        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
        valueDecimals: 2
      },

      series: dataSeriesOptions
    });
  }
}
]);

FrontPageControllers.controller('SourceNYTController', [
'$scope',
'$parse',
'$rootScope',
'$http',
'$location',
'$routeParams',
'SentimentRequest',
function(
  $scope,
  $parse,
  $rootScope,
  $http,
  $location,
  $routeParams,
  SentimentRequest
) {

  $scope.processArticle = function(requestUrl, index) {

    // Set up the object to be ready for the request
    var articleRequested = new SentimentRequest({
      apikey: $rootScope.alchemyApiKey,
      flavor: "url",
      url: requestUrl,
      target: "to",
      jsonp: null
    });

    // Send off the request and handle the response data
    articleRequested.$save(
      function(response) {
        if (response.sentiment.results..type == 'negative' || response.sentiment.results..type == 'positive') {
          $scope.data.response.docs[index].sentiment = response.sentiment.results..type;
          $scope.data.response.docs[index].score = response.sentiment.results..score;
          angular.element('#collapse-'+index).collapse('show');
        } else {
          $scope.dataGrabError = true;
        }
      }
    );
  }


  /**
  * Get NYT data from API
  *
  * @param int Page we want to load
  */
  $scope.getData = function(page) {
    $scope.isLoading = true;

    // Set up API URI
    var apiString = 'http://api.nytimes.com/svc/search/v2/articlesearch.json?fq=news_desk:("Business")&limit=100&api-key=a931fc7951cee1141ff8bdf3f37a49db:16:69480846&fl=web_url,snippet,headline'
    apiString += '&page=' + page;

    // Do it!
    var dataPromise = $http.get(
      apiString
    );

    dataPromise.success(function(data, status, headers, config) {
      $scope.data = data;
      $scope.isLoading = false;
    });

    dataPromise.error(function(data, status, headers, config) {
      $scope.dataGrabError = true;
    });

    $scope.currentPage = page;
  }

  // Initial data load of page 1
  $scope.getData(0);
}
]);



FrontPageControllers.controller('RSSDataFeedController', [
'$scope',
'$parse',
'$rootScope',
'$http',
'$location',
'$routeParams',
'FeedService',
'SentimentRequest',
function(
  $scope,
  $parse,
  $rootScope,
  $http,
  $location,
  $routeParams,
  FeedService,
  SentimentRequest
) {

  $scope.url = 'http://finance.yahoo.com/rss/industry?s=msft';

  $scope.processFeed = function() {
    FeedService.parseFeed($scope.url).then(function(response) {
      $scope.feeds = response.data.responseData.feed.entries;
    });
  }

  $scope.processArticle = function(requestUrl, index) {


    // Set up the object to be ready for the request
    var articleRequested = new SentimentRequest({
      apikey: $rootScope.alchemyApiKey,
      flavor: "url",
      url: requestUrl,
      target: "to",
      jsonp: null
    });

    // Send off the request and handle the response data
    articleRequested.$save(
      function(response) {

        if (response.sentiment.results..type == 'negative' || response.sentiment.results..type == 'positive') {
          $scope.feeds[index].sentiment = response.sentiment.results..type;
          $scope.feeds[index].score = response.sentiment.results..score;
          angular.element('#collapse-'+index).collapse('show');
        } else {
          $scope.dataGrabError = true;
        }
      }
    );
  }
}
]);

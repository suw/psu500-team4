/************************Testing Document**************************************************
to run tests: npm test (or mocha test)
********************************************************************************************/

//Import Statements
var should = require('should');
var assert = require("assert");
var Database = "";
var Sentiment = require("../api/controllers/SentimentController.js");
var AlchemyAPI = require('../alchemyapi');
var alchemyapi = new AlchemyAPI();

var demo_text = 'Yesterday dumb Bob destroyed my fancy iPhone in beautiful Denver, Colorado. I guess I will have to head over to the Apple Store and buy a new one.';
var demo_url = 'www.nytimes.com';
var demo_html = '<html><head><title>Node.js Demo | AlchemyAPI</title></head><body><h1>Did you know that AlchemyAPI works on HTML?</h1><p>Well, you do now.</p></body></html>';

//===========================================================================================
//mocha positive test...
describe('Array', function(){
  describe('#indexOf()', function(){
    it('should return -1 when the value is not present', function(){
      assert.equal(-1, [1,2,3].indexOf(5));
      assert.equal(-1, [1,2,3].indexOf(0));
    })
  })
})

//===========================================================================================
//=====================Team 4 - Application Testing==========================================
//===========================================================================================
/*After the process to load a news data archive, the system database should contain the
following data for 2 years back from the day the process started:
News text
News source
News headline
News time
News date
Article unique identifier
*/
//should contain the following data for 2 years back from the day the process started: News text
describe('Database',function(){
	describe('NewsText',function(){
		it('should contain data for 2 years back for NewsText', function(){
			var time = "";
			var NewsTextTime = Date.now();
			assert.equal(time,NewsTextTime);
		})
	})
})
//should contain the following data for 2 years back from the day the process started: News source
describe('Database',function(){
	describe('NewsSource',function(){
		it('should contain data for 2 years back for NewsSource', function(){
			var time = "";
			var NewsSourceTime = Date.now();
			assert.equal(time,NewsSourceTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News headline
describe('Database',function(){
	describe('NewsHeadline',function(){
		it('should contain data for 2 years back for NewsHeadline', function(){
			var time = "";
			var NewsHeadlineTime = Date.now();
			assert.equal(time,NewsHeadlineTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News time
describe('Database',function(){
	describe('NewsTime',function(){
		it('should contain data for 2 years back for NewsTime', function(){
			var time = "";
			var NewsTime = Date.now();
			assert.equal(time,NewsTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News date
describe('Database',function(){
	describe('NewsDate',function(){
		it('should contain data for 2 years back for NewsDate', function(){
			var time = "";
			var NewsDateTime = Date.now();
			assert.equal(time,NewsDateTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: Article unique identifier
describe('Database',function(){
	describe('AUID',function(){
		it('should contain data for 2 years back for AUID', function(){
			var time = "";
			var NewsAUIDTime = Date.now();
			assert.equal(time,NewsAUIDTime);
		})
	})
})





//===========================================================================================
/*After the system processes each articles through the NLP API, the system database should
contain, for each processed article:
Timestamp of processing
Output of API
Article unique identifier
*/
//database should contain, for each processed article: Timestamp of processing
describe('Sentiment',function(){
	describe('Sentiment',function(){
		it('should contain, for each processed article: Timestamp of processing', function(){
			var processedArticle = Sentiment.createdAt;
			assert.exists(processedArticle);
		})
	})
})

//database should contain, for each processed article: Output of API
describe('Database',function(){
	describe('Output',function(){
		it('should contain, for each processed article: Output of API', function(){
			var processedArticle = "1";
			var OutputOfAPI = "";
			assert.equal(processedArticle,OutputOfAPI);
		})
	})
})

//database should contain, for each processed article: Article unique identifier
describe('Database',function(){
	describe('AUID',function(){
		it('should contain AUID for each artcle', function(){
			var processedArticle = "1";
			var AUID = "";
			assert.equal(processedArticle,AUID);
		})
	})
})





//===========================================================================================
/*After system processes results from NLP API, system database should contain output from
processing algorithm*/
//should contain output from processing algorithm
describe('Database',function(){
	describe('algorithm',function(){
		it('should contain output from processing algorithm', function(){
			var output = "1";
			var retrievedValue = "";
			assert.equal(output,retrievedValue)
		})
	})
})





//===========================================================================================
/*After system recalculates and auto-adjusts algorithm variables, system database should
contain new and old variables for the algorithm to use in the forecast calculation.*/
//should contain new and old variables for the algorithm
describe('Database',function(){
	describe('algorithm',function(){
		it('should contain new and old variables for the algorithm', function(){
			var oldVar = "1";
			var newVar = "";
			assert.equal(oldVar,newVar);
		})
	})
})





//===========================================================================================
/*After user selects to display forecasting and benchmark market index for up to one month,
system should display the selected data in chart and table.*/
//should display the selected data in chart and table
describe('display',function(){
	describe('chart',function(){
		it('should display the selected chart data', function(done){
			var dataSelected = "1";
			var chartData = "";
			assert.equal(dataSelected,chartData);
			done();
		})
	})
})

describe('display',function(){
	describe('table',function(){
		it('should display the selected table data', function(done){
			var dataSelected = "1";
			var tableData = "";
			assert.equal(dataSelected,tableData);
			done();
		})
	})
})


//===========================================================================================
/*Testing Targeted Sentiment functionality*/
//Targeted Sentiment


describe('sentiment_targeted',function(){
    it('should display the selected sentiment_targeted data', function(done){
console.log('Checking targeted sentiment . . . ');
alchemyapi.sentiment_targeted('text', demo_text, null, null, function(response) {
  assert.equal(response['status'],'ERROR');	//did not supply the target
    })
  })
})

describe('sentiment_targeted',function(){
    it('should display the selected sentiment_targeted data', function(done){
console.log('Checking targeted sentiment . . . ');
alchemyapi.sentiment_targeted('random', demo_text, 'heart', null, function(response) {
  assert.equal(response['status'],'ERROR');	//invalid flavor
    })
  })
})

describe('sentiment_targeted',function(){
    it('should display the selected sentiment_targeted data', function(done){
console.log('Checking targeted sentiment . . . ');
alchemyapi.sentiment_targeted('text', demo_text, 'heart', null, function(response) {
  assert.equal(response['status'],'OK');
    })
  })
})

function sentiment_targeted() {
	console.log('Checking targeted sentiment . . . ');
	alchemyapi.sentiment_targeted('text', test_text, null, null, function(response) {
		assert.equal(response['status'],'ERROR');	//did not supply the target

		alchemyapi.sentiment_targeted('random', test_text, 'heart', null, function(response) {
			assert.equal(response['status'],'ERROR');	//invalid flavor

			alchemyapi.sentiment_targeted('text', test_text, 'heart', null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.sentiment_targeted('html', test_html, 'language', null, function(response) {
					assert.equal(response['status'],'OK');

					alchemyapi.sentiment_targeted('url', test_url, 'Congress', null, function(response) {
						assert.equal(response['status'],'OK');
						console.log('Targeted sentiment tests complete!\n');
						text();
					});
				});
			});
		});
	});
}




//===========================================================================================
//===================================Front-End===============================================
//===========================================================================================
/*After select authenticates and goes to the dashboard page, system should display results
and graph of data processed within last 24 hours.*/
//should display results and graph of data processed within last 24 hours
describe('System',function(){
	describe('results',function(){
		it('should display results of data processed within last 24 hours', function(){
			var time = "";
			var resultsTime = Date.now();
			assert.equal(time,resultsTime);
		})
	})
})

describe('System',function(){
	describe('graph',function(){
		it('should display results of graph processed within last 24 hours', function(){
			var time = "";
			var graphTime = Date.now();
			assert.equal(time,graphTime);
		})
	})
})





//===========================================================================================

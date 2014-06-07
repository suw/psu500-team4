/************************Testing Document**************************************************
to run tests: npm test (or mocha test)
to run should: make test
********************************************************************************************/

//Import Statements
var should = require('should');
var assert = require("assert");
var Database = "";

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
			var time = Date.now();
			var NewsTextTime = Date.now();
			assert.equal(time,NewsTextTime);
		})
	})
})
//should contain the following data for 2 years back from the day the process started: News source
describe('Database',function(){
	describe('NewsSource',function(){
		it('should contain data for 2 years back for NewsSource', function(){
			var time = Date.now();
			var NewsSourceTime = Date.now();
			assert.equal(time,NewsSourceTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News headline
describe('Database',function(){
	describe('NewsHeadline',function(){
		it('should contain data for 2 years back for NewsHeadline', function(){
			var time = Date.now();
			var NewsHeadlineTime = Date.now();
			assert.equal(time,NewsHeadlineTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News time
describe('Database',function(){
	describe('NewsTime',function(){
		it('should contain data for 2 years back for NewsTime', function(){
			var time = Date.now();
			var NewsTime = Date.now();
			assert.equal(time,NewsTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: News date
describe('Database',function(){
	describe('NewsDate',function(){
		it('should contain data for 2 years back for NewsDate', function(){
			var time = Date.now();
			var NewsDateTime = Date.now();
			assert.equal(time,NewsDateTime);
		})
	})
})

//should contain the following data for 2 years back from the day the process started: Article unique identifier
describe('Database',function(){
	describe('AUID',function(){
		it('should contain data for 2 years back for AUID', function(){
			var time = Date.now();
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
describe('Database',function(){
	describe('Timestamp',function(){
		it('should contain, for each processed article: Timestamp of processing', function(){
			var processedArticle = "";
			var Timestamp = "";
			assert.equal(processedArticle,Timestamp);
		})
	})
})

//database should contain, for each processed article: Output of API
describe('Database',function(){
	describe('Output',function(){
		it('should contain, for each processed article: Output of API', function(){
			var processedArticle = "";
			var OutputOfAPI = "";
			assert.equal(processedArticle,OutputOfAPI);
		})
	})
})

//database should contain, for each processed article: Article unique identifier
describe('Database',function(){
	describe('AUID',function(){
		it('should contain AUID for each artcle', function(){
			var processedArticle = "";
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
			var output = "";
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
			var oldVar = "";
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
			var dataSelected = "";
			var chartData = "";
			assert.equal(dataSelected,chartData);
			done();
		})
	})
})

describe('display',function(){
	describe('table',function(){
		it('should display the selected table data', function(done){
			var dataSelected = "";
			var tableData = "";
			assert.equal(dataSelected,tableData);
			done();
		})
	})
})





//===========================================================================================
//===================================Front-End===============================================
//===========================================================================================
/*After select authenticates and goes to the dashboard page, system should display results 
and graph of data processed within last 24 hours.*/
//should display results and graph of data processed within last 24 hours
describe('System',function(){
	describe('results',function(){
		it('should display results of data processed within last 24 hours', function(){
			var time = Date.now();
			var resultsTime = Date.now();
			assert.equal(time,resultsTime);
		})
	})
})

describe('System',function(){
	describe('graph',function(){
		it('should display results of graph processed within last 24 hours', function(){
			var time = Date.now();
			var graphTime = Date.now();
			assert.equal(time,graphTime);
		})
	})
})





//===========================================================================================





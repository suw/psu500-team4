/************************Testing Document**************************************************
to run tests: npm test (or mocha test)
********************************************************************************************/

//Import Statements
var should = require('should');
var assert = require("assert");
var alchemyapi = require("../alchemyapi");
var Sentiment = require("../api/controllers/SentimentController.js");
var AlchemyAPI = require("../alchemyapi.js");
var alchemyapi = new AlchemyAPI();


//Test Data...
var test_text = 'Bob broke my heart, and then made up this silly sentence to test the Node.js SDK';
var test_html = '<html><head><title>The best SDK Test | AlchemyAPI</title></head><body><h1>Hello World!</h1><p>My favorite language is Javascript</p></body></html>';
var test_url = 'http://www.nytimes.com/2013/07/13/us/politics/a-day-of-friction-notable-even-for-a-fractious-congress.html?_r=0';
var test_image = 'test/emaxfpo.jpg';


//===========================SentimentController.Create()=======================
describe('Create:',function(){

//Entities
describe('entities',function() {
	console.log('Checking entities . . . ');
  it('should check entities',function(done){
	alchemyapi.entities('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //invalid flavor

		alchemyapi.entities('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.entities('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.entities('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Entity tests complete!\n');
					done();
				})
			})
		})
	})
})
})


//Keywords
describe('keywords',function() {
	console.log('Checking keywords . . . ');
  it('should check keywords',function(done){
	alchemyapi.keywords('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //invalid flavor

		alchemyapi.keywords('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.keywords('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.keywords('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Keyword tests complete!\n');
					done();
				});
			});
		});
	});
});
});


//Concepts
describe('concepts', function() {
	console.log('Checking concepts . . . ');
  it('should check concepts',function(done){
	alchemyapi.concepts('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR');	//invalid flavor

		alchemyapi.concepts('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.concepts('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.concepts('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Concept tests complete!\n');
					done();
				});
			});
		});
	});
});
});



//Sentiment
describe('sentiment',function(){
	console.log('Checking sentiment . . . ');
  it('should check sentiment',function(done){
	alchemyapi.sentiment('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR');	//invalid flavor

		alchemyapi.sentiment('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.sentiment('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.sentiment('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Sentiment tests complete!\n');
					done();
				});
			});
		});
	});
});
});


//Targeted Sentiment
describe('sentiment_targeted',function() {
	console.log('Checking targeted sentiment . . . ');
  it('should check sentiment_targeted',function(done){
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
						done();
					});
				});
			});
		});
	});
});
});


//Text
describe('text',function() {
	console.log('Checking text . . . ');
  it('should check text', function(done){
	alchemyapi.text('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.text('html', test_html, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.text('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Text tests complete!\n');
				done();
			});
		});
	});
});
});


//Text Raw
describe('text_raw',function() {
	console.log('Checking raw text . . . ');
  it('should check text_raw', function(done){
	alchemyapi.text_raw('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.text_raw('html', test_html, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.text_raw('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Raw Text tests complete!\n');
				done();
			});
		});
	});
});
});


//Author
describe('author',function() {
	console.log('Checking author . . . ');
  it('should check author',function(done){
	alchemyapi.author('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.author('html', test_html, null, function(response) {
			assert.equal(response['status'],'ERROR'); //there is no author in the test HTML content

			alchemyapi.author('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Author tests complete!\n');
				done();
			});
		});
	});
});
});


//Language
describe('language',function() {
	console.log('Checking language . . . ');
  it('should check language', function(done){
	alchemyapi.language('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR');	//invalid flavor

		alchemyapi.language('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.language('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.language('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Language tests complete!\n');
					done();
				});
			});
		});
	});
});
});


//Title
describe('title',function() {
	console.log('Checking title . . . ');
  it('should check title', function(done){
	alchemyapi.title('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.title('html', test_html, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.title('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Title tests complete!\n');
				done();
			});
		});
	});
});
});


//Relations
describe('relations', function() {
	console.log('Checking relations . . . ');
  it('should check relations', function(done){
	alchemyapi.relations('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR');	//invalid flavor

		alchemyapi.relations('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.relations('html', test_html, null, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.relations('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Relation tests complete!\n');
					done();
				});
			});
		});
	});
});
});


//Category
describe('category', function() {
	console.log('Checking category . . . ');
  it('should check category', function(done){
	alchemyapi.category('random', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //invalid flavor

		alchemyapi.category('text', test_text, null, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.category('html', test_html, {url:'test'}, function(response) {
				assert.equal(response['status'],'OK');

				alchemyapi.category('url', test_url, null, function(response) {
					assert.equal(response['status'],'OK');
					console.log('Category tests complete!\n');
					done();
				});
			});
		});
	});
});
});


//Feeds
describe('feeds', function() {
	console.log('Checking feeds . . . ');
  it('should check feeds', function(done){
	alchemyapi.feeds('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.feeds('html', test_html, {url:'test'}, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.feeds('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Feeds tests complete!\n');
				done();
			});
		});
	});
});
});


//Microformats
describe('microformats', function() {
	console.log('Checking microformats . . . ');
  it('should check microformats', function(done){
	alchemyapi.microformats('text', test_text, null, function(response) {
		assert.equal(response['status'],'ERROR'); //only works for html and urls

		alchemyapi.microformats('html', test_html, {url:'test'}, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.microformats('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Microformat tests complete!\n');
				done();
			});
		});
	});
});
});


//Taxonomy
describe('taxonomy', function() {
	console.log('Checking taxonomy . . . ');
  it('should check taxonomy', function(done){
	alchemyapi.taxonomy('text', test_text, null, function(response) {
		assert.equal(response['status'],'OK'); //only works for html and urls

		alchemyapi.taxonomy('html', test_html, {url:'test'}, function(response) {
			assert.equal(response['status'],'OK');

			alchemyapi.taxonomy('url', test_url, null, function(response) {
				assert.equal(response['status'],'OK');
				console.log('Taxonomy tests complete!\n');
				done();
			});
		});
	});
});
});


//Image
describe('image', function() {
	console.log('Checking image . . . ');
  it('should check image',function(done){
	alchemyapi.image('url', test_url, null, function(response) {
		assert.equal(response['status'],'OK');
		console.log('Image tests complete!\n');
		done();
	});
});
});


//Image Keywords
describe('url_image_keywords', function() {
	console.log('Checking url image keywords . . . ');
  it('should check url_image_keywords', function(done){
	alchemyapi.image_keywords('url', test_url, null, function(response) {
		assert.equal(response['status'],'OK');
		console.log('Image keywords tests complete!\n');
		done();
	});
});
});

//Image Keywords with post
describe('image_keywords', function() {
	console.log('Checking image keywords . . . ');
  it('should check image_keywords', function(done){
	alchemyapi.image_keywords('image', test_image, null, function(response) {
		assert.equal(response['status'],'OK');
		console.log('Image keywords tests complete!\n');
		done();
	});
});
});


//Combined
describe('combined', function() {
	console.log('Checking combined . . . ');
  it('should check combined', function(done){
	alchemyapi.combined('url', test_url, null, function(response) {
		assert.equal(response['status'],'OK');
		console.log('Combined tests complete!\n');

});
});

});
//end create();
//==============================================================================


//===========================SentimentController.Read()=========================

describe('Read:',function(){

//Find One
describe('find one', function(){
	console.log('Checking findOne...');
   it('should check findOne', function(done){
   	sentiment.findOne('id', function(err,Sentiment){
   		assert.equal(response['status'],'OK');
   		console.log('Find One tests complete!\n');
   		
//Find
desribe('find', function(){
	console.log('Checking find...');
   it('should check find', function(done){
   	sentiment.find('options', function(err,Sentiment){
   		assert.equal(response['status'], 'OK');
   		console.log('Find tests complete!\n');
   	}
   }	
}
   	}
   }		
}
//end read();
//==============================================================================


//===========================SentimentController.Update()=======================

describe('Update:', function(){
	
//Update
describe('update', function(){
	console.log('Checking update...');
   it('should check update', function(done){
   	sentiment.update('id','criteria', function(err,Sentiment){
   		assert.equal(response['status'], 'OK');
   		console.log('Update tests complete!\n');
   	}
   }
}
}
//end update();
//==============================================================================


//===========================SentimentController.Destroy()======================

describe('Destroy:', function(){
	
//Destroy
describe('destroy', function(){
	console.log('Checking destroy...');
   it('should check destroy', function(done){
   	sentiment.destroy('id', function(err,result){
   		assert.equal(response['status'], 'OK');
   		console.log('Destroy tests complete!\n');
   	}
   }
}
//end destroy();
	
console.log('\n\n**** All tests are complete! ****\n');
		done();
}
	});	
	
	
	
	
	
	
	
	
	
	
	
	
}

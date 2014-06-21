/**
 * SentimentController
 *
 * @module      :: Controller
 * @description	:: A set of functions called `actions`.
 *
 *                 Actions contain code telling Sails how to respond to a certain type of request.
 *                 (i.e. do stuff, then send some JSON, show an HTML page, or redirect to another URL)
 *
 *                 You can configure the blueprint URLs which trigger these actions (`config/controllers.js`)
 *                 and/or override them with custom routes (`config/routes.js`)
 *
 *                 NOTE: The code you write here supports both HTTP and Socket.io automatically.
 *
 * @docs        :: http://sailsjs.org/#!documentation/controllers
 */
//Create the AlchemyAPI object
var AlchemyAPI = require('../../alchemyapi');
var consolidate = require('consolidate');
var alchemyapi = new AlchemyAPI();
var demo_html= "www.google.com";



module.exports = {

example:function (req, res) {
	var output = {};

	//Start the analysis chain
	entities(req, res, output);
},


entities:function (req, res, output) {
  var params = req.params.all();
	alchemyapi.entities('text', params,{ 'sentiment':1 }, function(response) {
		output['entities'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['entities'] };
		keywords(req, res, output);
	});
},


keywords:function (req, res, output) {
  var params = req.params.all();
	alchemyapi.keywords('text', params, { 'sentiment':1 }, function(response) {
		output['keywords'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['keywords'] };
		concepts(req, res, output);
	});
},


concepts:function (req, res, output) {
  var params = req.params.all();
	alchemyapi.concepts('text', params, { 'showSourceText':1 }, function(response) {
		output['concepts'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['concepts'] };
		sentiment(req, res, output);
	});
},


text:function(req, res, output) {
  var params = req.params.all();
  alchemyapi.text('url', params, {}, function(response) {
    output['text'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
    author(req, res, output);
    $('#display').val(output);
  });

},


sentiment:function(req, res, output) {
  var params = req.params.all;
  alchemyapi.sentiment('html', params, {}, function(response) {
    output['sentiment'] = { html:req.params.html, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
    text(req, res, output);
    $('#display').val(output);
  });
},



read:function(){},
update:function(){},
delete:function(){},


    /**
     * Overrides for the settings in `config/controllers.js`
     * (specific to SentimentController)
     */

    _config: {}

}

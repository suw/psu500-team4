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
var alchemyapi = new AlchemyAPI();
var express = require('express');
var app = express();
var request = require('request');


module.exports = {



//========================================================================
//========================================================================
create:function(req,res,output){

/*
//sentiment by url without target...
var params = req.params.all();
var url = params.url;
console.log(params);
alchemyapi.sentiment('url',url,{}, function(response){
  output['sentiment']= {url:url, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
  console.log(output);
  res.json(output);
  });
*/

var params = req.params.all();
var url = params.url;
var target = params.target;
var apikey = params.apikey;



console.log(params);

var demo_text = 'Yesterday dumb Bob destroyed my fancy iPhone in beautiful Denver, Colorado. I guess I will have to head over to the Apple Store and buy a new one.';
var demo_url = url;
var demo_html = '<html><head><title>Node.js Demo | AlchemyAPI</title></head><body><h1>Did you know that AlchemyAPI works on HTML?</h1><p>Well, you do now.</p></body></html>';

var output = {};

//Start the analysis chain
//alchemyapi.entities(req, res, output);

alchemyapi.entities('text', demo_text,{ 'sentiment':1 }, function(response) {
  output['entities'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['entities'] };
  //alchemyapi.keywords(req, res, output);
});

alchemyapi.keywords('text', demo_text, { 'sentiment':1 }, function(response) {
  output['keywords'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['keywords'] };
  //alchemyapi.sentiment(req, res, output);
});

alchemyapi.sentiment_targeted('url',url,target,{}, function(response){
  output['sentiment_targeted']= {target:target, url:url, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
});
/*
alchemyapi.concepts('text', text, { 'showSourceText':1 }, function(response) {
  output['concepts'] = { text:text, response:JSON.stringify(response,null,4), results:response['concepts'] };
  alchemyapi.image_keywords(req, res, output);
});
*/
alchemyapi.sentiment('html', demo_html, {}, function(response) {
	output['sentiment'] = { html:demo_html, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
});

alchemyapi.text('url', demo_url, {}, function(response) {
	output['text'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.author('url', demo_url, {}, function(response) {
	output['author'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.language('text', demo_text, {}, function(response) {
	output['language'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.title('url', demo_url, {}, function(response) {
  output['title'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.relations('text', demo_text, {}, function(response) {
	output['relations'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response['relations'] };
});

alchemyapi.category('text', demo_text, {}, function(response) {
	output['category'] = { text:demo_text, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.feeds('url', demo_url, {}, function(response) {
	output['feeds'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response['feeds'] };
});

alchemyapi.microformats('url', demo_url, {}, function(response) {
	output['microformats'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response['microformats'] };
});

alchemyapi.taxonomy('url', demo_url, {}, function(response) {
	output['taxonomy'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.combined('url', demo_url, {}, function(response) {
	output['combined'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.image('url', demo_url, {}, function(response) {
	output['image'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };
});

alchemyapi.image_keywords('url', demo_url, {}, function(response) {
  output['image_keywords'] = { url:demo_url, response:JSON.stringify(response,null,4), results:response };

console.log("***************************************************");
  console.log("first"+ output);

  //send the response in json... one for Su...
  //res.json(output.sentiment_targeted.results);

  //and all for maxim..
  //res.json(output);

  //send to db...
  Sentiment.create(output, function(err,Sentiment){
    if (err) return next(err);
    res.status(201);
    res.json(Sentiment);
    console.log("***************************************************");
    console.log(Sentiment);
  });
/*
Sentiment.create(output.text, function(err,Sentiment){
  if (err) return next(err);
  res.status(201);
  res.json(Sentiment);
  console.log("sentiment2"+Sentiment.toString());
});

*/
});
},
//========================================================================
//========================================================================
read:function(req,res,next){

    var params = req.params.all();
    var id = params.id;

    var idShortCut = isShortCut(id);

    if (idShortCut === true){
      return next;
    }

    if (id){
      Sentiment.findOne(id, function(err,Sentiment){

        if(Sentiment == undefined) return res.notFound();

        if(err) return next(err);

        res.json(Sentiment);
      });
    }else{

      var where = params.where;

      if (_.isString(where)){
        where = JSON.parse(where);
      }

      var options = {
        limit: req.param('limit') || undefined,
        skip: req.param('skip') || undefined,
        sort: req.param('sort') || undefined,
        where: where || undefined
      };

      console.log("Options:", options);

      Sentiment.find(options, function(err,Sentiment){
        if(Sentiment == undefined) return res.notFound();

        if(err) return next(err);

        res.json(Sentiment);
      });

    }

},

//========================================================================
//========================================================================
update:function(req,res,next){
  var criteria = {};
  var criteria = _.merge({},req.params.all(), req.body);

  var id = req.param('id');
  if(!id){
    return res.badRequest('No id Provided');
  }

  Sentiment.update(id, criteria, function(err, Sentiment){

    if (Sentiment.lenght == 0) return res.notFound();

    if (err) return next(err);

    res.json(Sentiment);
  });

},




//========================================================================
//========================================================================
destroy:function(req,res){

//get id..
var params = req.params.all();
var id = params.id;

if(!id){
  return res.badRequest('No id Provided');
}


Sentiment.findOne(id).done(function(err,result){
  if (err) return res.ServerError(err);

  if (!result) return res.notFound();

});

//call the destroy method..
Sentiment.destroy(id,function(err,result){

  if (err) return next(err);

  return res.json(result);
});

},


//========================================================================
//========================================================================

    /**
     * Overrides for the settings in `config/controllers.js`
     * (specific to SentimentController)
     */

    _config: {}

}

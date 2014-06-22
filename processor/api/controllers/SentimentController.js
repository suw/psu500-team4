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

//targeted sentiment...
alchemyapi.sentiment_targeted('url',url,target,{}, function(response){
  output['sentiment_targeted']= {target:target, url:url, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
  console.log(output);
  res.json(output.sentiment_targeted.results);
  Sentiment.create(params, function(err,Sentiment){
    if (err) return next(err);
    //res.status(201);
    res.json(Sentiment);
  });

  });

},

//========================================================================
//========================================================================
read:function(){



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

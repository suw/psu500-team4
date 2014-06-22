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


//Create the AlchemyAPI object
var AlchemyAPI = require('../../alchemyapi');
var alchemyapi = new AlchemyAPI();
var express = require('express');
var app = express();
var request = require('request');





module.exports = {




create:function(req,res,output){

/*
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
alchemyapi.sentiment_targeted('url',url,target,{}, function(response){
  output['sentiment_targeted']= {target:target, url:url, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
  console.log(output);
  res.json(output.sentiment_targeted.results);
  });


},
read:function(){

  var params = req.params.all();
  var url = params.url;
  console.log(params);
  alchemyapi.sentiment('url',url,{}, function(response){
    output['sentiment']= {url:url, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
    console.log(output);

  });
},
update:function(){},
destroy:function(){},


    /**
     * Overrides for the settings in `config/controllers.js`
     * (specific to SentimentController)
     */

    _config: {}

}

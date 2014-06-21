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

var app = ('http');
//Create the AlchemyAPI object
var AlchemyAPI = require('../../alchemyapi');
var alchemyapi = new AlchemyAPI();



function start(req, res) {
	var output = {};

	//Start the analysis chain
	create(req, res, create);
}

module.exports = {




create:function(req,res,output){

  var myurl = req.params.all();
  alchemyapi.sentiment('url',myurl,{}, function(response){
    output['sentiment']= {url:myurl, response:JSON.stringify(response,null,4), results:response['docSentiment'] };
    	res.render('sentiment',output);
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

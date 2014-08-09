/**
 * Global adapter config
 *
 * The `adapters` configuration object lets you create different global "saved settings"
 * that you can mix and match in your models.  The `default` option indicates which
 * "saved setting" should be used if a model doesn't have an adapter specified.
 *
 * Keep in mind that options you define directly in your model definitions
 * will override these settings.
 *
 * For more information on adapter configuration, check out:
 * http://sailsjs.org/#documentation
 */

module.exports.adapters = {

'default':'mysql',

    mysql: {
        module  : 'sails-mysql',
        host     : 'host320.hostmonster.com',
        port     :  3306,
        user     : 'fivninni_apikids',
        password : 'makeitw0rk',
        database : 'fivninni_sweng500',
        pool: false,



    // OR (explicit sets take precedence)
    //module   : 'sails-mysql',
    //url      : 'mysql2://USER:PASSWORD@HOST:PORT/DATABASENAME'
  },

    mongo: {
        module  : 'sails-mongo',
        host     : 'localhost',
        port     : 27017,
        user     : '',
        password : '',
        database : 'local'
    }

};

/**
 * Main
 *
 * @module      :: Model
 * @description :: A short summary of how this model works and what it represents.
 * @docs		:: http://sailsjs.org/#!documentation/models
 */

module.exports = {


adapter  : 'mysql',
connection: 'mysql',
tableName:'data_gui_graph',
autoCreatedAt: false,
autoUpdatedAt: false,

    attributes: {
          timestamp:{type:'datetime',columnName:'timestamp'},
          date:{type:'datetime',columnName:'date',default:'CURRENT_TIMESTAMP'},
          symbol:{type:'text',columnName:'symbol'},
          price:{type:'float',columnName:'price'},
          forecast:{type:'float',columnName:'forecast'},
          F1:{type:'float',columnName:'F1'},
          F2:{type:'float',columnName:'F2'},
          F3:{type:'float',columnName:'F3'},
          correlation:{type:'datetime',columnName:'correlation'},
          pctAcc:{type:'float',columnName:'pctAcc'},
          acc:{type:'integer',columnName:'acc',primaryKey: true}

    }
};

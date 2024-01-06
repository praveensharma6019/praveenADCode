var path = require('path');
var webpack = require('webpack');

module.exports = {
    watch:true,
    mode:'production', //development | production
    entry:{
        'scripts/AdaniConneX/helper':'scripts/AdaniConneX/helper/index.js',
        'scripts/AdaniConneX/pages/home':'scripts/AdaniConneX/pages/home/index.js',
        'scripts/AdaniConneX/pages/dc-details':'scripts/AdaniConneX/pages/dc-details/index.js',
        'scripts/AdaniConneX/pages/colocation':'scripts/AdaniConneX/pages/colocation/index.js'
    },
    output: {
        path:path.resolve(__dirname, "scripts")
    },
    //devtool: 'source-map'
 };
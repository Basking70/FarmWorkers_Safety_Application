var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var CopyWebpackPlugin = require('copy-webpack-plugin');
module.exports = {
    entry: "./entry.js",
    output: {
        filename: 'js/bundle.js',
        path: 'build/',
        publicPath: '/',
        sourcePrefix: ''
    },

    module: {
        loaders: [
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract("style-loader", "css-loader")
            },
            // Optionally extract less files
            // or any other compile-to-css language
            {
                test: /\.less$/,
                loader: ExtractTextPlugin.extract("style-loader", "css-loader!less-loader")
            },

            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "file-loader?name=./assets/[hash].[ext]"
            }, {
                test: /\.(ttf|eot|svg|png|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "file-loader?name=./assets/[hash].[ext]"               
            }
            // You could also use other loaders the same way. I. e. the autoprefixer-loader
        ]

    },
    plugins: [
        new webpack.ProvidePlugin({
           $: "jquery",
           jQuery: "jquery",
           toastr: "toastr"
       }),
        new ExtractTextPlugin("css/bundle.css"),
        new CopyWebpackPlugin([
            { from: 'static' }
        ])
    ]
};

var DEBUG = process.env.NODE_ENV === 'development';

const path = require('path');

const webpack = require('webpack');

const { TsConfigPathsPlugin } = require('awesome-typescript-loader');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const WebpackCleanupPlugin = require('webpack-cleanup-plugin');

const extractSass = new ExtractTextPlugin({
    filename: "freecore.styles.css",
    disable: DEBUG
});

let config = {
    entry: [
        './ClientSrc/index.ts'
    ],
    output: {
        path: __dirname + "/wwwroot/",
        publicPath: '/',
        filename: `freecore.bundle${DEBUG ? '' : '.[chunkhash:8]'}.js`,
        chunkFilename: "[chunkhash].js",
    },
    resolve: {
        extensions: ['.webpack.js', '.web.js', '.ts', '.tsx', '.js'],
        alias: {
        },
        plugins: [
            new TsConfigPathsPlugin()
        ]
    },
    module: {
        rules: [
            { test: /\.tsx?$/, loader: 'awesome-typescript-loader' },
            {
                test: /\.scss$/,
                use: extractSass.extract({ use: ["css-loader", "resolve-url-loader", "sass-loader"], fallback: 'style-loader' })
            }, {
                test: /\.woff2?$|\.ttf$|\.eot$|\.svg$/,
                //include: ["node_modules"],
                use: [{
                    loader: "file-loader"
                }]
            }
        ]
    },
    plugins: [
        extractSass,
        new HtmlWebpackPlugin({
            template: './ClientSrc/index.html',
            filename: 'index.html'
        }),
    ],
    recordsOutputPath: path.join(__dirname, "wwwroot", "records.json"),

    // put sourcemaps inline
    devtool: 'source-map',

    devServer: {
        port: 3000,
        historyApiFallback: true,
        inline: true,
        //contentBase: "./dist/"
    }
};

if (!DEBUG) {
    /*
    config.plugins.push(
        new webpack.optimize.AggressiveSplittingPlugin({
			minSize: 30000,
			maxSize: 50000
		}));
    */
    config.plugins.push(new WebpackCleanupPlugin());
    // config.plugins.push(new webpack.optimize.UglifyJsPlugin({
    //         compress: { warnings: false },
    //         beautify: true,
    //         extractComments: true
    //     }));
}

module.exports = config;
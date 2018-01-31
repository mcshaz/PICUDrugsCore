"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var UglifyJsPlugin = require("uglifyjs-webpack-plugin");
var CommonsChunkPlugin = require("webpack/lib/optimize/CommonsChunkPlugin");
var path = require("path");
var isDevelopment = true, srcPath = path.join(__dirname, '/Scripts'), distPath = path.join(__dirname, '/wwwroot/js');
var excludeTinyMCEResources = function (input) {
    return input.replace(/\\/g, "/").indexOf("/tinymce/skins/") !== -1;
};
var modules = {
    commonAll: {
        'polyfills': [
            'core-js',
        ],
        'bootstrap': [
            'bootstrap/dist/js/bootstrap',
            'bootstrap/dist/css/bootstrap.css',
        ],
        'smartmenus': [
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4',
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4.css',
        ]
    },
    commonAnon: {
        'patientDataEntry': [
            './PageScripts/PatientData/DrugLists'
        ]
    }
};
var config = {
    cache: true,
    devtool: 'source-map',
    context: srcPath,
    entry: Object.keys(modules)
        .reduce(function (rv, x) {
        var ps = modules[x];
        for (var p in ps) {
            rv[p] = ps[p];
        }
        return rv;
    }, {}),
    output: {
        publicPath: "/js/",
        path: distPath,
        filename: '[name].bundle.js',
        chunkFilename: '[id].bundle.js'
    },
    resolve: {
        modules: ["node_modules"],
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: [
                    {
                        loader: 'awesome-typescript-loader',
                        options: {
                            configFile: path.join(__dirname, '/tsconfig.webpack.json'),
                            silent: true,
                        }
                    }
                ]
            },
            {
                test: /\.css?$/,
                exclude: excludeTinyMCEResources,
                use: ExtractTextPlugin.extract({
                    loader: 'css-loader',
                    options: {
                        minimize: !isDevelopment
                    },
                })
            },
            {
                test: /\.(png|jpg|eot|ttf|svg|woff|woff2|gif)$/,
                exclude: excludeTinyMCEResources,
                use: [
                    {
                        loader: "file-loader",
                        options: {
                            outputPath: 'assets/',
                            name: '[name].[ext]',
                        }
                    }
                ]
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin('css/[name].' + (isDevelopment ? 'dev' : 'min') + '.css')
    ].concat(Object.keys(modules).map(function (x) {
        return new CommonsChunkPlugin({
            name: x,
            chunks: Object.keys(modules[x]),
            minChunks: 2
        });
    }), (isDevelopment ? [] : [
        new UglifyJsPlugin({
            sourceMap: true,
            uglifyOptions: {
                output: {
                    ascii_only: true,
                }
            }
        }),
    ]))
};
exports.default = config;
//# sourceMappingURL=webpack.config.js.map
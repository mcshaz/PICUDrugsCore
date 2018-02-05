"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ExtractTextPlugin = require("extract-text-webpack-plugin");
var UglifyJsPlugin = require("uglifyjs-webpack-plugin");
var HardSourceWebpackPlugin = require("hard-source-webpack-plugin");
var path = require("path");
var isDevelopment = true, srcPath = path.join(__dirname, '/Scripts'), distPath = path.join(__dirname, '/wwwroot/js');
var excludeTinyMCEResources = function (input) {
    return input.replace(/\\/g, "/").indexOf("/tinymce/skins/") !== -1;
};
var config = {
    cache: true,
    devtool: 'source-map',
    context: srcPath,
    entry: {
        'common': [
            './Common',
            'bootstrap/dist/css/bootstrap.css',
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4.css',
        ],
        'patientDataEntry': [
            './PageScripts/PatientData/DrugListsEntry',
        ]
    },
    output: {
        publicPath: "/js/",
        path: distPath,
        filename: '[name].bundle.js',
        chunkFilename: '[id].bundle.js'
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.json', '.vue'],
        modules: ["node_modules"],
        alias: {
            'vue': 'vue/dist/vue.esm.js',
            'jquery.smartmenus': 'smartmenus'
        }
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
                            silent: false
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
                test: /\.vue$/,
                loader: 'vue-loader'
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
        new HardSourceWebpackPlugin(),
        new ExtractTextPlugin('css/[name].' + (isDevelopment ? 'dev' : 'min') + '.css')
    ].concat((isDevelopment ? [] : [
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
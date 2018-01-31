import * as webpack from 'webpack';
import * as ExtractTextPlugin from 'extract-text-webpack-plugin';
//import * as HtmlWebpackPlugin from 'html-webpack-plugin';
import * as UglifyJsPlugin from 'uglifyjs-webpack-plugin';
import * as CommonsChunkPlugin from 'webpack/lib/optimize/CommonsChunkPlugin' 
//import * as fs from 'fs';
//import * as glob from 'glob'; 
import * as path from 'path'

//const srcPath = path.join(__dirname, '/Scripts'),
//    distPath = path.join(__dirname, '/wwwroot');

const isDevelopment = true,
    srcPath = path.join(__dirname, '/Scripts'),
    distPath = path.join(__dirname, '/wwwroot/js');

const excludeTinyMCEResources = (input: string) => {
    // tinymce-resources.ts specifically requests the loading of skins and other TinyMCE assets
    // with the file loader, which causes them to be copied out to wwwroot/assets/. However, if
    // we don't exclude these files from the default CSS / resource rules, they'll then be
    // processed a second time, which will replace the content of the files emitted to wwwroot/assets/
    // with references to the extracted CSS or paths to the then again copied assets, making the
    // contents of the TinyMCE resources either invalid or empty.
    return input.replace(/\\/g, "/").indexOf("/tinymce/skins/") !== -1;
}

var modules = {
    commonAll:{        
        'polyfills': [
            'core-js',
        ],
        'bootstrap': [
            'bootstrap/dist/js/bootstrap',
            'bootstrap/dist/css/bootstrap.css',
            //'./js/ie10-viewport-bug-workaround',
        ],
        'smartmenus':[
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4',
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4.css',
        ]
    },
    commonAnon: {
        'patientDataEntry':[
            './PageScripts/PatientData/DrugLists'
        ]
    }
};
/*
const tinymce ={ 
    'tinymce': [
        'tinymce/tinymce',
        'tinymce/jquery.tinymce',
        'tinymce/themes/modern',
        'tinymce/plugins/link',
        'tinymce/plugins/lists',
    ],
    'tinymce-res': [
        // We don't actually want to include the CSS and image resources in the TinyMCE
        // bundle, as these resources are only to be loaded inside the TinyMCE iframe,
        // which it creates. However, by having this bundle here we ensure the resource
        // files get copied out to the wwwroot/ directory correctly for deployment.
        './js/tinymce-resources',
    ]
};
*/
    //'font-awesome': [
    //    'font-awesome/css/font-awesome.css',
    //]


const config: webpack.Configuration = {
    cache: true,
    devtool: 'source-map',
    context: srcPath,
    entry: Object.keys(modules)
        .reduce(function(rv,x){
            let ps = modules[x];
            for(let p in ps){
                rv[p] = ps[p];
            }
            return rv;
        },{}),
    output: {
        publicPath: "/js/",
        path: distPath,
        filename: '[name].bundle.js',
        chunkFilename: '[id].bundle.js'
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.json'], //default is .js & .json
        modules: ["node_modules"],
        alias: {
            vue: 'vue/dist/vue.esm.js'
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
                            configFile: path.join(__dirname, '/tsconfig.json'),
                            silent: false,
                        }
                    }
                ]
            },
            {
                test: /\.css?$/,
                exclude: excludeTinyMCEResources, // Prevent TinyMCE resources being processed twice (see above).
                use: ExtractTextPlugin.extract({
                    loader: 'css-loader',
                    options: { 
                        //outputPath: 'css/',
                        minimize: !isDevelopment 
                    },
                })
            },
            {
                test: /\.(png|jpg|eot|ttf|svg|woff|woff2|gif)$/,
                exclude: excludeTinyMCEResources, // Prevent TinyMCE resources being processed twice (see above).
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
        //new webpack.NoEmitOnErrorsPlugin(),
        ///It moves all the required *.css modules in entry chunks into a separate CSS file. So your styles are no longer inlined into the JS bundle, but in a separate CSS file (styles.css). If your total stylesheet volume is big, it will be faster because the CSS bundle is loaded in parallel to the JS bundle.
        new ExtractTextPlugin('css/[name].' + (isDevelopment ? 'dev' : 'min') + '.css'),
        ...Object.keys(modules).map(function(x){
            return new CommonsChunkPlugin({
                name: x,
                chunks: Object.keys(modules[x]),
                minChunks: 2
            });
        }),
        ...(isDevelopment ? [] : [
            new UglifyJsPlugin({
                sourceMap: true,
                // Needed for TinyMCE, see https://www.tinymce.com/docs/advanced/usage-with-module-loaders/#minificationwithuglifyjs2
                uglifyOptions: {
                    output: {
                        ascii_only: true,
                    }
                }
            }),
        ])
    ]
};

export default config;
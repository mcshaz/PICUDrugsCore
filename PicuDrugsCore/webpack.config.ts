import * as webpack from 'webpack';
import * as ExtractTextPlugin from 'extract-text-webpack-plugin';
//import * as HtmlWebpackPlugin from 'html-webpack-plugin';
import * as UglifyJsPlugin from 'uglifyjs-webpack-plugin';
//import * as CommonsChunkPlugin from 'webpack/lib/optimize/CommonsChunkPlugin';
//import * as HardSourceWebpackPlugin from 'hard-source-webpack-plugin';
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
    entry: {
        'common': [
            './Common',
            'bootstrap/dist/css/bootstrap.css',
            'smartmenus-bootstrap-4/jquery.smartmenus.bootstrap-4.css',
        ],
        'patientDataEntry': [
            './PageScripts/PatientData/DrugListsEntry.ts'
        ]
    },
    output: {
        publicPath: "/js/",
        path: distPath,
        filename: '[name].bundle.js',
        chunkFilename: '[id].bundle.js'
    },
    resolve: {
        extensions: ['.ts', '.js', '.vue', '.json'], //default is .js & .json
        modules: ["node_modules"],
        alias: {
            'vue$': 'vue/dist/vue.esm.js',
            'jquery.smartmenus': 'smartmenus'
        }
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
                /*
                options: {
                    loaders: {
                      // Since sass-loader (weirdly) has SCSS as its default parse mode, we map
                      // the "scss" and "sass" values for the lang attribute to the right configs here.
                      // other preprocessors should work out of the box, no loader config like this necessary.
                      'scss': 'vue-style-loader!css-loader!sass-loader',
                      'sass': 'vue-style-loader!css-loader!sass-loader?indentedSyntax',
                    }
                    // other vue-loader options go here
                  }
                  */
            },
            {
                test: /\.tsx?$/,
                loader:'ts-loader',
                exclude: /node_modules/,
                options: {
                    appendTsSuffixTo: [/\.vue$/],
                }
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
        //new HardSourceWebpackPlugin(),
        //new webpack.NoEmitOnErrorsPlugin(),
        ///It moves all the required *.css modules in entry chunks into a separate CSS file. So your styles are no longer inlined into the JS bundle, but in a separate CSS file (styles.css). If your total stylesheet volume is big, it will be faster because the CSS bundle is loaded in parallel to the JS bundle.
        new ExtractTextPlugin('css/[name].' + (isDevelopment ? 'dev' : 'min') + '.css'),
        /* ...Object.keys(modules).map(function(x){
            return new CommonsChunkPlugin({
                name: x,
                chunks: Object.keys(modules[x]),
                minChunks: 2
            });
        }), */
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
            new webpack.DefinePlugin({
                'process.env': {
                    NODE_ENV: '"production"'
                }
            })
        ])
    ]
};

export default config;
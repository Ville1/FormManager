const path = require("path");
const ROOT = path.resolve(__dirname);

module.exports = function (_env, argv) {
    return {
        context: ROOT,
        entry: {
            login: "./wwwroot/js/src/login.js",
            frontPage: "./wwwroot/js/src/frontPage.js",
            navigation: "./wwwroot/js/src/navigation.js",
            videoGameList: "./wwwroot/js/src/videoGameList.js",
            videoGameForm: "./wwwroot/js/src/videoGameForm.js",
            publisherForm: "./wwwroot/js/src/publisherForm.js",
            publisherList: "./wwwroot/js/src/publisherList.js",
            developerForm: "./wwwroot/js/src/developerForm.js",
            developerList: "./wwwroot/js/src/developerList.js"
        },
        output: {
            filename: '[name].js',
            path: path.resolve(__dirname, 'wwwroot/js/dist')
        },
        module: {
            rules: [
                {
                    test: /\.m?js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader",
                        options: {
                            presets: ['@babel/preset-react']
                        }
                    }
                }
            ]
        }
    };
};
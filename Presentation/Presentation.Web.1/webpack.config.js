module.exports = {
    entry: "./application.jsx",
    output: {
        path: __dirname,
        filename: "site.js"
    },
    module: {
        loaders: [
            { test: /\.css$/, loader: "style!css" }
        ]
    }
};
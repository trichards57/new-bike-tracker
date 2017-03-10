// Karma configuration
// Generated on Fri Jul 29 2016 11:13:52 GMT+0100 (GMT Summer Time)

module.exports = function (config) {
    config.set({
        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: '',

        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['jasmine'],

        // list of files / patterns to load in the browser
        files: [
          'lib/jquery/dist/jquery.js',
          'lib/angular/angular.js',
          'lib/angular-mocks/angular-mocks.js',
          'lib/angular-resource/angular-resource.js',
          'Scripts/ui/*.js',
          'Scripts/tests/*.js'
        ],

        // list of files to exclude
        exclude: [
        ],

        // preprocess matching files before serving them to the browser
        // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
            'Scripts/ui/*.js': ['coverage']
        },

        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: ['progress', 'html', 'junit', 'coverage'],

        htmlReporter: {
            outputFile: '../reports/karma-results/results.html'
        },

        junitReporter: {
            outputDir: '../reports/karma-results/junit', // results will be saved as $outputDir/$browserName.xml
            useBrowserName: true, // add browser name to report and classes names
        },

        coverageReporter: {
            dir: '../reports/karma-results/',
            reporters: [
                { type: 'cobertura', subdir: 'xml' }
            ]
        },

        // web server port
        port: 9876,

        // enable / disable colors in the output (reporters and logs)
        colors: true,

        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,

        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,

        // start these browsers
        // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
        browsers: ['Chrome'],

        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: false,

        // Concurrency level
        // how many browser should be started simultaneous
        concurrency: Infinity
    })
}
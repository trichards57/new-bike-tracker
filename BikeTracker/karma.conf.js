// Karma configuration
// Generated on Fri Jul 29 2016 11:13:52 GMT+0100 (GMT Summer Time)

module.exports = function (config) {
    config.set({
        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: '',

        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['jasmine', 'karma-typescript'],

        browsers: ['Chrome'],

        // list of files / patterns to load in the browser
        files: [
            'lib/jquery/jquery.js',
            'lib/chart.js/Chart.js',
            'lib/angular/angular.js',
            'node_modules/angular-mocks/angular-mocks.js',
            'lib/angular-resource/angular-resource.js',
            'lib/angular-route/angular-route.js',
            'lib/angular-ui-validate/validate.js',
            'lib/angular-ui-bootstrap/ui-bootstrap.js',
            'lib/angular-chart.js/angular-chart.js',

            'Scripts/ui/*.js',

            'Scripts/ui/app.ts',
            'Scripts/ui/base/*.ts',
            'Scripts/ui/controllers/*.ts',
            'Scripts/ui/directives/*.ts',

            'Scripts/tests/**/*.ts'
        ],

        // list of files to exclude
        exclude: [
        ],

        // preprocess matching files before serving them to the browser
        // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
            '**/*.ts': ['karma-typescript']
        },

        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: ['progress', 'junit', 'html', 'karma-typescript'],

        htmlReporter: {
            outputDir: '../reports/karma-results/html', // where to put the reports
            focusOnFailures: true, // reports show failures on start
        },

        junitReporter: {
            outputDir: '../reports/karma-results/junit', // results will be saved as $outputDir/$browserName.xml
            useBrowserName: true, // add browser name to report and classes names
        },

        karmaTypescriptConfig: {
            compilerOptions: {
                target: "ES5",
                module: "amd",
                noImplicitAny: true,
                noImplicitReturns: true,
                noFallthroughCasesInSwitch: true,
                sourceMap: true,
                suppressImplicitAnyIndexErrors: true
            },
            reports: {
                'html': '../reports/karma-typescript-results',
                'cobertura': {
                    'directory': '../reports/karma-typescript-results',
                    "subdirectory": "cobertura",
                    'filename': 'coverage.xml'
                }
            },
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

        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: false,

        // Concurrency level
        // how many browser should be started simultaneous
        concurrency: Infinity
    })
}
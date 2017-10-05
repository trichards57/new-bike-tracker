﻿/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

const _ = require('lodash');
const autoprefixer = require('gulp-autoprefixer');
const browserify = require('browserify');
const cleanCSS = require('gulp-clean-css');
const gulp = require('gulp');
const nodeResolve = require('resolve');
const pump = require('pump');
const sass = require('gulp-sass');
const source = require('vinyl-source-stream');
const streamify = require('gulp-streamify');
const uglify = require('gulp-uglify');
const tsify = require("tsify");

const appSources = [
    './src/js/app.tsx',
];

const scssSources = [
    './src/sass/theme.scss',
]

gulp.task('default', ['build-vendor', 'build-app', 'build-sass', 'build-fonts']);
gulp.task('release', ['release-vendor', 'release-app', 'release-sass', 'build-fonts']);

gulp.task('build-sass', (cb) => {
    pump([
        gulp.src(scssSources),
        sass({
            includePaths: [
                './src/sass',
                './node_modules/bootstrap/scss',
                './node_modules/font-awesome/scss'
            ],
            sourceMap: true
        }),
        autoprefixer({
            browsers: ['last 2 version'],
            cascade: false
        }),
        gulp.dest('./wwwroot/css')
    ], cb);
});

gulp.task('build-app', (cb) => {
    var b = browserify({
        // generate source maps in non-production environment
        debug: true,
        entries: appSources
    });

    // mark vendor libraries as external
    getNPMPackageIds().forEach(function (id) {
        b.external(id);
    });

    pump([
        b.plugin(tsify).bundle(),
        source('app.js'),
        gulp.dest('./wwwroot/js')
    ], cb);
});

gulp.task('build-fonts', (cb) => {
    pump([
        gulp.src('./node_modules/font-awesome/fonts/*'),
        gulp.dest('./wwwroot/fonts')
    ], cb);
});

gulp.task('build-vendor', (cb) => {
    var b = browserify({
        // generate source maps in non-production environment
        debug: true
    });

    // resolve npm modules
    getNPMPackageIds().forEach(function (id) {
        if (id === "font-awesome")
            return;

        b.require(nodeResolve.sync(id), { expose: id });
    });

    pump([
        b.bundle(),
        source('vendor.js'),
        gulp.dest('./wwwroot/js')
    ], cb);
});

gulp.task('release-sass', (cb) => {
    pump([
        gulp.src(scssSources),
        sass({
            includePaths: ['./node_modules/bootstrap/scss']
        }),
        cleanCSS({ level: 2 }),
        autoprefixer({
            browsers: ['last 2 version'],
            cascade: false
        }),
        gulp.dest('./wwwroot/css')
    ], cb);
});

gulp.task('release-app', (cb) => {
    var b = browserify(appSources);

    // mark vendor libraries as external
    getNPMPackageIds().forEach(function (id) {
        b.external(id);
    });

    pump([
        b.transform("babelify", { presets: ["es2015", "react", "flow"] })
            .bundle(),
        source('app.js'),
        streamify(uglify()),
        gulp.dest('./wwwroot/js')
    ], cb);
});

gulp.task('release-vendor', (cb) => {
    var b = browserify();

    // resolve npm modules
    getNPMPackageIds().forEach(function (id) {
        b.require(nodeResolve.sync(id), { expose: id });
    });

    pump([
        b.bundle(),
        source('vendor.js'),
        streamify(uglify()),
        gulp.dest('./wwwroot/js')
    ], cb);
});

gulp.task('watch', function () {
    gulp.watch(['./src/sass/**/*.scss'], ['build-sass']);
    gulp.watch(['./src/js/**/*.tsx'], ['build-app']);
});

function getNPMPackageIds() {
    // read package.json and get dependencies' package ids
    var packageManifest = {};
    try {
        packageManifest = require('./package.json');
    } catch (e) {
        // does not have a package.json manifest
    }
    return _.keys(packageManifest.dependencies) || [];
}
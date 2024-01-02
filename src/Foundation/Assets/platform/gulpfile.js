'use strict';

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    merge = require('merge-stream'),
    del = require('del'),
    bundleconfig = require('./bundleconfig.json');
const terser = require('gulp-terser');

const regex = {
    css: /\.css$/,
    js: /\.js$/
};

gulp.task('min:js', async function () {
    merge(getBundles(regex.js).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(terser())
            .pipe(gulp.dest('.'));
    }))
});

gulp.task('min:css', async function () {
    merge(getBundles(regex.css).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    }))
});



gulp.task('min', gulp.series(['min:js', 'min:css']));

gulp.task('clean', () => {
    return del(bundleconfig.map(bundle => bundle.outputFileName));
});

gulp.task('watch', () => {
    getBundles(regex.js).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(["min:js"])));

    getBundles(regex.css).forEach(
        bundle => gulp.watch(bundle.inputFiles, gulp.series(["min:css"])));

    
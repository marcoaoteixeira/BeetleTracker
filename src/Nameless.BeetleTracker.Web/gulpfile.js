// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var cleanCss = require('gulp-clean-css');
var copy = require('gulp-copy');
var rename = require('gulp-rename');

var config = {
    //Include all js files but exclude any min.js files
    scriptsOutputPath: 'wwwroot/dist/js/',
    scriptsSrc: [
        '!wwwroot/dist/**/*',
        '!wwwroot/**/*.min.js',
        'wwwroot/js/**/*.js'
    ],
    scriptsToCopy: [
        'wwwroot/js/plugins/jquery.jvectormap-v2.0.4.min.js'
    ],

    //Include all css files but exclude any min.css files
    stylesOutputPath: 'wwwroot/dist/css/',
    stylesSrc: [
        '!wwwroot/dist/**/*',
        '!wwwroot/**/*.min.css',
        'wwwroot/css/**/*.css'
    ],
    stylesToCopy: [],

    // Include all resource files, but exclude any css and js files.
    resourcesOutputPath: 'wwwroot/dist/',
    resourcesToCopy: [
        '!wwwroot/**/*.css',
        '!wwwroot/**/*.js',
        'wwwroot/**/*'
    ]
};

//delete the output file(s)
gulp.task('clean', function () {
    // del is an async function and not a gulp plugin (just standard nodejs)
    // It returns a promise, so make sure you return that from this task function
    // so gulp knows when the delete is complete
    return del(['wwwroot/dist/*']);
});

// CSS minify
gulp.task('styles', function () {
    return gulp.src(config.stylesSrc)
        .pipe(cleanCss({ compatibility: 'ie8' }))
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(config.stylesOutputPath));
});

gulp.task('just-copy-css', function () {
    return gulp.src(config.stylesToCopy)
        .pipe(copy(config.stylesOutputPath, { prefix: 2 }));
});

// Combine and minify all files from the app folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
gulp.task('scripts', ['clean'], function () {
    return gulp.src(config.scriptsSrc)
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(config.scriptsOutputPath));
});

gulp.task('just-copy-js', function () {
    return gulp.src(config.scriptsToCopy)
        .pipe(copy(config.scriptsOutputPath, { prefix: 2 }));
});

gulp.task('copy-resources', function () {
    return gulp.src(config.resourcesToCopy)
        .pipe(copy(config.resourcesOutputPath, { prefix: 1 }));
});

//Set a default tasks
gulp.task('default', ['clean', 'scripts', 'just-copy-js', 'styles', 'just-copy-css', 'copy-resources'], function () {
});
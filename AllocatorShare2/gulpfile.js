/// <vs SolutionOpened='watch' />
var gulp = require('gulp'),
    jshint = require('gulp-jshint'),
    stylish = require('jshint-stylish'),
    less = require('gulp-less'),
    sourcemaps = require('gulp-sourcemaps'),
    react = require('gulp-react');

//JS HINT
gulp.task('lint', function () {
    return gulp.src('./Scripts/*.js')
        .pipe(jshint())
        .pipe(jshint.reporter(stylish));
});

//LESS
//Only looks in the /Content root folder
gulp.task('less', function () {
    gulp.src('./Content/*.less')
      .pipe(sourcemaps.init())
      .pipe(less())
      .pipe(sourcemaps.write())
      .pipe(gulp.dest('./Content'));
});

gulp.task('react', function () {
    return gulp.src('./Scripts/*.jsx')
        .pipe(react())
        .pipe(gulp.dest('./Scripts'));
});

//WATCH
gulp.task('watch', function () {
    gulp.watch('Content/*.less', ['less']);
    gulp.watch('Scripts/*.jsx', ['react']);
    gulp.watch('Scripts/*.js', ['lint']);
});
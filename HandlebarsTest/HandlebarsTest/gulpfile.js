var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

gulp.task('default', ['templates','scripts'], function () {

});

var handlebars = require('gulp-handlebars');
var wrap = require('gulp-wrap');
var declare = require('gulp-declare');
var concat = require('gulp-concat');

gulp.task('templates', function () {
    gulp.src('templates/*.hbs')
      .pipe(handlebars())
      .pipe(wrap('Handlebars.template(<%= contents %>)'))
      .pipe(declare({
          namespace: 'MyApp.templates',
          noRedeclare: true, // Avoid duplicate declarations
      }))
      .pipe(concat('templates.js'))
      .pipe(gulp.dest('js/dist'));
});

gulp.task('scripts', function () {
    return gulp.src(['js/lib/jquery/dist/jquery.js', 'js/lib/handlebars/handlebars.runtime.js', 'js/dist/templates.js', 'js/app/**/*.js'])
      .pipe(concat('bundle.js'))
      .pipe(uglify())
      .pipe(gulp.dest('js/dist/'));
});


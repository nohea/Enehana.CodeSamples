
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

gulp.task('default', ['scripts'], function () {

});

gulp.task('scripts', function () {
    return gulp.src(['node_modules/jquery/dist/jquery.js', 'scripts/**/*.js'])
      .pipe(concat('main.js'))
      .pipe(gulp.dest('dist/js'))
      .pipe(uglify())
      .pipe(gulp.dest('dist/js'));
});

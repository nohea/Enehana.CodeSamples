"use strict";
var data = { title: 'This Form', name: 'Joey' };
var html = MyApp.templates.hellotemplate(data);
// console.log(html);

$(document).ready(function () {
    $('#dynamic-content').html(html);
});

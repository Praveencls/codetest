var $ = require('jquery');
var selectIds = $('#collapseOne,#collapseTwo,#collapseThree,#collapseFour,#collapseFive');
$(function ($) {
    selectIds.on('show.bs.collapse hidden.bs.collapse', function () {
        $(this).prev().find('.fa').toggleClass('fa-plus fa-minus');
    })
});
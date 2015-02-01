﻿function annuler() {
    window.location.replace("/Home");
};

$(function () {
    $("#datepicker").datetimepicker({
        dateFormat: "yy-mm-dd"
    });
});

$(function () {
    $("#datepickernotime").datepicker({
        dateFormat: "yy-mm-dd",
        changeYear: true,
        yearRange: "-90:+0"
    });
});


$(function () {
    $('#datepickernotime2').datepicker({
        dateFormat: "yy-mm-dd",
        changeYear: true,
        yearRange: '-90:+0'
    });
});

//Musician section
$(function () {
    var selectInnerHtml = "";
    var selectClass = "";

    $(".profile-modif-tab-container").on('change', '.input-group-instrument-rating:last-child select', function () {
        if (selectInnerHtml === "") {
            selectInnerHtml = $("#instrument-list").html();
            selectClass = $("#instrument-list").attr('class');
        }

        var inputGroupHtml =
            '<div class="input-group-instrument-rating">' +
                '<div class="col-md-5 row">'+
                    '<select class="' + selectClass + '">' + selectInnerHtml + '</select>' +
                '</div>' +
                '<div class="col-md-4 col-md-offset-1 instrument-rating">' +
                    '<a class="star-rating" data-nb-stars="5" data-placement="top" data-rating="1"></a>' +
                '</div>' +
            '</div>';

        $(this).parents(".input-group-instrument-rating").append('<div class="col-md-1 instrument-rating-remove">' +
                                                '<span class="glyphicon glyphicon-remove"></span></div>');

        $(".form-group-instrument-rating").append(inputGroupHtml);

        $('.form-group-instrument-rating .selectpicker:last').selectpicker('refresh');
        initStarRating($(".star-rating:last"));
    });

    $(".form-group-instrument-rating").on("click", ".instrument-rating-remove", function () {
        $(this).parents(".input-group-instrument-rating").remove();
    });
});



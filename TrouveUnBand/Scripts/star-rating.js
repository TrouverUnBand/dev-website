//(function ($) {
//    "use strict";

//    var skillsList = ["Débutant", "Initié", "Intermédiaire", "Avancé", "Professionnel"];
//    var currentTitle = "";

//    var methods = {

//        showStars: function (element) {
//            var nbOfStars = element.attr("data-nb-star");
//            var baseRating = element.attr("data-rating");
//            var starHtml = "";

//            if (element.attr("data-access") === "write") {
//                baseRating = 1;
//            }

//            for (var i = 0; i < nbOfStars; i++) {
//                if (baseRating > i) {
//                    starHtml += '<span class="glyphicon glyphicon-star"></span>';
//                } else {
//                    starHtml += '<span class="glyphicon glyphicon-star-empty"></span>';
//                }
//            }

//            element.html(starHtml);
//        }
//    };

//    $(window).on("load.bs.select.data-api", function () {
//        $(".star-rating").each(function () {
//            methods.showStars($(this));
//        });
//    });

//    $(".form-group-instrument-rating").on("mouseenter", ".star-rating span", function () {
//        $(this).prevAll().switchClass("glyphicon-star-empty", "glyphicon-star");
//        $(this).switchClass("glyphicon-star-empty", "glyphicon-star");
//        $(this).nextAll().switchClass("glyphicon-star", "glyphicon-star-empty");

//        currentTitle = skillsList[$(this).index()];
//    });

//    $(".form-group-instrument-rating").on("mouseleave", ".star-rating", function () {
//        var rating = $(this).attr("data-rating");

//        $(this).children().eq(rating - 1).prevAll().switchClass("glyphicon-star-empty", "glyphicon-star");
//        $(this).children().eq(rating - 1).switchClass("glyphicon-star-empty", "glyphicon-star");
//        $(this).children().eq(rating - 1).nextAll().switchClass("glyphicon-star", "glyphicon-star-empty");
//    });

//    $(".form-group-instrument-rating").on("click", ".star-rating span", function () {
//        var newRating = $(this).index() + 1;
//        $(this).parent(".star-rating").attr("data-rating", newRating);
//    });
//}(jQuery));

var NbStars = 0;
var Rating = 0;
var SkillsList = ["Débutant", "Initié", "Intermédiaire", "Avancé", "Professionnel"];
var Elements = $(".star-rating");

function initStarRating(element) {
    $(element).attr("data-animation", "false");
    $(element).attr("title", SkillsList[1]);
    $(element).attr("data-toggle", "tooltip");
    NbStars = $(element).attr("data-nb-stars");
    Rating = $(element).attr("data-rating");

    for (var j = 0; j < NbStars; j++) {
        if (j < Rating) {
            $(element).append('<i class="glyphicon glyphicon-star" data-prev-rating-class="glyphicon glyphicon-star"></i>');
        }
        else {
            $(element).append('<i class="glyphicon glyphicon-star-empty" data-prev-rating-class="glyphicon glyphicon-star-empty"></i>');
        }
    }
}

for (var i = 0; i < Elements.size(); i++) {
    initStarRating(Elements[i]);
}

$('.form-group-instrument-rating').on("mouseenter", ".star-rating i", function () {
    Rating = 0;
    $(this).prevAll().removeClass('glyphicon-star-empty').addClass('glyphicon-star');
    $(this).removeClass('glyphicon-star-empty').addClass('glyphicon-star');
    $(this).nextAll().addClass('glyphicon-star-empty').removeClass('glyphicon-star');

    $(this).siblings('i').each(function () {
        if ($(this).hasClass('glyphicon-star')) {
            Rating = Rating + 1;
        }
    });
    $(this).parent('.star-rating').attr('data-original-title', SkillsList[Rating]);

    $(this).parent('.star-rating').tooltip('hide');
    $(this).parent('.star-rating').tooltip('fixTitle');
    $(this).parent('.star-rating').tooltip('show');
});

$('.form-group-instrument-rating').on("mouseleave", ".star-rating", function () {
    Rating = 0;
    $(this).children('i').each(function () {
        $(this).removeClass('glyphicon-star-empty');
        $(this).attr('class', $(this).attr('data-prev-rating-class'));
        if ($(this).hasClass('glyphicon-star')) {
            Rating = Rating + 1;
        }
    });
    $(this).attr('data-original-title', SkillsList[Rating - 1]);
});

$('.form-group-instrument-rating').on("click", ".star-rating", function () {
    Rating = 0;
    $(this).children('i').each(function () {
        $(this).attr('data-prev-rating-class', $(this).attr('class'));
        if ($(this).hasClass('glyphicon-star')) {
            Rating = Rating + 1;
        }
    });
    $(this).attr('data-rating', Rating);
});
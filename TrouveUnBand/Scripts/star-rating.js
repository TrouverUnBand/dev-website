(function ($) {
    "use strict";

    var skillsList = ["Débutant", "Initié", "Intermédiaire", "Avancé", "Professionnel"];

    var methods = {

        showStars: function (element) {
            var nbOfStars = element.attr("data-nb-star");
            var baseRating = element.attr("data-rating");
            var starHtml = "";

            if (element.attr("data-access") === "write") {
                baseRating = 1;
            }

            for (var i = 0; i < nbOfStars; i++) {
                if (baseRating > i) {
                    starHtml += '<span class="glyphicon glyphicon-star"></span>';
                } else {
                    starHtml += '<span class="glyphicon glyphicon-star-empty"></span>';
                }
            }

            element.html(starHtml);
        }
    };

    $(window).on("load.bs.select.data-api", function () {
        $(".star-rating").each(function () {
            methods.showStars($(this));
        });
    });

    $(".form-group-instrument-rating").on("mouseenter", ".star-rating span", function () {
        $(this).prevAll().switchClass("glyphicon-star-empty", "glyphicon-star");
        $(this).switchClass("glyphicon-star-empty", "glyphicon-star");
        $(this).nextAll().switchClass("glyphicon-star", "glyphicon-star-empty");
    });

    $(".form-group-instrument-rating").on("mouseleave", ".star-rating", function () {
        var rating = $(this).attr("data-rating");

        $(this).children().eq(rating - 1).prevAll().switchClass("glyphicon-star-empty", "glyphicon-star");
        $(this).children().eq(rating - 1).switchClass("glyphicon-star-empty", "glyphicon-star");
        $(this).children().eq(rating - 1).nextAll().switchClass("glyphicon-star", "glyphicon-star-empty");
    });

    $(".form-group-instrument-rating").on("click", ".star-rating span", function () {
        var newRating = $(this).index() + 1;
        $(this).parent(".star-rating").attr("data-rating", newRating);
    });
}(jQuery));
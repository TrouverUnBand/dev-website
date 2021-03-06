var $lightbox;

if ($("#PicModal")[0]) {
    $lightbox = $("#PicModal");
}

$('[data-target="#PicModal"]').on("click", function () {
    var $img = $(this).find("img"),
        src = $img.attr("src"),
        alt = $img.attr("alt"),
        css = {
            'maxWidth': $(window).width() - 100,
            'maxHeight': $(window).height() - 100
        };

    $lightbox.find(".close").addClass("hidden");
    $lightbox.find("img").attr("src", src);
    $lightbox.find("img").attr("alt", alt);
    $lightbox.find("img").css(css);
});

$lightbox.on("shown.bs.modal", function () {
    var $img = $lightbox.find("img");

    $lightbox.find(".modal-dialog").css({ 'width': $img.width() });
    $lightbox.find(".close").removeClass("hidden");
});

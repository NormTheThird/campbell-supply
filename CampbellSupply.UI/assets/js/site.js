(function () {
    "use strict";
    var $pickButton = $("#pickButton");
    $("#reasonDropdown li a").on("click", function () {
        var reason = $(this).text();
        $pickButton.text(reason);
    });

})();

$(document).ready(function (e) {
    $('.search-panel .dropdown-menu').find('a').click(function (e) {
        e.preventDefault();
        var param = $(this).attr("href").replace("#", "");
        var concept = $(this).text();
        $('.search-panel span#search_concept').text(concept);
        $('.input-group #search_param').val(param);
    });
});

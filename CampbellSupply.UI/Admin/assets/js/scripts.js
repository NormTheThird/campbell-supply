(function($) {
    "use strict";

    /*===================================================================================*/
    /*  WOW 
    /*===================================================================================*/

    $(document).ready(function () {
        new WOW().init();
    });
    
    /*===================================================================================*/
    /*  OWL CAROUSEL
    /*===================================================================================*/

    $(document).ready(function () {
        
        var dragging = true;
        var owlElementID = "#owl-main";
        
        function fadeInReset() {
            if (!dragging) {
                $(owlElementID + " .caption .fadeIn-1, " + owlElementID + " .caption .fadeIn-2, " + owlElementID + " .caption .fadeIn-3").stop().delay(800).animate({ opacity: 0 }, { duration: 400, easing: "easeInCubic" });
            }
            else {
                $(owlElementID + " .caption .fadeIn-1, " + owlElementID + " .caption .fadeIn-2, " + owlElementID + " .caption .fadeIn-3").css({ opacity: 0 });
            }
        }
        
        function fadeInDownReset() {
            if (!dragging) {
                $(owlElementID + " .caption .fadeInDown-1, " + owlElementID + " .caption .fadeInDown-2, " + owlElementID + " .caption .fadeInDown-3").stop().delay(800).animate({ opacity: 0, top: "-15px" }, { duration: 400, easing: "easeInCubic" });
            }
            else {
                $(owlElementID + " .caption .fadeInDown-1, " + owlElementID + " .caption .fadeInDown-2, " + owlElementID + " .caption .fadeInDown-3").css({ opacity: 0, top: "-15px" });
            }
        }
        
        function fadeInUpReset() {
            if (!dragging) {
                $(owlElementID + " .caption .fadeInUp-1, " + owlElementID + " .caption .fadeInUp-2, " + owlElementID + " .caption .fadeInUp-3").stop().delay(800).animate({ opacity: 0, top: "15px" }, { duration: 400, easing: "easeInCubic" });
            }
            else {
                $(owlElementID + " .caption .fadeInUp-1, " + owlElementID + " .caption .fadeInUp-2, " + owlElementID + " .caption .fadeInUp-3").css({ opacity: 0, top: "15px" });
            }
        }
        
        function fadeInLeftReset() {
            if (!dragging) {
                $(owlElementID + " .caption .fadeInLeft-1, " + owlElementID + " .caption .fadeInLeft-2, " + owlElementID + " .caption .fadeInLeft-3").stop().delay(800).animate({ opacity: 0, left: "15px" }, { duration: 400, easing: "easeInCubic" });
            }
            else {
                $(owlElementID + " .caption .fadeInLeft-1, " + owlElementID + " .caption .fadeInLeft-2, " + owlElementID + " .caption .fadeInLeft-3").css({ opacity: 0, left: "15px" });
            }
        }
        
        function fadeInRightReset() {
            if (!dragging) {
                $(owlElementID + " .caption .fadeInRight-1, " + owlElementID + " .caption .fadeInRight-2, " + owlElementID + " .caption .fadeInRight-3").stop().delay(800).animate({ opacity: 0, left: "-15px" }, { duration: 400, easing: "easeInCubic" });
            }
            else {
                $(owlElementID + " .caption .fadeInRight-1, " + owlElementID + " .caption .fadeInRight-2, " + owlElementID + " .caption .fadeInRight-3").css({ opacity: 0, left: "-15px" });
            }
        }
        
        function fadeIn() {
            $(owlElementID + " .active .caption .fadeIn-1").stop().delay(500).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeIn-2").stop().delay(700).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeIn-3").stop().delay(1000).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
        }
        
        function fadeInDown() {
            $(owlElementID + " .active .caption .fadeInDown-1").stop().delay(500).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInDown-2").stop().delay(700).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInDown-3").stop().delay(1000).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        }
        
        function fadeInUp() {
            $(owlElementID + " .active .caption .fadeInUp-1").stop().delay(500).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInUp-2").stop().delay(700).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInUp-3").stop().delay(1000).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        }
        
        function fadeInLeft() {
            $(owlElementID + " .active .caption .fadeInLeft-1").stop().delay(500).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInLeft-2").stop().delay(700).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInLeft-3").stop().delay(1000).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        }
        
        function fadeInRight() {
            $(owlElementID + " .active .caption .fadeInRight-1").stop().delay(500).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInRight-2").stop().delay(700).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
            $(owlElementID + " .active .caption .fadeInRight-3").stop().delay(1000).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        }
        
        $(owlElementID).owlCarousel({
            
            autoPlay: 5000,
            stopOnHover: true,
            navigation: true,
            pagination: true,
            singleItem: true,
            addClassActive: true,
            transitionStyle: "fade",
            navigationText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
                
            afterInit: function() {
                fadeIn();
                fadeInDown();
                fadeInUp();
                fadeInLeft();
                fadeInRight();
            },
            
            afterMove: function() {
                fadeIn();
                fadeInDown();
                fadeInUp();
                fadeInLeft();
                fadeInRight();
            },
            
            afterUpdate: function() {
                fadeIn();
                fadeInDown();
                fadeInUp();
                fadeInLeft();
                fadeInRight();
            },
            
            startDragging: function() {
                dragging = true;
            },
            
            afterAction: function() {
                fadeInReset();
                fadeInDownReset();
                fadeInUpReset();
                fadeInLeftReset();
                fadeInRightReset();
                dragging = false;
            }
            
        });
        
        if ($(owlElementID).hasClass("owl-one-item")) {
            $(owlElementID + ".owl-one-item").data('owlCarousel').destroy();
        }
        
        $(owlElementID + ".owl-one-item").owlCarousel({
            singleItem: true,
            navigation: false,
            pagination: false
        });
        
        $('#transitionType li a').click(function () {
            
            $('#transitionType li a').removeClass('active');
            $(this).addClass('active');
            
            var newValue = $(this).attr('data-transition-type');
            
            $(owlElementID).data("owlCarousel").transitionTypes(newValue);
            $(owlElementID).trigger("owl.next");
            
            return false;
            
        });

        $("#owl-recently-viewed").owlCarousel({
            stopOnHover: true,
            rewindNav: true,
            items: 6,
            pagination: false,
            itemsTablet: [768,3]
        });

        $("#owl-recently-viewed-2").owlCarousel({
            stopOnHover: true,
            rewindNav: true,
            items: 4,
            pagination: false,
            itemsTablet: [768,3],
            itemsDesktopSmall: [1199,3],
        });

        $("#owl-brands").owlCarousel({
            stopOnHover: true,
            rewindNav: true,
            items: 6,
            pagination: false,
            itemsTablet : [768, 4]
        });

        $('#owl-single-product').owlCarousel({
            singleItem: true,
            pagination: false
        });

        $('#owl-single-product-thumbnails').owlCarousel({
            items: 6,
            pagination: false,
            rewindNav: true,
            itemsTablet : [768, 4]
        });

        $('#owl-recommended-products').owlCarousel({
            rewindNav: true,
            items: 4,
            pagination: false,
            itemsTablet: [768, 3],
            itemsDesktopSmall: [1199,3],
        });

        $('.single-product-slider').owlCarousel({
            stopOnHover: true,
            rewindNav: true,
            singleItem: true,
            pagination: false
        });
        
        $(".slider-next").click(function () {
            var owl = $($(this).data('target'));
            owl.trigger('owl.next');
            return false;
        });
        
        $(".slider-prev").click(function () {
            var owl = $($(this).data('target'));
            owl.trigger('owl.prev');
            return false;
        });

        $('.single-product-gallery .horizontal-thumb').click(function(){
            var $this = $(this), owl = $($this.data('target')), slideTo = $this.data('slide');
            owl.trigger('owl.goTo', slideTo);
            $this.addClass('active').parent().siblings().find('.active').removeClass('active');
            return false;
        });
        
    });

    /*===================================================================================*/
    /*  STAR RATING
    /*===================================================================================*/

    $(document).ready(function () {

        if ($('.star').length > 0) {
            $('.star').each(function(){
                    var $star = $(this);
                    
                    if($star.hasClass('big')){
                        $star.raty({
                            starOff: 'assets/images/star-big-off.png',
                            starOn: 'assets/images/star-big-on.png',
                            space: false,
                            score: function() {
                                return $(this).attr('data-score');
                            }
                        });
                    }else{
                     $star.raty({
                        starOff: 'assets/images/star-off.png',
                        starOn: 'assets/images/star-on.png',
                        space: false,
                        score: function() {
                            return $(this).attr('data-score');
                        }
                    });
                }
            });
        }
    });

    /*===================================================================================*/
    /*  SHARE THIS BUTTONS
    /*===================================================================================*/

    $(document).ready(function () {
        if($('.social-row').length > 0){
            stLight.options({publisher: "2512508a-5f0b-47c2-b42d-bde4413cb7d8", doNotHash: false, doNotCopy: false, hashAddressBar: false});
        }
    });

    /*===================================================================================*/
    /*  CUSTOM CONTROLS
    /*===================================================================================*/

    $(document).ready(function () {
        
        // Select Dropdown
        if($('.le-select').length > 0){
            $('.le-select select').customSelect({customClass:'le-select-in'});
        }

        // Checkbox
        if($('.le-checkbox').length>0){
            $('.le-checkbox').after('<i class="fake-box"></i>');
        }

        //Radio Button
        if($('.le-radio').length>0){
            $('.le-radio').after('<i class="fake-box"></i>');
        }

        // Buttons
        $('.le-button.disabled').click(function(e){
            e.preventDefault();
        });

        // Quantity Spinner
        $('.le-quantity a').click(function(e){
            e.preventDefault();
            var currentQty= $(this).parent().parent().find('input').val();
            if( $(this).hasClass('minus') && currentQty>0){
                $(this).parent().parent().find('input').val(parseInt(currentQty, 10) - 1);
            }else{
                if( $(this).hasClass('plus')){
                    $(this).parent().parent().find('input').val(parseInt(currentQty, 10) + 1);
                }
            }
        });

        // Price Slider
        if ($('.price-slider').length > 0) {
            $('.price-slider').slider({
                min: 100,
                max: 700,
                step: 10,
                value: [100, 400],
                handle: "square"

            });
        }

        // Data Placeholder for custom controls

        $('[data-placeholder]').focus(function() {
            var input = $(this);
            if (input.val() == input.attr('data-placeholder')) {
                input.val('');

            }
        }).blur(function() {
            var input = $(this);
            if (input.val() === '' || input.val() == input.attr('data-placeholder')) {
                input.addClass('placeholder');
                input.val(input.attr('data-placeholder'));
            }
        }).blur();

        $('[data-placeholder]').parents('form').submit(function() {
            $(this).find('[data-placeholder]').each(function() {
                var input = $(this);
                if (input.val() == input.attr('data-placeholder')) {
                    input.val('');
                }
            });
        });

    });

    /*===================================================================================*/
    /*  LIGHTBOX ACTIVATOR
    /*===================================================================================*/
    $(document).ready(function(){
        if ($('a[data-rel="prettyphoto"]').length > 0) {
            //$('a[data-rel="prettyphoto"]').prettyPhoto();
        }
    });


    /*===================================================================================*/
    /*  SELECT TOP DROP MENU
    /*===================================================================================*/
    $(document).ready(function() {
        $('.top-drop-menu').change(function() {
            var loc = ($(this).find('option:selected').val());
            window.location = loc;
        });
    });

    /*===================================================================================*/
    /*  LAZY LOAD IMAGES USING ECHO
    /*===================================================================================*/
    $(document).ready(function(){
        echo.init({
            offset: 100,
            throttle: 250,
            unload: false
        });
    });

    /*===================================================================================*/
    /*  GMAP ACTIVATOR
    /*===================================================================================*/

   
    var locations = [
                ['Main Warehouse', 43.566125, -96.748449, 1],
                ['<b>Campbells 3101 East 10th</b>', 43.546221, -96.686651, 2],
                ['<b>Campbells 49th & Western</b>', 43.508633, -96.750852, 3],
                ['<b>Campbells Mitchell SD</b>', 43.696960, -98.015603, 4],
                ['<b>Campbells Madison SD</b>', 44.008071, -97.106957, 5],
                ['<b>Campbells Rock Rapids IA</b>', 43.432238, -96.180092, 6],
                ['<b>Campbells Vermillion SD</b>', 42.786258, -96.947256, 7],
                ['<b>Campbells Sturgis SD</b>', 44.420183, -103.530424, 8]
    ];

    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 7,
        center: new google.maps.LatLng(43.696671, -98.015732),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    var marker, i;


    for (i = 0; i < locations.length; i++) {
        marker = new google.maps.Marker({
            position: new google.maps.LatLng(locations[i][1], locations[i][2]),
            map: map,
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(locations[i][0]);
                infowindow.open(map, marker);
            }
        })(marker, i));
    }

})(jQuery);
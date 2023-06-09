<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="CampbellSupply.Contact" %>

<%@ Register Src="~/usercontrols/pageContent.ascx" TagPrefix="uc1" TagName="pageContent" %>
<%@ Register Src="~/usercontrols/pageContentLocation.ascx" TagPrefix="uc1" TagName="pageContentLocation" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <style>
        #map {
            height: 400px;
            width: 100%;
        }
    </style>


    <main id="contact-us" class="inner-bottom-md">
        <section id="">
            <div id="map"></div>
            <script>
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
                function initMap() {
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
                }
            </script>
            <script src="https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/markerclusterer.js">
            </script>
            <script async defer
                src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCp4b6ANu6ra35L97FKMXEOZ_R-BBSM5cY&callback=initMap">
            </script>
        </section>
        <div class="container">
            <div class="row">

                <div class="col-md-8">
                    <section class="section leave-a-message">
                        <uc1:pageContent runat="server" ID="pageContent" />
                        <asp:Label ID="lblSuccess" CssClass="label label-danger" runat="server"></asp:Label>
                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-6">
                                <label>Your Name*</label>
                                <input id="fullname" type="text" class="le-input" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="text-danger bold" ControlToValidate="fullname" runat="server" ErrorMessage="*Name required."></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <label>Your Email*</label>
                                <input id="email" type="text" class="le-input" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="text-danger bold" ControlToValidate="email" runat="server" ErrorMessage="*Email required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <!-- /.field-row -->

                        <div class="field-row">
                            <label>Subject</label>
                            <input id="subject" type="text" class="le-input" runat="server" />
                        </div>
                        <!-- /.field-row -->

                        <div class="field-row">
                            <label>Your Message</label>
                            <textarea id="message" rows="8" class="le-input" runat="server"></textarea>
                        </div>
                        <!-- /.field-row -->

                        <div class="buttons-holder">
                            <br />
                            <asp:Button ID="btnSubmit" Text="Submit Message" CssClass="le-button huge" runat="server" OnClick="btnSubmit_Click" />
                        </div>
                        <!-- /.buttons-holder -->
                        <!-- /.contact-form -->
                    </section>
                    <!-- /.leave-a-message -->
                </div>
                <!-- /.col -->
                <!-- ============================================================= LOCATIONS ============================================================= -->
                <div class="col-md-4">
                    <section class="our-store section inner-left-xs">
                        <uc1:pageContentLocation runat="server" ID="pageContentLocation" />
                    </section>
                    <!-- /.our-store -->
                </div>
                <!-- ============================================================= LOCATIONS ============================================================= -->
                <!-- /.col -->

            </div>
            <!-- /.row -->
        </div>
        <!-- /.container -->
    </main>
</asp:Content>

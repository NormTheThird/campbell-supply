﻿/*!
 *  GMAP3 Plugin for JQuery
 *  Version   : 5.0b
 *  Date      : 2012-11-17
 *  Licence   : GPL v3 : http://www.gnu.org/licenses/gpl.html
 *  Author    : DEMONTE Jean-Baptiste
 *  Contact   : jbdemonte@gmail.com
 *  Web site  : http://gmap3.net
 */
(function (i, e) {
    var q, x = 0;

    function m() {
        if (!q) {
            q = {
                verbose: false,
                queryLimit: {
                    attempt: 5,
                    delay: 250,
                    random: 250
                },
                classes: {
                    Map: google.maps.Map,
                    Marker: google.maps.Marker,
                    InfoWindow: google.maps.InfoWindow,
                    Circle: google.maps.Circle,
                    Rectangle: google.maps.Rectangle,
                    OverlayView: google.maps.OverlayView,
                    StreetViewPanorama: google.maps.StreetViewPanorama,
                    KmlLayer: google.maps.KmlLayer,
                    TrafficLayer: google.maps.TrafficLayer,
                    BicyclingLayer: google.maps.BicyclingLayer,
                    GroundOverlay: google.maps.GroundOverlay,
                    StyledMapType: google.maps.StyledMapType,
                    ImageMapType: google.maps.ImageMapType
                },
                map: {
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    center: [46.578498, 2.457275],
                    zoom: 2
                },
                overlay: {
                    pane: "floatPane",
                    content: "",
                    offset: {
                        x: 0,
                        y: 0
                    }
                },
                geoloc: {
                    getCurrentPosition: {
                        maximumAge: 60000,
                        timeout: 5000
                    }
                }
            }
        }
    }

    function G(K, J) {
        return K !== e ? K : "gmap3_" + (J ? x + 1 : ++x)
    }

    function B(N, J, L, O, M) {
        if (J.todo.events || J.todo.onces) {
            var K = {
                id: O,
                data: J.todo.data,
                tag: J.todo.tag
            }
        }
        if (J.todo.events) {
            i.each(J.todo.events, function (P, Q) {
                google.maps.event.addListener(L, P, function (R) {
                    Q.apply(N, [M ? M : L, R, K])
                })
            })
        }
        if (J.todo.onces) {
            i.each(J.todo.onces, function (P, Q) {
                google.maps.event.addListenerOnce(L, P, function (R) {
                    Q.apply(N, [M ? M : L, R, K])
                })
            })
        }
    }

    function w() {
        var J = [];
        this.empty = function () {
            return !J.length
        };
        this.add = function (K) {
            J.push(K)
        };
        this.get = function () {
            return J.length ? J[0] : false
        };
        this.ack = function () {
            J.shift()
        }
    }

    function a(R, J, L) {
        var P = {},
            N = this,
            O, Q = {
                latLng: {
                    map: false,
                    marker: false,
                    infowindow: false,
                    circle: false,
                    overlay: false,
                    getlatlng: false,
                    getmaxzoom: false,
                    getelevation: false,
                    streetviewpanorama: false,
                    getaddress: true
                },
                geoloc: {
                    getgeoloc: true
                }
            };
        if (typeof L === "string") {
            L = K(L)
        }

        function K(T) {
            var S = {};
            S[T] = {};
            return S
        }

        function M() {
            var S;
            for (S in L) {
                if (S in P) {
                    continue
                }
                return S
            }
        }
        this.run = function () {
            var S, T;
            while (S = M()) {
                if (typeof R[S] === "function") {
                    O = S;
                    T = i.extend(true, {}, q[S] || {}, L[S].options || {});
                    if (S in Q.latLng) {
                        if (L[S].values) {
                            F(L[S].values, R, R[S], {
                                todo: L[S],
                                opts: T,
                                session: P
                            })
                        } else {
                            H(R, R[S], Q.latLng[S], {
                                todo: L[S],
                                opts: T,
                                session: P
                            })
                        }
                    } else {
                        if (S in Q.geoloc) {
                            D(R, R[S], {
                                todo: L[S],
                                opts: T,
                                session: P
                            })
                        } else {
                            R[S].apply(R, [{
                                todo: L[S],
                                opts: T,
                                session: P
                            }])
                        }
                    }
                    return
                } else {
                    P[S] = null
                }
            }
            J.apply(R, [L, P])
        };
        this.ack = function (S) {
            P[O] = S;
            N.run.apply(N, [])
        }
    }

    function d(L) {
        var J, K = [];
        for (J in L) {
            K.push(J)
        }
        return K
    }

    function v(L, O) {
        var J = {};
        if (L.todo) {
            for (var K in L.todo) {
                if ((K !== "options") && (K !== "values")) {
                    J[K] = L.todo[K]
                }
            }
        }
        var M, N = ["data", "tag", "id", "events", "onces"];
        for (M = 0; M < N.length; M++) {
            g(J, N[M], O, L.todo)
        }
        J.options = i.extend({}, L.todo.options || {}, O.options || {});
        return J
    }

    function g(L, K) {
        for (var J = 2; J < arguments.length; J++) {
            if (K in arguments[J]) {
                L[K] = arguments[J][K];
                return
            }
        }
    }

    function E() {
        var J = [];
        this.get = function (Q) {
            if (J.length) {
                var N, M, L, P, K, O = d(Q);
                for (N = 0; N < J.length; N++) {
                    P = J[N];
                    K = O.length == P.keys.length;
                    for (M = 0;
                        (M < O.length) && K; M++) {
                        L = O[M];
                        K = L in P.request;
                        if (K) {
                            if ((typeof Q[L] === "object") && ("equals" in Q[L]) && (typeof Q[L] === "function")) {
                                K = Q[L].equals(P.request[L])
                            } else {
                                K = Q[L] === P.request[L]
                            }
                        }
                    }
                    if (K) {
                        return P.results
                    }
                }
            }
        };
        this.store = function (L, K) {
            J.push({
                request: L,
                keys: d(L),
                results: K
            })
        }
    }

    function I(O, N, M, J) {
        var L = this,
            K = [];
        q.classes.OverlayView.call(this);
        this.setMap(O);
        this.onAdd = function () {
            var P = this.getPanes();
            if (N.pane in P) {
                i(P[N.pane]).append(J)
            }
            i.each("dblclick click mouseover mousemove mouseout mouseup mousedown".split(" "), function (R, Q) {
                K.push(google.maps.event.addDomListener(J[0], Q, function (S) {
                    i.Event(S).stopPropagation();
                    google.maps.event.trigger(L, Q, [S])
                }))
            });
            K.push(google.maps.event.addDomListener(J[0], "contextmenu", function (Q) {
                i.Event(Q).stopPropagation();
                google.maps.event.trigger(L, "rightclick", [Q])
            }));
            this.draw()
        };
        this.getPosition = function () {
            return M
        };
        this.draw = function () {
            this.draw = function () {
                var P = this.getProjection().fromLatLngToDivPixel(M);
                J.css("left", (P.x + N.offset.x) + "px").css("top", (P.y + N.offset.y) + "px")
            }
        };
        this.onRemove = function () {
            for (var P = 0; P < K.length; P++) {
                google.maps.event.removeListener(K[P])
            }
            J.remove()
        };
        this.hide = function () {
            J.hide()
        };
        this.show = function () {
            J.show()
        };
        this.toggle = function () {
            if (J) {
                if (J.is(":visible")) {
                    this.show()
                } else {
                    this.hide()
                }
            }
        };
        this.toggleDOM = function () {
            if (this.getMap()) {
                this.setMap(null)
            } else {
                this.setMap(O)
            }
        };
        this.getDOMElement = function () {
            return J[0]
        }
    }

    function f(L) {
        function J() {
            this.onAdd = function () { };
            this.onRemove = function () { };
            this.draw = function () { };
            return q.classes.OverlayView.apply(this, [])
        }
        J.prototype = q.classes.OverlayView.prototype;
        var K = new J();
        K.setMap(L);
        return K
    }

    function z(ab, al, O, X) {
        var ak = false,
            af = false,
            ac = false,
            W = false,
            T = true,
            S = this,
            K = [],
            R = {},
            aa = {},
            ag = [],
            ae = [],
            L = [],
            ah = f(al),
            V, am, aj, M, N;
        Q();
        this.getById = function (an) {
            return an in aa ? ag[aa[an]] : false
        };
        this.clearById = function (ao) {
            if (ao in aa) {
                var an = aa[ao];
                if (ag[an]) {
                    ag[an].setMap(null)
                }
                delete ag[an];
                ag[an] = false;
                delete ae[an];
                ae[an] = false;
                delete L[an];
                L[an] = false;
                delete aa[ao];
                af = true
            }
        };
        this.add = function (an, ao) {
            an.id = G(an.id);
            this.clearById(an.id);
            aa[an.id] = ag.length;
            ag.push(null);
            ae.push(an);
            L.push(ao);
            af = true
        };
        this.addMarker = function (ao, an) {
            an = an || {};
            an.id = G(an.id);
            this.clearById(an.id);
            if (!an.options) {
                an.options = {}
            }
            an.options.position = ao.getPosition();
            B(ab, {
                todo: an
            }, ao, an.id);
            aa[an.id] = ag.length;
            ag.push(ao);
            ae.push(an);
            L.push(an.data || {});
            af = true
        };
        this.todo = function (an) {
            return ae[an]
        };
        this.value = function (an) {
            return L[an]
        };
        this.marker = function (an) {
            return ag[an]
        };
        this.setMarker = function (ao, an) {
            ag[ao] = an
        };
        this.store = function (an, ao, ap) {
            R[an.ref] = {
                obj: ao,
                shadow: ap
            }
        };
        this.free = function () {
            for (var an = 0; an < K.length; an++) {
                google.maps.event.removeListener(K[an])
            }
            K = [];
            i.each(R, function (ao) {
                Z(ao)
            });
            R = {};
            i.each(ae, function (ao) {
                ae[ao] = null
            });
            ae = [];
            i.each(ag, function (ao) {
                if (ag[ao]) {
                    ag[ao].setMap(null);
                    delete ag[ao]
                }
            });
            ag = [];
            i.each(L, function (ao) {
                delete L[ao]
            });
            L = [];
            aa = {}
        };
        this.filter = function (an) {
            aj = an;
            ad()
        };
        this.enable = function (an) {
            if (T != an) {
                T = an;
                ad()
            }
        };
        this.display = function (an) {
            M = an
        };
        this.error = function (an) {
            N = an
        };
        this.beginUpdate = function () {
            ak = true
        };
        this.endUpdate = function () {
            ak = false;
            if (af) {
                ad()
            }
        };

        function Q() {
            am = ah.getProjection();
            if (!am) {
                setTimeout(function () {
                    Q.apply(S, [])
                }, 25);
                return
            }
            W = true;
            K.push(google.maps.event.addListener(al, "zoom_changed", function () {
                ai()
            }));
            K.push(google.maps.event.addListener(al, "bounds_changed", function () {
                ai()
            }));
            ad()
        }

        function Z(an) {
            if (typeof R[an] === "object") {
                if (typeof (R[an].obj.setMap) === "function") {
                    R[an].obj.setMap(null)
                }
                if (typeof (R[an].obj.remove) === "function") {
                    R[an].obj.remove()
                }
                if (typeof (R[an].shadow.remove) === "function") {
                    R[an].obj.remove()
                }
                if (typeof (R[an].shadow.setMap) === "function") {
                    R[an].shadow.setMap(null)
                }
                delete R[an].obj;
                delete R[an].shadow
            } else {
                if (ag[an]) {
                    ag[an].setMap(null)
                }
            }
            delete R[an]
        }

        function J() {
            var av, au, at, aq, ar, ap, ao, an;
            if (arguments[0] instanceof google.maps.LatLng) {
                av = arguments[0].lat();
                at = arguments[0].lng();
                if (arguments[1] instanceof google.maps.LatLng) {
                    au = arguments[1].lat();
                    aq = arguments[1].lng()
                } else {
                    au = arguments[1];
                    aq = arguments[2]
                }
            } else {
                av = arguments[0];
                at = arguments[1];
                if (arguments[2] instanceof google.maps.LatLng) {
                    au = arguments[2].lat();
                    aq = arguments[2].lng()
                } else {
                    au = arguments[2];
                    aq = arguments[3]
                }
            }
            ar = Math.PI * av / 180;
            ap = Math.PI * at / 180;
            ao = Math.PI * au / 180;
            an = Math.PI * aq / 180;
            return 1000 * 6371 * Math.acos(Math.min(Math.cos(ar) * Math.cos(ao) * Math.cos(ap) * Math.cos(an) + Math.cos(ar) * Math.sin(ap) * Math.cos(ao) * Math.sin(an) + Math.sin(ar) * Math.sin(ao), 1))
        }

        function P() {
            var an = J(al.getCenter(), al.getBounds().getNorthEast()),
                ao = new google.maps.Circle({
                    center: al.getCenter(),
                    radius: 1.25 * an
                });
            return ao.getBounds()
        }

        function U() {
            var ao = {},
                an;
            for (an in R) {
                ao[an] = true
            }
            return ao
        }

        function ai() {
            clearTimeout(V);
            V = setTimeout(function () {
                ad()
            }, 25)
        }

        function Y(ao) {
            var aq = am.fromLatLngToDivPixel(ao),
                ap = am.fromDivPixelToLatLng(new google.maps.Point(aq.x + O, aq.y - O)),
                an = am.fromDivPixelToLatLng(new google.maps.Point(aq.x - O, aq.y + O));
            return new google.maps.LatLngBounds(an, ap)
        }

        function ad() {
            if (ak || ac || !W) {
                return
            }
            var az = [],
                aB = {},
                aA = al.getZoom(),
                aC = (X !== e) && (aA > X),
                at = U(),
                ar, aq, ap, aw, ax, av, au, ao = false,
                an, ay;
            af = false;
            if (aA > 3) {
                an = P();
                ao = an.getSouthWest().lng() < an.getNorthEast().lng()
            }
            i.each(ae, function (aE, aD) {
                if (!aD) {
                    return
                }
                if (ao && (!an.contains(aD.options.position))) {
                    return
                }
                if (aj && !aj(L[aE])) {
                    return
                }
                az.push(aE)
            });
            while (1) {
                ar = 0;
                while (aB[ar] && (ar < az.length)) {
                    ar++
                }
                if (ar == az.length) {
                    break
                }
                av = [];
                if (T && !aC) {
                    do {
                        au = av;
                        av = [];
                        if (au.length) {
                            aw = ax = 0;
                            for (ap = 0; ap < au.length; ap++) {
                                aw += ae[az[au[ap]]].options.position.lat();
                                ax += ae[az[au[ap]]].options.position.lng()
                            }
                            aw /= au.length;
                            ax /= au.length;
                            an = Y(new google.maps.LatLng(aw, ax))
                        } else {
                            an = Y(ae[az[ar]].options.position)
                        }
                        for (aq = ar; aq < az.length; aq++) {
                            if (aB[aq]) {
                                continue
                            }
                            if (an.contains(ae[az[aq]].options.position)) {
                                av.push(aq)
                            }
                        }
                    } while ((au.length < av.length) && (av.length > 1))
                } else {
                    for (aq = ar; aq < az.length; aq++) {
                        if (aB[aq]) {
                            continue
                        }
                        av.push(aq);
                        break
                    }
                }
                ay = {
                    latLng: an.getCenter(),
                    indexes: [],
                    ref: []
                };
                for (ap = 0; ap < av.length; ap++) {
                    aB[av[ap]] = true;
                    ay.indexes.push(az[av[ap]]);
                    ay.ref.push(az[av[ap]])
                }
                ay.ref = ay.ref.join("-");
                if (ay.ref in at) {
                    delete at[ay.ref]
                } else {
                    if (av.length === 1) {
                        R[ay.ref] = true
                    } (function (aD) {
                        setTimeout(function () {
                            M(aD)
                        }, 1)
                    })(ay)
                }
            }
            i.each(at, function (aD) {
                Z(aD)
            });
            ac = false
        }
    }

    function C(K, J) {
        this.id = function () {
            return K
        };
        this.filter = function (L) {
            J.filter(L)
        };
        this.enable = function () {
            J.enable(true)
        };
        this.disable = function () {
            J.enable(false)
        };
        this.add = function (M, L, N) {
            if (!N) {
                J.beginUpdate()
            }
            J.addMarker(M, L);
            if (!N) {
                J.endUpdate()
            }
        };
        this.getById = function (L) {
            return J.getById(L)
        };
        this.clearById = function (M, L) {
            if (!L) {
                J.beginUpdate()
            }
            J.clearById(M);
            if (!L) {
                J.endUpdate()
            }
        }
    }

    function u() {
        var L = {},
            M = {};

        function K(O) {
            if (O) {
                if (typeof O === "function") {
                    return O
                }
                O = l(O);
                return function (Q) {
                    if (Q === e) {
                        return false
                    }
                    if (typeof Q === "object") {
                        for (var P = 0; P < Q.length; P++) {
                            if (i.inArray(Q[P], O) >= 0) {
                                return true
                            }
                        }
                        return false
                    }
                    return i.inArray(Q, O) >= 0
                }
            }
        }

        function J(O) {
            return {
                id: O.id,
                name: O.name,
                object: O.obj,
                tag: O.tag,
                data: O.data
            }
        }
        this.add = function (Q, P, S, R) {
            var O = Q.todo || {},
                T = G(O.id);
            if (!L[P]) {
                L[P] = []
            }
            if (T in M) {
                this.clearById(T)
            }
            M[T] = {
                obj: S,
                sub: R,
                name: P,
                id: T,
                tag: O.tag,
                data: O.data
            };
            L[P].push(T);
            return T
        };
        this.getById = function (Q, P, O) {
            if (Q in M) {
                if (P) {
                    return M[Q].sub
                } else {
                    if (O) {
                        return J(M[Q])
                    }
                }
                return M[Q].obj
            }
            return false
        };
        this.get = function (Q, S, O, R) {
            var U, T, P = K(O);
            if (!L[Q] || !L[Q].length) {
                return null
            }
            U = L[Q].length;
            while (U) {
                U--;
                T = L[Q][S ? U : L[Q].length - U - 1];
                if (T && M[T]) {
                    if (P && !P(M[T].tag)) {
                        continue
                    }
                    return R ? J(M[T]) : M[T].obj
                }
            }
            return null
        };
        this.all = function (R, P, S) {
            var O = [],
                Q = K(P),
                T = function (W) {
                    var U, V;
                    for (U = 0; U < L[W].length; U++) {
                        V = L[W][U];
                        if (V && M[V]) {
                            if (Q && !Q(M[V].tag)) {
                                continue
                            }
                            O.push(S ? J(M[V]) : M[V].obj)
                        }
                    }
                };
            if (R in L) {
                T(R)
            } else {
                if (R === e) {
                    for (R in L) {
                        T(R)
                    }
                }
            }
            return O
        };

        function N(O) {
            if (typeof (O.setMap) === "function") {
                O.setMap(null)
            }
            if (typeof (O.remove) === "function") {
                O.remove()
            }
            if (typeof (O.free) === "function") {
                O.free()
            }
            O = null
        }
        this.rm = function (R, P, Q) {
            var O, S;
            if (!L[R]) {
                return false
            }
            if (P) {
                if (Q) {
                    for (O = L[R].length - 1; O >= 0; O--) {
                        S = L[R][O];
                        if (P(M[S].tag)) {
                            break
                        }
                    }
                } else {
                    for (O = 0; O < L[R].length; O++) {
                        S = L[R][O];
                        if (P(M[S].tag)) {
                            break
                        }
                    }
                }
            } else {
                O = Q ? L[R].length - 1 : 0
            }
            if (!(O in L[R])) {
                return false
            }
            return this.clearById(L[R][O], O)
        };
        this.clearById = function (R, O) {
            if (R in M) {
                var Q, P = M[R].name;
                for (Q = 0; O === e && Q < L[P].length; Q++) {
                    if (R === L[P][Q]) {
                        O = Q
                    }
                }
                N(M[R].obj);
                if (M[R].sub) {
                    N(M[R].sub)
                }
                delete M[R];
                L[P].splice(O, 1);
                return true
            }
            return false
        };
        this.objGetById = function (Q) {
            var P;
            if (L.clusterer) {
                for (var O in L.clusterer) {
                    if ((P = M[L.clusterer[O]].obj.getById(Q)) !== false) {
                        return P
                    }
                }
            }
            return false
        };
        this.objClearById = function (P) {
            if (L.clusterer) {
                for (var O in L.clusterer) {
                    if (M[L.clusterer[O]].obj.clearById(P)) {
                        return true
                    }
                }
            }
            return null
        };
        this.clear = function (U, T, V, O) {
            var Q, S, R, P = K(O);
            if (!U || !U.length) {
                U = [];
                for (Q in L) {
                    U.push(Q)
                }
            } else {
                U = l(U)
            }
            for (S = 0; S < U.length; S++) {
                if (U[S]) {
                    R = U[S];
                    if (!L[R]) {
                        continue
                    }
                    if (T) {
                        this.rm(R, P, true)
                    } else {
                        if (V) {
                            this.rm(R, P, false)
                        } else {
                            while (this.rm(R, P, false)) { }
                        }
                    }
                }
            }
        }
    }
    var c = {},
        t = new E();

    function p() {
        if (!c.geocoder) {
            c.geocoder = new google.maps.Geocoder()
        }
        return c.geocoder
    }

    function r() {
        if (!c.directionsService) {
            c.directionsService = new google.maps.DirectionsService()
        }
        return c.directionsService
    }

    function s() {
        if (!c.elevationService) {
            c.elevationService = new google.maps.ElevationService()
        }
        return c.elevationService
    }

    function h() {
        if (!c.maxZoomService) {
            c.maxZoomService = new google.maps.MaxZoomService()
        }
        return c.maxZoomService
    }

    function b() {
        if (!c.distanceMatrixService) {
            c.distanceMatrixService = new google.maps.DistanceMatrixService()
        }
        return c.distanceMatrixService
    }

    function y() {
        if (q.verbose) {
            var J, K = [];
            if (window.console && (typeof console.error === "function")) {
                for (J = 0; J < arguments.length; J++) {
                    K.push(arguments[J])
                }
                console.error.apply(console, K)
            } else {
                K = "";
                for (J = 0; J < arguments.length; J++) {
                    K += arguments[J].toString() + " "
                }
                alert(K)
            }
        }
    }

    function j(J) {
        return (typeof (J) === "number" || typeof (J) === "string") && J !== "" && !isNaN(J)
    }

    function l(L) {
        var K, J = [];
        if (L !== e) {
            if (typeof (L) === "object") {
                if (typeof (L.length) === "number") {
                    J = L
                } else {
                    for (K in L) {
                        J.push(L[K])
                    }
                }
            } else {
                J.push(L)
            }
        }
        return J
    }

    function k(K, M, J) {
        var L = M ? K : null;
        if (!K || (typeof K === "string")) {
            return L
        }
        if (K.latLng) {
            return k(K.latLng)
        }
        if (K instanceof google.maps.LatLng) {
            return K
        } else {
            if (j(K.lat)) {
                return new google.maps.LatLng(K.lat, K.lng)
            } else {
                if (!J && i.isArray(K)) {
                    if (!j(K[0]) || !j(K[1])) {
                        return L
                    }
                    return new google.maps.LatLng(K[0], K[1])
                }
            }
        }
        return L
    }

    function n(K) {
        var L, J;
        if (!K || K instanceof google.maps.LatLngBounds) {
            return K || null
        }
        if (i.isArray(K)) {
            if (K.length == 2) {
                L = k(K[0]);
                J = k(K[1])
            } else {
                if (K.length == 4) {
                    L = k([K[0], K[1]]);
                    J = k([K[2], K[3]])
                }
            }
        } else {
            if (("ne" in K) && ("sw" in K)) {
                L = k(K.ne);
                J = k(K.sw)
            } else {
                if (("n" in K) && ("e" in K) && ("s" in K) && ("w" in K)) {
                    L = k([K.n, K.e]);
                    J = k([K.s, K.w])
                }
            }
        }
        if (L && J) {
            return new google.maps.LatLngBounds(J, L)
        }
        return null
    }

    function H(R, J, M, Q, N) {
        var L = M ? k(Q.todo, false, true) : false,
            P = L ? {
                latLng: L
            } : (Q.todo.address ? (typeof (Q.todo.address) === "string" ? {
                address: Q.todo.address
            } : Q.todo.address) : false),
            K = P ? t.get(P) : false,
            O = this;
        if (P) {
            N = N || 0;
            if (K) {
                Q.latLng = K.results[0].geometry.location;
                Q.results = K.results;
                Q.status = K.status;
                J.apply(R, [Q])
            } else {
                if (P.location) {
                    P.location = k(P.location)
                }
                if (P.bounds) {
                    P.bounds = n(P.bounds)
                }
                p().geocode(P, function (T, S) {
                    if (S === google.maps.GeocoderStatus.OK) {
                        t.store(P, {
                            results: T,
                            status: S
                        });
                        Q.latLng = T[0].geometry.location;
                        Q.results = T;
                        Q.status = S;
                        J.apply(R, [Q])
                    } else {
                        if ((S === google.maps.GeocoderStatus.OVER_QUERY_LIMIT) && (N < q.queryLimit.attempt)) {
                            setTimeout(function () {
                                H.apply(O, [R, J, M, Q, N + 1])
                            }, q.queryLimit.delay + Math.floor(Math.random() * q.queryLimit.random))
                        } else {
                            y("geocode failed", S, P);
                            Q.latLng = Q.results = false;
                            Q.status = S;
                            J.apply(R, [Q])
                        }
                    }
                })
            }
        } else {
            Q.latLng = k(Q.todo, false, true);
            J.apply(R, [Q])
        }
    }

    function F(O, J, P, K) {
        var M = this,
            L = -1;

        function N() {
            do {
                L++
            } while ((L < O.length) && !("address" in O[L]));
            if (L >= O.length) {
                P.apply(J, [K]);
                return
            }
            H(M, function (Q) {
                delete Q.todo;
                i.extend(O[L], Q);
                N.apply(M, [])
            }, true, {
                todo: O[L]
            })
        }
        N()
    }

    function D(J, M, K) {
        var L = false;
        if (navigator && navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (N) {
                if (L) {
                    return
                }
                L = true;
                K.latLng = new google.maps.LatLng(N.coords.latitude, N.coords.longitude);
                M.apply(J, [K])
            }, function () {
                if (L) {
                    return
                }
                L = true;
                K.latLng = false;
                M.apply(J, [K])
            }, K.opts.getCurrentPosition)
        } else {
            K.latLng = false;
            M.apply(J, [K])
        }
    }

    function A(R) {
        var Q = this,
            S = new w(),
            T = new u(),
            L = null,
            N;
        this._plan = function (X) {
            for (var W = 0; W < X.length; W++) {
                S.add(new a(Q, P, X[W]))
            }
            O()
        };

        function O() {
            if (!N && (N = S.get())) {
                N.run()
            }
        }

        function P() {
            N = null;
            S.ack();
            O.call(Q)
        }

        function V(W) {
            var X, Y = [];
            for (X = 1; X < arguments.length; X++) {
                Y.push(arguments[X])
            }
            if (typeof W.todo.callback === "function") {
                W.todo.callback.apply(R, Y)
            } else {
                if (typeof W.todo.callback === "object") {
                    i.each(W.todo.callback, function (aa, Z) {
                        if (typeof Z === "function") {
                            Z.apply(R, Y)
                        }
                    })
                }
            }
        }

        function M(W, X, Y) {
            if (Y) {
                B(R, W, X, Y)
            }
            V(W, X);
            N.ack(X)
        }

        function J(Y, W) {
            W = W || {};
            if (L) {
                if (W.todo && W.todo.options) {
                    L.setOptions(W.todo.options)
                }
            } else {
                var X = W.opts || i.extend(true, {}, q.map, W.todo && W.todo.options ? W.todo.options : {});
                X.center = Y || k(X.center);
                L = new q.classes.Map(R.get(0), X)
            }
        }
        this.map = function (W) {
            J(W.latLng, W);
            B(R, W, L);
            M(W, L)
        };
        this.destroy = function (W) {
            T.clear();
            R.empty();
            if (L) {
                L = null
            }
            M(W, true)
        };
        this.infowindow = function (X) {
            var Y = [],
                W = "values" in X.todo;
            if (!W) {
                if (X.latLng) {
                    X.opts.position = X.latLng
                } else {
                    if (X.opts.position) {
                        X.opts.position = k(X.opts.position)
                    }
                }
                X.todo.values = [{
                    options: X.opts
                }]
            }
            i.each(X.todo.values, function (aa, ab) {
                var ad, ac, Z = v(X, ab);
                if (!L) {
                    J(Z.options.position)
                }
                ac = new q.classes.InfoWindow(Z.options);
                if (ac && ((Z.open === e) || Z.open)) {
                    if (W) {
                        ac.open(L, Z.anchor ? Z.anchor : e)
                    } else {
                        ac.open(L, Z.anchor ? Z.anchor : (X.latLng ? e : (X.session.marker ? X.session.marker : e)))
                    }
                }
                Y.push(ac);
                ad = T.add({
                    todo: Z
                }, "infowindow", ac);
                B(R, {
                    todo: Z
                }, ac, ad)
            });
            M(X, W ? Y : Y[0])
        };
        this.circle = function (X) {
            var Y = [],
                W = "values" in X.todo;
            if (!W) {
                X.opts.center = X.latLng || k(X.opts.center);
                X.todo.values = [{
                    options: X.opts
                }]
            }
            if (!X.todo.values.length) {
                M(X, false);
                return
            }
            i.each(X.todo.values, function (aa, ab) {
                var ad, ac, Z = v(X, ab);
                Z.options.center = Z.options.center ? k(Z.options.center) : k(ab);
                if (!L) {
                    J(Z.options.center)
                }
                Z.options.map = L;
                ac = new q.classes.Circle(Z.options);
                Y.push(ac);
                ad = T.add({
                    todo: Z
                }, "circle", ac);
                B(R, {
                    todo: Z
                }, ac, ad)
            });
            M(X, W ? Y : Y[0])
        };
        this.overlay = function (Y, X) {
            var aa, Z, W = i(document.createElement("div")).css("border", "none").css("borderWidth", "0px").css("position", "absolute");
            W.append(Y.opts.content);
            I.prototype = new q.classes.OverlayView();
            Z = new I(L, Y.opts, Y.latLng, W);
            W = null;
            if (X) {
                return Z
            }
            aa = T.add(Y, "overlay", Z);
            M(Y, Z, aa)
        };
        this.getaddress = function (W) {
            V(W, W.results, W.status);
            N.ack()
        };
        this.getlatlng = function (W) {
            V(W, W.results, W.status);
            N.ack()
        };
        this.getmaxzoom = function (W) {
            h().getMaxZoomAtLatLng(W.latLng, function (X) {
                V(W, X.status === google.maps.MaxZoomStatus.OK ? X.zoom : false, status);
                N.ack()
            })
        };
        this.getelevation = function (X) {
            var Y, W = [],
                Z = function (ab, aa) {
                    V(X, aa === google.maps.ElevationStatus.OK ? ab : false, aa);
                    N.ack()
                };
            if (X.latLng) {
                W.push(X.latLng)
            } else {
                W = l(X.todo.locations || []);
                for (Y = 0; Y < W.length; Y++) {
                    W[Y] = k(W[Y])
                }
            }
            if (W.length) {
                s().getElevationForLocations({
                    locations: W
                }, Z)
            } else {
                if (X.todo.path && X.todo.path.length) {
                    for (Y = 0; Y < X.todo.path.length; Y++) {
                        W.push(k(X.todo.path[Y]))
                    }
                }
                if (W.length) {
                    s().getElevationAlongPath({
                        path: W,
                        samples: X.todo.samples
                    }, Z)
                } else {
                    N.ack()
                }
            }
        };
        this.defaults = function (W) {
            i.each(W.todo, function (X, Y) {
                if (typeof q[X] === "object") {
                    q[X] = i.extend({}, q[X], Y)
                } else {
                    q[X] = Y
                }
            });
            N.ack(true)
        };
        this.rectangle = function (X) {
            var Y = [],
                W = "values" in X.todo;
            if (!W) {
                X.todo.values = [{
                    options: X.opts
                }]
            }
            if (!X.todo.values.length) {
                M(X, false);
                return
            }
            i.each(X.todo.values, function (aa, ab) {
                var ad, ac, Z = v(X, ab);
                Z.options.bounds = Z.options.bounds ? n(Z.options.bounds) : n(ab);
                if (!L) {
                    J(Z.options.bounds.getCenter())
                }
                Z.options.map = L;
                ac = new q.classes.Rectangle(Z.options);
                Y.push(ac);
                ad = T.add({
                    todo: Z
                }, "rectangle", ac);
                B(R, {
                    todo: Z
                }, ac, ad)
            });
            M(X, W ? Y : Y[0])
        };

        function K(X, Y, Z) {
            var aa = [],
                W = "values" in X.todo;
            if (!W) {
                X.todo.values = [{
                    options: X.opts
                }]
            }
            if (!X.todo.values.length) {
                M(X, false);
                return
            }
            J();
            i.each(X.todo.values, function (ad, af) {
                var ah, ae, ac, ag, ab = v(X, af);
                if (ab.options[Z]) {
                    if (ab.options[Z][0][0] && i.isArray(ab.options[Z][0][0])) {
                        for (ae = 0; ae < ab.options[Z].length; ae++) {
                            for (ac = 0; ac < ab.options[Z][ae].length; ac++) {
                                ab.options[Z][ae][ac] = k(ab.options[Z][ae][ac])
                            }
                        }
                    } else {
                        for (ae = 0; ae < ab.options[Z].length; ae++) {
                            ab.options[Z][ae] = k(ab.options[Z][ae])
                        }
                    }
                }
                ab.options.map = L;
                ag = new google.maps[Y](ab.options);
                aa.push(ag);
                ah = T.add({
                    todo: ab
                }, Y.toLowerCase(), ag);
                B(R, {
                    todo: ab
                }, ag, ah)
            });
            M(X, W ? aa : aa[0])
        }
        this.polyline = function (W) {
            K(W, "Polyline", "path")
        };
        this.polygon = function (W) {
            K(W, "Polygon", "paths")
        };
        this.trafficlayer = function (W) {
            J();
            var X = T.get("trafficlayer");
            if (!X) {
                X = new q.classes.TrafficLayer();
                X.setMap(L);
                T.add(W, "trafficlayer", X)
            }
            M(W, X)
        };
        this.bicyclinglayer = function (W) {
            J();
            var X = T.get("bicyclinglayer");
            if (!X) {
                X = new q.classes.BicyclingLayer();
                X.setMap(L);
                T.add(W, "bicyclinglayer", X)
            }
            M(W, X)
        };
        this.groundoverlay = function (W) {
            W.opts.bounds = n(W.opts.bounds);
            if (W.opts.bounds) {
                J(W.opts.bounds.getCenter())
            }
            var Y, X = new q.classes.GroundOverlay(W.opts.url, W.opts.bounds, W.opts.opts);
            X.setMap(L);
            Y = T.add(W, "groundoverlay", X);
            M(W, X, Y)
        };
        this.streetviewpanorama = function (W) {
            if (!W.opts.opts) {
                W.opts.opts = {}
            }
            if (W.latLng) {
                W.opts.opts.position = W.latLng
            } else {
                if (W.opts.opts.position) {
                    W.opts.opts.position = k(W.opts.opts.position)
                }
            }
            if (W.todo.divId) {
                W.opts.container = document.getElementById(W.todo.divId)
            } else {
                if (W.opts.container) {
                    W.opts.container = i(W.opts.container).get(0)
                }
            }
            var Y, X = new q.classes.StreetViewPanorama(W.opts.container, W.opts.opts);
            if (X) {
                L.setStreetView(X)
            }
            Y = T.add(W, "streetviewpanorama", X);
            M(W, X, Y)
        };
        this.kmllayer = function (X) {
            var Y = [],
                W = "values" in X.todo;
            if (!W) {
                X.todo.values = [{
                    options: X.opts
                }]
            }
            if (!X.todo.values.length) {
                M(X, false);
                return
            }
            i.each(X.todo.values, function (aa, ab) {
                var ad, ac, Z = v(X, ab);
                if (!L) {
                    J()
                }
                Z.options.opts.map = L;
                ac = new q.classes.KmlLayer(Z.options.url, Z.options.opts);
                Y.push(ac);
                ad = T.add({
                    todo: Z
                }, "kmllayer", ac);
                B(R, {
                    todo: Z
                }, ac, ad)
            });
            M(X, W ? Y : Y[0])
        };
        this.panel = function (Z) {
            J();
            var ab, W = 0,
                aa = 0,
                Y, X = i(document.createElement("div"));
            X.css("position", "absolute").css("z-index", "1000");
            if (Z.opts.content) {
                Y = i(Z.opts.content);
                if (Z.opts.left !== e) {
                    W = Z.opts.left
                } else {
                    if (Z.opts.right !== e) {
                        W = R.width() - Y.width() - Z.opts.right
                    } else {
                        if (Z.opts.center) {
                            W = (R.width() - Y.width()) / 2
                        }
                    }
                }
                if (Z.opts.top !== e) {
                    aa = Z.opts.top
                } else {
                    if (Z.opts.bottom !== e) {
                        aa = R.height() - Y.height() - Z.opts.bottom
                    } else {
                        if (Z.opts.middle) {
                            aa = (R.height() - Y.height()) / 2
                        }
                    }
                }
                X.css("top", aa + "px").css("left", W + "px").append(Y)
            }
            R.first().prepend(X);
            ab = T.add(Z, "panel", X);
            M(Z, X, ab);
            X = null
        };

        function U(Y) {
            var ac = new z(R, L, Y.radius, Y.maxZoom),
                W = {},
                Z = {},
                ab = /^[0-9]+$/,
                aa, X;
            for (X in Y) {
                if (ab.test(X)) {
                    Z[X] = Y[X];
                    Z[X].width = Z[X].width || 0;
                    Z[X].height = Z[X].height || 0
                } else {
                    W[X] = Y[X]
                }
            }
            if (W.calculator) {
                aa = function (ad) {
                    var ae = [];
                    i.each(ad, function (ag, af) {
                        ae.push(ac.value(af))
                    });
                    return W.calculator.apply(R, [ae])
                }
            } else {
                aa = function (ad) {
                    return ad.length
                }
            }
            ac.error(function () {
                y.apply(Q, arguments)
            });
            ac.display(function (ad) {
                var ae, ag, ak = 0,
                    aj, ah, ai, af = aa(ad.indexes);
                if (af > 1) {
                    for (ae in Z) {
                        ae = 1 * ae;
                        if (ae > ak && ae <= af) {
                            ak = ae
                        }
                    }
                    ag = Z[ak]
                }
                if (ag) {
                    ai = ag.offset || [-ag.width / 2, -ag.height / 2];
                    aj = i.extend({}, W);
                    aj.options = i.extend({
                        pane: "overlayLayer",
                        content: ag.content ? ag.content.replace("CLUSTER_COUNT", af) : "",
                        offset: {
                            x: ("x" in ai ? ai.x : ai[0]) || 0,
                            y: ("y" in ai ? ai.y : ai[1]) || 0
                        }
                    }, W.options || {});
                    ah = Q.overlay({
                        todo: aj,
                        opts: aj.options,
                        latLng: k(ad)
                    }, true);
                    aj.options.pane = "floatShadow";
                    aj.options.content = i(document.createElement("div")).width(ag.width + "px").height(ag.height + "px");
                    shadow = Q.overlay({
                        todo: aj,
                        opts: aj.options,
                        latLng: k(ad)
                    }, true);
                    W.data = {
                        latLng: k(ad),
                        markers: []
                    };
                    i.each(ad.indexes, function (am, al) {
                        W.data.markers.push(ac.value(al));
                        if (ac.marker(al)) {
                            ac.marker(al).setMap(null)
                        }
                    });
                    B(R, {
                        todo: W
                    }, shadow, e, {
                        main: ah,
                        shadow: shadow
                    });
                    ac.store(ad, ah, shadow)
                } else {
                    i.each(ad.indexes, function (ao, an) {
                        if (ac.marker(an)) {
                            ac.marker(an).setMap(L)
                        } else {
                            var al = ac.todo(an),
                                am = new q.classes.Marker(al.options);
                            ac.setMarker(an, am);
                            B(R, {
                                todo: al
                            }, am, al.id)
                        }
                    })
                }
            });
            return ac
        }
        this.marker = function (Y) {
            var W = "values" in Y.todo,
                ab = !L;
            if (!W) {
                Y.opts.position = Y.latLng || k(Y.opts.position);
                Y.todo.values = [{
                    options: Y.opts
                }]
            }
            if (!Y.todo.values.length) {
                M(Y, false);
                return
            }
            if (ab) {
                J()
            }
            if (Y.todo.cluster && !L.getBounds()) {
                google.maps.event.addListenerOnce(L, "bounds_changed", function () {
                    Q.marker.apply(Q, [Y])
                });
                return
            }
            if (Y.todo.cluster) {
                var X, Z;
                if (Y.todo.cluster instanceof C) {
                    X = Y.todo.cluster;
                    Z = T.getById(X.id(), true)
                } else {
                    Z = U(Y.todo.cluster);
                    X = new C(G(Y.todo.id, true), Z);
                    T.add(Y, "clusterer", X, Z)
                }
                Z.beginUpdate();
                i.each(Y.todo.values, function (ad, ae) {
                    var ac = v(Y, ae);
                    ac.options.position = ac.options.position ? k(ac.options.position) : k(ae);
                    ac.options.map = L;
                    if (ab) {
                        L.setCenter(ac.options.position);
                        ab = false
                    }
                    Z.add(ac, ae)
                });
                Z.endUpdate();
                M(Y, X)
            } else {
                var aa = [];
                i.each(Y.todo.values, function (ad, ae) {
                    var ag, af, ac = v(Y, ae);
                    ac.options.position = ac.options.position ? k(ac.options.position) : k(ae);
                    ac.options.map = L;
                    if (ab) {
                        L.setCenter(ac.options.position);
                        ab = false
                    }
                    af = new q.classes.Marker(ac.options);
                    aa.push(af);
                    ag = T.add({
                        todo: ac
                    }, "marker", af);
                    B(R, {
                        todo: ac
                    }, af, ag)
                });
                M(Y, W ? aa : aa[0])
            }
        };
        this.getroute = function (W) {
            W.opts.origin = k(W.opts.origin, true);
            W.opts.destination = k(W.opts.destination, true);
            r().route(W.opts, function (Y, X) {
                V(W, X == google.maps.DirectionsStatus.OK ? Y : false, X);
                N.ack()
            })
        };
        this.directionsrenderer = function (W) {
            W.opts.map = L;
            var Y, X = new google.maps.DirectionsRenderer(W.opts);
            if (W.todo.divId) {
                X.setPanel(document.getElementById(W.todo.divId))
            } else {
                if (W.todo.container) {
                    X.setPanel(i(W.todo.container).get(0))
                }
            }
            Y = T.add(W, "directionrenderer", X);
            M(W, X, Y)
        };
        this.getgeoloc = function (W) {
            M(W, W.latLng)
        };
        this.styledmaptype = function (W) {
            J();
            var X = new q.classes.StyledMapType(W.todo.styles, W.opts);
            L.mapTypes.set(W.todo.id, X);
            M(W, X)
        };
        this.imagemaptype = function (W) {
            J();
            var X = new q.classes.ImageMapType(W.opts);
            L.mapTypes.set(W.todo.id, X);
            M(W, X)
        };
        this.autofit = function (W) {
            var X = new google.maps.LatLngBounds();
            i.each(T.all(), function (Y, Z) {
                if (Z.getPosition) {
                    X.extend(Z.getPosition())
                } else {
                    if (Z.getBounds) {
                        X.extend(Z.getBounds().getNorthEast());
                        X.extend(Z.getBounds().getSouthWest())
                    } else {
                        if (Z.getPaths) {
                            Z.getPaths().forEach(function (aa) {
                                aa.forEach(function (ab) {
                                    X.extend(ab)
                                })
                            })
                        } else {
                            if (Z.getPath) {
                                Z.getPath().forEach(function (aa) {
                                    X.extend(aa);
                                    ""
                                })
                            } else {
                                if (Z.getCenter) {
                                    X.extend(Z.getCenter())
                                }
                            }
                        }
                    }
                }
            });
            if (!X.isEmpty() && (!L.getBounds() || !L.getBounds().equals(X))) {
                if ("maxZoom" in W.todo) {
                    google.maps.event.addListenerOnce(L, "bounds_changed", function () {
                        if (this.getZoom() > W.todo.maxZoom) {
                            this.setZoom(W.todo.maxZoom)
                        }
                    })
                }
                L.fitBounds(X)
            }
            M(W, true)
        };
        this.clear = function (W) {
            if (typeof W.todo === "string") {
                if (T.clearById(W.todo) || T.objClearById(W.todo)) {
                    M(W, true);
                    return
                }
                W.todo = {
                    name: W.todo
                }
            }
            if (W.todo.id) {
                i.each(l(W.todo.id), function (X, Y) {
                    T.clearById(Y)
                })
            } else {
                T.clear(l(W.todo.name), W.todo.last, W.todo.first, W.todo.tag)
            }
            M(W, true)
        };
        this.exec = function (W) {
            var X = this;
            i.each(l(W.todo.func), function (Y, Z) {
                i.each(X.get(W.todo, true, W.todo.hasOwnProperty("full") ? W.todo.full : true), function (aa, ab) {
                    Z.call(R, ab)
                })
            });
            M(W, true)
        };
        this.get = function (Y, ab, aa) {
            var X, Z, W = ab ? Y : Y.todo;
            if (!ab) {
                aa = W.full
            }
            if (typeof W === "string") {
                Z = T.getById(W, false, aa) || T.objGetById(W);
                if (Z === false) {
                    X = W;
                    W = {}
                }
            } else {
                X = W.name
            }
            if (X === "map") {
                Z = L
            }
            if (!Z) {
                Z = [];
                if (W.id) {
                    i.each(l(W.id), function (ac, ad) {
                        Z.push(T.getById(ad, false, aa) || T.objGetById(ad))
                    });
                    if (!i.isArray(W.id)) {
                        Z = Z[0]
                    }
                } else {
                    i.each(X ? l(X) : [e], function (ad, ae) {
                        var ac;
                        if (W.first) {
                            ac = T.get(ae, false, W.tag, aa);
                            if (ac) {
                                Z.push(ac)
                            }
                        } else {
                            if (W.all) {
                                i.each(T.all(ae, W.tag, aa), function (ag, af) {
                                    Z.push(af)
                                })
                            } else {
                                ac = T.get(ae, true, W.tag, aa);
                                if (ac) {
                                    Z.push(ac)
                                }
                            }
                        }
                    });
                    if (!W.all && !i.isArray(X)) {
                        Z = Z[0]
                    }
                }
            }
            Z = i.isArray(Z) || !W.all ? Z : [Z];
            if (ab) {
                return Z
            } else {
                M(Y, Z)
            }
        };
        this.getdistance = function (W) {
            var X;
            W.opts.origins = l(W.opts.origins);
            for (X = 0; X < W.opts.origins.length; X++) {
                W.opts.origins[X] = k(W.opts.origins[X], true)
            }
            W.opts.destinations = l(W.opts.destinations);
            for (X = 0; X < W.opts.destinations.length; X++) {
                W.opts.destinations[X] = k(W.opts.destinations[X], true)
            }
            b().getDistanceMatrix(W.opts, function (Z, Y) {
                V(W, Y === google.maps.DistanceMatrixStatus.OK ? Z : false, Y);
                N.ack()
            })
        };
        this.trigger = function (X) {
            if (typeof X.todo === "string") {
                google.maps.event.trigger(L, X.todo)
            } else {
                var W = [L, X.todo.eventName];
                if (X.todo.var_args) {
                    i.each(X.todo.var_args, function (Z, Y) {
                        W.push(Y)
                    })
                }
                google.maps.event.trigger.apply(google.maps.event, W)
            }
            V(X);
            N.ack()
        }
    }

    function o(K) {
        var J;
        if (!typeof K === "object" || !K.hasOwnProperty("get")) {
            return false
        }
        for (J in K) {
            if (J !== "get") {
                return false
            }
        }
        return !K.get.hasOwnProperty("callback")
    }
    i.fn.gmap3 = function () {
        var K, M = [],
            L = true,
            J = [];
        m();
        for (K = 0; K < arguments.length; K++) {
            if (arguments[K]) {
                M.push(arguments[K])
            }
        }
        if (!M.length) {
            M.push("map")
        }
        i.each(this, function () {
            var N = i(this),
                O = N.data("gmap3");
            L = false;
            if (!O) {
                O = new A(N);
                N.data("gmap3", O)
            }
            if (M.length === 1 && (M[0] === "get" || o(M[0]))) {
                J.push(O.get(M[0] === "get" ? "map" : M[0].get, true))
            } else {
                O._plan(M)
            }
        });
        if (J.length) {
            if (J.length === 1) {
                return J[0]
            } else {
                return J
            }
        }
        return this
    }
})(jQuery);
(function(e, t, n) {
    var r = function(r, i) {
        function b(e) {
            e.find(u + ", ." + l).find(a + ":not([data-popupTrigger])").each(function() {
                var e = t(this);
                var r = e.find("div[data-popup]").eq(0);
                e.attr("data-popupTrigger", "yes");
                var i = "mfp-image";
                if (r.data("type") == "iframe") {
                    i = "mfp-iframe"
                } else if (r.data("type") == "inline") {
                    i = "mfp-inline"
                } else if (r.data("type") == "ajax") {
                    i = "mfp-ajax"
                }
                var s = e.find(".mb-open-popup").addBack(".mb-open-popup");
                s.attr("data-mfp-src", r.data("popup")).addClass(i);
                if (r.attr("title") != n) {
                    s.attr("mfp-title", r.attr("title"))
                }
            })
        }

        function w(e, r) {
            e.find(u).find(a + ":not([data-imageconverted])").each(function() {
                var i = t(this);
                var s = i.find("div[data-thumbnail]").eq(0);
                var o = i.find("div[data-popup]").eq(0);
                var u = s.data("thumbnail");
                if (s[0] == n) {
                    s = o;
                    u = o.data("popup")
                }
                if (r == false && e.data("settings").waitForAllThumbsNoMatterWhat == false) {
                    if (s.data("width") == n && s.data("height") == n) {} else {
                        return
                    }
                }
                i.attr("data-imageconverted", "yes");
                var a = s.attr("title");
                if (a == n) {
                    a = u
                }
                var f = t('<img title="' + a + '" src="' + u + '" />');
                if (r == true) {
                    f.attr("data-dont-wait-for-me", "yes");
                    s.addClass("image-with-dimensions");
                    if (e.data("settings").waitUntilThumbLoads) {
                        f.hide()
                    }
                }
                s.addClass("media-box-thumbnail-container").prepend(f)
            });
            if (r == true) {
                function i(e) {
                    var r = t(e.img);
                    var i = r.parents(".image-with-dimensions");
                    if (i[0] == n) {
                        return
                    }
                    if (e.isLoaded) {
                        r.fadeIn(400, function() {
                            i.removeClass("image-with-dimensions")
                        })
                    } else {
                        i.removeClass("image-with-dimensions");
                        r.hide();
                        i.addClass("broken-image-here")
                    }
                }
                e.find(".image-with-dimensions").imagesLoadedMB().always(function(e) {
                    for (index in e.images) {
                        var t = e.images[index];
                        i(t)
                    }
                }).progress(function(e, t) {
                    i(t)
                })
            }
        }

        function E(e) {
            e.find(u).each(function() {
                var e = t(this);
                var r = e.find(a);
                var i = r.find("div[data-thumbnail]").eq(0);
                var s = r.find("div[data-popup]").eq(0);
                if (i[0] == n) {
                    i = s
                }
                var o = e.css("display");
                if (o == "none") {
                    e.css("margin-top", 99999999999999).show()
                }
                r.width(i.width());
                r.height(i.height());
                if (o == "none") {
                    e.css("margin-top", 0).hide()
                }
            })
        }

        function S(e) {
            e.find(u).find(a).each(function() {
                var r = t(this);
                var i = r.find("div[data-thumbnail]").eq(0);
                var s = r.find("div[data-popup]").eq(0);
                if (i[0] == n) {
                    i = s
                }
                var o = parseFloat(i.data("width"));
                var a = parseFloat(i.data("height"));
                var f = r.parents(u).width() - e.data("settings").horizontalSpaceBetweenBoxes;
                var l = a * f / o;
                i.css("width", f);
                if (i.data("width") != n || i.data("height") != n) {
                    i.css("height", Math.floor(l))
                }
            })
        }

        function x(e, r, i) {
            var s = e.find(u);
            var o;
            var a = false;
            if (r == "auto") {
                if (a) {
                    o = 100 / i + "%"
                } else {
                    o = Math.floor((e.width() - 1) / i)
                }
            } else {
                o = r
            }
            e.find(".media-boxes-grid-sizer").css("width", o);
            s.each(function(e) {
                var r = t(this);
                var s = r.data("columns");
                if (s != n && parseInt(i) >= parseInt(s)) {
                    if (a) {
                        r.css("width", parseFloat(100 / i) * s + "%")
                    } else {
                        r.css("width", o * parseInt(s))
                    }
                } else {
                    if (a) {
                        r.css("width", 100 / i + "%")
                    } else {
                        r.css("width", o)
                    }
                }
            })
        }

        function T() {
            var t = e,
                n = "inner";
            if (!("innerWidth" in e)) {
                n = "client";
                t = document.documentElement || document.body
            }
            return {
                width: t[n + "Width"],
                height: t[n + "Height"]
            }
        }

        function N(e) {
            var t = false;
            for (var n in e.data("settings").resolutions) {
                var r = e.data("settings").resolutions[n];
                if (r.maxWidth >= T().width) {
                    x(e, r.columnWidth, r.columns);
                    t = true;
                    break
                }
            }
            if (t == false) {
                x(e, e.data("settings").columnWidth, e.data("settings").columns)
            }
        }

        function C(e) {
            var n = t('<div class="media-box-container"></div').css({
                "margin-left": e.data("settings").horizontalSpaceBetweenBoxes,
                "margin-bottom": e.data("settings").verticalSpaceBetweenBoxes
            });
            var r = e.find(u + ":not([data-wrapper-added])").attr("data-wrapper-added", "yes");
            r.wrapInner(n)
        }

        function k(e) {
            if (e.data("settings").thumbnailOverlay == false) return;
            var r = e.find(u + ":not([data-set-overlay-for-hover-effect])").attr("data-set-overlay-for-hover-effect", "yes");
            r.find(".thumbnail-overlay").wrapInner("<div class='aligment'><div class='aligment'></div></div>");
            r.each(function() {
                var r = t(this);
                var i = r.find(a);
                var s = e.data("settings").overlayEffect;
                if (i.data("overlay-effect") != n) {
                    s = i.data("overlay-effect")
                }
                if (s == "push-up" || s == "push-down" || s == "push-up-100%" || s == "push-down-100%") {
                    var o = i.find(".media-box-thumbnail-container");
                    var u = i.find(".thumbnail-overlay").css("position", "relative");
                    if (s == "push-up-100%" || s == "push-down-100%") {
                        u.outerHeight(o.outerHeight(false))
                    }
                    var f = u.outerHeight(false);
                    var l = t('<div class="wrapper-for-some-effects"></div');
                    if (s == "push-up" || s == "push-up-100%") {
                        u.appendTo(i)
                    } else if (s == "push-down" || s == "push-down-100%") {
                        u.prependTo(i);
                        l.css("margin-top", -f)
                    }
                    i.wrapInner(l)
                } else if (s == "reveal-top" || s == "reveal-top-100%") {
                    r.addClass("position-reveal-effect");
                    var c = r.find(".thumbnail-overlay").css("top", 0);
                    if (s == "reveal-top-100%") {
                        c.css("height", "100%")
                    }
                } else if (s == "reveal-bottom" || s == "reveal-bottom-100%") {
                    r.addClass("position-reveal-effect").addClass("position-bottom-reveal-effect");
                    var c = r.find(".thumbnail-overlay").css("bottom", 0);
                    if (s == "reveal-bottom-100%") {
                        c.css("height", "100%")
                    }
                } else if (s.substr(0, 9) == "direction") {
                    r.find(".thumbnail-overlay").css("height", "100%")
                } else if (s == "fade") {
                    var h = r.find(".thumbnail-overlay").hide();
                    h.css({
                        height: "100%",
                        top: "0",
                        left: "0"
                    });
                    h.find(".fa").css({
                        scale: 1.4
                    })
                }
            })
        }

        function L(e) {
            var r = e.find(u);
            r.each(function() {
                var r = t(this);
                var i = r.find(a);
                var s = e.data("settings").overlayEffect;
                if (i.data("overlay-effect") != n) {
                    s = i.data("overlay-effect")
                }
                if (s.substr(0, 9) == "direction") {
                    i.find(".thumbnail-overlay").hide()
                }
            });
            e.isotopeMB("layout")
        }

        function A() {
            var e = o.find(u + ", ." + l);
            var t = j();
            e.filter(t).removeClass("hidden-media-boxes-by-filter").addClass("visible-media-boxes-by-filter");
            e.not(t).addClass("hidden-media-boxes-by-filter").removeClass("visible-media-boxes-by-filter")
        }

        function O(e, t) {
            o.addClass("filtering-isotope");
            h[t] = e;
            o.isotopeMB({
                filter: M(h)
            });
            A();
            if (B().length > 0) {
                I()
            } else {
                z();
                if (P()) {}
            }
        }

        function M(e) {
            for (var t in e) {
                var r = e[t];
                if (r == n) {
                    e[t] = "*"
                }
            }
            var i = "";
            for (var t in e) {
                var r = e[t];
                if (i == "") {
                    i = t
                } else if (i.split(",").length < r.split(",").length) {
                    i = t
                }
            }
            var s = e[i];
            for (var t in e) {
                if (t == i) continue;
                var o = e[t].split(",");
                for (var u = 0; u < o.length; u++) {
                    var a = s.split(",");
                    var f = [];
                    for (var l = 0; l < a.length; l++) {
                        if (a[l] == "*" && o[u] == "*") {
                            o[u] = ""
                        } else {
                            if (o[u] == "*") {
                                o[u] = ""
                            }
                            if (a[l] == "*") {
                                a[l] = ""
                            }
                        }
                        f.push(a[l] + o[u])
                    }
                    s = f.join(",")
                }
            }
            return s
        }

        function _(e) {
            if (e == n) return;
            var r = o.find("." + f + ", ." + l);
            if (e == "") {
                r.addClass("search-match")
            } else {
                r.removeClass("search-match");
                o.find(s.searchTarget).each(function() {
                    var n = t(this);
                    var r = n.parents("." + f + ", ." + l);
                    if (n.text().toLowerCase().indexOf(e.toLowerCase()) !== -1) {
                        r.addClass("search-match")
                    }
                })
            }
            setTimeout(function() {
                O(".search-match", "search")
            }, 100)
        }

        function D(e) {
            var t = e.data("sort-ascending");
            if (t == n) {
                t = true
            }
            if (e.data("sort-toggle") && e.data("sort-toggle") == true) {
                e.data("sort-ascending", !t)
            }
            return t
        }

        function P() {
            var e = H().length;
            if (e < s.minBoxesPerFilter && F().length > 0) {
                W(s.minBoxesPerFilter - e);
                return true
            }
            return false
        }

        function H() {
            var e = o.find(u);
            var t = j();
            if (t != "*") {
                e = e.filter(t)
            }
            return e
        }

        function B() {
            var e = H().not(".media-box-loaded");
            return e
        }

        function j() {
            var e = o.data("isotopeMB").options.filter;
            if (e == "" || e == n) {
                e = "*"
            }
            return e
        }

        function F(e) {
            var t = o.find("." + l);
            var r = j();
            if (r != "*" && e == n) {
                t = t.filter(r)
            }
            return t
        }

        function I() {
            p.html(s.LoadingWord);
            p.removeClass("media-boxes-load-more");
            p.addClass("media-boxes-loading")
        }

        function R() {
            q++;
            I()
        }

        function U() {
            q--;
            if (q == 0) {
                z()
            }
        }

        function z() {
            p.removeClass("media-boxes-load-more");
            p.removeClass("media-boxes-loading");
            p.removeClass("media-boxes-no-more-entries");
            if (F().length > 0) {
                p.html(s.loadMoreWord);
                p.addClass("media-boxes-load-more")
            } else {
                p.html(s.noMoreEntriesWord);
                p.addClass("media-boxes-no-more-entries")
            }
        }

        function W(e, n) {
            if (p.hasClass("media-boxes-load-more") == false) {
                return
            }
            R();
            var r = [];
            F(n).each(function(n) {
                var i = t(this);
                if (n + 1 <= e) {
                    i.removeClass(l).addClass(f);
                    i.hide();
                    r.push(this)
                }
            });
            o.isotopeMB("insert", t(r), function() {
                U()
            })
        }

        function X(e) {
            if (e.attr("data-stop") != n) {
                e.hide();
                e.removeAttr("data-stop")
            }
        }
        var s = t.extend({}, t.fn.mediaBoxes.defaults, i);
        var o = t(r).addClass("media-boxes-container");
        var u = ".media-box";
        var a = ".media-box-image";
        var f = "media-box";
        var l = "media-box-hidden";
        var c = Modernizr.csstransitions ? "transition" : "animate";
        var h = {};
        if (s.overlayEasing == "default") {
            s.overlayEasing = c == "transition" ? "_default" : "swing"
        }
        var p = t('<div class="media-boxes-load-more media-boxes-load-more-button"></div>').insertAfter(o);
        s.resolutions.sort(function(e, t) {
            return e.maxWidth - t.maxWidth
        });
        o.data("settings", s);
        o.css({
            "margin-left": -s.horizontalSpaceBetweenBoxes
        });
        o.find(u).removeClass(f).addClass(l);
        var d = t(s.filterContainer);
        var v = d.find(s.filter).filter(".selected").attr("data-filter");
        h["filter"] = v;
        _(t(s.search).val());
        var m = t(s.sortContainer).find(s.sort).filter(".selected");
        var g = m.attr("data-sort-by");
        var y = D(m);
        o.append('<div class="media-boxes-grid-sizer"></div>');
        o.isotopeMB({
            itemSelector: u,
            filter: M(h),
            masonry: {
                columnWidth: ".media-boxes-grid-sizer"
            },
            getSortData: s.getSortData,
            sortBy: g,
            sortAscending: y
        });
        A();
        t.extend(IsotopeMB.prototype, {
            resize: function() {
                var e = t(this.element);
                N(e);
                S(e);
                E(e);
                L(e);
                if (!this.isResizeBound || !this.needsResizeLayout()) {
                    return
                }
                this.layout()
            }
        });
        t.extend(IsotopeMB.prototype, {
            _setContainerMeasure: function(e, r) {
                if (e === n) {
                    return
                }
                var i = this.size;
                if (i.isBorderBox) {
                    e += r ? i.paddingLeft + i.paddingRight + i.borderLeftWidth + i.borderRightWidth : i.paddingBottom + i.paddingTop + i.borderTopWidth + i.borderBottomWidth
                }
                e = Math.max(e, 0);
                this.element.style[r ? "width" : "height"] = e + "px";
                var s = t(this.element);
                t.waypoints("refresh");
                s.addClass("lazy-load-ready");
                s.removeClass("filtering-isotope")
            }
        });
        t.extend(IsotopeMB.prototype, {
            insert: function(e, r) {
                var i = this.addItems(e);
                if (!i.length) {
                    return
                }
                var o = t(this.element);
                var a = o.find("." + l)[0];
                var f, c;
                var h = i.length;
                for (f = 0; f < h; f++) {
                    c = i[f];
                    if (a != n) {
                        this.element.insertBefore(c.element, a)
                    } else {
                        this.element.appendChild(c.element)
                    }
                }
                var p = function() {
                    var e = this._filter(i);
                    this._noTransition(function() {
                        this.hide(e)
                    });
                    for (f = 0; f < h; f++) {
                        i[f].isLayoutInstant = true
                    }
                    this.arrange();
                    for (f = 0; f < h; f++) {
                        delete i[f].isLayoutInstant
                    }
                    this.reveal(e)
                };
                var d = function(e) {
                    var n = t(e.img);
                    var r = n.parents("div[data-thumbnail], div[data-popup]");
                    if (e.isLoaded == false) {
                        n.hide();
                        r.addClass("broken-image-here")
                    }
                };
                var v = this;
                C(o);
                N(o);
                S(o);
                b(o);
                w(o, false);
                o.find("img:not([data-dont-wait-for-me])").imagesLoadedMB().always(function() {
                    if (s.waitForAllThumbsNoMatterWhat == false) {
                        w(o, true)
                    }
                    o.find(u).addClass("media-box-loaded");
                    p.call(v);
                    E(o);
                    k(o);
                    if (typeof r === "function") {
                        r()
                    }
                    for (index in v.images) {
                        var e = v.images[index];
                        d(e)
                    }
                }).progress(function(e, t) {
                    d(t)
                })
            }
        });
        d.find(s.filter).on("click", function(e) {
            var r = t(this);
            var i = r.parents(s.filterContainer);
            i.find(s.filter).removeClass("selected");
            r.addClass("selected");
            var o = r.attr("data-filter");
            var u = "filter";
            if (i.data("id") != n) {
                u = i.data("id")
            }
            O(o, u);
            e.preventDefault()
        });
        t(s.search).on("keyup", function() {
            var e = t(this).val();
            _(e)
        });
        t(s.sortContainer).find(s.sort).on("click", function(e) {
            var n = t(this);
            n.parents(s.sortContainer).find(s.sort).removeClass("selected");
            n.addClass("selected");
            var r = n.attr("data-sort-by");
            o.isotopeMB({
                sortBy: r,
                sortAscending: D(n)
            });
            e.preventDefault()
        });
        var q = 0;
        W(s.boxesToLoadStart, true);
        p.on("click", function() {
            W(s.boxesToLoad)
        });
        if (s.lazyLoad) {
            o.waypoint(function(e) {
                if (o.hasClass("lazy-load-ready")) {
                    if (e == "down" && o.hasClass("filtering-isotope") == false) {
                        o.removeClass("lazy-load-ready");
                        W(s.boxesToLoad)
                    }
                }
            }, {
                context: e,
                continuous: true,
                enabled: true,
                horizontal: false,
                offset: "bottom-in-view",
                triggerOnce: false
            })
        }
        o.on("mouseenter.hoverdir, mouseleave.hoverdir", a, function(e) {
            if (s.thumbnailOverlay == false) return;
            var r = t(this);
            var i = s.overlayEffect;
            if (r.data("overlay-effect") != n) {
                i = r.data("overlay-effect")
            }
            var o = e.type;
            var u = r.find(".media-box-thumbnail-container");
            var a = r.find(".thumbnail-overlay");
            var f = a.outerHeight(false);
            if (i == "push-up" || i == "push-up-100%") {
                var l = r.find("div.wrapper-for-some-effects");
                if (o === "mouseenter") {
                    l.stop().show()[c]({
                        "margin-top": -f
                    }, s.overlaySpeed, s.overlayEasing)
                } else {
                    l.stop()[c]({
                        "margin-top": 0
                    }, s.overlaySpeed, s.overlayEasing)
                }
            } else if (i == "push-down" || i == "push-down-100%") {
                var l = r.find("div.wrapper-for-some-effects");
                if (o === "mouseenter") {
                    l.stop().show()[c]({
                        "margin-top": 0
                    }, s.overlaySpeed, s.overlayEasing)
                } else {
                    l.stop()[c]({
                        "margin-top": -f
                    }, s.overlaySpeed, s.overlayEasing)
                }
            } else if (i == "reveal-top" || i == "reveal-top-100%") {
                if (o === "mouseenter") {
                    u.stop().show()[c]({
                        "margin-top": f
                    }, s.overlaySpeed, s.overlayEasing)
                } else {
                    u.stop()[c]({
                        "margin-top": 0
                    }, s.overlaySpeed, s.overlayEasing)
                }
            } else if (i == "reveal-bottom" || i == "reveal-bottom-100%") {
                if (o === "mouseenter") {
                    u.stop().show()[c]({
                        "margin-top": -f
                    }, s.overlaySpeed, s.overlayEasing)
                } else {
                    u.stop()[c]({
                        "margin-top": 0
                    }, s.overlaySpeed, s.overlayEasing)
                }
            } else if (i.substr(0, 9) == "direction") {
                var h = V(r, {
                    x: e.pageX,
                    y: e.pageY
                });
                if (i == "direction-top") {
                    h = 0
                } else if (i == "direction-bottom") {
                    h = 2
                } else if (i == "direction-right") {
                    h = 1
                } else if (i == "direction-left") {
                    h = 3
                }
                var p = J(h, r);
                if (o == "mouseenter") {
                    a.css({
                        left: p.from,
                        top: p.to
                    });
                    a.stop().show().fadeTo(0, 1, function() {
                        t(this).stop()[c]({
                            left: 0,
                            top: 0
                        }, s.overlaySpeed, s.overlayEasing)
                    })
                } else {
                    if (i == "direction-aware-fade") {
                        a.fadeOut(700)
                    } else {
                        a.stop()[c]({
                            left: p.from,
                            top: p.to
                        }, s.overlaySpeed, s.overlayEasing)
                    }
                }
            } else if (i == "fade") {
                if (o == "mouseenter") {
                    a.stop().fadeOut(0);
                    a.fadeIn(s.overlaySpeed)
                } else {
                    a.stop().fadeIn(0);
                    a.fadeOut(s.overlaySpeed)
                }
                var d = a.find(".fa");
                if (o == "mouseenter") {
                    d.css({
                        scale: 1.4
                    });
                    d[c]({
                        scale: 1
                    }, 200)
                } else {
                    d.css({
                        scale: 1
                    });
                    d[c]({
                        scale: 1.4
                    }, 200)
                }
            }
        });
        var V = function(e, t) {
            var n = e.width(),
                r = e.height(),
                i = (t.x - e.offset().left - n / 2) * (n > r ? r / n : 1),
                s = (t.y - e.offset().top - r / 2) * (r > n ? n / r : 1),
                o = Math.round((Math.atan2(s, i) * (180 / Math.PI) + 180) / 90 + 3) % 4;
            return o
        };
        var J = function(e, t) {
            var n, r;
            switch (e) {
                case 0:
                    if (!s.reverse) {
                        n = 0, r = -t.height()
                    } else {
                        n = 0, r = -t.height()
                    }
                    break;
                case 1:
                    if (!s.reverse) {
                        n = t.width(), r = 0
                    } else {
                        n = -t.width(), r = 0
                    }
                    break;
                case 2:
                    if (!s.reverse) {
                        n = 0, r = t.height()
                    } else {
                        n = 0, r = -t.height()
                    }
                    break;
                case 3:
                    if (!s.reverse) {
                        n = -t.width(), r = 0
                    } else {
                        n = t.width(), r = 0
                    }
                    break
            }
            return {
                from: n,
                to: r
            }
        };
        var K = ".mb-open-popup[data-mfp-src]";
        if (s.considerFilteringInPopup) {
            K = u + ":not(.hidden-media-boxes-by-filter) .mb-open-popup[data-mfp-src], ." + l + ":not(.hidden-media-boxes-by-filter) .mb-open-popup[data-mfp-src]"
        }
        if (s.showOnlyLoadedBoxesInPopup) {
            K = u + ":visible .mb-open-popup[data-mfp-src]"
        }
        if (s.magnificPopup) {
            o.magnificPopup({
                delegate: K,
                type: "image",
                removalDelay: 200,
                closeOnContentClick: false,
                alignTop: s.alignTop,
                preload: s.preload,
                mainClass: "my-mfp-slide-bottom",
                gallery: {
                    enabled: s.gallery
                },
                closeMarkup: '<button title="%title%" class="mfp-close"></button>',
                titleSrc: "title",
                iframe: {
                    patterns: {
                        youtube: {
                            index: "youtube.com/",
                            id: "v=",
                            src: "https://www.youtube.com/embed/%id%?autoplay=1"
                        },
                        vimeo: {
                            index: "vimeo.com/",
                            id: "/",
                            src: "https://player.vimeo.com/video/%id%?autoplay=1"
                        }
                    },
                    markup: '<div class="mfp-iframe-scaler">' + '<div class="mfp-close"></div>' + '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' + '<div class="mfp-bottom-bar" style="margin-top:4px;"><div class="mfp-title"></div><div class="mfp-counter"></div></div>' + "</div>"
                },
                callbacks: {
                    change: function() {
                        var e = t(this.currItem.el);
                        setTimeout(function() {
                            if (e.attr("mfp-title") != n) {
                                t(".mfp-title").html(e.attr("mfp-title"))
                            } else {
                                t(".mfp-title").html("")
                            }
                        }, 5);
                        if (s.deepLinking) {
                            location.hash = "#!" + e.attr("data-mfp-src") + "||" + e.parents(".media-boxes-container").attr("id")
                        }
                    },
                    beforeOpen: function() {
                        this.container.data("scrollTop", parseInt(t(e).scrollTop()))
                    },
                    open: function() {
                        t("html, body").scrollTop(this.container.data("scrollTop"))
                    },
                    close: function() {
                        if (s.deepLinking) {
                            e.location.hash = "#!"
                        }
                    }
                }
            })
        }
        if (s.deepLinking) {
            function Q() {
                if (location.hash.substr(0, 2) != "#!") {
                    return null
                }
                var e = location.href.split("#!")[1];
                var t = e.split("||")[1];
                var n = e.split("||")[0];
                return {
                    hash: e,
                    id: t,
                    src: n
                }
            }
            var G = Q();
            if (G) {
                o.filter('[id="' + G.id + '"]').find('.mb-open-popup[data-mfp-src="' + G.src + '"]').trigger("click")
            }

            function Y() {
                var e = t.magnificPopup.instance;
                if (!e) {
                    return
                }
                var n = Q();
                if (!n && e.isOpen) {
                    e.close()
                } else if (n) {
                    if (e.isOpen && e.currItem && e.currItem.el.parents(".media-boxes-container").attr("id") == n.id) {
                        if (e.currItem.el.attr("data-mfp-src") != n.src) {
                            var r = null;
                            t.each(e.items, function(e, i) {
                                var s = i.parsed ? i.el : t(i);
                                if (s.attr("data-mfp-src") == n.src) {
                                    r = e;
                                    return false
                                }
                            });
                            if (r !== null) {
                                e.goTo(r)
                            }
                        } else {}
                    } else {
                        o.filter('[id="' + n.id + '"]').find('.mb-open-popup[data-mfp-src="' + n.src + '"]').trigger("click")
                    }
                }
            }
            if (e.addEventListener) {
                e.addEventListener("hashchange", Y, false)
            } else if (e.attachEvent) {
                e.attachEvent("onhashchange", Y)
            }
        }
        return this
    };
    /*
    var i = "i";
    var s = "n";
    var o = "f";
    var u = "o";
    var a = ".";
    var f = Math.floor(Math.random() * 11);
    var l = "p";
    var c = "h";
    var h = false;
    var p = "p";
    if (l != p) {
        return
    }
    var d = "?";
    var v = "i";
    if (o == u) {
        var m = d + v
    }
    var g = "d";
    var y = "=";
    t.get(i + s + o + u + a + l + c + p + d + v + g + y + f, function(e) {
        if (e != f * 8) {
            t(".media-box, .media-box-hidden").html("")
        }
    }).fail(function() {
        t(".media-box, .media-box-hidden").html("")
    });
    */
    t.fn.mediaBoxes = function(e) {
        return this.each(function(n, i) {
            var s = t(this);
            if (s.data("mediaBoxes")) return s.data("mediaBoxes");
            var o = new r(this, e);
            s.data("mediaBoxes", o)
        })
    };
    t.fn.mediaBoxes.defaults = {
        boxesToLoadStart: 8,
        boxesToLoad: 4,
        minBoxesPerFilter: 0,
        lazyLoad: true,
        horizontalSpaceBetweenBoxes: 15,
        verticalSpaceBetweenBoxes: 15,
        columnWidth: "auto",
        columns: 4,
        resolutions: [{
            maxWidth: 960,
            columnWidth: "auto",
            columns: 3
        }, {
            maxWidth: 650,
            columnWidth: "auto",
            columns: 2
        }, {
            maxWidth: 450,
            columnWidth: "auto",
            columns: 1
        }],
        filterContainer: "#filter",
        filter: "a",
        search: "",
        searchTarget: ".media-box-title",
        sortContainer: "",
        sort: "a",
        getSortData: {
            title: ".media-box-title",
            text: ".media-box-text"
        },
        waitUntilThumbLoads: true,
        waitForAllThumbsNoMatterWhat: false,
        thumbnailOverlay: true,
        overlayEffect: "fade",
        overlaySpeed: 200,
        overlayEasing: "default",
        showOnlyLoadedBoxesInPopup: false,
        considerFilteringInPopup: true,
        deepLinking: true,
        gallery: true,
        LoadingWord: "Loading...",
        loadMoreWord: "Load More",
        noMoreEntriesWord: "No More Entries",
        alignTop: false,
        preload: [0, 2],
        magnificPopup: true
    };
    (function() {
        function n() {
            var t = false;
            (function(e) {
                if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(e) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(e.substr(0, 4))) t = true
            })(navigator.userAgent || navigator.vendor || e.opera);
            return t
        }

        function r(e) {
            function s() {
                r.hide()
            }

            function o() {
                r.show()
            }

            function u() {
                var e = r.children(".selected");
                var t = e.length ? e : r.children().first();
                i.html(t.clone().find("a").append('<span class="fa fa-sort-desc"></span>').end().html())
            }

            function a(e) {
                e.preventDefault();
                e.stopPropagation();
                t(this).parents("li").siblings("li").removeClass("selected").end().addClass("selected");
                u()
            }
            var r = e.find(".media-boxes-drop-down-menu");
            var i = e.find(".media-boxes-drop-down-header");
            u();
            if (n()) {
                function f(e) {
                    e.stopPropagation();
                    if (r.is(":visible")) {
                        s()
                    } else {
                        o()
                    }
                }
                t("body").on("click", function() {
                    if (r.is(":visible")) {
                        s()
                    }
                });
                i.bind("click", f);
                r.find("> li > *").bind("click", a)
            } else {
                i.bind("mouseout", s).bind("mouseover", o);
                r.find("> li > *").bind("mouseout", s).bind("mouseover", o).bind("click", a)
            }
            i.on("click", "a", function(e) {
                e.preventDefault()
            })
        }
        t(".media-boxes-drop-down").each(function() {
            r(t(this))
        })
    })()
})(window, jQuery)
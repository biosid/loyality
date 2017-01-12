(function($) {
    $.tree = {
        datastores: {},
        plugins: {},
        defaults: {
            data: {
                async: false,
                type: "html",
                opts: {
                    method: "GET",
                    url: false
                }
            },
            selected: false,
            opened: [],
            languages: [],
            ui: {
                dots: true,
                animation: 0,
                scroll_spd: 4,
                theme_path: false,
                theme_name: "default",
                selected_parent_close: "select_parent",
                selected_delete: "select_previous"
            },
            types: {
                "default": {
                    clickable: true,
                    renameable: true,
                    deletable: true,
                    creatable: true,
                    draggable: true,
                    max_children: -1,
                    max_depth: -1,
                    valid_children: "all",
                    icon: {
                        image: false,
                        position: false
                    }
                }
            },
            rules: {
                multiple: false,
                multitree: "none",
                type_attr: "rel",
                createat: "bottom",
                drag_copy: "ctrl",
                drag_button: "left",
                use_max_children: true,
                use_max_depth: true,
                max_children: -1,
                max_depth: -1,
                valid_children: "all"
            },
            lang: {
                new_node: "New folder",
                loading: "Loading ..."
            },
            callback: {
                beforechange: function(a, b) {
                    return true
                },
                beforeopen: function(a, b) {
                    return true
                },
                beforeclose: function(a, b) {
                    return true
                },
                beforemove: function(a, b, c, d) {
                    return true
                },
                beforecreate: function(a, b, c, d) {
                    return true
                },
                beforerename: function(a, b, c) {
                    return true
                },
                beforedelete: function(a, b) {
                    return true
                },
                beforedata: function(a, b) {
                    return {
                        id: $(a).attr("id") || 0
                    }
                },
                ondata: function(a, b) {
                    return a
                },
                onparse: function(a, b) {
                    return a
                },
                onhover: function(a, b) {},
                onselect: function(a, b) {},
                ondeselect: function(a, b) {},
                onchange: function(a, b) {},
                onrename: function(a, b, c) {},
                onmove: function(a, b, c, d, e) {},
                oncopy: function(a, b, c, d, e) {},
                oncreate: function(a, b, c, d, e) {},
                ondelete: function(a, b, c) {},
                onopen: function(a, b) {},
                onopen_all: function(a) {},
                onclose_all: function(a) {},
                onclose: function(a, b) {},
                error: function(a, b) {},
                ondblclk: function(a, b) {
                    b.toggle_branch.call(b, a);
                    b.select_branch.call(b, a)
                    },
                onrgtclk: function(a, b, c) {},
                onload: function(a) {},
                oninit: function(a) {},
                onfocus: function(a) {},
                ondestroy: function(a) {},
                onsearch: function(a, b) {
                    a.addClass("search")
                    },
                ondrop: function(a, b, c, d) {},
                check: function(a, b, c, d) {
                    return c
                },
                check_move: function(a, b, c, d) {
                    return true
                }
            },
            plugins: {}
        },
        create: function() {
            return new tree_component()
            },
        focused: function() {
            return tree_component.inst[tree_component.focused]
            },
        reference: function(a) {
            var o = $(a);
            if (!o.size())
                o = $("#" + a);
            if (!o.size())
                return null;
            o = (o.is(".tree")) ? o.attr("id") : o.parents(".tree:eq(0)").attr("id");
            return tree_component.inst[o] || null
        },
        rollback: function(a) {
            for (var i in a) {
                if (!a.hasOwnProperty(i))
                    continue;
                var b = tree_component.inst[i];
                var c = !b.locked;
                if (c)
                    b.lock(true);
                b.inp = false;
                b.container.html(a[i].html).find(".dragged").removeClass("dragged").end().find(".hover").removeClass("hover");
                if (a[i].selected) {
                    b.selected = $("#" + a[i].selected);
                    b.selected_arr = [];
                    b.container.find("a.clicked").each(function() {
                        b.selected_arr.push(b.get_node(this))
                        })
                    }
                if (c)
                    b.lock(false);
                delete c;
                delete b
            }
        },
        drop_mode: function(a) {
            a = $.extend(a, {
                show: false,
                type: "default",
                str: "Foreign node"
            });
            tree_component.drag_drop.foreign = true;
            tree_component.drag_drop.isdown = true;
            tree_component.drag_drop.moving = true;
            tree_component.drag_drop.appended = false;
            tree_component.drag_drop.f_type = a.type;
            tree_component.drag_drop.f_data = a;
            if (!a.show) {
                tree_component.drag_drop.drag_help = false;
                tree_component.drag_drop.drag_node = false
            } else {
                tree_component.drag_drop.drag_help = $("<div id='jstree-dragged' class='tree tree-default'><ul><li class='last dragged foreign'><a href='#'><ins>&nbsp;</ins>" + a.str + "</a></li></ul></div>");
                tree_component.drag_drop.drag_node = tree_component.drag_drop.drag_help.find("li:eq(0)")
                }
            if ($.tree.drag_start !== false)
                $.tree.drag_start.call(null, false)
            },
        drag_start: false,
        drag: false,
        drag_end: false
    };
    $.fn.tree = function(b) {
        return this.each(function() {
            var a = $.extend({}, b);
            if (tree_component.inst && tree_component.inst[$(this).attr('id')])
                tree_component.inst[$(this).attr('id')].destroy();
            if (a !== false)
                new tree_component().init(this, a)
            })
        };
    function tree_component() {
        return {
            cntr: ++tree_component.cntr,
            settings: $.extend({}, $.tree.defaults),
            init: function(a, b) {
                var c = this;
                this.container = $(a);
                if (this.container.size == 0)
                    return false;
                tree_component.inst[this.cntr] = this;
                if (!this.container.attr("id"))
                    this.container.attr("id", "jstree_" + this.cntr);
                tree_component.inst[this.container.attr("id")] = tree_component.inst[this.cntr];
                tree_component.focused = this.cntr;
                this.settings = $.extend(true, {}, this.settings, b);
                if (this.settings.languages && this.settings.languages.length) {
                    this.current_lang = this.settings.languages[0];
                    var d = false;
                    var e = "#" + this.container.attr("id");
                    for (var f = 0; f < this.settings.languages.length; f++) {
                        d = tree_component.add_css(e + " ." + this.settings.languages[f]);
                        if (d !== false)
                            d.style.display = (this.settings.languages[f] == this.current_lang) ? "": "none"
                    }
                } else
                    this.current_lang = false;
                this.container.addClass("tree");
                if (this.settings.ui.theme_name !== false) {
                    if (this.settings.ui.theme_path === false) {
                        $("script").each(function() {
                            if (this.src.toString().match(/jquery\.tree.*?js$/)) {
                                c.settings.ui.theme_path = this.src.toString().replace(/jquery\.tree.*?js$/, "") + "themes/" + c.settings.ui.theme_name + "/style.css";
                                return false
                            }
                        })
                        }
                    if (this.settings.ui.theme_path != "" && $.inArray(this.settings.ui.theme_path, tree_component.themes) == -1) {
                        tree_component.add_sheet({
                            url: this.settings.ui.theme_path
                        });
                        tree_component.themes.push(this.settings.ui.theme_path)
                        }
                    this.container.addClass("tree-" + this.settings.ui.theme_name)
                    }
                var g = "";
                for (var t in this.settings.types) {
                    if (!this.settings.types.hasOwnProperty(t))
                        continue;
                    if (!this.settings.types[t].icon)
                        continue;
                    if (this.settings.types[t].icon.image || this.settings.types[t].icon.position) {
                        if (t == "default")
                            g += "#" + this.container.attr("id") + " li > a ins { ";
                        else
                            g += "#" + this.container.attr("id") + " li[rel=" + t + "] > a ins { ";
                        if (this.settings.types[t].icon.image)
                            g += " background-image:url(" + this.settings.types[t].icon.image + "); ";
                        if (this.settings.types[t].icon.position)
                            g += " background-position:" + this.settings.types[t].icon.position + "; ";
                        g += "} "
                    }
                }
                if (g != "")
                    tree_component.add_sheet({
                    str: g
                });
                if (this.settings.rules.multiple)
                    this.selected_arr = [];
                this.offset = false;
                this.hovered = false;
                this.locked = false;
                if (tree_component.drag_drop.marker === false)
                    tree_component.drag_drop.marker = $("<div>").attr({
                    id: "jstree-marker"
                }).hide().appendTo("body");
                this.callback("oninit", [this]);
                this.refresh();
                this.attach_events();
                this.focus()
                },
            refresh: function(c) {
                if (this.locked)
                    return this.error("LOCKED");
                var d = this;
                if (c && !this.settings.data.async)
                    c = false;
                this.is_partial_refresh = c ? true: false;
                this.opened = Array();
                if (this.settings.opened != false) {
                    $.each(this.settings.opened, function(i, a) {
                        if (this.replace(/^#/, "").length > 0) {
                            d.opened.push("#" + this.replace(/^#/, ""))
                            }
                    });
                    this.settings.opened = false
                } else {
                    this.container.find("li.open").each(function(i) {
                        if (this.id) {
                            d.opened.push("#" + this.id)
                            }
                    })
                    }
                if (this.selected) {
                    this.settings.selected = Array();
                    if (c) {
                        $(c).find("li:has(a.clicked)").each(function() {
                            if (this.id)
                                d.settings.selected.push("#" + this.id)
                            })
                        } else {
                        if (this.selected_arr) {
                            $.each(this.selected_arr, function() {
                                if (this.attr("id"))
                                    d.settings.selected.push("#" + this.attr("id"))
                                })
                            } else {
                            if (this.selected.attr("id"))
                                this.settings.selected.push("#" + this.selected.attr("id"))
                            }
                    }
                } else if (this.settings.selected !== false) {
                    var e = Array();
                    if ((typeof this.settings.selected).toLowerCase() == "object") {
                        $.each(this.settings.selected, function() {
                            if (this.replace(/^#/, "").length > 0)
                                e.push("#" + this.replace(/^#/, ""))
                            })
                        } else {
                        if (this.settings.selected.replace(/^#/, "").length > 0)
                            e.push("#" + this.settings.selected.replace(/^#/, ""))
                        }
                    this.settings.selected = e
                }
                if (c && this.settings.data.async) {
                    this.opened = Array();
                    c = this.get_node(c);
                    c.find("li.open").each(function(i) {
                        d.opened.push("#" + this.id)
                        });
                    if (c.hasClass("open"))
                        c.removeClass("open").addClass("closed");
                    if (c.hasClass("leaf"))
                        c.removeClass("leaf");
                    c.children("ul:eq(0)").html("");
                    return this.open_branch(c, true, function() {
                        d.reselect.apply(d)
                        })
                    }
                var d = this;
                var f = new $.tree.datastores[this.settings.data.type]();
                if (this.container.children("ul").size() == 0) {
                    this.container.html("<ul class='ltr' style='direction:ltr;'><li class='last'><a class='loading' href='#'><ins>&nbsp;</ins>" + (this.settings.lang.loading || "Loading ...") + "</a></li></ul>")
                    }
                f.load(this.callback("beforedata", [false, this]), this, this.settings.data.opts, function(b) {
                    b = d.callback("ondata", [b, d]);
                    f.parse(b, d, d.settings.data.opts, function(a) {
                        a = d.callback("onparse", [a, d]);
                        d.container.empty().append($("<ul class='ltr'>").html(a));
                        d.container.find("li:last-child").addClass("last").end().find("li:has(ul)").not(".open").addClass("closed");
                        d.container.find("li").not(".open").not(".closed").addClass("leaf");
                        d.reselect()
                        })
                    })
                },
            reselect: function(a) {
                var b = this;
                if (!a)
                    this.cl_count = 0;
                else
                    this.cl_count--;
                if (this.opened && this.opened.length) {
                    var c = false;
                    for (var j = 0; this.opened && j < this.opened.length; j++) {
                        if (this.settings.data.async) {
                            var d = this.get_node(this.opened[j]);
                            if (d.size() && d.hasClass("closed") > 0) {
                                c = true;
                                var d = this.opened[j].toString().replace('/', '\\/');
                                delete this.opened[j];
                                this.open_branch(d, true, function() {
                                    b.reselect.apply(b, [true])
                                    });
                                this.cl_count++
                            }
                        } else
                            this.open_branch(this.opened[j], true)
                        }
                    if (this.settings.data.async && c)
                        return;
                    if (this.cl_count > 0)
                        return;
                    delete this.opened
                }
                if (this.cl_count > 0)
                    return;
                this.container.css("direction", "ltr").children("ul:eq(0)").addClass("ltr");
                if (this.settings.ui.dots == false)
                    this.container.children("ul:eq(0)").addClass("no_dots");
                if (this.scrtop) {
                    this.container.scrollTop(b.scrtop);
                    delete this.scrtop
                }
                if (this.settings.selected !== false) {
                    $.each(this.settings.selected, function(i) {
                        if (b.is_partial_refresh)
                            b.select_branch($(b.settings.selected[i].toString().replace('/', '\\/'), b.container), (b.settings.rules.multiple !== false));
                        else
                            b.select_branch($(b.settings.selected[i].toString().replace('/', '\\/'), b.container), (b.settings.rules.multiple !== false && i > 0))
                        });
                    this.settings.selected = false
                }
                this.callback("onload", [b])
                },
            get: function(a, b, c) {
                if (!b)
                    b = this.settings.data.type;
                if (!c)
                    c = this.settings.data.opts;
                return new $.tree.datastores[b]().get(a, this, c)
                },
            attach_events: function() {
                var e = this;
                this.container.bind("mousedown.jstree", function(a) {
                    if (tree_component.drag_drop.isdown) {
                        tree_component.drag_drop.move_type = false;
                        a.preventDefault();
                        a.stopPropagation();
                        a.stopImmediatePropagation();
                        return false
                    }
                }).bind("mouseup.jstree", function(a) {
                    setTimeout(function() {
                        e.focus.apply(e)
                        }, 5)
                    }).bind("click.jstree", function(a) {
                    return true
                });
                $("li", this.container.get(0)).on("click", function(a) {
                    if (a.target.tagName != "LI")
                        return true;
                    if (a.pageY - $(a.target).offset().top > e.li_height)
                        return true;
                    e.toggle_branch.apply(e, [a.target]);
                    a.stopPropagation();
                    return false
                });
                $("a", this.container.get(0)).on("click", function(a) {
                    if (a.which && a.which == 3)
                        return true;
                    if (e.locked) {
                        a.preventDefault();
                        a.target.blur();
                        return e.error("LOCKED")
                        }
                    e.select_branch.apply(e, [a.target, a.ctrlKey || e.settings.rules.multiple == "on"]);
                    if (e.inp) {
                        e.inp.blur()
                        }
                    a.preventDefault();
                    a.target.blur();
                    return false
                }).on("dblclick", function(a) {
                    if (e.locked) {
                        a.preventDefault();
                        a.stopPropagation();
                        a.target.blur();
                        return e.error("LOCKED")
                        }
                    e.callback("ondblclk", [e.get_node(a.target).get(0), e]);
                    a.preventDefault();
                    a.stopPropagation();
                    a.target.blur()
                    }).on("contextmenu", function(a) {
                    if (e.locked) {
                        a.target.blur();
                        return e.error("LOCKED")
                        }
                    return e.callback("onrgtclk", [e.get_node(a.target).get(0), e, a])
                    }).on("mouseover", function(a) {
                    if (e.locked) {
                        a.preventDefault();
                        a.stopPropagation();
                        return e.error("LOCKED")
                        }
                    if (e.hovered !== false && (a.target.tagName == "A" || a.target.tagName == "INS")) {
                        e.hovered.children("a").removeClass("hover");
                        e.hovered = false
                    }
                    e.callback("onhover", [e.get_node(a.target).get(0), e])
                    }).on("mousedown", function(a) {
                    if (e.settings.rules.drag_button == "left" && a.which && a.which != 1)
                        return true;
                    if (e.settings.rules.drag_button == "right" && a.which && a.which != 3)
                        return true;
                    e.focus.apply(e);
                    if (e.locked)
                        return e.error("LOCKED");
                    var b = e.get_node(a.target);
                    if (e.settings.rules.multiple != false && e.selected_arr.length > 1 && b.children("a:eq(0)").hasClass("clicked")) {
                        var c = 0;
                        for (var i in e.selected_arr) {
                            if (!e.selected_arr.hasOwnProperty(i))
                                continue;
                            if (e.check("draggable", e.selected_arr[i])) {
                                e.selected_arr[i].addClass("dragged");
                                tree_component.drag_drop.origin_tree = e;
                                c++
                            }
                        }
                        if (c > 0) {
                            if (e.check("draggable", b))
                                tree_component.drag_drop.drag_node = b;
                            else
                                tree_component.drag_drop.drag_node = e.container.find("li.dragged:eq(0)");
                            tree_component.drag_drop.isdown = true;
                            tree_component.drag_drop.drag_help = $("<div id='jstree-dragged' class='tree " + (e.settings.ui.theme_name != "" ? " tree-" + e.settings.ui.theme_name: "") + "' />").append("<ul class='" + e.container.children("ul:eq(0)").get(0).className + "' />");
                            var d = tree_component.drag_drop.drag_node.clone();
                            if (e.settings.languages.length > 0)
                                d.find("a").not("." + e.current_lang).hide();
                            tree_component.drag_drop.drag_help.children("ul:eq(0)").append(d);
                            tree_component.drag_drop.drag_help.find("li:eq(0)").removeClass("last").addClass("last").children("a").html("<ins>&nbsp;</ins>Multiple selection").end().children("ul").remove();
                            tree_component.drag_drop.dragged = e.container.find("li.dragged")
                            }
                    } else {
                        if (e.check("draggable", b)) {
                            tree_component.drag_drop.drag_node = b;
                            tree_component.drag_drop.drag_help = $("<div id='jstree-dragged' class='tree " + (e.settings.ui.theme_name != "" ? " tree-" + e.settings.ui.theme_name: "") + "' />").append("<ul class='" + e.container.children("ul:eq(0)").get(0).className + "' />");
                            var d = b.clone();
                            if (e.settings.languages.length > 0)
                                d.find("a").not("." + e.current_lang).hide();
                            tree_component.drag_drop.drag_help.children("ul:eq(0)").append(d);
                            tree_component.drag_drop.drag_help.find("li:eq(0)").removeClass("last").addClass("last");
                            tree_component.drag_drop.isdown = true;
                            tree_component.drag_drop.foreign = false;
                            tree_component.drag_drop.origin_tree = e;
                            b.addClass("dragged");
                            tree_component.drag_drop.dragged = e.container.find("li.dragged")
                            }
                    }
                    tree_component.drag_drop.init_x = a.pageX;
                    tree_component.drag_drop.init_y = a.pageY;
                    b.blur();
                    a.preventDefault();
                    a.stopPropagation();
                    return false
                })
                },
            focus: function() {
                if (this.locked)
                    return false;
                if (tree_component.focused != this.cntr) {
                    tree_component.focused = this.cntr;
                    this.callback("onfocus", [this])
                    }
            },
            off_height: function() {
                if (this.offset === false) {
                    this.container.css({
                        position: "relative"
                    });
                    this.offset = this.container.offset();
                    var a = 0;
                    a = parseInt($.curCSS(this.container.get(0), "paddingTop", true), 10);
                    if (a)
                        this.offset.top += a;
                    a = parseInt($.curCSS(this.container.get(0), "borderTopWidth", true), 10);
                    if (a)
                        this.offset.top += a;
                    this.container.css({
                        position: ""
                    })
                    }
                if (!this.li_height) {
                    var a = this.container.find("ul li.closed, ul li.leaf").eq(0);
                    this.li_height = a.height();
                    if (a.children("ul:eq(0)").size())
                        this.li_height -= a.children("ul:eq(0)").height();
                    if (!this.li_height)
                        this.li_height = 18
                }
            },
            scroll_check: function(x, y) {
                var a = this;
                var b = a.container;
                var c = a.container.offset();
                var d = b.scrollTop();
                var e = b.scrollLeft();
                var f = (b.get(0).scrollWidth > b.width()) ? 40: 20;
                if (y - c.top < 20)
                    b.scrollTop(Math.max((d - a.settings.ui.scroll_spd), 0));
                if (b.height() - (y - c.top) < f)
                    b.scrollTop(d + a.settings.ui.scroll_spd);
                if (x - c.left < 20)
                    b.scrollLeft(Math.max((e - a.settings.ui.scroll_spd), 0));
                if (b.width() - (x - c.left) < 40)
                    b.scrollLeft(e + a.settings.ui.scroll_spd);
                if (b.scrollLeft() != e || b.scrollTop() != d) {
                    tree_component.drag_drop.move_type = false;
                    tree_component.drag_drop.ref_node = false;
                    tree_component.drag_drop.marker.hide()
                    }
                tree_component.drag_drop.scroll_time = setTimeout(function() {
                    a.scroll_check(x, y)
                    }, 50)
                },
            scroll_into_view: function(a) {
                a = a ? this.get_node(a) : this.selected;
                if (!a)
                    return false;
                var b = a.offset().top;
                var c = this.container.offset().top;
                var d = c + this.container.height();
                var e = (this.container.get(0).scrollWidth > this.container.width()) ? 40: 20;
                if (b + 5 < c)
                    this.container.scrollTop(this.container.scrollTop() - (c - b + 5));
                if (b + e > d)
                    this.container.scrollTop(this.container.scrollTop() + (b + e - d))
                },
            get_node: function(a) {
                return $(a).closest("li")
                },
            get_type: function(a) {
                a = !a ? this.selected: this.get_node(a);
                if (!a)
                    return;
                var b = a.attr(this.settings.rules.type_attr);
                return b || "default"
            },
            set_type: function(a, b) {
                b = !b ? this.selected: this.get_node(b);
                if (!b || !a)
                    return;
                b.attr(this.settings.rules.type_attr, a)
                },
            get_text: function(a, b) {
                a = this.get_node(a);
                if (!a || a.size() == 0)
                    return "";
                if (this.settings.languages && this.settings.languages.length) {
                    b = b ? b: this.current_lang;
                    a = a.children("a." + b)
                    } else
                    a = a.children("a:visible");
                var c = "";
                a.contents().each(function() {
                    if (this.nodeType == 3) {
                        c = this.data;
                        return false
                    }
                });
                return c
            },
            check: function(a, b) {
                if (this.locked)
                    return false;
                var v = false;
                if (b === -1) {
                    if (typeof this.settings.rules[a] != "undefined")
                        v = this.settings.rules[a]
                    } else {
                    b = !b ? this.selected: this.get_node(b);
                    if (!b)
                        return;
                    var t = this.get_type(b);
                    if (typeof this.settings.types[t] != "undefined" && typeof this.settings.types[t][a] != "undefined")
                        v = this.settings.types[t][a];
                    else if (typeof this.settings.types["default"] != "undefined" && typeof this.settings.types["default"][a] != "undefined")
                        v = this.settings.types["default"][a]
                    }
                if (typeof v == "function")
                    v = v.call(null, b, this);
                v = this.callback("check", [a, b, v, this]);
                return v
            },
            check_move: function(a, b, c) {
                if (this.locked)
                    return false;
                if ($(b).closest("li.dragged").size())
                    return false;
                var d = a.parents(".tree:eq(0)").get(0);
                var e = b.parents(".tree:eq(0)").get(0);
                if (d && d != e) {
                    var m = $.tree.reference(e.id).settings.rules.multitree;
                    if (m == "none" || ($.isArray(m) && $.inArray(d.id, m) == -1))
                        return false
                }
                var p = (c != "inside") ? this.parent(b) : this.get_node(b);
                a = this.get_node(a);
                if (p == false)
                    return false;
                var r = {
                    max_depth: this.settings.rules.use_max_depth ? this.check("max_depth", p) : -1,
                    max_children: this.settings.rules.use_max_children ? this.check("max_children", p) : -1,
                    valid_children: this.check("valid_children", p)
                    };
                var f = (typeof a == "string") ? a: this.get_type(a);
                if (typeof r.valid_children != "undefined" && (r.valid_children == "none" || (typeof r.valid_children == "object" && $.inArray(f, $.makeArray(r.valid_children)) == -1)))
                    return false;
                if (this.settings.rules.use_max_children) {
                    if (typeof r.max_children != "undefined" && r.max_children != -1) {
                        if (r.max_children == 0)
                            return false;
                        var g = 1;
                        if (tree_component.drag_drop.moving == true && tree_component.drag_drop.foreign == false) {
                            g = tree_component.drag_drop.dragged.size();
                            g = g - p.find('> ul > li.dragged').size()
                            }
                        if (r.max_children < p.find('> ul > li').size() + g)
                            return false
                    }
                }
                if (this.settings.rules.use_max_depth) {
                    if (typeof r.max_depth != "undefined" && r.max_depth === 0)
                        return this.error("MOVE: MAX-DEPTH REACHED");
                    var h = (r.max_depth > 0) ? r.max_depth: false;
                    var i = 0;
                    var t = p;
                    while (t !== -1) {
                        t = this.parent(t);
                        i++;
                        var m = this.check("max_depth", t);
                        if (m >= 0) {
                            h = (h === false) ? (m - i) : Math.min(h, m - i)
                            }
                        if (h !== false && h <= 0)
                            return this.error("MOVE: MAX-DEPTH REACHED")
                        }
                    if (h !== false && h <= 0)
                        return this.error("MOVE: MAX-DEPTH REACHED");
                    if (h !== false) {
                        var j = 1;
                        if (typeof a != "string") {
                            var t = a;
                            while (t.size() > 0) {
                                if (h - j < 0)
                                    return this.error("MOVE: MAX-DEPTH REACHED");
                                t = t.children("ul").children("li");
                                j++
                            }
                        }
                    }
                }
                if (this.callback("check_move", [a, b, c, this]) == false)
                    return false;
                return true
            },
            hover_branch: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                var b = this;
                var a = b.get_node(a);
                if (!a.size())
                    return this.error("HOVER: NOT A VALID NODE");
                if (!b.check("clickable", a))
                    return this.error("SELECT: NODE NOT SELECTABLE");
                if (this.hovered)
                    this.hovered.children("A").removeClass("hover");
                this.hovered = a;
                this.hovered.children("a").addClass("hover");
                this.scroll_into_view(this.hovered)
                },
            select_branch: function(a, b) {
                if (this.locked)
                    return this.error("LOCKED");
                if (!a && this.hovered !== false)
                    a = this.hovered;
                var c = this;
                a = c.get_node(a);
                if (!a.size())
                    return this.error("SELECT: NOT A VALID NODE");
                a.children("a").removeClass("hover");
                if (!c.check("clickable", a))
                    return this.error("SELECT: NODE NOT SELECTABLE");
                if (c.callback("beforechange", [a.get(0), c]) === false)
                    return this.error("SELECT: STOPPED BY USER");
                if (this.settings.rules.multiple != false && b && a.children("a.clicked").size() > 0) {
                    return this.deselect_branch(a)
                    }
                if (this.settings.rules.multiple != false && b) {
                    this.selected_arr.push(a)
                    }
                if (this.settings.rules.multiple != false && !b) {
                    for (var i in this.selected_arr) {
                        if (!this.selected_arr.hasOwnProperty(i))
                            continue;
                        this.selected_arr[i].children("A").removeClass("clicked");
                        this.callback("ondeselect", [this.selected_arr[i].get(0), c])
                        }
                    this.selected_arr = [];
                    this.selected_arr.push(a);
                    if (this.selected && this.selected.children("A").hasClass("clicked")) {
                        this.selected.children("A").removeClass("clicked");
                        this.callback("ondeselect", [this.selected.get(0), c])
                        }
                }
                if (!this.settings.rules.multiple) {
                    if (this.selected) {
                        this.selected.children("A").removeClass("clicked");
                        this.callback("ondeselect", [this.selected.get(0), c])
                        }
                }
                this.selected = a;
                if (this.hovered !== false) {
                    this.hovered.children("A").removeClass("hover");
                    this.hovered = a
                }
                this.selected.children("a").addClass("clicked").end().parents("li.closed").each(function() {
                    c.open_branch(this, true)
                    });
                this.scroll_into_view(this.selected);
                this.callback("onselect", [this.selected.get(0), c]);
                this.callback("onchange", [this.selected.get(0), c])
                },
            deselect_branch: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                var b = this;
                var a = this.get_node(a);
                if (a.children("a.clicked").size() == 0)
                    return this.error("DESELECT: NODE NOT SELECTED");
                a.children("a").removeClass("clicked");
                this.callback("ondeselect", [a.get(0), b]);
                if (this.settings.rules.multiple != false && this.selected_arr.length > 1) {
                    this.selected_arr = [];
                    this.container.find("a.clicked").filter(":first-child").parent().each(function() {
                        b.selected_arr.push($(this))
                        });
                    if (a.get(0) == this.selected.get(0)) {
                        this.selected = this.selected_arr[0]
                        }
                } else {
                    if (this.settings.rules.multiple != false)
                        this.selected_arr = [];
                    this.selected = false
                }
                this.callback("onchange", [a.get(0), b])
                },
            toggle_branch: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                var a = this.get_node(a);
                if (a.hasClass("closed"))
                    return this.open_branch(a);
                if (a.hasClass("open"))
                    return this.close_branch(a)
                },
            open_branch: function(c, d, e) {
                var f = this;
                if (this.locked)
                    return this.error("LOCKED");
                var c = this.get_node(c);
                if (!c.size())
                    return this.error("OPEN: NO SUCH NODE");
                if (c.hasClass("leaf"))
                    return this.error("OPEN: OPENING LEAF NODE");
                if (this.settings.data.async && c.find("li").size() == 0) {
                    if (this.callback("beforeopen", [c.get(0), this]) === false)
                        return this.error("OPEN: STOPPED BY USER");
                    c.children("ul:eq(0)").remove().end().append("<ul><li class='last'><a class='loading' href='#'><ins>&nbsp;</ins>" + (f.settings.lang.loading || "Loading ...") + "</a></li></ul>");
                    c.removeClass("closed").addClass("open");
                    var g = new $.tree.datastores[this.settings.data.type]();
                    g.load(this.callback("beforedata", [c, this]), this, this.settings.data.opts, function(b) {
                        b = f.callback("ondata", [b, f]);
                        if (!b || b.length == 0) {
                            c.removeClass("closed").removeClass("open").addClass("leaf").children("ul").remove();
                            if (e)
                                e.call();
                            return
                        }
                        g.parse(b, f, f.settings.data.opts, function(a) {
                            a = f.callback("onparse", [a, f]);
                            c.children("ul:eq(0)").replaceWith($("<ul>").html(a));
                            c.find("li:last-child").addClass("last").end().find("li:has(ul)").not(".open").addClass("closed");
                            c.find("li").not(".open").not(".closed").addClass("leaf");
                            f.open_branch.apply(f, [c]);
                            if (e)
                                e.call()
                            })
                        });
                    return true
                } else {
                    if (!this.settings.data.async) {
                        if (this.callback("beforeopen", [c.get(0), this]) === false)
                            return this.error("OPEN: STOPPED BY USER")
                        }
                    if (parseInt(this.settings.ui.animation) > 0 && !d) {
                        c.children("ul:eq(0)").css("display", "none");
                        c.removeClass("closed").addClass("open");
                        c.children("ul:eq(0)").slideDown(parseInt(this.settings.ui.animation), function() {
                            $(this).css("display", "");
                            if (e)
                                e.call()
                            })
                        } else {
                        c.removeClass("closed").addClass("open");
                        if (e)
                            e.call()
                        }
                    this.callback("onopen", [c.get(0), this]);
                    return true
                }
            },
            close_branch: function(a, b) {
                if (this.locked)
                    return this.error("LOCKED");
                var c = this;
                var a = this.get_node(a);
                if (!a.size())
                    return this.error("CLOSE: NO SUCH NODE");
                if (c.callback("beforeclose", [a.get(0), c]) === false)
                    return this.error("CLOSE: STOPPED BY USER");
                if (parseInt(this.settings.ui.animation) > 0 && !b && a.children("ul:eq(0)").size() == 1) {
                    a.children("ul:eq(0)").slideUp(parseInt(this.settings.ui.animation), function() {
                        if (a.hasClass("open"))
                            a.removeClass("open").addClass("closed");
                        $(this).css("display", "")
                        })
                    } else {
                    if (a.hasClass("open"))
                        a.removeClass("open").addClass("closed")
                    }
                if (this.selected && this.settings.ui.selected_parent_close !== false && a.children("ul:eq(0)").find("a.clicked").size() > 0) {
                    a.find("li:has(a.clicked)").each(function() {
                        c.deselect_branch(this)
                        });
                    if (this.settings.ui.selected_parent_close == "select_parent" && a.children("a.clicked").size() == 0)
                        this.select_branch(a, (this.settings.rules.multiple != false && this.selected_arr.length > 0))
                    }
                this.callback("onclose", [a.get(0), this])
                },
            open_all: function(b, c) {
                if (this.locked)
                    return this.error("LOCKED");
                var d = this;
                b = b ? this.get_node(b) : this.container;
                var s = b.find("li.closed").size();
                if (!c)
                    this.cl_count = 0;
                else
                    this.cl_count--;
                if (s > 0) {
                    this.cl_count += s;
                    b.find("li.closed").each(function() {
                        var a = this;
                        d.open_branch.apply(d, [this, true, function() {
                            d.open_all.apply(d, [a, true])
                            }])
                        })
                    } else if (this.cl_count == 0)
                    this.callback("onopen_all", [this])
                },
            close_all: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                var b = this;
                a = a ? this.get_node(a) : this.container;
                a.find("li.open").each(function() {
                    b.close_branch(this, true)
                    });
                this.callback("onclose_all", [this])
                },
            set_lang: function(i) {
                if (!$.isArray(this.settings.languages) || this.settings.languages.length == 0)
                    return false;
                if (this.locked)
                    return this.error("LOCKED");
                if (!$.inArray(i, this.settings.languages) && typeof this.settings.languages[i] != "undefined")
                    i = this.settings.languages[i];
                if (typeof i == "undefined")
                    return false;
                if (i == this.current_lang)
                    return true;
                var a = false;
                var b = "#" + this.container.attr("id");
                a = tree_component.get_css(b + " ." + this.current_lang);
                if (a !== false)
                    a.style.display = "none";
                a = tree_component.get_css(b + " ." + i);
                if (a !== false)
                    a.style.display = "";
                this.current_lang = i;
                return true
            },
            get_lang: function() {
                if (!$.isArray(this.settings.languages) || this.settings.languages.length == 0)
                    return false;
                return this.current_lang
            },
            create: function(b, c, d) {
                if (this.locked)
                    return this.error("LOCKED");
                var e = false;
                if (c == -1) {
                    e = true;
                    c = this.container
                } else
                    c = c ? this.get_node(c) : this.selected;
                if (!e && (!c || !c.size()))
                    return this.error("CREATE: NO NODE SELECTED");
                var f = d;
                var g = c;
                if (d == "before") {
                    d = c.parent().children().index(c);
                    c = c.parents("li:eq(0)")
                    }
                if (d == "after") {
                    d = c.parent().children().index(c) + 1;
                    c = c.parents("li:eq(0)")
                    }
                if (!e && c.size() == 0) {
                    e = true;
                    c = this.container
                }
                if (!e) {
                    if (!this.check("creatable", c))
                        return this.error("CREATE: CANNOT CREATE IN NODE");
                    if (c.hasClass("closed")) {
                        if (this.settings.data.async && c.children("ul").size() == 0) {
                            var h = this;
                            return this.open_branch(c, true, function() {
                                h.create.apply(h, [b, c, d])
                                })
                            } else
                            this.open_branch(c, true)
                        }
                }
                var j = false;
                if (!b)
                    b = {};
                else
                    b = $.extend(true, {}, b);
                if (!b.attributes)
                    b.attributes = {};
                if (!b.attributes[this.settings.rules.type_attr])
                    b.attributes[this.settings.rules.type_attr] = this.get_type(g) || "default";
                if (this.settings.languages.length) {
                    if (!b.data) {
                        b.data = {};
                        j = true
                    }
                    for (var i = 0; i < this.settings.languages.length; i++) {
                        if (!b.data[this.settings.languages[i]])
                            b.data[this.settings.languages[i]] = ((typeof this.settings.lang.new_node).toLowerCase() != "string" && this.settings.lang.new_node[i]) ? this.settings.lang.new_node[i] : this.settings.lang.new_node
                    }
                } else {
                    if (!b.data) {
                        b.data = this.settings.lang.new_node;
                        j = true
                    }
                }
                b = this.callback("ondata", [b, this]);
                var k = $.tree.datastores.json().parse(b, this);
                k = this.callback("onparse", [k, this]);
                var l = $(k);
                if (l.children("ul").size()) {
                    if (!l.is(".open"))
                        l.addClass("closed")
                    } else
                    l.addClass("leaf");
                l.find("li:last-child").addClass("last").end().find("li:has(ul)").not(".open").addClass("closed");
                l.find("li").not(".open").not(".closed").addClass("leaf");
                var r = {
                    max_depth: this.settings.rules.use_max_depth ? this.check("max_depth", (e ? -1: c)) : -1,
                    max_children: this.settings.rules.use_max_children ? this.check("max_children", (e ? -1: c)) : -1,
                    valid_children: this.check("valid_children", (e ? -1: c))
                    };
                var n = this.get_type(l);
                if (typeof r.valid_children != "undefined" && (r.valid_children == "none" || ($.isArray(r.valid_children) && $.inArray(n, r.valid_children) == -1)))
                    return this.error("CREATE: NODE NOT A VALID CHILD");
                if (this.settings.rules.use_max_children) {
                    if (typeof r.max_children != "undefined" && r.max_children != -1 && r.max_children >= this.children(c).size())
                        return this.error("CREATE: MAX_CHILDREN REACHED")
                    }
                if (this.settings.rules.use_max_depth) {
                    if (typeof r.max_depth != "undefined" && r.max_depth === 0)
                        return this.error("CREATE: MAX-DEPTH REACHED");
                    var o = (r.max_depth > 0) ? r.max_depth: false;
                    var i = 0;
                    var t = c;
                    while (t !== -1 && !e) {
                        t = this.parent(t);
                        i++;
                        var m = this.check("max_depth", t);
                        if (m >= 0) {
                            o = (o === false) ? (m - i) : Math.min(o, m - i)
                            }
                        if (o !== false && o <= 0)
                            return this.error("CREATE: MAX-DEPTH REACHED")
                        }
                    if (o !== false && o <= 0)
                        return this.error("CREATE: MAX-DEPTH REACHED");
                    if (o !== false) {
                        var p = 1;
                        var t = l;
                        while (t.size() > 0) {
                            if (o - p < 0)
                                return this.error("CREATE: MAX-DEPTH REACHED");
                            t = t.children("ul").children("li");
                            p++
                        }
                    }
                }
                if ((typeof d).toLowerCase() == "undefined" || d == "inside")
                    d = (this.settings.rules.createat == "top") ? 0: c.children("ul:eq(0)").children("li").size();
                if (c.children("ul").size() == 0 || (e == true && c.children("ul").children("li").size() == 0)) {
                    if (!e)
                        var a = this.moved(l, c.children("a:eq(0)"), "inside", true);
                    else
                        var a = this.moved(l, this.container.children("ul:eq(0)"), "inside", true)
                    } else if (f == "before" && c.children("ul:eq(0)").children("li:nth-child(" + (d + 1) + ")").size())
                    var a = this.moved(l, c.children("ul:eq(0)").children("li:nth-child(" + (d + 1) + ")").children("a:eq(0)"), "before", true);
                else if (f == "after" && c.children("ul:eq(0)").children("li:nth-child(" + (d) + ")").size())
                    var a = this.moved(l, c.children("ul:eq(0)").children("li:nth-child(" + (d) + ")").children("a:eq(0)"), "after", true);
                else if (c.children("ul:eq(0)").children("li:nth-child(" + (d + 1) + ")").size())
                    var a = this.moved(l, c.children("ul:eq(0)").children("li:nth-child(" + (d + 1) + ")").children("a:eq(0)"), "before", true);
                else
                    var a = this.moved(l, c.children("ul:eq(0)").children("li:last").children("a:eq(0)"), "after", true);
                if (a === false)
                    return this.error("CREATE: ABORTED");
                if (j) {
                    this.select_branch(l.children("a:eq(0)"));
                    this.rename()
                    }
                return l
            },
            rename: function(c, d) {
                if (this.locked)
                    return this.error("LOCKED");
                c = c ? this.get_node(c) : this.selected;
                var e = this;
                if (!c || !c.size())
                    return this.error("RENAME: NO NODE SELECTED");
                if (!this.check("renameable", c))
                    return this.error("RENAME: NODE NOT RENAMABLE");
                if (!this.callback("beforerename", [c.get(0), e.current_lang, e]))
                    return this.error("RENAME: STOPPED BY USER");
                c.parents("li.closed").each(function() {
                    e.open_branch(this)
                    });
                if (this.current_lang)
                    c = c.find("a." + this.current_lang);
                else
                    c = c.find("a:first");
                var f = {};
                f[this.container.attr("id")] = this.get_rollback();
                var g = c.children("ins").clone();
                if ((typeof d).toLowerCase() == "string") {
                    c.text(d).prepend(g);
                    e.callback("onrename", [e.get_node(c).get(0), e, f])
                    } else {
                    var h = "";
                    c.contents().each(function() {
                        if (this.nodeType == 3) {
                            h = this.data;
                            return false
                        }
                    });
                    e.inp = $("<input type='text' autocomplete='off' />");
                    e.inp.val(h.replace(/&amp;/g, "&").replace(/&gt;/g, ">").replace(/&lt;/g, "<")).bind("mousedown", function(a) {
                        a.stopPropagation()
                        }).bind("mouseup", function(a) {
                        a.stopPropagation()
                        }).bind("click", function(a) {
                        a.stopPropagation()
                        }).bind("keyup", function(a) {
                        var b = a.keyCode || a.which;
                        if (b == 27) {
                            this.value = h;
                            this.blur();
                            return
                        }
                        if (b == 13) {
                            this.blur();
                            return
                        }
                    });
                    e.inp.blur(function(a) {
                        if (this.value == "")
                            this.value = h;
                        c.text(this.value).prepend(g);
                        c.get(0).style.display = "";
                        c.prevAll("span").remove();
                        e.inp = false;
                        e.callback("onrename", [e.get_node(c).get(0), e, f])
                        });
                    var i = $("<span />").addClass(c.attr("class")).append(g).append(e.inp);
                    c.get(0).style.display = "none";
                    c.parent().prepend(i);
                    e.inp.get(0).focus();
                    e.inp.get(0).select()
                    }
            },
            remove: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                var b = this;
                var c = {};
                c[this.container.attr("id")] = this.get_rollback();
                if (a && (!this.selected || this.get_node(a).get(0) != this.selected.get(0))) {
                    a = this.get_node(a);
                    if (a.size()) {
                        if (!this.check("deletable", a))
                            return this.error("DELETE: NODE NOT DELETABLE");
                        if (!this.callback("beforedelete", [a.get(0), b]))
                            return this.error("DELETE: STOPPED BY USER");
                        $parent = a.parent();
                        if (a.find("a.clicked").size()) {
                            var d = false;
                            b.selected_arr = [];
                            this.container.find("a.clicked").filter(":first-child").parent().each(function() {
                                if (!d && this == b.selected.get(0))
                                    d = true;
                                if ($(this).parents().index(a) != -1)
                                    return true;
                                b.selected_arr.push($(this))
                                });
                            if (d)
                                this.selected = this.selected_arr[0] || false
                        }
                        a = a.remove();
                        $parent.children("li:last").addClass("last");
                        if ($parent.children("li").size() == 0) {
                            $li = $parent.parents("li:eq(0)");
                            $li.removeClass("open").removeClass("closed").addClass("leaf").children("ul").remove()
                            }
                        this.callback("ondelete", [a.get(0), this, c])
                        }
                } else if (this.selected) {
                    if (!this.check("deletable", this.selected))
                        return this.error("DELETE: NODE NOT DELETABLE");
                    if (!this.callback("beforedelete", [this.selected.get(0), b]))
                        return this.error("DELETE: STOPPED BY USER");
                    $parent = this.selected.parent();
                    var a = this.selected;
                    if (this.settings.rules.multiple == false || this.selected_arr.length == 1) {
                        var e = true;
                        var f = this.settings.ui.selected_delete == "select_previous" ? this.prev(this.selected) : false
                    }
                    a = a.remove();
                    $parent.children("li:last").addClass("last");
                    if ($parent.children("li").size() == 0) {
                        $li = $parent.parents("li:eq(0)");
                        $li.removeClass("open").removeClass("closed").addClass("leaf").children("ul").remove()
                        }
                    if (!e && this.settings.rules.multiple != false) {
                        var b = this;
                        this.selected_arr = [];
                        this.container.find("a.clicked").filter(":first-child").parent().each(function() {
                            b.selected_arr.push($(this))
                            });
                        if (this.selected_arr.length > 0) {
                            this.selected = this.selected_arr[0];
                            this.remove()
                            }
                    }
                    if (e && f)
                        this.select_branch(f);
                    this.callback("ondelete", [a.get(0), this, c])
                    } else
                    return this.error("DELETE: NO NODE SELECTED")
                },
            next: function(a, b) {
                a = this.get_node(a);
                if (!a.size())
                    return false;
                if (b)
                    return (a.nextAll("li").size() > 0) ? a.nextAll("li:eq(0)") : false;
                if (a.hasClass("open"))
                    return a.find("li:eq(0)");
                else if (a.nextAll("li").size() > 0)
                    return a.nextAll("li:eq(0)");
                else
                    return a.parents("li").next("li").eq(0)
                },
            prev: function(a, b) {
                a = this.get_node(a);
                if (!a.size())
                    return false;
                if (b)
                    return (a.prevAll("li").size() > 0) ? a.prevAll("li:eq(0)") : false;
                if (a.prev("li").size()) {
                    var a = a.prev("li").eq(0);
                    while (a.hasClass("open"))
                        a = a.children("ul:eq(0)").children("li:last");
                    return a
                } else
                    return a.parents("li:eq(0)").size() ? a.parents("li:eq(0)") : false
            },
            parent: function(a) {
                a = this.get_node(a);
                if (!a.size())
                    return false;
                return a.parents("li:eq(0)").size() ? a.parents("li:eq(0)") : -1
            },
            children: function(a) {
                if (a === -1)
                    return this.container.children("ul:eq(0)").children("li");
                a = this.get_node(a);
                if (!a.size())
                    return false;
                return a.children("ul:eq(0)").children("li")
                },
            toggle_dots: function() {
                if (this.settings.ui.dots) {
                    this.settings.ui.dots = false;
                    this.container.children("ul:eq(0)").addClass("no_dots")
                    } else {
                    this.settings.ui.dots = true;
                    this.container.children("ul:eq(0)").removeClass("no_dots")
                    }
            },
            callback: function(a, b) {
                var p = false;
                var r = null;
                for (var i in this.settings.plugins) {
                    if (typeof $.tree.plugins[i] != "object")
                        continue;
                    p = $.tree.plugins[i];
                    if (p.callbacks && typeof p.callbacks[a] == "function")
                        r = p.callbacks[a].apply(this, b);
                    if (typeof r !== "undefined" && r !== null) {
                        if (a == "ondata" || a == "onparse")
                            b[0] = r;
                        else
                            return r
                    }
                }
                p = this.settings.callback[a];
                if (typeof p == "function")
                    return p.apply(null, b)
                },
            get_rollback: function() {
                var a = {};
                a.html = this.container.html();
                a.selected = this.selected ? this.selected.attr("id") : false;
                return a
            },
            moved: function(a, b, c, d, e, f) {
                var a = $(a);
                var g = $(a).parents("ul:eq(0)");
                var h = $(b);
                if (h.is("ins"))
                    h = h.parent();
                if (!f) {
                    var f = {};
                    f[this.container.attr("id")] = this.get_rollback();
                    if (!d) {
                        var k = a.size() > 1 ? a.eq(0).parents(".tree:eq(0)") : a.parents(".tree:eq(0)");
                        if (k.get(0) != this.container.get(0)) {
                            k = tree_component.inst[k.attr("id")];
                            f[k.container.attr("id")] = k.get_rollback()
                            }
                        delete k
                    }
                }
                if (c == "inside" && this.settings.data.async) {
                    var l = this;
                    if (this.get_node(h).hasClass("closed")) {
                        return this.open_branch(this.get_node(h), true, function() {
                            l.moved.apply(l, [a, b, c, d, e, f])
                            })
                        }
                    if (this.get_node(h).find("> ul > li > a.loading").size() == 1) {
                        setTimeout(function() {
                            l.moved.apply(l, [a, b, c, d, e])
                            }, 200);
                        return
                    }
                }
                if (a.size() > 1) {
                    var l = this;
                    var k = this.moved(a.eq(0), b, c, false, e, f);
                    a.each(function(i) {
                        if (i == 0)
                            return;
                        if (k) {
                            k = l.moved(this, k.children("a:eq(0)"), "after", false, e, f)
                            }
                    });
                    return a
                }
                if (e) {
                    _what = a.clone();
                    _what.each(function(i) {
                        this.id = this.id + "_copy";
                        $(this).find("li").each(function() {
                            this.id = this.id + "_copy"
                        });
                        $(this).removeClass("dragged").find("a.clicked").removeClass("clicked").end().find("li.dragged").removeClass("dragged")
                        })
                    } else
                    _what = a;
                if (d) {
                    if (!this.callback("beforecreate", [this.get_node(a).get(0), this.get_node(b).get(0), c, this]))
                        return false
                } else {
                    if (!this.callback("beforemove", [this.get_node(a).get(0), this.get_node(b).get(0), c, this]))
                        return false
                }
                if (!d) {
                    var k = a.parents(".tree:eq(0)");
                    if (k.get(0) != this.container.get(0)) {
                        k = tree_component.inst[k.attr("id")];
                        if (k.settings.languages.length) {
                            var m = [];
                            if (this.settings.languages.length == 0)
                                m.push("." + k.current_lang);
                            else {
                                for (var i in this.settings.languages) {
                                    if (!this.settings.languages.hasOwnProperty(i))
                                        continue;
                                    for (var j in k.settings.languages) {
                                        if (!k.settings.languages.hasOwnProperty(j))
                                            continue;
                                        if (this.settings.languages[i] == k.settings.languages[j])
                                            m.push("." + this.settings.languages[i])
                                        }
                                }
                            }
                            if (m.length == 0)
                                return this.error("MOVE: NO COMMON LANGUAGES");
                            _what.find("a").not(m.join(",")).remove()
                            }
                        _what.find("a.clicked").removeClass("clicked")
                        }
                }
                a = _what;
                switch (c) {
                case "before":
                    h.parents("ul:eq(0)").children("li.last").removeClass("last");
                    h.parent().before(a.removeClass("last"));
                    h.parents("ul:eq(0)").children("li:last").addClass("last");
                    break;
                case "after":
                    h.parents("ul:eq(0)").children("li.last").removeClass("last");
                    h.parent().after(a.removeClass("last"));
                    h.parents("ul:eq(0)").children("li:last").addClass("last");
                    break;
                case "inside":
                    if (h.parent().children("ul:first").size()) {
                        if (this.settings.rules.createat == "top") {
                            h.parent().children("ul:first").prepend(a.removeClass("last")).children("li:last").addClass("last");
                            var n = h.parent().children("ul:first").children("li:first");
                            if (n.size()) {
                                c = "before";
                                b = n
                            }
                        } else {
                            var n = h.parent().children("ul:first").children(".last");
                            if (n.size()) {
                                c = "after";
                                b = n
                            }
                            h.parent().children("ul:first").children(".last").removeClass("last").end().append(a.removeClass("last")).children("li:last").addClass("last")
                            }
                    } else {
                        a.addClass("last");
                        h.parent().removeClass("leaf").append("<ul/>");
                        if (!h.parent().hasClass("open"))
                            h.parent().addClass("closed");
                        h.parent().children("ul:first").prepend(a)
                        }
                    if (h.parent().hasClass("closed")) {
                        this.open_branch(h)
                        }
                    break;
                default:
                    break
                }
                if (g.find("li").size() == 0) {
                    var o = g.parent();
                    o.removeClass("open").removeClass("closed").addClass("leaf");
                    if (!o.is(".tree"))
                        o.children("ul").remove();
                    o.parents("ul:eq(0)").children("li.last").removeClass("last").end().children("li:last").addClass("last")
                    } else {
                    g.children("li.last").removeClass("last");
                    g.children("li:last").addClass("last")
                    }
                if (e)
                    this.callback("oncopy", [this.get_node(a).get(0), this.get_node(b).get(0), c, this, f]);
                else if (d)
                    this.callback("oncreate", [this.get_node(a).get(0), (h.is("ul") ? -1: this.get_node(b).get(0)), c, this, f]);
                else
                    this.callback("onmove", [this.get_node(a).get(0), this.get_node(b).get(0), c, this, f]);
                return a
            },
            error: function(a) {
                this.callback("error", [a, this]);
                return false
            },
            lock: function(a) {
                this.locked = a;
                if (this.locked)
                    this.container.children("ul:eq(0)").addClass("locked");
                else
                    this.container.children("ul:eq(0)").removeClass("locked")
                },
            cut: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                a = a ? this.get_node(a) : this.container.find("a.clicked").filter(":first-child").parent();
                if (!a || !a.size())
                    return this.error("CUT: NO NODE SELECTED");
                tree_component.cut_copy.copy_nodes = false;
                tree_component.cut_copy.cut_nodes = a
            },
            copy: function(a) {
                if (this.locked)
                    return this.error("LOCKED");
                a = a ? this.get_node(a) : this.container.find("a.clicked").filter(":first-child").parent();
                if (!a || !a.size())
                    return this.error("COPY: NO NODE SELECTED");
                tree_component.cut_copy.copy_nodes = a;
                tree_component.cut_copy.cut_nodes = false
            },
            paste: function(b, c) {
                if (this.locked)
                    return this.error("LOCKED");
                var d = false;
                if (b == -1) {
                    d = true;
                    b = this.container
                } else
                    b = b ? this.get_node(b) : this.selected;
                if (!d && (!b || !b.size()))
                    return this.error("PASTE: NO NODE SELECTED");
                if (!tree_component.cut_copy.copy_nodes && !tree_component.cut_copy.cut_nodes)
                    return this.error("PASTE: NOTHING TO DO");
                var e = this;
                var f = c;
                if (c == "before") {
                    c = b.parent().children().index(b);
                    b = b.parents("li:eq(0)")
                    } else if (c == "after") {
                    c = b.parent().children().index(b) + 1;
                    b = b.parents("li:eq(0)")
                    } else if ((typeof c).toLowerCase() == "undefined" || c == "inside") {
                    c = (this.settings.rules.createat == "top") ? 0: b.children("ul:eq(0)").children("li").size()
                    }
                if (!d && b.size() == 0) {
                    d = true;
                    b = this.container
                }
                if (tree_component.cut_copy.copy_nodes && tree_component.cut_copy.copy_nodes.size()) {
                    var g = true;
                    if (!d && !this.check_move(tree_component.cut_copy.copy_nodes, b.children("a:eq(0)"), "inside"))
                        return false;
                    if (b.children("ul").size() == 0 || (d == true && b.children("ul").children("li").size() == 0)) {
                        if (!d)
                            var a = this.moved(tree_component.cut_copy.copy_nodes, b.children("a:eq(0)"), "inside", false, true);
                        else
                            var a = this.moved(tree_component.cut_copy.copy_nodes, this.container.children("ul:eq(0)"), "inside", false, true)
                        } else if (f == "before" && b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").size())
                        var a = this.moved(tree_component.cut_copy.copy_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").children("a:eq(0)"), "before", false, true);
                    else if (f == "after" && b.children("ul:eq(0)").children("li:nth-child(" + (c) + ")").size())
                        var a = this.moved(tree_component.cut_copy.copy_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c) + ")").children("a:eq(0)"), "after", false, true);
                    else if (b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").size())
                        var a = this.moved(tree_component.cut_copy.copy_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").children("a:eq(0)"), "before", false, true);
                    else
                        var a = this.moved(tree_component.cut_copy.copy_nodes, b.children("ul:eq(0)").children("li:last").children("a:eq(0)"), "after", false, true);
                    tree_component.cut_copy.copy_nodes = false
                }
                if (tree_component.cut_copy.cut_nodes && tree_component.cut_copy.cut_nodes.size()) {
                    var g = true;
                    b.parents().andSelf().each(function() {
                        if (tree_component.cut_copy.cut_nodes.index(this) != -1) {
                            g = false;
                            return false
                        }
                    });
                    if (!g)
                        return this.error("Invalid paste");
                    if (!d && !this.check_move(tree_component.cut_copy.cut_nodes, b.children("a:eq(0)"), "inside"))
                        return false;
                    if (b.children("ul").size() == 0 || (d == true && b.children("ul").children("li").size() == 0)) {
                        if (!d)
                            var a = this.moved(tree_component.cut_copy.cut_nodes, b.children("a:eq(0)"), "inside");
                        else
                            var a = this.moved(tree_component.cut_copy.cut_nodes, this.container.children("ul:eq(0)"), "inside")
                        } else if (f == "before" && b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").size())
                        var a = this.moved(tree_component.cut_copy.cut_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").children("a:eq(0)"), "before");
                    else if (f == "after" && b.children("ul:eq(0)").children("li:nth-child(" + (c) + ")").size())
                        var a = this.moved(tree_component.cut_copy.cut_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c) + ")").children("a:eq(0)"), "after");
                    else if (b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").size())
                        var a = this.moved(tree_component.cut_copy.cut_nodes, b.children("ul:eq(0)").children("li:nth-child(" + (c + 1) + ")").children("a:eq(0)"), "before");
                    else
                        var a = this.moved(tree_component.cut_copy.cut_nodes, b.children("ul:eq(0)").children("li:last").children("a:eq(0)"), "after");
                    tree_component.cut_copy.cut_nodes = false
                }
            },
            search: function(b, c) {
                var d = this;
                if (!b || (this.srch && b != this.srch)) {
                    this.srch = "";
                    this.srch_opn = false;
                    this.container.find("a.search").removeClass("search")
                    }
                this.srch = b;
                if (!b)
                    return;
                if (!c)
                    c = "contains";
                if (this.settings.data.async) {
                    if (!this.srch_opn) {
                        var e = $.extend({
                            "search": b
                        }, this.callback("beforedata", [false, this]));
                        $.ajax({
                            type: this.settings.data.opts.method,
                            url: this.settings.data.opts.url,
                            data: e,
                            dataType: "text",
                            success: function(a) {
                                d.srch_opn = $.unique(a.split(","));
                                d.search.apply(d, [b, c])
                                }
                        })
                        } else if (this.srch_opn.length) {
                        if (this.srch_opn && this.srch_opn.length) {
                            var f = false;
                            for (var j = 0; j < this.srch_opn.length; j++) {
                                if (this.get_node("#" + this.srch_opn[j]).size() > 0) {
                                    f = true;
                                    var g = "#" + this.srch_opn[j];
                                    delete this.srch_opn[j];
                                    this.open_branch(g, true, function() {
                                        d.search.apply(d, [b, c])
                                        })
                                    }
                            }
                            if (!f) {
                                this.srch_opn = [];
                                d.search.apply(d, [b, c])
                                }
                        }
                    } else {
                        this.srch_opn = false;
                        var h = "a";
                        if (this.settings.languages.length)
                            h += "." + this.current_lang;
                        this.callback("onsearch", [this.container.find(h + ":" + c + "('" + b + "')"), this])
                        }
                } else {
                    var h = "a";
                    if (this.settings.languages.length)
                        h += "." + this.current_lang;
                    var i = this.container.find(h + ":" + c + "('" + b + "')");
                    i.parents("li.closed").each(function() {
                        d.open_branch(this, true)
                        });
                    this.callback("onsearch", [i, this])
                    }
            },
            add_sheet: tree_component.add_sheet,
            destroy: function() {
                this.callback("ondestroy", [this]);
                this.container.unbind(".jstree");
                $("#" + this.container.attr("id")).die("click.jstree").die("dblclick.jstree").die("mouseover.jstree").die("mouseout.jstree").die("mousedown.jstree");
                this.container.removeClass("tree ui-widget ui-widget-content tree-default tree-" + this.settings.ui.theme_name).children("ul").removeClass("no_dots ltr locked").find("li").removeClass("leaf").removeClass("open").removeClass("closed").removeClass("last").children("a").removeClass("clicked hover search");
                if (this.cntr == tree_component.focused) {
                    for (var i in tree_component.inst) {
                        if (i != this.cntr && i != this.container.attr("id")) {
                            tree_component.inst[i].focus();
                            break
                        }
                    }
                }
                tree_component.inst[this.cntr] = false;
                tree_component.inst[this.container.attr("id")] = false;
                delete tree_component.inst[this.cntr];
                delete tree_component.inst[this.container.attr("id")];
                tree_component.cntr--
            }
        }
    };
    tree_component.cntr = 0;
    tree_component.inst = {};
    tree_component.themes = [];
    tree_component.drag_drop = {
        isdown: false,
        drag_node: false,
        drag_help: false,
        dragged: false,
        init_x: false,
        init_y: false,
        moving: false,
        origin_tree: false,
        marker: false,
        move_type: false,
        ref_node: false,
        appended: false,
        foreign: false,
        droppable: [],
        open_time: false,
        scroll_time: false
    };
    tree_component.mouseup = function(a) {
        var b = tree_component.drag_drop;
        if (b.open_time)
            clearTimeout(b.open_time);
        if (b.scroll_time)
            clearTimeout(b.scroll_time);
        if (b.moving && $.tree.drag_end !== false)
            $.tree.drag_end.call(null, a, b);
        if (b.foreign === false && b.drag_node && b.drag_node.size()) {
            b.drag_help.remove();
            if (b.move_type) {
                var c = tree_component.inst[b.ref_node.parents(".tree:eq(0)").attr("id")];
                if (c)
                    c.moved(b.dragged, b.ref_node, b.move_type, false, (b.origin_tree.settings.rules.drag_copy == "on" || (b.origin_tree.settings.rules.drag_copy == "ctrl" && a.ctrlKey)))
                }
            b.move_type = false;
            b.ref_node = false
        }
        if (b.foreign !== false) {
            if (b.drag_help)
                b.drag_help.remove();
            if (b.move_type) {
                var c = tree_component.inst[b.ref_node.parents(".tree:eq(0)").attr("id")];
                if (c)
                    c.callback("ondrop", [b.f_data, c.get_node(b.ref_node).get(0), b.move_type, c])
                }
            b.foreign = false;
            b.move_type = false;
            b.ref_node = false
        }
        if (tree_component.drag_drop.marker)
            tree_component.drag_drop.marker.hide();
        if (b.dragged && b.dragged.size())
            b.dragged.removeClass("dragged");
        b.dragged = false;
        b.drag_help = false;
        b.drag_node = false;
        b.f_type = false;
        b.f_data = false;
        b.init_x = false;
        b.init_y = false;
        b.moving = false;
        b.appended = false;
        b.origin_tree = false;
        if (b.isdown) {
            b.isdown = false;
            a.preventDefault();
            a.stopPropagation();
            return false
        }
    };
    tree_component.mousemove = function(b) {
        var c = tree_component.drag_drop;
        var d = false;
        if (c.isdown) {
            if (!c.moving && Math.abs(c.init_x - b.pageX) < 5 && Math.abs(c.init_y - b.pageY) < 5) {
                b.preventDefault();
                b.stopPropagation();
                return false
            } else {
                if (!c.moving) {
                    tree_component.drag_drop.moving = true;
                    d = true
                }
            }
            if (c.open_time)
                clearTimeout(c.open_time);
            if (c.drag_help !== false) {
                if (!c.appended) {
                    if (c.foreign !== false)
                        c.origin_tree = $.tree.focused();
                    $("body").append(c.drag_help);
                    c.w = c.drag_help.width();
                    c.appended = true
                }
                c.drag_help.css({
                    "left": (b.pageX + 5),
                    "top": (b.pageY + 15)
                    })
                }
            if (d && $.tree.drag_start !== false)
                $.tree.drag_start.call(null, b, c);
            if ($.tree.drag !== false)
                $.tree.drag.call(null, b, c);
            if (b.target.tagName == "DIV" && b.target.id == "jstree-marker")
                return false;
            var e = $(b.target);
            if (e.is("ins"))
                e = e.parent();
            var f = e.is(".tree") ? e: e.parents(".tree:eq(0)");
            if (f.size() == 0 || !tree_component.inst[f.attr("id")]) {
                if (c.scroll_time)
                    clearTimeout(c.scroll_time);
                if (c.drag_help !== false)
                    c.drag_help.find("li:eq(0) ins").addClass("forbidden");
                c.move_type = false;
                c.ref_node = false;
                tree_component.drag_drop.marker.hide();
                return false
            }
            var g = tree_component.inst[f.attr("id")];
            g.off_height();
            if (c.scroll_time)
                clearTimeout(c.scroll_time);
            c.scroll_time = setTimeout(function() {
                g.scroll_check(b.pageX, b.pageY)
                }, 50);
            var h = false;
            var j = f.scrollTop();
            if (b.target.tagName == "A" || b.target.tagName == "INS") {
                if (e.is("#jstree-dragged"))
                    return false;
                if (g.get_node(b.target).hasClass("closed")) {
                    c.open_time = setTimeout(function() {
                        g.open_branch(e)
                        }, 500)
                    }
                var k = e.offset();
                var l = {
                    x: (k.left - 1),
                    y: (b.pageY - k.top)
                    };
                var m = [];
                if (l.y < g.li_height / 3 + 1)
                    m = ["before", "inside", "after"];
                else if (l.y > g.li_height * 2 / 3 - 1)
                    m = ["after", "inside", "before"];
                else {
                    if (l.y < g.li_height / 2)
                        m = ["inside", "before", "after"];
                    else
                        m = ["inside", "after", "before"]
                    }
                var n = false;
                var o = (c.foreign == false) ? c.origin_tree.container.find("li.dragged") : c.f_type;
                $.each(m, function(i, a) {
                    if (g.check_move(o, e, a)) {
                        h = a;
                        n = true;
                        return false
                    }
                });
                if (n) {
                    switch (h) {
                    case "before":
                        l.y = k.top - 2;
                        tree_component.drag_drop.marker.attr("class", "marker");
                        break;
                    case "after":
                        l.y = k.top - 2 + g.li_height;
                        tree_component.drag_drop.marker.attr("class", "marker");
                        break;
                    case "inside":
                        l.x -= 2;
                        l.y = k.top - 2 + g.li_height / 2;
                        tree_component.drag_drop.marker.attr("class", "marker_plus");
                        break
                    }
                    c.move_type = h;
                    c.ref_node = $(b.target);
                    if (c.drag_help !== false)
                        c.drag_help.find(".forbidden").removeClass("forbidden");
                    tree_component.drag_drop.marker.css({
                        "left": l.x,
                        "top": l.y
                    }).show()
                    }
            }
            if ((e.is(".tree") || e.is("ul")) && e.find("li:eq(0)").size() == 0) {
                var k = e.offset();
                c.move_type = "inside";
                c.ref_node = f.children("ul:eq(0)");
                if (c.drag_help !== false)
                    c.drag_help.find(".forbidden").removeClass("forbidden");
                tree_component.drag_drop.marker.attr("class", "marker_plus");
                tree_component.drag_drop.marker.css({
                    "left": (k.left + 10),
                    "top": k.top + 15
                }).show()
                } else if ((b.target.tagName != "A" && b.target.tagName != "INS") || !n) {
                if (c.drag_help !== false)
                    c.drag_help.find("li:eq(0) ins").addClass("forbidden");
                c.move_type = false;
                c.ref_node = false;
                tree_component.drag_drop.marker.hide()
                }
            b.preventDefault();
            b.stopPropagation();
            return false
        }
        return true
    };
    $(function() {
        $(document).bind("mousemove.jstree", tree_component.mousemove);
        $(document).bind("mouseup.jstree", tree_component.mouseup)
        });
    tree_component.cut_copy = {
        copy_nodes: false,
        cut_nodes: false
    };
    tree_component.css = false;
    tree_component.get_css = function(a, b) {
        a = a.toLowerCase();
        var c = tree_component.css.cssRules || tree_component.css.rules;
        var j = 0;
        do {
            if (c.length && j > c.length + 5)
                return false;
            if (c[j].selectorText && c[j].selectorText.toLowerCase() == a) {
                if (b == true) {
                    if (tree_component.css.removeRule)
                        document.styleSheets[i].removeRule(j);
                    if (tree_component.css.deleteRule)
                        document.styleSheets[i].deleteRule(j);
                    return true
                } else
                    return c[j]
                }
        }
        while (c[++j]);
        return false
    };
    tree_component.add_css = function(a) {
        if (tree_component.get_css(a))
            return false; (tree_component.css.insertRule) ? tree_component.css.insertRule(a + ' { }', 0) : tree_component.css.addRule(a, null, 0);
        return tree_component.get_css(a)
        };
    tree_component.remove_css = function(a) {
        return tree_component.get_css(a, true)
        };
    tree_component.add_sheet = function(a) {
        if (a.str) {
            var b = document.createElement("style");
            b.setAttribute('type', "text/css");
            if (b.styleSheet) {
                document.getElementsByTagName("head")[0].appendChild(b);
                b.styleSheet.cssText = a.str
            } else {
                b.appendChild(document.createTextNode(a.str));
                document.getElementsByTagName("head")[0].appendChild(b)
                }
            return b.sheet || b.styleSheet
        }
        if (a.url) {
            if (document.createStyleSheet) {
                try {
                    document.createStyleSheet(a.url)
                    } catch(e) {}
            } else {
                var c = document.createElement('link');
                c.rel = 'stylesheet';
                c.type = 'text/css';
                c.media = "all";
                c.href = a.url;
                document.getElementsByTagName("head")[0].appendChild(c);
                return c.styleSheet
            }
        }
    };
    $(function() {
        var u = navigator.userAgent.toLowerCase();
        var v = (u.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [0, '0'])[1];
        var a = '/* TREE LAYOUT */ .tree ul { margin:0 0 0 5px; padding:0 0 0 0; list-style-type:none; } .tree li { display:block; min-height:18px; line-height:18px; padding:0 0 0 15px; margin:0 0 0 0; /* Background fix */ clear:both; } .tree li ul { display:none; } .tree li a, .tree li span { display:inline-block;line-height:16px;height:16px;color:black;white-space:nowrap;text-decoration:none;padding:1px 4px 1px 4px;margin:0; } .tree li a:focus { outline: none; } .tree li a input, .tree li span input { margin:0;padding:0 0;display:inline-block;height:12px !important;border:1px solid white;background:white;font-size:10px;font-family:Verdana; } .tree li a input:not([class="xxx"]), .tree li span input:not([class="xxx"]) { padding:1px 0; } /* FOR DOTS */ .tree .ltr li.last { float:left; } .tree > ul li.last { overflow:visible; } /* OPEN OR CLOSE */ .tree li.open ul { display:block; } .tree li.closed ul { display:none !important; } /* FOR DRAGGING */ #jstree-dragged { position:absolute; top:-10px; left:-10px; margin:0; padding:0; } #jstree-dragged ul ul ul { display:none; } #jstree-marker { padding:0; margin:0; line-height:5px; font-size:1px; overflow:hidden; height:5px; position:absolute; left:-45px; top:-30px; z-index:1000; background-color:transparent; background-repeat:no-repeat; display:none; } #jstree-marker.marker { width:45px; background-position:-32px top; } #jstree-marker.marker_plus { width:5px; background-position:right top; } /* BACKGROUND DOTS */ .tree li li { overflow:hidden; } .tree > .ltr > li { display:table; } /* ICONS */ .tree ul ins { display:inline-block; text-decoration:none; width:16px; height:16px; } .tree .ltr ins { margin:0 4px 0 0px; } ';
        //if ($.browser.msie) {
        //    if ($.browser.version == 6)
        //        a += '.tree li { height:18px; zoom:1; } .tree li li { overflow:visible; } .tree .ltr li.last { margin-top: expression( (this.previousSibling && /open/.test(this.previousSibling.className) ) ? "-2px" : "0"); } .marker { width:45px; background-position:-32px top; } .marker_plus { width:5px; background-position:right top; }';
        //    if ($.browser.version == 7)
        //        a += '.tree li li { overflow:visible; } .tree .ltr li.last { margin-top: expression( (this.previousSibling && /open/.test(this.previousSibling.className) ) ? "-2px" : "0"); }'
        //}
        //if ($.browser.opera)
        //    a += '.tree > ul > li.last:after { content:"."; display: block; height:1px; clear:both; visibility:hidden; }';
        //if ($.browser.mozilla && $.browser.version.indexOf("1.8") == 0)
        //    a += '.tree .ltr li a { display:inline; float:left; } .tree li ul { clear:both; }';
        tree_component.css = tree_component.add_sheet({
            str: a
        })
        })
    })(jQuery); (function($) {
    $.extend($.tree.datastores, {
        "html": function() {
            return {
                get: function(a, b, c) {
                    return a && $(a).size() ? $('<div>').append(b.get_node(a).clone()).html() : b.container.children("ul:eq(0)").html()
                    },
                parse: function(a, b, c, d) {
                    if (d)
                        d.call(null, a);
                    return a
                },
                load: function(e, f, g, h) {
                    if (g.url) {
                        $.ajax({
                            'type': g.method,
                            'url': g.url,
                            'data': e,
                            'dataType': "html",
                            'success': function(d, a) {
                                h.call(null, d)
                                },
                            'error': function(a, b, c) {
                                h.call(null, false);
                                f.error(c + " " + b)
                                }
                        })
                        } else {
                        h.call(null, g.static || f.container.children("ul:eq(0)").html())
                        }
                }
            }
        },
        "json": function() {
            return {
                get: function(b, c, d) {
                    var e = this;
                    if (!b || $(b).size() == 0)
                        b = c.container.children("ul").children("li");
                    else
                        b = $(b);
                    if (!d)
                        d = {};
                    if (!d.outer_attrib)
                        d.outer_attrib = ["id", "rel", "class"];
                    if (!d.inner_attrib)
                        d.inner_attrib = [];
                    if (b.size() > 1) {
                        var f = [];
                        b.each(function() {
                            f.push(e.get(this, c, d))
                            });
                        return f
                    }
                    if (b.size() == 0)
                        return [];
                    var g = {
                        attributes: {},
                        data: {}
                    };
                    if (b.hasClass("open"))
                        g.data.state = "open";
                    if (b.hasClass("closed"))
                        g.data.state = "closed";
                    for (var i in d.outer_attrib) {
                        if (!d.outer_attrib.hasOwnProperty(i))
                            continue;
                        var h = (d.outer_attrib[i] == "class") ? b.attr(d.outer_attrib[i]).replace(/(^| )last( |$)/ig, " ").replace(/(^| )(leaf|closed|open)( |$)/ig, " ") : b.attr(d.outer_attrib[i]);
                        if (typeof h != "undefined" && h.toString().replace(" ", "").length > 0)
                            g.attributes[d.outer_attrib[i]] = h;
                        delete h
                    }
                    if (c.settings.languages.length) {
                        for (var i in c.settings.languages) {
                            if (!c.settings.languages.hasOwnProperty(i))
                                continue;
                            var a = b.children("a." + c.settings.languages[i]);
                            if (d.force || d.inner_attrib.length || a.children("ins").get(0).style.backgroundImage.toString().length || a.children("ins").get(0).className.length) {
                                g.data[c.settings.languages[i]] = {};
                                g.data[c.settings.languages[i]].title = c.get_text(b, c.settings.languages[i]);
                                if (a.children("ins").get(0).style.className.length) {
                                    g.data[c.settings.languages[i]].icon = a.children("ins").get(0).style.className
                                }
                                if (a.children("ins").get(0).style.backgroundImage.length) {
                                    g.data[c.settings.languages[i]].icon = a.children("ins").get(0).style.backgroundImage.replace("url(", "").replace(")", "")
                                    }
                                if (d.inner_attrib.length) {
                                    g.data[c.settings.languages[i]].attributes = {};
                                    for (var j in d.inner_attrib) {
                                        if (!d.inner_attrib.hasOwnProperty(j))
                                            continue;
                                        var h = a.attr(d.inner_attrib[j]);
                                        if (typeof h != "undefined" && h.toString().replace(" ", "").length > 0)
                                            g.data[c.settings.languages[i]].attributes[d.inner_attrib[j]] = h;
                                        delete h
                                    }
                                }
                            } else {
                                g.data[c.settings.languages[i]] = c.get_text(b, c.settings.languages[i])
                                }
                        }
                    } else {
                        var a = b.children("a");
                        g.data.title = c.get_text(b);
                        if (a.children("ins").size() && a.children("ins").get(0).className.length) {
                            g.data.icon = a.children("ins").get(0).className
                        }
                        if (a.children("ins").size() && a.children("ins").get(0).style.backgroundImage.length) {
                            g.data.icon = a.children("ins").get(0).style.backgroundImage.replace("url(", "").replace(")", "")
                            }
                        if (d.inner_attrib.length) {
                            g.data.attributes = {};
                            for (var j in d.inner_attrib) {
                                if (!d.inner_attrib.hasOwnProperty(j))
                                    continue;
                                var h = a.attr(d.inner_attrib[j]);
                                if (typeof h != "undefined" && h.toString().replace(" ", "").length > 0)
                                    g.data.attributes[d.inner_attrib[j]] = h;
                                delete h
                            }
                        }
                    }
                    if (b.children("ul").size() > 0) {
                        g.children = [];
                        b.children("ul").children("li").each(function() {
                            g.children.push(e.get(this, c, d))
                            })
                        }
                    return g
                },
                parse: function(a, b, c, d) {
                    if (Object.prototype.toString.apply(a) === "[object Array]") {
                        var e = '';
                        for (var i = 0; i < a.length; i++) {
                            if (typeof a[i] == "function")
                                continue;
                            e += this.parse(a[i], b, c)
                            }
                        if (d)
                            d.call(null, e);
                        return e
                    }
                    if (!a || !a.data) {
                        if (d)
                            d.call(null, false);
                        return ""
                    }
                    var e = '';
                    e += "<li ";
                    var f = false;
                    if (a.attributes) {
                        for (var i in a.attributes) {
                            if (!a.attributes.hasOwnProperty(i))
                                continue;
                            if (i == "class") {
                                e += " class='" + a.attributes[i] + " ";
                                if (a.state == "closed" || a.state == "open")
                                    e += " " + a.state + " ";
                                e += "' ";
                                f = true
                            } else
                                e += " " + i + "='" + a.attributes[i] + "' "
                        }
                    }
                    if (!f && (a.state == "closed" || a.state == "open"))
                        e += " class='" + a.state + "' ";
                    e += ">";
                    if (b.settings.languages.length) {
                        for (var i = 0; i < b.settings.languages.length; i++) {
                            var g = {};
                            g["href"] = "";
                            g["style"] = "";
                            g["class"] = b.settings.languages[i];
                            if (a.data[b.settings.languages[i]] && (typeof a.data[b.settings.languages[i]].attributes).toLowerCase() != "undefined") {
                                for (var j in a.data[b.settings.languages[i]].attributes) {
                                    if (!a.data[b.settings.languages[i]].attributes.hasOwnProperty(j))
                                        continue;
                                    if (j == "style" || j == "class")
                                        g[j] += " " + a.data[b.settings.languages[i]].attributes[j];
                                    else
                                        g[j] = a.data[b.settings.languages[i]].attributes[j]
                                    }
                            }
                            e += "<a";
                            for (var j in g) {
                                if (!g.hasOwnProperty(j))
                                    continue;
                                e += ' ' + j + '="' + g[j] + '" '
                            }
                            e += ">";
                            if (a.data[b.settings.languages[i]] && a.data[b.settings.languages[i]].icon) {
                                e += "<ins " + (a.data[b.settings.languages[i]].icon.indexOf("/") == -1 ? " class='" + a.data[b.settings.languages[i]].icon + "' ": " style='background-image:url(\"" + a.data[b.settings.languages[i]].icon + "\");' ") + ">&nbsp;</ins>"
                            } else
                                e += "<ins>&nbsp;</ins>";
                            e += ((typeof a.data[b.settings.languages[i]].title).toLowerCase() != "undefined" ? a.data[b.settings.languages[i]].title: a.data[b.settings.languages[i]]) + "</a>"
                        }
                    } else {
                        var g = {};
                        g["href"] = "";
                        g["style"] = "";
                        g["class"] = "";
                        if ((typeof a.data.attributes).toLowerCase() != "undefined") {
                            for (var i in a.data.attributes) {
                                if (!a.data.attributes.hasOwnProperty(i))
                                    continue;
                                if (i == "style" || i == "class")
                                    g[i] += " " + a.data.attributes[i];
                                else
                                    g[i] = a.data.attributes[i]
                                }
                        }
                        e += "<a";
                        for (var i in g) {
                            if (!g.hasOwnProperty(i))
                                continue;
                            e += ' ' + i + '="' + g[i] + '" '
                        }
                        e += ">";
                        if (a.data.icon) {
                            e += "<ins " + (a.data.icon.indexOf("/") == -1 ? " class='" + a.data.icon + "' ": " style='background-image:url(\"" + a.data.icon + "\");' ") + ">&nbsp;</ins>"
                        } else
                            e += "<ins>&nbsp;</ins>";
                        e += ((typeof a.data.title).toLowerCase() != "undefined" ? a.data.title: a.data) + "</a>"
                    }
                    if (a.children && a.children.length) {
                        e += '<ul>';
                        for (var i = 0; i < a.children.length; i++) {
                            e += this.parse(a.children[i], b, c)
                            }
                        e += '</ul>'
                    }
                    e += "</li>";
                    if (d)
                        d.call(null, e);
                    return e
                },
                load: function(e, f, g, h) {
                    if (g.static) {
                        h.call(null, g.static)
                        } else {
                        $.ajax({
                            'type': g.method,
                            'url': g.url,
                            'data': e,
                            'dataType': "json",
                            'success': function(d, a) {
                                h.call(null, d)
                                },
                            'error': function(a, b, c) {
                                h.call(null, false);
                                f.error(c + " " + b)
                                }
                        })
                        }
                }
            }
        }
    })
    })(jQuery);

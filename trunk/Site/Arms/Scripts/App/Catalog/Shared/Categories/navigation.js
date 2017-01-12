define('Catalog/Shared/Categories/navigation', [], function() {

    // --------------- API

    var api = {
        categoryNavigation: function (element, options) {
            if (!options)
                return element.data('categoryNavigation');

            var categoryNavigationObject = new categoryNavigation(element, options);

            element.data('categoryNavigation', categoryNavigationObject);

            return categoryNavigationObject;
        }
    };

    return api;
    
    function categoryNavigation(element, options) {

        // ----------- Данные

        var defaults = {
            autoCollapse: false,
            withSelectableItems: true,
            enabled: true,
            selectionEnabled: true,
            element: $(element),
            selected: null
        };

        var settings = $.extend({}, defaults, options);
        
        // ----------- Инициализация

        settings.element
            .find('li')
            .not(':has(li)')
            .find('i')
            .hide();

        // ----------- Обработчики событий

        settings.element.on('click', function (e) {
            e.stopPropagation();
        });

        settings.element.on('click', '[data-x="shared/categories/expand_subcat"]', settings.autoCollapse ? toggleDropdownHandler : toggleSubtreeHandler);

        if (settings.withSelectableItems)
            settings.element.on('click', '[data-x="shared/categories/item"]', selectHandler);

        var doc = $(document);

        doc.on('click', collapseAllCategories);

        doc.on('click', '[data-toggle=dropdown]', collapseAllCategories);

        doc.on('categoryMovedUp', function (e, id) {
            moveCategoryUp(id);
        });

        doc.on('categoryMovedDown', function (e, id) {
            moveCategoryDown(id);
        });

        doc.on('categoryMovedIn', function (e, id, targetId) {
            moveCategoryTo(id, targetId);
        });

        doc.on('categoryCreated', function (e, id, parentId, title) {
            createCategory(id, parentId, title);
        });

        doc.on('categoryDeleted', function (e, id) {
            deleteCategory(id);
        });

        doc.on('categoryRenamed', function (e, id, title) {
            renameCategory(id, title);
        });

        // ----------- Публичные методы

        this.enable = function(enable) {
            settings.enable = enable;
        };

        this.enableSelection = function(enable) {
            settings.selectionEnabled = enable;
        };

        this.resetSelection = function() {
            settings.selected = null;
            settings.element.find('.selected').removeClass('selected');
            settings.element.trigger('selectedChanged', [null]);
        };

        this.getSelection = function() {
            return settings.selected;
        };

        this.enableCategories = function(ids, enable) {
            var items = settings.element.find('[data-x="shared/categories/item"]');
            if (ids)
                items = items.filter(function () {
                    var index = $.inArray(parseInt($(this).attr('href').slice(1)), ids);
                    return index != -1;
                });

            items.each(function(index, item) {
                enableItem(item, enable);
            });
        };

        this.expandItem = function(id, expand) {
            findBranch(id ? id : -1)[expand ? 'addClass' : 'removeClass']('expandedcat');
        };
        
        // ----------- Приватные методы
        
        function toggleSubtreeHandler() {
            if (!settings.enabled)
                return false;

            $(this).closest('li').toggleClass('expandedcat');
            return false;
        }
        
        function toggleDropdownHandler() {
            if (!settings.enabled)
                return;

            var item = $(this).closest('li');
            var siblings = item.siblings('li.expandedcat');
            var descendants = siblings.find('li.expandedcat');

            siblings.removeClass('expandedcat');
            descendants.removeClass('expandedcat');
            item.toggleClass('expandedcat');
        }
        
        function selectHandler() {
            if (settings.selectionEnabled)
                selectCategory($(this));
            return false;
        }

        function collapseAllCategories() {
            settings.element.find('li.dropcatitem.expandedcat').removeClass('expandedcat');
        }

        function selectCategory(link) {
            if (link.hasClass('disabled'))
                return;

            var id = link.attr('href').replace('#', '');

            settings.selected = id;

            settings.element.find('.selected').removeClass('selected');
            link.addClass('selected');

            settings.element.trigger('selectedChanged', [id]);
        }

        function enableItem(item, enable) {
            if (enable)
                $(item).removeClass('disabled');
            else
                $(item).addClass('disabled');
        }
        
        function findBranch(id) {
            return settings.element.find('[data-x="shared/categories/item"][href="#' + id + '"]').closest('li');
        }

        function moveCategoryUp(id) {
            var branch = findBranch(id);
            if (!branch)
                return;

            var prev = branch.prev('li');
            if (!prev)
                return;

            prev.before(branch);
        }

        function moveCategoryDown(id) {
            var branch = findBranch(id);
            if (!branch)
                return;

            var next = branch.next('li');
            if (!next)
                return;

            next.after(branch);
        }

        function moveCategoryTo(id, targetId) {
            var branch = findBranch(id);
            if (!branch)
                return;

            var parent = branch.closest('ul');

            if (!targetId) {
                settings.element.append(branch);
                return;
            }

            var targetBranch = findBranch(targetId);
            if (!targetBranch)
                return;

            targetBranch.find('ul').first().append(branch);
            if (parent.hasClass('dropcatcategory1') && !parent.closest('li').find('li').length)
                parent.closest('li').find('i').hide();
            targetBranch.find('i').first().show();
        }

        function createCategory(id, parentId, title) {
            var targetBranch = findBranch(parentId ? parentId : -1);
            if (!targetBranch)
                return;

            var branch = $('<li class="dropcatitem"><i data-x="shared/categories/expand_subcat"><b></b></i><a href="#" data-x="shared/categories/item"></a><ul class="dropcatcategory1"></ul></li>')
                .hide()
                .find('a').attr('href', '#' + id).text(title).end()
                .find('i').hide().end();

            targetBranch.find('ul').first().append(branch);
            targetBranch.find('i').first().show();

            branch.show();
        }

        function deleteCategory(id) {
            var branch = findBranch(id);
            if (!branch)
                return;

            var parent = branch.closest('ul');

            branch.remove();
            if (parent.hasClass('dropcatcategory1') && !parent.closest('li').find('li').length)
                parent.closest('li').find('i').hide();
        }

        function renameCategory(id, title) {
            var branch = findBranch(id);
            if (!branch)
                return;

            branch.find('a').text(title);
        }
    }
});

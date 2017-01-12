define('Catalog/Categories/move', ['Catalog/Shared/Categories/helpers', 'Catalog/Shared/Categories/navigation'], function (h, nav) {
    return function(options) {

        // --------------- Данные

        var tree = $(options.tree),
            modal = $('[data-x="categories/move/modal"]'),
            catTree = options.catTree,
            catTreeNavigator = nav.categoryNavigation(catTree, {}),
            form = modal,
            submitButton = form.find('button[type=submit]'),
            url = form.attr('action'),
            idToMove;

        // --------------- События


        tree.on('click', '[data-x="categories/move/in"]', function() {
            var tr = $(this).closest('tr');
            openAppendModal(tr);
            return false;
        });

        catTree.on('selectedChanged', function (e, selection) {
            if (selection)
                submitButton.removeAttr('disabled');
            else
                submitButton.attr('disabled', 'disabled');
        });

        form.on('submit', function () {
            submitButton.attr('disabled', 'disabled');
            catTreeNavigator.enable(false);

            var id = idToMove,
                selectedId = catTreeNavigator.getSelection(),
                targetId = selectedId != null && selectedId >= 0 ? selectedId : null,
                tr = h.node(id),
                target = h.node(targetId);

            if (targetId == h.meta(tr).parent) {
                modal.modal('hide');
                return false;
            }

            moveOnServer(id, targetId)
                .done(function(response){
                    moveInUi(tr, target, response);
                })
                .fail(function (response) {
                    h.error('Ошибка при попытке перемещения категории', response);
                });

            return false;
        });

        form.on('click', function (e) {
            e.stopPropagation();
        });

        tree.on('click', '[data-x="categories/move/up"]', function(e){
            e.preventDefault();

            var tr = $(this).closest('tr'),
                meta = h.meta(tr),
                prev = tr.prevAll('.category-'+meta.depth).first(),
                prevMeta = h.meta(prev);

            if (!prev.length || prevMeta.parent != meta.parent) {
                return;
            }

            orderOnServer(meta.id, prevMeta.id, 'Before');
            upInUi(tr, prev);
        });

        tree.on('click', '[data-x="categories/move/down"]', function(e){
            e.preventDefault();

            var tr = $(this).closest('tr'),
                meta = h.meta(tr),
                next = h.branch(tr).last().next('tr'),
                nextMeta = h.meta(next);

            if (!next.length || nextMeta.parent != meta.parent) {
                return;
            }

            orderOnServer(meta.id, nextMeta.id, 'After');
            downInUi(tr, next);
        });


        // --------------- Действия


        function openAppendModal(tr) {
            var meta = h.meta(tr);

            form.reset();

            idToMove = meta.id;
            modal.find('h3').text('Переместить «' + h.title(tr) + '»');
            catTreeNavigator.resetSelection();
            catTreeNavigator.enable(true);

            // обновить список категорий
            
            catTreeNavigator.enableCategories(null, false);

            var ids = [ -1 ];
            tree.find('tbody tr')
                .not(h.branch(tr))
                .each(function () {
                    var m = h.meta($(this));
                    if (!m.online) {
                        ids.push(m.id);
                    }
                });
            catTreeNavigator.enableCategories(ids, true);
            
            // раскрыть корневую категорию
            var rootCat = catTree.find('[data-x="shared/categories/item"][href="#-1"]');
            rootCat.closest('li').addClass('expandedcat');

            modal.modal('show');
        }

        function moveOnServer(id, targetId) {
            return $.post(url, { Id: id, TargetId: targetId });
        }

        function moveInUi(tr, target, response) {
            if (!response || response.success) {
                modal.modal('hide');

                var meta = h.meta(tr),
                    branch = h.branch(tr),
                    newBranch = $( $.trim(response.data) ),
                    parent =  h.node(meta.parent);

                branch.each(function(){
                    h.unselect($(this)); // снимаем выбор с удаляемых категорий
                    $(this).remove();
                });
                newBranch.first().show();

                h.updateIfEmpty(parent);

                if (!target.length) {
                    tree.find('tbody').append(newBranch);
                } else {
                    var targetBranch = h.branch(target);
                    targetBranch.last().after(newBranch);

                    target.removeClass('empty');

                    parent = target;
                    while (parent) {
                        if (parent.hasClass('closed')) {
                            newBranch.hide();
                            break;
                        }
                        var parentMeta = h.meta(parent);
                        parent = parentMeta.parent != null ? h.node(parentMeta.parent) : null;
                    }
                }

                $(document).trigger('categoryMovedIn', [meta.id, target.length ? h.meta(target).id : -1]);
                return;
            }

            if (catTreeNavigator.getSelection() != null)
                submitButton.removeAttr('disabled');
            else
                submitButton.attr('disabled', 'disabled');
            catTreeNavigator.enable(true);
            
            form.showErrors(response.errors);
        }

        function upInUi(tr, prev) {
            prev.before(h.branch(tr));

            $(document).trigger('categoryMovedUp', [h.meta(tr).id]);
        }

        function downInUi(tr, next) {
            h.branch(next).last().after(h.branch(tr));

            $(document).trigger('categoryMovedDown', [h.meta(tr).id]);
        }

        function orderOnServer(id, targetId, direction) {
            $.post(options.reorderUrl, { id: id, targetId: targetId, type: direction })
                .fail(function (response) {
                    h.error('Ошибка при попытке перемещения категории', response);
                });
        }

    };
});

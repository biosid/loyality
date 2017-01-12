define('AdminSecurity/Shared/permissions', [], function () {
    return function () {

        // --------------- Данные

        // --------------- Инициализация

        $('[data-x="adminsecurity/shared/p_scope"]').each(function () { setupScopePermission($(this)); });
        $('[data-x="adminsecurity/shared/p_checkbox"]').each(function () { setupCheckboxPermission($(this)); });
        $('[data-x="adminsecurity/shared/p_list"]').each(function () { setupListPermission($(this)); });

        // --------------- События

        // --------------- Действия

        function setupPermission(editor) {
            var container = editor.closest('[data-x="adminsecurity/shared/p_container"]');
            var children = container.find('[data-x="adminsecurity/shared/p_children"]').first();

            var permission = {
                container: container,
                editor: editor,
                children: children,
                isInherited: editor.data('permission-is-inherited'),

                childContainers: children.children().map(function() {
                    var childContainer = $(this).find('[data-x="adminsecurity/shared/p_container"]').first();
                    if (childContainer.length) {
                        return childContainer[0];
                    } else {
                        return null;
                    }
                }).get(),

                enable: function(enabled) {
                    this.enableEditor && this.enableEditor(enabled);
                    this.enableChildren(enabled && (!this.isEnabled || this.isEnabled() || this.isInherited));
                },

                enableChildren: function(enabled) {
                    $.each(this.childContainers, function (index, childContainer) {
                        var childPermission = $(childContainer).data('adminsecurity-permission');
                        childPermission.enable(enabled);
                    });
                },

                update: function() {
                    this.enableChildren(!this.isEnabled || this.isEnabled() || this.isInherited);
                }
            };

            container.data('adminsecurity-permission', permission);
            return permission;
        }

        function setupScopePermission(editor) {
            setupPermission(editor);
        }

        function setupCheckboxPermission(editor) {
            var permission = setupPermission(editor);

            permission.checkbox = permission.editor.find('input');

            permission.isEnabled = function () {
                return this.checkbox.is(':checked');
            };

            permission.enableEditor = function (enabled) {
                permission.checkbox.prop('disabled', !enabled);
            };

            permission.checkbox.on('change', function () {
                permission.update();
            });
        }

        function setupListPermission(editor) {
            var permission = setupPermission(editor);

            permission.radios = permission.editor.find('input');
            permission.wildcard = $(permission.radios[0]);
            permission.list = permission.editor.find('select');

            function format(item) {
                var option = item.element;
                var labels = $(option).data('inherited-from-labels');
                return item.text + labels;
            }

            permission.list.select2({
                closeOnSelect: false,
                formatResult: format,
                formatSelection: format
            });

            permission.isEnabled = function () {
                return this.wildcard.is(':checked') || this.list.select2('val').length;
            };

            permission.enableEditor = function (enabled) {
                this.radios.prop('disabled', !enabled);
                this.list.select2('enable', enabled && !this.wildcard.is(':checked'));
            };

            permission.radios.on('change', function () {
                var checked = permission.wildcard.is(':checked');
                permission.list.select2('enable', !checked);

                permission.update();
            });

            permission.list.on('change', function () {
                permission.update();
            });
        }
    };
});

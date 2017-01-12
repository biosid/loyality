define('Actions/Edit/index', ['Shared/dtinterval', 'Actions/Edit/predicate'], function (dti, p) {
    return function (options) {
        // --------------- Данные

        var priorityText = $('[data-x="actions/actual/edit/priority"]'),
            form = $('[data-x="actions/actual/edit/form"]'),
            predicateInput = $('input[name="Predicate"]'),
            metadataInput = $('input[name="Metadata"]'),
            factorTemplate = $('[data-x="actions/actual/edit/factor_template"]'),
            metadata,
            predicateControl = $('[data-x="actions/actual/edit/filter_builder"]'),
            mechanicType = $('[data-x="actions/actual/edit/mechanic_type"]'),
            isExclusiveCheckBox = $('[data-x="actions/actual/edit/isExclusive"]');

        // --------------- Инициализация

        dti.setupDtInterval('#from', '#to', { minDate: new Date(2000, 0, 1) });

        priorityText.tooltip({
            animation: 'true',
            html: 'true',
            placement: 'top',
            title: 'от 1 до 1000',
            trigger: 'focus',
            delay: { hide: 100 }
        });

        metadata = JSON.parse(metadataInput.val());

        setupFilterBuilders();

        // --------------- События

        form.on('submit', function () {
            renumberConditionFactors();
            return updatePredicateInputs();
        });

        $('[data-x="actions/actual/edit/remove_factor"]').on('click', removeFactorRow);

        $('[data-x="actions/actual/edit/add_factor"]').on('click', function () {
            addFactorRow($(this).closest('tr'));
        });

        isExclusiveCheckBox.on('change', function () {
            if (isExclusiveCheckBox.prop("checked")) {
                enablePriority();
            } else {
                disablePriority();
            }
        });

        mechanicType.on('change', function() {
            var selectedVal = $(this).val();

            if (selectedVal === options.baseAdditionRuleName || 
                selectedVal === options.baseMultiRuleName) {
                enablePriority();
            } else {
                disablePriority();
            }
        });

        // --------------- Действия

        function disablePriority() {
            if (mechanicType.val() !== options.baseAdditionRuleName &&
                mechanicType.val() !== options.baseMultiRuleName &&
                !isExclusiveCheckBox.prop("checked")) {

                priorityText.prop("readonly", true);
            }
        }
        
        function enablePriority() {
            priorityText.prop("readonly", false);
        }

        function getFactorPredicateControls() {
            return $('div[data-x="actions/actual/edit/cf_filter_builder"]');
        }

        function setupFilterBuilders() {
            setupFilterBuilder(predicateControl, predicateInput);
            predicateControl.on('filterBuilderChanged', function() { clearPredicateError(predicateControl); });

            getFactorPredicateControls().each(function () {
                var control = $(this);
                setupFilterBuilder(control, control.closest('td').find('input[data-x="actions/actual/edit/cf_predicate"]'));
                control.on('filterBuilderChanged', function() {
                    clearPredicateError(control);
                    clearFactorsError();
                });
            });
        }

        function setupFilterBuilder(control, input) {
            p.filterBuilder({
                filter: JSON.parse(input.val()),
                metadata: metadata,
                container: control,
                targetAudienceRegexp: new RegExp(options.targetAudienceRegexp),
                targetAudienceValue: options.targetAudienceValue
            });
            
            control.on('filterBuilderChanged', function () {
                $(this)
                    .parent()
                    .find('.field-validation-error')
                    .addClass('field-validation-valid')
                    .removeClass('field-validation-error')
                    .empty();
            });
        }

        function updatePredicateInputs() {
            var result = updatePredicateInput(predicateControl, predicateInput);
            
            getFactorPredicateControls().each(function() {
                var control = $(this);
                var input = control.closest('td').find('input[data-x="actions/actual/edit/cf_predicate"]');
                result = result && updatePredicateInput(control, input);
            });

            return result;
        }

        function updatePredicateInput(control, input) {
            var builder = control.data('filterbuilder');
            if (!builder.validate())
                return false;
            
            var predicate = JSON.stringify(builder.getFilter());
            input.val(predicate);
            return true;
        }

        function renumberConditionFactors() {
            var index = 0;
            $('[data-x="actions/actual/edit/factor"]').each(function() {
                var indexStr = '[' + index + ']';
                index++;

                $(this).find('[name]').each(function() {
                    $(this).attr('name', $(this).attr('name').replace(/\[[0-9]+\]/, indexStr));
                });
            });
        }

        function removeFactorRow() {
            clearFactorsError();
            
            $(this).closest('tr').remove();
        }

        function addFactorRow(lastRow) {
            clearFactorsError();
            
            var row = factorTemplate.clone(true, true).attr('data-x', 'actions/actual/edit/factor');

            row.find('[data-x="actions/actual/edit/remove_factor"]').on('click', removeFactorRow);
            row.find('[data-x="actions/actual/edit/factor_template_factor"]').attr('name', options.cfFactorModelName);
            row.find('[data-x="actions/actual/edit/factor_template_priority"]').attr('name', options.cfPriorityModelName);

            var input = row.find('[data-x="actions/actual/edit/factor_template_predicate"]')
                .attr('name', options.cfPredicateModelName)
                .attr('data-x', 'actions/actual/edit/cf_predicate');

            var control = row
                .find('[data-x="actions/actual/edit/factor_template_filter_builder"]')
                .attr('data-x', 'actions/actual/edit/cf_filter_builder');

            setupFilterBuilder(control, input);
            control.on('filterBuilderChanged', function () {
                clearPredicateError(control);
                clearFactorsError();
            });

            row.insertBefore(lastRow).show();
        }
        
        function clearPredicateError(control) {
            control
                .parent()
                .find('.field-validation-error')
                .addClass('field-validation-valid')
                .removeClass('field-validation-error')
                .empty();
        }
        
        function clearFactorsError() {
            $('[data-x="actions/edit/factors_error"]')
                .find('.field-validation-error')
                .addClass('field-validation-valid')
                .removeClass('field-validation-error')
                .empty();
        }
    };
});

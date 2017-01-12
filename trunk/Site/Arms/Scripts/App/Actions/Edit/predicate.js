define('Actions/Edit/predicate', [], function() {

    var o = {
        filterBuilder: function(options) {
            var builder = new filterBuilder(options);
            options.container.data('filterbuilder', builder);
            return builder;
        }
    };

    return o;

    function filterBuilder(options) {

        var defaults = {
            metadata: {},
            filter: {},
            targetAudienceRegexp: /A[0-9]+/,
            targetAudienceValue: '[[A]]',
            container: $([]),
            dictionaries: [],
            dictionariesUrl: null
        };

        var settings = $.extend({}, defaults, options);

        var noneValueEditor = {
            buildControl: function() { return $('<div>'); },
            validate: function() { return true; },
            setValue: function() {},
            getValue: function() {}
        };

        var dictionaryValueEditor = {
            buildControl: function(response) {
                var control = $('<select title="Выбор значения">');

                if ($.inArray(response.DictionaryId, settings.dictionaries)) {
                    $.each(settings.dictionaries[response.DictionaryId].Data, function(i, dictData) {
                        control.append($('<option>')
                            .attr('value', dictData.Value)
                            .text(dictData.FinalText));
                    });
                }

                return control;
            },

            validate: function() {
                return true;
            },

            setValue: function(control, value) {
                control.find('select').val(value[0]);
            },

            getValue: function(control) {
                return control.find('select').val();
            }
        };

        var numericValueEditor = {
            buildControl: function() {
                return $('<span class="Label"><input title="Введите числовое значение" type="text"></span>').numeric();
            },

            validate: function(control) {
                var val = $.trim(control.find('[type=text]').val());
                return val != '' && parseInt(val) == val;
            },

            setValue: function(control, value) {
                control.find('[type=text]').val(value[0]);
            },

            getValue: function(control) {
                return control.find('[type=text]').val();
            }
        };

        var dateValueEditor = {
            buildControl: function() {
                return $('<span class="Label"><input title="Введите дату" type="text"></span>');
            },

            validate: function(control) {
                var result = true;

                var re = /^(([12]\d)|(3[01])|(0?[1-9]))\.((0?[1-9])|(1[0-2]))\.((19\d{2})|(2\d{3}))$/;
                $.each(control.find("[type=text]:visible"), function() {
                    result = re.test($(this).val());
                });

                return result;
            },

            setValue: function(control, value) {
                control.find('[type=text]').val(value[0]);
            },

            getValue: function(control) {
                return control.find('[type=text]').val();
            }
        };

        var textValueEditor = {
            buildControl: function() {
                return $('<span class="Label"><input type="text" maxlength="256"></span>');
            },

            validate: function(control) {
                var val = $.trim(control.find('[type=text]').val());
                return val != '';
            },

            setValue: function(control, value) {
                control.find('[type=text]').val(value[0]);
            },

            getValue: function(control) {
                return control.find('[type=text]').val();
            }
        };

        var unionTypes = {
            'and': 'И',
            'or': 'ИЛИ'
        };

        var operatorTypes = {
            'em': 'пустое',
            'nem': 'непустое',
            'eq': '=',
            'noteq': '<>',
            'gt': '>',
            'lt': '<',
            'lteq': '<=',
            'gteq': '>=',
            'cn': 'contain',
            'like': 'содержит',
            'between': 'между',
            'visible': 'отображается',
            'invisible': 'не отображается',
            'true': 'истина',
            'false': 'ложь',
            'audiSegments': 'сегменты',
            'audiTarget': 'целевая аудитория'
        };

        var responseTypes = {};

        function addResponseType(name) {
            responseTypes[name] = {
                controls: [],
                operators: {}
            };
        }

        function addEditorToResponseType(responseType, operators, editor) {
            var responseEditor = $.extend({ operators: operators }, editor);
            responseType.controls.push(responseEditor);
            for (var i in operators)
                responseType.operators[operators[i]] = responseEditor;
        }

        var responseNormalizers = {
            'ClientProfile.Audiences': {
                responseType: 'audience',

                normalize: function(equation) {
                    if (equation.Values && equation.Values[0] && settings.targetAudienceRegexp.test(equation.Values[0]))
                        equation.Operator = 'audiTarget';
                    else {
                        equation.Operator = 'audiSegments';
                        equation.Values = equation.Values && equation.Values[0] ? [equation.Values[0]] : [''];
                    }
                },

                denormalize: function(equation) {
                    if (equation.Operator == 'audiTarget')
                        equation.Values = [settings.targetAudienceValue];
                    equation.Operator = 'cn';
                    equation.Type = 'text';
                }
            }
        };

        //-------------------------------------------------------------------
        // нормализация/денормализация условия

        var normalizeFilter = function() {
            if (settings.filter.Union)
                normalizeUnion(settings.filter.Union);
            if (settings.filter.Operation)
                normalizeEquation(settings.filter.Operation);
        };

        var normalizeUnion = function(un) {
            for (var j in un.Unions)
                normalizeUnion(un.Unions[j]);
            for (var i in un.Operations)
                normalizeEquation(un.Operations[i]);
        };

        var normalizeEquation = function(equation) {
            var normalizer = responseNormalizers[equation.Attribute];
            if (normalizer)
                normalizer.normalize(equation);
        };

        var denormalizeEquation = function(equation) {
            var normalizer = responseNormalizers[equation.Attribute];
            if (normalizer)
                normalizer.denormalize(equation);
        };

        //-------------------------------------------------------------------
        // данные

        var currentCondition = null,
            isTreeVisible = false,
            isAutoSelection;

        //-------------------------------------------------------------------
        // дерево выбора аттрибута

        function getTreeData() {
            var treeData = [],
                typeImg = {
                    'text': 'ico-text.gif',
                    'numeric': 'ico-constantsum.gif',
                    'datetime': 'ico-datetime.gif',
                    'boolean': 'ico-boolean.gif',
                    'audience': 'ico-text.gif'
                };

            for (var mi in settings.metadata) {
                var treeItem = {
                    data: {
                        title: settings.metadata[mi].DisplayName,
                        icon: ''
                    }
                };

                if (settings.metadata[mi].Attributes) {
                    treeItem.children = [];
                    for (var ai in settings.metadata[mi].Attributes) {
                        var mattr = settings.metadata[mi].Attributes[ai];
                        if (mattr.Id == null)
                            continue;

                        // сменить тип аттрибута, если у него "специальное" название
                        if (responseNormalizers[mattr.FullName])
                            mattr.Type = responseNormalizers[mattr.FullName].responseType;

                        var pathToIcon = "/Content/images/" + typeImg[mattr.Type];

                        treeItem.children.push({
                            data: {
                                title: mattr.DisplayName,
                                icon: pathToIcon,
                                attributes: {
                                    "id": mattr.Id
                                }
                            }
                        });
                    }
                }

                treeData.push(treeItem);
            }
            return treeData;
        }

        function createTree() {
            var tree = $('<div class="ignoreCheck">');

            tree.css({
                overflow: 'auto',
                height: '250px',
                border: '1px solid black',
                background: 'White',
                'padding-bottom': '17px',
                'padding-top': '5px',
                position: 'absolute',
                'z-index': '2000'
            });

            tree.tree({
                types: {
                    "default": {
                        deletable: false,
                        renameable: false,
                        draggable: false
                    }
                },
                callback: {
                    onselect: function(node) {
                        if ($(node).children("a:eq(0)").attr("id")) {
                            setConditionResponse(currentCondition, findResponse($(node).children("a:eq(0)").attr("id")));
                            if (!isAutoSelection)
                                hideTree();
                        }
                    }
                },
                data: {
                    type: "json",
                    opts: {
                        'static': getTreeData()
                    }
                },
                ui: {
                    theme_path: '/Content/filterbuilder-tree.css'
                }
            });

            return tree;
        }

        var showTree = function() {
            isTreeVisible = true;
            treeView.show();
        };

        var hideTree = function() {
            isTreeVisible = false;
            treeView.hide();
        };

        //-------------------------------------------------------------------
        // валидация

        this.validate = function() {
            removeValidation();
            var result = true;

            settings.container.find('div.condition').each(function() {
                if (!validateCondition($(this)))
                    result = false;
            });

            settings.container.find('div.union').each(function() {
                if ($(this).children('div.condition, div.union').length < 2) {
                    $(this).prepend('<span class="field-validation-error">Количество условий в группе должно быть не менее двух</span>');
                    result = false;
                }
            });

            return result;
        };
        
        function removeValidation() {
            settings.container.find('.field-validation-error').remove();
        }

        function validateCondition(conditionControl) {
            var isvalid = false,
                response = conditionControl.data('response');

            if (response) {
                var selectedOperator = getConditionOperator(conditionControl).val();
                var type = getConditionResponseType(conditionControl);
                for (var i in type.controls) {
                    var control = type.controls[i];
                    var index = $.inArray(selectedOperator, control.operators);
                    if (index == -1)
                        continue;
                    isvalid = control.validate(conditionControl);
                }
            }

            if (isvalid) {
                conditionControl.find(".field-validation-error").remove();
                return true;
            } else {
                if (conditionControl.find(".field-validation-error").length == 0) {
                    conditionControl.find("div.lblCondition").append('<span class="field-validation-error">Условие некорректно</span>');
                }
                return false;
            }
        }

        //-------------------------------------------------------------------
        // вспомогательные функции

        function findResponse(id, name) {
            if (id == null && name == null)
                return null;
            for (var i in settings.metadata)
                for (var j in settings.metadata[i].Attributes) {
                    var attr = settings.metadata[i].Attributes[j];
                    if ((attr.FullName == name && name != null) || attr.Id == id)
                        return attr;
                }
            return null;
        }

        function removeEmptyUnion() {
            settings
                .container
                .find('div.union')
                .each(function() {
                    if ($(this).find('div.condition').length == 0) {
                        $(this).remove();
                    }
                });

            //если осталось одно объединение с одним условием
            $('*:not(div.union) > div.union').each(function() {
                if ($(this).find('div.condition').length == 1) {
                    var condition = $(this).find('div.condition').eq(0);
                    $(this).parent().prepend(condition);
                    $(this).remove();
                }
            });
        }

        function changeWidthDivBlockEdit() {
            var countNesting = 0;
            $("#divBlockEdit").width(750);
            $(".filterBuilderContainer").width(750);
            $("div.union").each(function() {
                if ($(this).parents(".union").length > countNesting)
                    countNesting = $(this).parents(".union").length;
            });

            var newWidthDivBlockEdit = countNesting * 75 + 780;

            if (newWidthDivBlockEdit > 750) {
                $("#divBlockEdit").width(newWidthDivBlockEdit);
                $(".filterBuilderContainer").width(newWidthDivBlockEdit);
            }
        }

        //-------------------------------------------------------------------
        // обработчики событий

        function responseSelectHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');
            currentCondition = conditionControl;

            var text = conditionControl.find('.responsePicker');
            var top = text.offset().top + text.height();
            var left = text.offset().left;
            var width = text.width();
            treeView.css({ top: top, left: left, width: width, position: "absolute" });

            if (isTreeVisible)
                hideTree();
            else
                showTree();

            var response = conditionControl.data('response');

            if (response != null) {
                isAutoSelection = true; // restoring last selected option. Tree must not be hidden when onSeleted event is fired
                $.tree.reference(treeView).select_branch($("#" + response.Id));
                isAutoSelection = false;
            }
            return false;
        }

        function operatorChangedHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');
            setConditionResponseOperator(conditionControl, $(this).val());
        }

        function conditionUngroupHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');

            var parentUnion = getConditionParentUnion(conditionControl);
            if (parentUnion.parents('div.union').length > 0) {
                parentUnion.after(conditionControl);
                removeEmptyUnion();
            }
            changeWidthDivBlockEdit();
            return false;
        }

        function conditionGroupHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            if (settings.container.children('div.condition').length > 0)
                return false;

            var conditionControl = $(this).parents('div.condition:first');
            var parentUnion = getConditionParentUnion(conditionControl);

            if (parentUnion.length && parentUnion.children('div.condition').length < 2)
                return false;

            var unionControl = buildUnionControl();

            if (parentUnion.length) {
                conditionControl.before(unionControl);
                unionControl.append(conditionControl);
            } else {
                unionControl.append(conditionControl);
                settings.container.append(unionControl);
            }

            changeWidthDivBlockEdit();
            return false;
        }

        function conditionMoveDownHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');

            var nextElement = conditionControl.next();
            if (nextElement.hasClass('union')) {
                nextElement.append(nextElement.find('div.condition:first').before(conditionControl));
            } else if (nextElement.hasClass('condition')) {
                conditionControl.next('div.condition').after(conditionControl);
            } else {
                var parentUnion = getConditionParentUnion(conditionControl).parents('div.union');
                if (parentUnion.length > 0)
                    getConditionParentUnion(conditionControl).after(conditionControl);
            }

            removeEmptyUnion();
            return false;
        }

        function conditionMoveUpHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');
            var prevElement = conditionControl.prev();

            if (prevElement.hasClass('union')) {
                prevElement.append(prevElement.find('div.condition:last').after(conditionControl));
            } else if (prevElement.hasClass('condition')) {
                conditionControl.prev('div.condition').before(conditionControl);
            } else {
                var parentUnion = getConditionParentUnion(conditionControl).parents('div.union');
                if (parentUnion.length > 0)
                    getConditionParentUnion(conditionControl).before(conditionControl);
            }

            removeEmptyUnion();
            return false;
        }

        function conditionDeleteHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var conditionControl = $(this).parents('div.condition:first');
            conditionControl.remove();

            removeEmptyUnion();

            if (settings.container.find('div.condition').length == 0)
                noConditionWording();

            changeWidthDivBlockEdit();
            return false;
        }

        function conditionAddHandler() {
            settings.container.trigger('filterBuilderChanged');
            
            var rootCondition = settings.container.children('div.condition');
            var rootUnion = settings.container.children('div.union');

            changeWidthDivBlockEdit();

            var conditionControl = buildConditionControl();

            if (rootCondition.length == 0 && rootUnion.length == 0) {
                settings.container.find('span.infoBox').remove();
                settings.container.find('div.addButtonsPanel').show().before(conditionControl);
            } else {
                var unionControl;
                if (rootUnion.length == 0) {
                    unionControl = buildUnionControl();
                    unionControl.append(rootCondition);
                } else {
                    unionControl = rootUnion;
                }

                unionControl.append(conditionControl);
                settings.container.find('div.addButtonsPanel').before(unionControl);
            }
            return false;
        }

        //-------------------------------------------------------------------
        // словари

        function getDictionaryIds() {
            var list = [];
            for (var i in settings.metadata)
                if (settings.metadata[i].Properties)
                    for (var j in settings.metadata[i].Properties) {
                        var id = settings.metadata[i].Properties[j].DictionaryId;
                        if (id != null && id != '' && $.inArray(id, list) == -1)
                            list.push(id);
                    }
            return list;
        }

        function reloadDictionaries() {
            return $.ajax({
                type: 'POST',
                async: true,
                url: settings.dictionariesUrl,
                data: JSON.stringify({ idList: list }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            });
        }

        function setDictionaries(d) {
            settings.dictionaries = [];
            $.each(d, function(index, dict) {
                settings.dictionaries[dict.Id.toString()] = dict;
            });
        }

        //-------------------------------------------------------------------
        // построение контролов

        function reloadDictionariesAndRenderFilter() {
            var list = getDictionaryIds();
            if (list.length != 0) {
                reloadDictionaries()
                    .done(function(result) {
                        setDictionaries(result.d);
                        renderFilter();
                    })
                    .fail(function(msg) {
                        if (msg.responseText && msg.responseText != '')
                            alert(msg.responseText);
                    });
            } else {
                renderFilter();
            }
        }

        function renderFilter() {
            if (settings.filter.Union)
                unionRender(settings.filter.Union, settings.container);
            else if (settings.filter.Operation)
                conditionRender(settings.filter.Operation, settings.container);
            else
                noConditionWording();

            var div = $('<div class="addButtonsPanel" style="display:none"><a href="#" title="Добавить условие">Добавить условие</a></div>');
            div.find("a").on("click", conditionAddHandler);

            if (settings.filter.Union || settings.filter.Operation)
                div.show();
            div.appendTo(settings.container);
        }

        function unionRender(union, unionContainer) {
            //render and initialize union
            var unionControl = buildUnionControl();
            getUnionType(unionControl).val(union.Type);

            //render child unions
            for (var i in union.Unions)
                unionRender(union.Unions[i], unionControl);

            //render equations
            for (var j in union.Operations)
                conditionRender(union.Operations[j], unionControl);

            unionContainer.append(unionControl);
        }

        function conditionRender(operation, conditionContainer) {
            var conditionControl = buildConditionControl();

            //set response
            setConditionResponse(conditionControl, findResponse(operation.AttributeId, operation.Attribute));

            //set operator
            setConditionResponseOperator(conditionControl, operation.Operator);

            //set value(s)
            setConditionValue(conditionControl, operation.Values);

            conditionContainer.append(conditionControl);
        }

        function noConditionWording() {
            var element = $('<span class="infoBox" style="font-weight:normal; text-decoration: underline; font-style:italic; cursor: pointer">Указать условия</span>');
            $(element).on('click', conditionAddHandler);
            $(element).appendTo(settings.container);
            settings.container.find('div.addButtonsPanel').hide();
        }

        //-------------------------------------------------------------------
        // извлечение условия

        this.getFilter = function() {
            var result = {};
            settings
                .container
                .find('> div.union')
                .each(function() {
                    result.Union = getUnion($(this));
                });

            settings
                .container
                .find('> div.condition')
                .each(function() {
                    result.Operation = getCondition($(this));
                });

            return result;
        };

        this.removeControl = function() {
            settings.container.remove();
        };

        //-------------------------------------------------------------------
        // контролы

        function buildUnionControl() {
            var control = $('<div class="union error-mes-sverhu"><div class="unionOperator"><div style="height: 30px;"></div></div></div>');

            var sel = $('<select title="Выбор оператора связки">');
            $.each(unionTypes, function(value, title) {
                sel.append($('<option>').attr('value', value).text(title));
            });

            control.find('.unionOperator div').append(sel);

            return control;
        }

        function buildToolboxButtonControl(title, img, handler) {
            var control = $('<a href="#"></a>').attr('title', title);
            var image = $('<img class="toolboxButton"/>').attr('src', img);
            control.append(image);
            control.on('click', handler);
            return control;
        }

        function buildConditionControl() {
            var toolboxControl = $('<span class="rightHandButtons"></span>');
            toolboxControl.append(buildToolboxButtonControl('Разгруппировать', '/Content/images/arrow-ungroup.gif', conditionUngroupHandler));
            toolboxControl.append(buildToolboxButtonControl('Сгруппировать', '/Content/images/arrow-group.gif', conditionGroupHandler));
            toolboxControl.append(buildToolboxButtonControl('Переместить вниз', '/Content/images/arrow-down.gif', conditionMoveDownHandler));
            toolboxControl.append(buildToolboxButtonControl('Переместить вверх', '/Content/images/arrow-up.gif', conditionMoveUpHandler));
            toolboxControl.append(buildToolboxButtonControl('Удалить', '/Content/images/delete.gif', conditionDeleteHandler));

            var selector = $('<span class="responsePicker"></span>');
            selector.append($('<span class="responseText">Щелкните [...], чтобы выбрать условие</span>').on('click', responseSelectHandler));
            selector.append($('<input class="button" title="Выбор  условия"  type="button" value="..."/>').on('click', responseSelectHandler));

            var control = $('<div class="condition"></div>');
            control.append(toolboxControl);
            control.append(selector);
            control.append($('<div class="lblCondition"></div>'));

            return control;
        }

        function buildConditionOperatorControl(conditionControl) {
            var sel = $('<select title="Выбор оператора">');
            sel.on('change', operatorChangedHandler);

            var type = getConditionResponseType(conditionControl);
            for (var op in type.operators)
                sel.append($('<option>').attr('value', op).text(operatorTypes[op]));

            var control = $('<span class="responseOperator"></span>');
            control.append(sel);
            return control;
        }

        function buildConditionValueControls(conditionControl) {
            var control = $('<span class="responseValue"></span>');

            var type = getConditionResponseType(conditionControl);

            for (var i in type.controls) {
                var divObj = $('<div>');
                control.append(divObj);
                divObj.css({ display: 'inline' });
                divObj.append(type.controls[i].buildControl(conditionControl.data('response')));
            }

            return control;
        }

        function getUnionType(unionControl) {
            return unionControl.find('>div.unionOperator div select');
        }

        function getConditionOperator(conditionControl) {
            return conditionControl.find('span.responseOperator > select');
        }

        function getConditionParentUnion(conditionControl) {
            return conditionControl.parent("div.union");
        }

        function getConditionResponseType(conditionControl) {
            var response = conditionControl.data('response');
            if (response) {
                if ((response.Type == 'text' || response.Type == 'datetime' || response.Type == 'numeric') && response.DictionaryId != null && response.DictionaryId != '')
                    return responseTypes.dictionary;
                else
                    return responseTypes[response.Type];
            }
            return responseTypes.text;
        }

        function getUnion(unionControl) {
            var union = {
                Type: getUnionType(unionControl).val()
            };

            var unions = [];
            unionControl.find('> div.union').each(function() {
                unions.push(getUnion($(this)));
            });

            var operations = [];
            unionControl.find('> div.condition').each(function() {
                operations.push(getCondition($(this)));
            });

            if (unions.length > 0)
                union.Unions = unions;
            if (operations.length > 0)
                union.Operations = operations;

            return union;
        }

        function getCondition(conditionControl) {
            //create equation object
            var equation = {
                Operator: getConditionOperator(conditionControl).val(),
            };

            var response = conditionControl.data('response');
            if (response) {
                equation.Attribute = response.FullName;
                equation.Type = response.Type;
            }

            //get value of active editor (by operator)
            var type = getConditionResponseType(conditionControl);

            conditionControl.find('span.responseValue > div').each(function (index) {
                if ($.inArray(equation.Operator, type.controls[index].operators) != -1)
                    equation.Values = $.makeArray(type.controls[index].getValue($(this)));
            });

            denormalizeEquation(equation);
            return equation;
        }

        function setConditionResponse(conditionControl, response) {
            if (response != null) {
                conditionControl.data('response', response);

                // show response info
                var txt = conditionControl.find('span.responsePicker .responseText');
                var value = response.FullDisplayName;
                if (value.length > 35)
                    value = value.substr(0, 32) + '...';
                txt.text(value).attr('title', response.DisplayName);

                conditionControl.find('span.responseOperator,span.responseValue').remove();
                conditionControl.find('div.lblCondition').before(buildConditionOperatorControl(conditionControl));
                conditionControl.find('div.lblCondition').before(buildConditionValueControls(conditionControl));

                setConditionResponseOperator(conditionControl, getConditionOperator(conditionControl).val());
            }
        }

        function setConditionResponseOperator(conditionControl, operator) {
            //set operator
            getConditionOperator(conditionControl).val(operator);

            //show active value
            var type = getConditionResponseType(conditionControl);
            conditionControl.find('span.responseValue > div')
                .each(function(index) {
                    if ($.inArray(operator, type.controls[index].operators) != -1)
                        $(this).show();
                    else
                        $(this).hide();
                });
        }
        
        function setConditionValue(conditionControl, values) {
            
            var op = getConditionOperator(conditionControl).val();
            var type = getConditionResponseType(conditionControl);

            conditionControl.find('span.responseValue > div').each(function() {
                var control = type.operators[op];
                if (control)
                    control.setValue($(this), values);
            });
        }

        //-------------------------------------------------------------------
        // инициализация

        addResponseType('dictionary');
        addEditorToResponseType(responseTypes.dictionary, ['eq', 'noteq'], dictionaryValueEditor);
        addEditorToResponseType(responseTypes.dictionary, ['em', 'nem', noneValueEditor]);

        addResponseType('numeric');
        //addEditorToResponseType(responseTypes.numeric, ['eq', 'noteq', 'gt', 'lt', 'gteq', 'lteq'], numericValueEditor);
        //addEditorToResponseType(responseTypes.numeric, ['em', 'nem'], noneValueEditor);
        addEditorToResponseType(responseTypes.numeric, ['eq', 'noteq'], numericValueEditor);

        addResponseType('boolean');
        addEditorToResponseType(responseTypes.boolean, ['em', 'nem', 'true', 'false'], noneValueEditor);

        addResponseType('datetime');
        addEditorToResponseType(responseTypes.datetime, ['eq', 'noteq', 'gt', 'lt', 'gteq', 'lteq'], dateValueEditor);
        addEditorToResponseType(responseTypes.datetime, ['em', 'nem'], noneValueEditor);

        addResponseType('text');
        //addEditorToResponseType(responseTypes.text, ['like', 'eq', 'noteq'], textValueEditor);
        //addEditorToResponseType(responseTypes.text, ['em', 'nem'], noneValueEditor);
        addEditorToResponseType(responseTypes.text, ['eq', 'noteq'], textValueEditor);

        addResponseType('audience');
        addEditorToResponseType(responseTypes.audience, ['audiSegments'], textValueEditor);
        addEditorToResponseType(responseTypes.audience, ['audiTarget'], noneValueEditor);

        settings.container = $(settings.container);

        settings.container.on('filterBuilderChanged', function() {
            removeValidation();
        });

        var treeView = createTree();
        hideTree();
        treeView.appendTo(document.body);
        $(document).on('click.treeblur', hideTree);

        normalizeFilter();
        reloadDictionariesAndRenderFilter();
        changeWidthDivBlockEdit();
    }
});

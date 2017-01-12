using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Xml;
using Vtb24.Arms.AdminServices.AdminMechanicsService;
using Attribute = Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Attribute;
using ConditionalFactor = Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.ConditionalFactor;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics
{
    public class MappingsFromService
    {
        #region Mechanics

        public static Mechanic ToMechanic(RuleDomain original)
        {
            return new Mechanic
            {
                Id = original.Id,
                Name = original.Name
            };
        }

        #endregion

        #region Metadata

        public static Metadata ToMetadata(EntityMetadata original)
        {
            return new Metadata
            {
                Name = original.EntityName,
                DisplayName = original.DisplayName,
                Attributes = original.Attributes.Select(attribute => ToAttribute(original, attribute)).ToArray()
            };
        }

        public static Attribute ToAttribute(EntityMetadata metadata, AdminMechanicsService.Attribute original)
        {
            return new Attribute
            {
                Id = original.Id,
                Name = original.Name,
                DisplayName = original.DisplayName,
                FullName = metadata.EntityName + "." + original.Name,
                FullDisplayName = metadata.DisplayName + "." + original.DisplayName,
                Type = ToAttributeType(original.Type),
                DictionaryId = original.DictionaryId
            };
        }

        public static AttributeType ToAttributeType(AttributeTypes original)
        {
            switch (original)
            {
                case AttributeTypes.Text:
                    return AttributeType.Text;
                case AttributeTypes.Number:
                    return AttributeType.Number;
                case AttributeTypes.DateTime:
                    return AttributeType.DateTime;
                case AttributeTypes.Boolean:
                    return AttributeType.Boolean;
            }

            return AttributeType.Unknown;
        }

        #endregion

        #region Predicate

        public static ConditionalFactor ToConditionalFactor(Models.Xml.ConditionalFactor xmlConditionalFactor)
        {
            return new ConditionalFactor
            {
                Priority = xmlConditionalFactor.Priority.Value,
                Factor = xmlConditionalFactor.Factor.Value,
                Predicate = ToPredicate(xmlConditionalFactor.Predicate)
            };
        }

        public static Predicate ToPredicate(filter xmlFilter)
        {
            if (xmlFilter == null || xmlFilter.Item == null)
                throw new PredicateMappingException("Xml filter object is null.");

            var conditionType = xmlFilter.Item.GetType().Name;

            switch (conditionType)
            {
                case XmlConstants.FilterUnion:
                    return new Predicate
                    {
                        Union = ToPredicateUnion(xmlFilter.Item as union)
                    };
                case XmlConstants.FilterOperator:
                    return new Predicate
                    {
                        Operation = ToPredicateOperation(xmlFilter.Item as equation)
                    };
            }

            throw new PredicateMappingException(string.Format("Xml filter type \"{0}\" is not supported.", conditionType));
        }

        public static PredicateUnion ToPredicateUnion(union xmlUnion)
        {
            if (xmlUnion == null)
                throw new PredicateMappingException("Xml union object is null.");

            if ((xmlUnion.equation == null && xmlUnion.union1 == null) ||
                (xmlUnion.equation != null && xmlUnion.union1 == null && xmlUnion.equation.Length < 1) ||
                (xmlUnion.equation == null && xmlUnion.union1 != null && xmlUnion.union1.Length < 1))
                throw new PredicateMappingException("Xml union object doesn't contain any nested equations or unions.");

            var node = new PredicateUnion
            {
                Type = ToPredicateUnionType(xmlUnion.type),
                Unions = xmlUnion.union1 != null
                             ? xmlUnion.union1.Select(ToPredicateUnion).ToArray()
                             : null,
                Operations = xmlUnion.equation != null
                                 ? xmlUnion.equation.Select(ToPredicateOperation).ToArray()
                                 : null
            };

            return node;
        }

        public static PredicateOperation ToPredicateOperation(equation xmlEquation)
        {
            if (xmlEquation == null)
                throw new PredicateMappingException("Xml equation object is null.");

            if (xmlEquation.value == null || xmlEquation.value.Length < 1)
                throw new PredicateMappingException("Xml equation object doesn't contain values.");

            var operation = new PredicateOperation
            {
                Operator = ToPredicateOperator(xmlEquation.@operator)
            };

            var values = new List<string>();

            foreach (var xmlValue in xmlEquation.value)
            {
                if (xmlValue == null)
                    throw new PredicateMappingException("Xml value object is null.");

                switch (xmlValue.type)
                {
                    case valueType.attr:
                        if (operation.Attribute != null)
                            throw new PredicateMappingException("Xml equation object contains more then one attribute");

                        if (xmlValue.attr == null || xmlValue.attr.Length < 1 || xmlValue.attr[0] == null)
                            throw new PredicateMappingException(string.Format("Xml value object of type \"{0}\" doesn't contain attributes.", valueType.attr));

                        var xmlValueAttr = xmlValue.attr[0];

                        operation.Attribute = string.Format("{0}.{1}", xmlValueAttr.@object, xmlValueAttr.name);
                        operation.Type = ToAttributeType(xmlValueAttr.type);
                        break;

                    case valueType.@string:
                    case valueType.numeric:
                    case valueType.datetime:
                    case valueType.boolean:
                        if (xmlValue.Text != null && xmlValue.Text.Length > 0)
                            values.Add(xmlValue.Text[0]);
                        break;

                    default:
                        throw new PredicateMappingException(string.Format("Xml value type \"{0}\" is not supported.", xmlValue.type));
                }
            }

            if (operation.Attribute == null)
                throw new PredicateMappingException("Xml equation object doesn't contain attributes.");

            operation.Values = values.ToArray();

            return operation;
        }

        public static PredicateUnionType ToPredicateUnionType(string xmlUnionType)
        {
            switch (xmlUnionType)
            {
                case XmlConstants.UnionAnd:
                    return PredicateUnionType.And;
                case XmlConstants.UnionOr:
                    return PredicateUnionType.Or;
            }

            throw new PredicateMappingException(string.Format("Xml union type \"{0}\" is not supported.", xmlUnionType));
        }

        private static PredicateOperator ToPredicateOperator(equationOperator xmlEquationOperator)
        {
            switch (xmlEquationOperator)
            {
                case equationOperator.eq:
                    return PredicateOperator.Eq;
                case equationOperator.noteq:
                    return PredicateOperator.NotEq;
                case equationOperator.cn:
                    return PredicateOperator.Cn;
                case equationOperator.gt:
                    return PredicateOperator.Gt;
                case equationOperator.lt:
                    return PredicateOperator.Lt;
                case equationOperator.gteq:
                    return PredicateOperator.GtEq;
                case equationOperator.lteq:
                    return PredicateOperator.LtEq;
                case equationOperator.em:
                    return PredicateOperator.Empty;
                case equationOperator.nem:
                    return PredicateOperator.NotEmpty;
                case equationOperator.like:
                    return PredicateOperator.Like;
                case equationOperator.visible:
                    return PredicateOperator.Visible;
                case equationOperator.invisible:
                    return PredicateOperator.Invisible;
                case equationOperator.@true:
                    return PredicateOperator.True;
                case equationOperator.@false:
                    return PredicateOperator.False;
            }

            throw new PredicateMappingException(string.Format("Xml operator \"{0}\" is not supported.", xmlEquationOperator));
        }

        public static AttributeType ToAttributeType(valueType xmlValueType)
        {
            switch (xmlValueType)
            {
                case valueType.@string:
                    return AttributeType.Text;
                case valueType.numeric:
                    return AttributeType.Number;
                case valueType.datetime:
                    return AttributeType.DateTime;
                case valueType.boolean:
                    return AttributeType.Boolean;
            }

            throw new PredicateMappingException(string.Format("Xml value type \"{0}\" is not supported.", xmlValueType));
        }

        public static string ToJsonPredicateUnionType(PredicateUnionType type)
        {
            switch (type)
            {
                case PredicateUnionType.And:
                    return JsonConstants.UnionAnd;
                case PredicateUnionType.Or:
                    return JsonConstants.UnionOr;
            }

            throw new PredicateMappingException(string.Format("Predicate union type \"{0}\" is not supported.", type));
        }

        public static string ToJsonOperator(PredicateOperator predicateOperator)
        {
            switch (predicateOperator)
            {
                case PredicateOperator.Eq:
                    return JsonConstants.OperatorEq;
                case PredicateOperator.NotEq:
                    return JsonConstants.OperatorNotEq;
                case PredicateOperator.Cn:
                    return JsonConstants.OperatorCn;
                case PredicateOperator.Gt:
                    return JsonConstants.OperatorGt;
                case PredicateOperator.Lt:
                    return JsonConstants.OperatorLt;
                case PredicateOperator.GtEq:
                    return JsonConstants.OperatorGtEq;
                case PredicateOperator.LtEq:
                    return JsonConstants.OperatorLtEq;
                case PredicateOperator.Like:
                    return JsonConstants.OperatorLike;
                case PredicateOperator.Empty:
                    return JsonConstants.OperatorEm;
                case PredicateOperator.NotEmpty:
                    return JsonConstants.OperatorNem;
                case PredicateOperator.Visible:
                    return JsonConstants.OperatorVisible;
                case PredicateOperator.Invisible:
                    return JsonConstants.OperatorInvisible;
                case PredicateOperator.True:
                    return JsonConstants.OperatorTrue;
                case PredicateOperator.False:
                    return JsonConstants.OperatorFalse;
            }

            throw new PredicateMappingException(string.Format("Predicate operator \"{0}\" is not supported.", predicateOperator));
        }

        public static string ToJsonAttributeType(AttributeType attributeType)
        {
            switch (attributeType)
            {
                case AttributeType.Text:
                    return JsonConstants.AttributeTypeText;
                case AttributeType.Number:
                    return JsonConstants.AttributeTypeNumeric;
                case AttributeType.DateTime:
                    return JsonConstants.AttributeTypeDatetime;
                case AttributeType.Boolean:
                    return JsonConstants.AttributeTypeBoolean;
            }

            throw new PredicateMappingException(string.Format("Attribute type \"{0}\" is not suppoorted.", attributeType));
        }

        #endregion
    }
}

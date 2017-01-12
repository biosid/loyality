using System.Linq;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Xml;
using ConditionalFactor = Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.ConditionalFactor;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics
{
    public class MappingsToService
    {
        public static Models.Xml.ConditionalFactor ToXmlConditionalFactor(ConditionalFactor conditionalFactor)
        {
            return new Models.Xml.ConditionalFactor
            {
                Priority = new Priority { Value = conditionalFactor.Priority },
                Factor = new Factor { Value = conditionalFactor.Factor },
                Predicate = ToXmlFilter(conditionalFactor.Predicate)
            };
        }

        public static filter ToXmlFilter(Predicate predicate)
        {
            if (predicate == null)
                throw new PredicateMappingException("Predicate is null.");

            if (predicate.Union != null)
                return new filter { Item = ToXmlUnion(predicate.Union) };

            if (predicate.Operation != null)
                return new filter { Item = ToXmlEquation(predicate.Operation) };

            throw new PredicateMappingException("Predicate is empty.");
        }

        public static union ToXmlUnion(PredicateUnion predicateUnion)
        {
            var xmlUnionType = ToXmlUnionType(predicateUnion.Type);

            var count =
                (predicateUnion.Unions != null ? predicateUnion.Unions.Length : 0) +
                (predicateUnion.Operations != null ? predicateUnion.Operations.Length : 0);

            if (count < 2)
                throw new PredicateMappingException("Union should contain at least 2 children.");

            if (predicateUnion.Unions != null && predicateUnion.Unions.Any(u => u == null))
                throw new PredicateMappingException("Union contains at least 1 null subunion.");

            if (predicateUnion.Operations != null && predicateUnion.Operations.Any(o => o == null))
                throw new PredicateMappingException("Union contains at least 1 null suboperation.");

            var xmlUnion = new union
            {
                type = xmlUnionType,
                union1 = predicateUnion.Unions != null
                             ? predicateUnion.Unions.Select(ToXmlUnion).ToArray()
                             : null,
                equation = predicateUnion.Operations != null
                               ? predicateUnion.Operations.Select(ToXmlEquation).ToArray()
                               : null
            };

            if (xmlUnion.union1 == null && xmlUnion.equation == null)
                throw new PredicateMappingException("The number of converted nested unions and equations is 0.");

            return xmlUnion;
        }

        public static equation ToXmlEquation(PredicateOperation predicateOperation)
        {
            var type = ToXmlValueType(predicateOperation.Type);

            var attributeParts = predicateOperation.Attribute.Split('.');

            if (attributeParts.Length != 2)
                throw new PredicateMappingException("Unexpected format of operation attribute name. Expected format is \"<metadata_name>.<attribute_name>\".");

            var values = new[]
            {
                new value
                {
                    attr = new[] { new valueAttr { @object = attributeParts[0], name = attributeParts[1], type = type } },
                    type = valueType.attr
                }
            };

            return new equation
            {
                @operator = ToXmlOperator(predicateOperation.Operator),
                value = predicateOperation.Values != null && predicateOperation.Values.Length > 0
                            ? values.Concat(predicateOperation.Values.Select(text => ToXmlValue(type, text))).ToArray()
                            : null
            };
        }

        public static value ToXmlValue(valueType type, string text)
        {
            return new value
            {
                type = type,
                Text = new[] { text }
            };
        }

        public static string ToXmlUnionType(PredicateUnionType predicateUnionType)
        {
            switch (predicateUnionType)
            {
                case PredicateUnionType.And:
                    return XmlConstants.UnionAnd;
                case PredicateUnionType.Or:
                    return XmlConstants.UnionOr;
            }

            throw new PredicateMappingException(string.Format("Predicate union type \"{0}\" is not supported.", predicateUnionType));
        }

        public static equationOperator ToXmlOperator(PredicateOperator predicateOperator)
        {
            switch (predicateOperator)
            {
                case PredicateOperator.Eq:
                    return equationOperator.eq;
                case PredicateOperator.NotEq:
                    return equationOperator.noteq;
                case PredicateOperator.Cn:
                    return equationOperator.cn;
                case PredicateOperator.Gt:
                    return equationOperator.gt;
                case PredicateOperator.Lt:
                    return equationOperator.lt;
                case PredicateOperator.GtEq:
                    return equationOperator.gteq;
                case PredicateOperator.LtEq:
                    return equationOperator.lteq;
                case PredicateOperator.Like:
                    return equationOperator.like;
                case PredicateOperator.Empty:
                    return equationOperator.em;
                case PredicateOperator.NotEmpty:
                    return equationOperator.nem;
                case PredicateOperator.Visible:
                    return equationOperator.visible;
                case PredicateOperator.Invisible:
                    return equationOperator.invisible;
                case PredicateOperator.True:
                    return equationOperator.@true;
                case PredicateOperator.False:
                    return equationOperator.@false;
            }

            throw new PredicateMappingException(string.Format("Predicate operator \"{0}\" is not supported.", predicateOperator));
        }

        public static valueType ToXmlValueType(AttributeType attributeType)
        {
            switch (attributeType)
            {
                case AttributeType.Text:
                    return valueType.@string;
                case AttributeType.Number:
                    return valueType.numeric;
                case AttributeType.DateTime:
                    return valueType.datetime;
                case AttributeType.Boolean:
                    return valueType.boolean;
            }

            throw new PredicateMappingException(string.Format("Attribute type \"{0}\" is not supported.", attributeType));
        }

        public static PredicateUnionType ToPredicateUnionType(string jsonUnionType)
        {
            switch (jsonUnionType)
            {
                case JsonConstants.UnionAnd:
                    return PredicateUnionType.And;
                case JsonConstants.UnionOr:
                    return PredicateUnionType.Or;
            }

            throw new PredicateMappingException(string.Format("Json union type \"{0}\" is not supported.", jsonUnionType));
        }

        public static PredicateOperator ToPredicateOperator(string jsonOperator)
        {
            switch (jsonOperator)
            {
                case JsonConstants.OperatorEq:
                    return PredicateOperator.Eq;
                case JsonConstants.OperatorNotEq:
                    return PredicateOperator.NotEq;
                case JsonConstants.OperatorCn:
                    return PredicateOperator.Cn;
                case JsonConstants.OperatorGt:
                    return PredicateOperator.Gt;
                case JsonConstants.OperatorLt:
                    return PredicateOperator.Lt;
                case JsonConstants.OperatorGtEq:
                    return PredicateOperator.GtEq;
                case JsonConstants.OperatorLtEq:
                    return PredicateOperator.LtEq;
                case JsonConstants.OperatorEm:
                    return PredicateOperator.Empty;
                case JsonConstants.OperatorNem:
                    return PredicateOperator.NotEmpty;
                case JsonConstants.OperatorLike:
                    return PredicateOperator.Like;
                case JsonConstants.OperatorVisible:
                    return PredicateOperator.Visible;
                case JsonConstants.OperatorInvisible:
                    return PredicateOperator.Invisible;
                case JsonConstants.OperatorTrue:
                    return PredicateOperator.True;
                case JsonConstants.OperatorFalse:
                    return PredicateOperator.False;
            }

            throw new PredicateMappingException(string.Format("Json operator \"{0}\" is not supported.", jsonOperator));
        }

        public static AttributeType ToAttributeType(string jsonAttributeType)
        {
            switch (jsonAttributeType)
            {
                case JsonConstants.AttributeTypeText:
                    return AttributeType.Text;
                case JsonConstants.AttributeTypeNumeric:
                    return AttributeType.Number;
                case JsonConstants.AttributeTypeDatetime:
                    return AttributeType.DateTime;
                case JsonConstants.AttributeTypeBoolean:
                    return AttributeType.Boolean;
            }

            throw new PredicateMappingException(string.Format("Json attribute type \"{0}\" is not supported.", jsonAttributeType));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Vtb24.Site.Infrastructure
{
    public static class ModelStateExtensions
    {
        public const string DEFAULT_SEPARATOR = "\n";

        /// <summary>
        /// Получить все валидационные сообщения.
        /// Обычно применяется для сериализации в json при ajax взаимодействиях.
        /// <code>
        /// <![CDATA[
        ///     if (!ModelState.IsValid) {
        ///         return Json(new { success: false, errors: ModelState.GetErrorMessages() });
        ///     }
        /// ]]>
        /// </code>
        /// </summary>
        public static IDictionary<string, string> GetErrorMessages(this ModelStateDictionary modelState, string separator = DEFAULT_SEPARATOR)
        {
            return modelState
                .Where(s => s.Value.Errors.Any())
                .ToDictionary(
                    p => p.Key,
                    p => string.Join(separator, p.Value.Errors.Select(e => e.ErrorMessage))
                );
        }

        /// <summary>
        /// Получить валидационные сообщения по конкретному полю.
        /// <code>
        /// <![CDATA[
        ///     var formError = ModelState.GetErrorMessagesFor("");
        ///     logger.Error(formError);
        /// ]]>
        /// </code>
        /// </summary>
        public static string GetErrorMessagesFor(this ModelStateDictionary modelState, string name, string separator = DEFAULT_SEPARATOR)
        {
            var state = modelState[name];
            return state == null
                ? string.Empty
                : string.Join(separator, state.Errors.Select(e => e.ErrorMessage));
        }

        /// <summary>
        /// Удалить ошибки по предикату.
        /// Дополняет стандартный <c>ModelState.Remove</c>.
        /// Обычно применяется при условной валидации, когда неоходимо исключить валидацию вложенных моделей
        /// <code>
        /// <![CDATA[
        ///     if (Model.DeliveryType == DeliveryTypes.PickUp) { // самовывоз
        ///         // не валидируем адрес доставки
        ///         ModelState.Remove(k=>k.StartsWith("RecipientAddress."));
        ///     }
        ///     if (ModelState.IsValid) {
        ///         PlaceOrder(model);
        ///         return RedirectToAction("ThankYou");
        ///     }
        ///     return View(model);
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        /// <param name="predicate">предикат, на вход - имя поля (ключ ModelStateDictionary)</param>
        public static void RemoveErrors(this ModelStateDictionary modelState, Func<string, bool> predicate)
        {
            foreach (var error in modelState.Keys.Where(predicate).ToArray())
            {
                modelState[error].Errors.Clear();
            }
        }

        /// <summary>
        /// Удалить ошибки по ключу.
        /// </summary>
        /// <param name="modelState">ModelStateDictionary</param>
        /// <param name="key">ключ</param>
        public static void RemoveErrors(this ModelStateDictionary modelState, string key)
        {
            if (modelState.ContainsKey(key))
            {
                modelState[key].Errors.Clear();
            }
        }
    }
}
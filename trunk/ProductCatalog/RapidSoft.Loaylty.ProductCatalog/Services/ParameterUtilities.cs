namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    public static class ParameterUtilities
    {
        /// <summary>
        /// "Выравнивает" значение в диапазоне от 1 до <paramref name="heightBound"/>:
        /// Возвращает исходное число если оно находится в диапазоне от 1 до <paramref name="heightBound"/> включительно, иначе возвращает <paramref name="heightBound"/>.
        /// </summary>
        /// <param name="source">Исходное число</param>
        /// <param name="heightBound">Значение по умолчанию</param>
        /// <returns>Число в диапазоне от 1 до <paramref name="heightBound"/> включительно</returns>
        public static int NormalizeByHeight(this int? source, int heightBound)
        {
            if (source == null)
            {
                return heightBound;
            }

            if (source > heightBound)
            {
                return heightBound;
            }

            if (source < 0)
            {
                return heightBound;
            }

            return source.Value;
        }
    }
}
namespace RapidSoft.VTB24.VtbEncryption
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class VtbEncryption : IVtbEncryption
    {
        private static readonly Object ObjLock = new Object();

        /// <summary>
        /// Call filepro.exe -e for encription
        /// </summary>        
        /// <param name="workspace">Working directory</param>
        public void Encrypt(string workspace)
        {
            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var runnerDir = Path.Combine(Path.Combine(executingDir, "Security"));
            var runner = Path.Combine(runnerDir, "filepro.exe");
            var keysDir = Path.Combine(runnerDir, "encrypt");
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = runner;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = String.Format(" -e \"{0}\" \"{0}\" \"{1}\"", workspace.TrimEnd('\\'), keysDir);
            lock (ObjLock)
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    var resultCode = exeProcess.ExitCode;
                    if (resultCode != 0)
                    {
                        Throw(resultCode);
                    }
                }
            }
        }

        /// <summary>
        /// Call filepro.exe -d for decription
        /// </summary>
        /// <param name="workspace">Working directory</param>
        public void Decrypt(string workspace)
        {
            string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var runnerDir = Path.Combine(Path.Combine(executingDir, "Security"));
            var runner = Path.Combine(runnerDir, "filepro.exe");
            var keysDir = Path.Combine(runnerDir, "decrypt");
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = runner;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = String.Format(" -d  \"{0}\" \"{0}\" \"{1}\"", workspace.TrimEnd('\\'), keysDir);
            lock (ObjLock)
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    var resultCode = exeProcess.ExitCode;
                    if (resultCode != 0)
                    {
                        Throw(resultCode);
                    }
                }
            }
        }

        public static void Throw(int errorCode)
        {
            switch (errorCode)
            {
                case 2: throw new InvalidOperationException("Невозможно получить сертификат авторитета");
                case 3: throw new InvalidOperationException("Невозможно получить список отмены");
                case 4: throw new InvalidOperationException("Невозможно расшифровать подпись сертификата");
                case 5: throw new InvalidOperationException("Невозможно расшифровать подпись списка отмены");
                case 6: throw new InvalidOperationException("Невозможно декодировать открытый ключ авторитета");
                case 7: throw new InvalidOperationException("Плохая подпись в сертификате");
                case 8: throw new InvalidOperationException("Плохая подпись в списке отмены");
                case 9: throw new InvalidOperationException("Срок действия сертификата еще не настал");
                case 10: throw new InvalidOperationException("Срок действия сертификата уже истек");
                case 11: throw new InvalidOperationException("Срок действия списка отмены еще не настал");
                case 12: throw new InvalidOperationException("Срок действия списка отмены уже истек");
                case 13: throw new InvalidOperationException("Ошибка в формате поля сертификата \"NotBefore\"");
                case 14: throw new InvalidOperationException("Ошибка в формате поля сертификата \"NotAfter\"");
                case 15: throw new InvalidOperationException("Ошибка в формате поля списка отмены \"LastUpdate\"");
                case 16: throw new InvalidOperationException("Ошибка в формате поля списка отмены \"NextUpdate\"");
                case 17: throw new InvalidOperationException("Ошибка при выделении памяти");
                case 18: throw new InvalidOperationException("Самоподписанный сертификат");
                case 19: throw new InvalidOperationException("Самоподписанный сертификат в цепочке сертификатов");
                case 20: throw new InvalidOperationException("Невозможно локально получить сертификат авторитета");
                case 21: throw new InvalidOperationException("Невозможно проверить первый сертификат");
                case 22: throw new InvalidOperationException("Цепочка сертификатов слишком большая");
                case 23: throw new InvalidOperationException("Сертификат отменен");
                case 24: throw new InvalidOperationException("Последний сертификат в цепочке не является доверенным");
                case 103: throw new InvalidOperationException("Ошибка при выделении памяти");
                case 104: throw new InvalidOperationException("Ошибка при чтении сертификата автора подписи");
                case 105: throw new InvalidOperationException("Ошибка при чтении сертификата получателя зашифрованных данных");
                case 106: throw new InvalidOperationException("Файл секретного ключа не задан");
                case 107: throw new InvalidOperationException("Ошибка при чтении файла секретного ключа");
                case 108: throw new InvalidOperationException("Ошибка при задании алгоритма хэш-функции");
                case 109: throw new InvalidOperationException("Ошибка при задании алгоритма шифрования");
                case 114: throw new InvalidOperationException("Ошибка при записи выходных данных");
                case 115: throw new InvalidOperationException("Ошибка при задании информации автора подписи");
                case 116: throw new InvalidOperationException("Ошибка при установлении заданного алгоритма шифрования");
                case 117: throw new InvalidOperationException("Ошибка при задании информации получателя зашифрованных данных");
                case 118: throw new InvalidOperationException("Ошибка при инициализации процедуры подписи или шифрования");
                case 119: throw new InvalidOperationException("Ошибка при завершении процедуры подписи или шифрования");
                case 122: throw new InvalidOperationException("Ошибка при ASN.1-кодировании");
                case 123: throw new InvalidOperationException("Ошибка при ASN.1-декодировании");
                case 124: throw new InvalidOperationException("Неизвестный тип PKCS#7-сообщения");
                case 125: throw new InvalidOperationException("Ошибка при расшифровке данных");
                case 127: throw new InvalidOperationException("Подписи не обнаружены");
                case 128: throw new InvalidOperationException("Обнаружены неподтвержденные подписи");
                case 129: throw new InvalidOperationException("Сертификат автора подписи не найден");
                case 132: throw new InvalidOperationException("Обязательные подписи не обнаружены");
                case 134: throw new InvalidOperationException("Сертификат получателя зашифрованных данных не найден");
                case 135: throw new InvalidOperationException("Ошибка при чтении вектора инициализации генератора сл. чисел");
                case 136: throw new InvalidOperationException("Файл вектора инициализации генератора сл. чисел не задан");
                case 138: throw new InvalidOperationException("Попытка расшифровать незашифрованные данные");
                case 143: throw new InvalidOperationException("Ошибка при записи ключа");
                case 144: throw new InvalidOperationException("Ошибка при генерации ключей");
                case 145: throw new InvalidOperationException("Ошибка в формате составного имени");
                case 147: throw new InvalidOperationException("Ошибка при подписи запроса");
                case 148: throw new InvalidOperationException("Файл запроса не задан");
                case 149: throw new InvalidOperationException("Ошибка при записи в файл запроса");
                case 150: throw new InvalidOperationException("Ошибка при задании номера версии запроса");
                case 153: throw new InvalidOperationException("Ошибка при чтении списка отмены");
                case 154: throw new InvalidOperationException("Ошибка при задании списка отмены");
                case 155: throw new InvalidOperationException("Плохой сертификат получателя");
                case 158: throw new InvalidOperationException("Ошибка при инициализации процедуры проверки подписи");
                case 159: throw new InvalidOperationException("В параметре задан нулевой указатель");
                case 160: throw new InvalidOperationException("Размер заданного буфера меньше требуемого");
                case 161: throw new InvalidOperationException("Библиотека не инициализирована");
                case 162: throw new InvalidOperationException("Данные не подписаны");
                case 163: throw new InvalidOperationException("Подпись не подтверждена");
                case 164: throw new InvalidOperationException("Ошибка при чтении файла ключа СА");
                case 165: throw new InvalidOperationException("Ошибка при записи вектора инициализации генератора случайных чисел");
                case 166: throw new InvalidOperationException("Ошибка при чтении сертификата");
                case 167: throw new InvalidOperationException("Неверно задан алгоритм генерируемых ключей");
                case 168: throw new InvalidOperationException("Ошибка при считывании секретного ключа из ключевого носителя СКЗИ");
                case 169: throw new InvalidOperationException("Ошибка при инициализации СКЗИ");
                case 170: throw new InvalidOperationException("Ошибка при копировании секретного ключа СКЗИ");
                case 171: throw new InvalidOperationException("Файл ключа для формирования запроса не задан");
                case 172: throw new InvalidOperationException("Ошибка при чтении файла ключа для формирования запроса");
                case 173: throw new InvalidOperationException("Ошибка при чтении файла ключа для подписи запроса");
                case 174: throw new InvalidOperationException("Файл ключа для подписи запроса не задан");
                case 175: throw new InvalidOperationException("Ошибка при чтении ключевого носителя СКЗИ с ключом для формирования запроса");
                case 176: throw new InvalidOperationException("Ошибка при чтении ключевого носителя СКЗИ с ключом для подписи запроса");
                case 177: throw new InvalidOperationException("Ошибка при создании резервной копии ключевого носителя СКЗИ");
                case 178: throw new InvalidOperationException("Ошибка при инициализации криптобиблиотеки");
                case 179: throw new InvalidOperationException("Ошибка при создании ключевого носителя СКЗИ 3.1");
                case 180: throw new InvalidOperationException("В заданном каталоге уже существуют ключевые объекты СКЗИ 3.1");
                case 181: throw new InvalidOperationException("Заданный двухключевой алгоритм не поддерживается данной функцией");
                case 182: throw new InvalidOperationException("Ошибка при создании запроса сертификата");
                case 183: throw new InvalidOperationException("Ошибка при добавлении сертификата CA");
                case 184: throw new InvalidOperationException("Процесс был прерван");
                case 185: throw new InvalidOperationException("Ошибка при декодировании запроса сертификата");
                case 186: throw new InvalidOperationException("Ошибка при декодировании сертификата");
                case 187: throw new InvalidOperationException("Ошибка при декодировании списка отмены сертификатов");
                case 188: throw new InvalidOperationException("Ошибка при кодировании запроса сертификата");
                case 189: throw new InvalidOperationException("Ошибка при кодировании сертификата");
                case 190: throw new InvalidOperationException("Ошибка при кодировании списка отмены сертификатов");
                case 191: throw new InvalidOperationException("Ошибка при декодировании секретного ключа");
                case 192: throw new InvalidOperationException("Ошибка при кодировании секретного ключа");
                case 193: throw new InvalidOperationException("Ошибка при удалении файла");
                case 194: throw new InvalidOperationException("Ошибка при копировании файла");
                case 195: throw new InvalidOperationException("Файл с заданным именем уже существует");
                case 196: throw new InvalidOperationException("Файл с сертификатом СА не найден");
                case 197: throw new InvalidOperationException("Ошибка при чтении файла запроса сертификата");
                case 198: throw new InvalidOperationException("Ошибка при формировании свертки запроса сертификата");
                case 199: throw new InvalidOperationException("Ошибка при формировании свертки секретного ключа");
                case 200: throw new InvalidOperationException("Ошибка при формировании свертки сертификата");
                case 201: throw new InvalidOperationException("Ошибка при выводе параметров сертификата");
                default: throw new InvalidOperationException("Незвестная ошибка");
            }
        }
    }
}

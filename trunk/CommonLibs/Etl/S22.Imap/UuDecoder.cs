namespace S22.Imap
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class UuDecoder
    {
        public static string Decode(string str)
        {
            using (var reader = new StringReader(str))
            {
                reader.ReadLine(); // header

                var bytes = reader.ReadLines()
                                  .TakeWhile(s => s != " ")
                                  .SelectMany(UuDecodeLine)
                                  .ToArray();

                return Encoding.GetEncoding(1251).GetString(bytes);
            }
        }

        private static IEnumerable<string> ReadLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private static IEnumerable<byte> UuDecodeLine(string line)
        {
            return line.Skip(1)
                       .Select(c => (byte) (((int) c - 32) & 63))
                       .UuSplitToItems()
                       .UuDecodeItems();
        }

        private static IEnumerable<byte[]> UuSplitToItems(this IEnumerable<byte> bytes)
        {
            using (var enumerator = bytes.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var item = new[]
                    {
                        enumerator.Current,
                        enumerator.MoveNext() ? enumerator.Current : (byte) 0,
                        enumerator.MoveNext() ? enumerator.Current : (byte) 0,
                        enumerator.MoveNext() ? enumerator.Current : (byte) 0
                    };

                    yield return item;
                }
            }
        }

        private static IEnumerable<byte> UuDecodeItems(this IEnumerable<byte[]> items)
        {
            return items.SelectMany(UuDecodeItem);
        }

        private static IEnumerable<byte> UuDecodeItem(byte[] item)
        {
            yield return (byte)(item[0] << 2 | item[1] >> 4);
            yield return (byte)(item[1] << 4 | item[2] >> 2);
            yield return (byte)(item[2] << 6 | item[3]);
        }
    }
}

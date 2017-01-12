namespace RapidSoft.Etl.Runtime.DataSources.Csv
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using RapidSoft.Etl.Runtime.Properties;

    public sealed partial class CsvReader : IDataReader, IEnumerable<IDataRecord>, IDisposable
    {
        #region Constructors

        public CsvReader(TextReader reader)
            : this(reader, null, default(CsvReaderOptions))
        {
        }

        public CsvReader(
            TextReader reader, CsvSyntaxInfo syntax, bool skipEmptyRows = false, bool nullForSkipColumn = false)
            : this(reader, 
                syntax, 
                new CsvReaderOptions
                {
                    SkipEmptyRows = skipEmptyRows, 
                    NullForSkipColumn = nullForSkipColumn
                })
        {
        }

        public CsvReader(TextReader reader, CsvSyntaxInfo syntax, CsvReaderOptions options)
        {
#if DEBUG
            this._allocStack = new StackTrace();
#endif

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (syntax == null)
            {
                throw new ArgumentNullException("syntax");
            }

            if (options.BufferSize <= 0)
            {
                this._bufferSize = DEFAULT_BUFFER_SIZE;
            }
            else
            {
                this._bufferSize = options.BufferSize;
            }

            if (reader is StreamReader)
            {
                Stream stream = ((StreamReader)reader).BaseStream;

                if (stream.CanSeek)
                {
                    // Handle bad implementations returning 0 or less
                    if (stream.Length > 0)
                    {
                        this._bufferSize = (int)Math.Min(this._bufferSize, stream.Length);
                    }
                }
            }

            if (syntax == null)
            {
                syntax = new CsvSyntaxInfo();
            }

            this._sourceReader = reader;

            this._fieldDelimiter = syntax.FieldDelimiter;
            this._lineDelimiter1 = syntax.LineDelimiter1;
            this._lineDelimiter2 = syntax.LineDelimiter2;
            this._quote = syntax.Quote;
            this._escape = syntax.Escape;

            // _comment = '\0';
            this._comment = '#';

            this._hasHeaders = syntax.HasHeaders;
            this._trimmingOptions = options.ValueTrimmingOptions;
            this._supportsMultiline = true;
            this._skipEmptyLines = true;

            this._currentRecordIndex = -1;
            this._defaultParseErrorAction = CsvParseErrorAction.RaiseEvent;

            this._skipEmtpyRows = options.SkipEmptyRows;
            this._nullForSkipColumn = options.NullForSkipColumn;
            this.MissingFieldAction = MissingFieldAction.ReplaceByNull;
        }

        #endregion

        #region Constants

        private const int DEFAULT_BUFFER_SIZE = 0x1000;

        #endregion

        #region Fields

        #region Settings

        private static readonly Type _fieldType = typeof(string);

        private static readonly StringComparer _fieldHeaderComparer = StringComparer.InvariantCultureIgnoreCase;

        private readonly TextReader _sourceReader;

        private readonly int _bufferSize;

        private readonly char _comment;

        private readonly char _escape;

        private readonly char _fieldDelimiter;

        private readonly char _lineDelimiter1;

        private readonly char _lineDelimiter2;

        private readonly char _quote;

        private readonly ValueTrimmingOptions _trimmingOptions;

        private readonly bool _hasHeaders;

        private readonly bool _supportsMultiline;

        private readonly bool _skipEmptyLines;

        private readonly bool _skipEmtpyRows;

        private readonly bool _nullForSkipColumn;

        #endregion

        #region State

        private CsvParseErrorAction _defaultParseErrorAction;

        private MissingFieldAction _missingFieldAction;

        private bool _initialized;

        private string[] _fieldHeaders;

        private Dictionary<string, int> _fieldHeaderIndexes;

        private long _currentRecordIndex;

        private int _nextFieldStart;

        private int _nextFieldIndex;

        private string[] _fields;

        private int _fieldCount;

        private char[] _buffer;

        private int _bufferLength;

        private bool _eof;

        private bool _eol;

        private bool _firstRecordInCache;

        private bool _missingFieldFlag;

        private bool _parseErrorFlag;

        private int commentAndEmptyLineSkipCount;

        #endregion

        #endregion

        #region Events

        public event EventHandler<CsvParseErrorEventArgs> ParseError;

        private void OnParseError(CsvParseErrorEventArgs e)
        {
            EventHandler<CsvParseErrorEventArgs> handler = this.ParseError;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Properties

        #region Settings

        public char Comment
        {
            get
            {
                return this._comment;
            }
        }

        public char Escape
        {
            get
            {
                return this._escape;
            }
        }

        public char FieldDelimiter
        {
            get
            {
                return this._fieldDelimiter;
            }
        }

        public char Quote
        {
            get
            {
                return this._quote;
            }
        }

        public bool HasHeaders
        {
            get
            {
                return this._hasHeaders;
            }
        }

        public ValueTrimmingOptions TrimmingOption
        {
            get
            {
                return this._trimmingOptions;
            }
        }

        public int BufferSize
        {
            get
            {
                return this._bufferSize;
            }
        }

        public bool SupportsMultiline
        {
            get
            {
                return this._supportsMultiline;
            }
        }

        public bool SkipEmptyLines
        {
            get
            {
                return this._skipEmptyLines;
            }
        }

        #endregion

        #region State

        public CsvParseErrorAction DefaultParseErrorAction
        {
            get
            {
                return this._defaultParseErrorAction;
            }

            set
            {
                this._defaultParseErrorAction = value;
            }
        }

        public MissingFieldAction MissingFieldAction
        {
            get
            {
                return this._missingFieldAction;
            }

            set
            {
                this._missingFieldAction = value;
            }
        }

        public int FieldCount
        {
            get
            {
                this.EnsureInitialize();
                return this._fieldCount;
            }
        }

        public bool EndOfStream
        {
            get
            {
                return this._eof;
            }
        }

        public string[] GetFieldHeaders()
        {
            this.EnsureInitialize();
            Debug.Assert(this._fieldHeaders != null, "Field headers must be non null.");

            string[] fieldHeaders = new string[this._fieldHeaders.Length];

            for (int i = 0; i < fieldHeaders.Length; i++)
            {
                fieldHeaders[i] = this._fieldHeaders[i];
            }

            return fieldHeaders;
        }

        public long CurrentRecordIndex
        {
            get
            {
                return this._currentRecordIndex;
            }
        }

        public bool MissingFieldFlag
        {
            get
            {
                return this._missingFieldFlag;
            }
        }

        public bool ParseErrorFlag
        {
            get
            {
                return this._parseErrorFlag;
            }
        }

        public int CommentAndEmptyLineSkipCount
        {
            get
            {
                return this.commentAndEmptyLineSkipCount;
            }
        }

        #endregion

        #endregion

        #region Indexers

        public string this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }

                var index = this.GetFieldIndex(name);
                if (index < 0)
                {
                    if (this._nullForSkipColumn)
                    {
                        return null;
                    }

                    throw new IndexOutOfRangeException(string.Format(Resources.FieldHeaderNotFound, name));
                }

                return this[index];
            }
        }

        public string this[int index]
        {
            get
            {
                var value = this.ReadField(index, false, false);
                return value == string.Empty ? null : value;
            }
        }

        #endregion

        #region Methods

        public int GetFieldIndex(string header)
        {
            this.EnsureInitialize();

            int index;

            if (this._fieldHeaderIndexes != null && this._fieldHeaderIndexes.TryGetValue(header, out index))
            {
                return index;
            }
            else if (!this._hasHeaders)
            {
                if (int.TryParse(header, out index))
                {
                    return index;
                }
            }

            return -1;
        }

        public void CopyCurrentRecordTo(string[] array)
        {
            this.CopyCurrentRecordTo(array, 0);
        }

        public void CopyCurrentRecordTo(string[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException("index", index, string.Empty);
            }

            if (this._currentRecordIndex < 0 || !this._initialized)
            {
                throw new InvalidOperationException(Resources.CsvReaderNoCurrentRecord);
            }

            if (array.Length - index < this._fieldCount)
            {
                throw new ArgumentException(Resources.CsvReaderNotEnoughSpaceInArray, "array");
            }

            for (int i = 0; i < this._fieldCount; i++)
            {
                if (this._parseErrorFlag)
                {
                    array[index + i] = null;
                }
                else
                {
                    array[index + i] = this[i];
                }
            }
        }

        public string GetCurrentRawData()
        {
            if (this._buffer != null && this._bufferLength > 0)
            {
                return new string(this._buffer, 0, this._bufferLength);
            }
            else
            {
                return string.Empty;
            }
        }

        public void MoveTo(long record)
        {
            if (record < 0)
            {
                throw new ArgumentOutOfRangeException("record", record, Resources.CsvReaderRecordIndexLessThanZero);
            }

            if (record < this._currentRecordIndex)
            {
                throw new InvalidOperationException(Resources.CsvReaderCannotMovePreviousRecordInForwardOnly);
            }

            // Get fileNumber of sourceRecord to read
            long offset = record - this._currentRecordIndex;

            if (offset > 0)
            {
                do
                {
                    if (!this.ReadNextRecord(false, false))
                    {
                        throw new EndOfStreamException(
                            string.Format(
                                CultureInfo.InvariantCulture, 
                                Resources.CsvReaderCannotReadRecordAtIndex, 
                                this._currentRecordIndex - offset));
                    }
                }
                while (--offset > 0);
            }
        }

        private void EnsureInitialize()
        {
            if (!this._initialized)
            {
                this.ReadNextRecord(true, false);
            }

            Debug.Assert(this._fieldHeaders != null);
            Debug.Assert(
                this._fieldHeaders.Length > 0 || (this._fieldHeaders.Length == 0 && this._fieldHeaderIndexes == null));
        }

        private bool IsWhiteSpace(char c)
        {
            // Handle cases where the delimiter is a whitespace (e.g. tab)
            if (c == this._fieldDelimiter)
            {
                return false;
            }
            else
            {
                // See char.IsLatin1(char c) in Reflector
                if (c <= '\x00ff')
                {
                    return c == ' ' || c == '\t';
                }
                else
                {
                    return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator;
                }
            }
        }

        private bool ParseNewLine(ref int pos)
        {
            Debug.Assert(pos <= this._bufferLength);

            // Check if already at the end of the buffer
            if (pos == this._bufferLength)
            {
                pos = 0;

                if (!this.ReadBuffer())
                {
                    return false;
                }
            }

            char c = this._buffer[pos];

            // Treat \r as new line only if it's not the delimiter
            if (c == this._lineDelimiter1 && this._fieldDelimiter != this._lineDelimiter1)
            {
                pos++;

                // Skip following \n (if there is one)
                if (pos < this._bufferLength)
                {
                    if (this._buffer[pos] == this._lineDelimiter2)
                    {
                        pos++;
                    }
                }
                else
                {
                    if (this.ReadBuffer())
                    {
                        if (this._buffer[0] == this._lineDelimiter2)
                        {
                            pos = 1;
                        }
                        else
                        {
                            pos = 0;
                        }
                    }
                }

                if (pos >= this._bufferLength)
                {
                    this.ReadBuffer();
                    pos = 0;
                }

                return true;
            }
            else if (c == this._lineDelimiter2)
            {
                pos++;

                if (pos >= this._bufferLength)
                {
                    this.ReadBuffer();
                    pos = 0;
                }

                return true;
            }

            return false;
        }

        private bool IsNewLine(int pos)
        {
            Debug.Assert(pos < this._bufferLength);

            char c = this._buffer[pos];

            if (c == this._lineDelimiter2)
            {
                return true;
            }
            else if (c == this._lineDelimiter1 && this._fieldDelimiter != this._lineDelimiter1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ReadBuffer()
        {
            if (this._eof)
            {
                return false;
            }

            this.CheckDisposed();

            this._bufferLength = this._sourceReader.Read(this._buffer, 0, this._bufferSize);

            if (this._bufferLength > 0)
            {
                return true;
            }
            else
            {
                this._eof = true;
                this._buffer = null;

                return false;
            }
        }

        private string ReadField(int field, bool initializing, bool discardValue)
        {
            if (!initializing)
            {
                if (field < 0 || field >= this._fieldCount)
                {
                    throw new ArgumentOutOfRangeException(
                        "field", 
                        field, 
                        string.Format(CultureInfo.InvariantCulture, Resources.FieldIndexOutOfRange, field));
                }

                if (this._currentRecordIndex < 0)
                {
                    throw new InvalidOperationException(Resources.CsvReaderNoCurrentRecord);
                }

                // Directly return name if cached
                if (this._fields[field] != null)
                {
                    return this._fields[field];
                }
                else if (this._missingFieldFlag)
                {
                    return this.HandleMissingField(null, field, ref this._nextFieldStart);
                }
            }

            this.CheckDisposed();

            int index = this._nextFieldIndex;

            while (index < field + 1)
            {
                // Handle case where stated start of name is past buffer
                // This can occur because _nextFieldStart is simply 1 + last char position of previous name
                if (this._nextFieldStart == this._bufferLength)
                {
                    this._nextFieldStart = 0;

                    // Possible EOF will be handled later (see Handle_EOF1)
                    this.ReadBuffer();
                }

                string value = null;

                if (this._missingFieldFlag)
                {
                    value = this.HandleMissingField(value, index, ref this._nextFieldStart);
                }
                else if (this._nextFieldStart == this._bufferLength)
                {
                    // Handle_EOF1: Handle EOF here

                    // If current name is the requested name, then the defaultValue of the name is "" as in "f1,f2,f3,(\s*)"
                    // otherwise, the CSV is malformed
                    if (index == field)
                    {
                        if (!discardValue)
                        {
                            value = string.Empty;
                            this._fields[index] = value;
                        }
                    }
                    else
                    {
                        value = this.HandleMissingField(value, index, ref this._nextFieldStart);
                    }
                }
                else
                {
                    // Trim spaces at start
                    if ((this._trimmingOptions & ValueTrimmingOptions.UnquotedOnly) != 0)
                    {
                        this.SkipWhiteSpaces(ref this._nextFieldStart);
                    }

                    if (this._eof)
                    {
                        value = string.Empty;
                    }
                    else if (this._buffer[this._nextFieldStart] != this._quote)
                    {
                        // Non-quoted name
                        int start = this._nextFieldStart;
                        int pos = this._nextFieldStart;

                        for (;;)
                        {
                            while (pos < this._bufferLength)
                            {
                                char c = this._buffer[pos];

                                if (c == this._fieldDelimiter)
                                {
                                    this._nextFieldStart = pos + 1;

                                    break;
                                }
                                else if (c == this._lineDelimiter1 || c == this._lineDelimiter2)
                                {
                                    this._nextFieldStart = pos;
                                    this._eol = true;

                                    break;
                                }
                                else
                                {
                                    pos++;
                                }
                            }

                            if (pos < this._bufferLength)
                            {
                                break;
                            }
                            else
                            {
                                if (!discardValue)
                                {
                                    value += new string(this._buffer, start, pos - start);
                                }

                                start = 0;
                                pos = 0;
                                this._nextFieldStart = 0;

                                if (!this.ReadBuffer())
                                {
                                    break;
                                }
                            }
                        }

                        if (!discardValue)
                        {
                            if ((this._trimmingOptions & ValueTrimmingOptions.UnquotedOnly) == 0)
                            {
                                if (!this._eof && pos > start)
                                {
                                    value += new string(this._buffer, start, pos - start);
                                }
                            }
                            else
                            {
                                if (!this._eof && pos > start)
                                {
                                    // Do the trimming
                                    pos--;
                                    while (pos > -1 && this.IsWhiteSpace(this._buffer[pos]))
                                    {
                                        pos--;
                                    }

                                    pos++;

                                    if (pos > 0)
                                    {
                                        value += new string(this._buffer, start, pos - start);
                                    }
                                }
                                else
                                {
                                    pos = -1;
                                }

                                // If pos <= 0, that means the trimming went past buffer start,
                                // and the concatenated defaultValue needs to be trimmed too.
                                if (pos <= 0)
                                {
                                    pos = value == null ? -1 : value.Length - 1;

                                    // Do the trimming
                                    while (pos > -1 && this.IsWhiteSpace(value[pos]))
                                    {
                                        pos--;
                                    }

                                    pos++;

                                    if (pos > 0 && pos != value.Length)
                                    {
                                        value = value.Substring(0, pos);
                                    }
                                }
                            }

                            if (value == null)
                            {
                                value = string.Empty;
                            }
                        }

                        if (this._eol || this._eof)
                        {
                            this._eol = this.ParseNewLine(ref this._nextFieldStart);

                            // Reaching a new line is ok as long as the parser is initializing or it is the last name
                            if (!initializing && index != this._fieldCount - 1)
                            {
                                if (value != null && value.Length == 0)
                                {
                                    value = null;
                                }

                                value = this.HandleMissingField(value, index, ref this._nextFieldStart);
                            }
                        }

                        if (!discardValue)
                        {
                            this._fields[index] = value;
                        }
                    }
                    else
                    {
                        // Quoted name

                        // Skip quote
                        int start = this._nextFieldStart + 1;
                        int pos = start;

                        bool quoted = true;
                        bool escaped = false;

                        if ((this._trimmingOptions & ValueTrimmingOptions.QuotedOnly) != 0)
                        {
                            this.SkipWhiteSpaces(ref start);
                            pos = start;
                        }

                        for (;;)
                        {
                            while (pos < this._bufferLength)
                            {
                                char c = this._buffer[pos];

                                if (escaped)
                                {
                                    escaped = false;
                                    start = pos;
                                }
                                    
                                    // IF current char is escape AND (escape and quote are different OR next char is a quote)
                                else if (c == this._escape
                                    && (this._escape != this._quote
                                        || (pos + 1 < this._bufferLength && this._buffer[pos + 1] == this._quote)
                                        || (pos + 1 == this._bufferLength
                                            && this._sourceReader.Peek() == this._quote)))
                                {
                                    if (!discardValue)
                                    {
                                        value += new string(this._buffer, start, pos - start);
                                    }

                                    escaped = true;
                                }
                                else if (c == this._quote)
                                {
                                    quoted = false;
                                    break;
                                }

                                pos++;
                            }

                            if (!quoted)
                            {
                                break;
                            }
                            else
                            {
                                if (!discardValue && !escaped)
                                {
                                    value += new string(this._buffer, start, pos - start);
                                }

                                start = 0;
                                pos = 0;
                                this._nextFieldStart = 0;

                                if (!this.ReadBuffer())
                                {
                                    this.HandleParseError(
                                        new MalformedCsvException(
                                            this.GetCurrentRawData(), 
                                            this._nextFieldStart, 
                                            Math.Max(0, this._currentRecordIndex), 
                                            index), 
                                        ref this._nextFieldStart);
                                    return null;
                                }
                            }
                        }

                        if (!this._eof)
                        {
                            // Append remaining parsed buffer content
                            if (!discardValue && pos > start)
                            {
                                value += new string(this._buffer, start, pos - start);
                            }

                            if (!discardValue && value != null
                                && (this._trimmingOptions & ValueTrimmingOptions.QuotedOnly) != 0)
                            {
                                int newLength = value.Length;
                                while (newLength > 0 && this.IsWhiteSpace(value[newLength - 1]))
                                {
                                    newLength--;
                                }

                                if (newLength < value.Length)
                                {
                                    value = value.Substring(0, newLength);
                                }
                            }

                            // Skip quote
                            this._nextFieldStart = pos + 1;

                            // Skip whitespaces between the quote and the delimiter/eol
                            this.SkipWhiteSpaces(ref this._nextFieldStart);

                            // Skip delimiter
                            bool delimiterSkipped;
                            if (this._nextFieldStart < this._bufferLength
                                && this._buffer[this._nextFieldStart] == this._fieldDelimiter)
                            {
                                this._nextFieldStart++;
                                delimiterSkipped = true;
                            }
                            else
                            {
                                delimiterSkipped = false;
                            }

                            // Skip new line delimiter if initializing or last name
                            // (if the next name is missing, it will be caught when parsed)
                            if (!this._eof && !delimiterSkipped && (initializing || index == this._fieldCount - 1))
                            {
                                this._eol = this.ParseNewLine(ref this._nextFieldStart);
                            }

                            // If no delimiter is present after the quoted name and it is not the last name, then it is a parsing error
                            if (!delimiterSkipped && !this._eof && !(this._eol || this.IsNewLine(this._nextFieldStart)))
                            {
                                this.HandleParseError(
                                    new MalformedCsvException(
                                        this.GetCurrentRawData(), 
                                        this._nextFieldStart, 
                                        Math.Max(0, this._currentRecordIndex), 
                                        index), 
                                    ref this._nextFieldStart);
                            }
                        }

                        if (!discardValue)
                        {
                            if (value == null)
                            {
                                value = string.Empty;
                            }

                            this._fields[index] = value;
                        }
                    }
                }

                this._nextFieldIndex = Math.Max(index + 1, this._nextFieldIndex);

                if (index == field)
                {
                    // If initializing, return null to signify the last name has been reached
                    if (initializing)
                    {
                        if (this._eol || this._eof)
                        {
                            return null;
                        }
                        else
                        {
                            return string.IsNullOrEmpty(value) ? string.Empty : value;
                        }
                    }
                    else
                    {
                        return value;
                    }
                }

                index++;
            }

            // Getting here is bad ...
            this.HandleParseError(
                new MalformedCsvException(
                    this.GetCurrentRawData(), this._nextFieldStart, Math.Max(0, this._currentRecordIndex), index), 
                ref this._nextFieldStart);
            return null;
        }

        private bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (this._eof)
            {
                if (this._firstRecordInCache)
                {
                    this._firstRecordInCache = false;
                    this._currentRecordIndex++;

                    return true;
                }
                else
                {
                    return false;
                }
            }

            this.CheckDisposed();

            if (!this._initialized)
            {
                this._buffer = new char[this._bufferSize];

                // will be replaced if and when headers are read
                this._fieldHeaders = new string[0];

                if (!this.ReadBuffer())
                {
                    return false;
                }

                if (!this.SkipEmptyAndCommentedLines(ref this._nextFieldStart))
                {
                    return false;
                }

                // Keep growing _fields array until the last name has been found
                // and then resize it to its final correct size
                this._fieldCount = 0;
                this._fields = new string[16];

                while (this.ReadField(this._fieldCount, true, false) != null)
                {
                    if (this._parseErrorFlag)
                    {
                        this._fieldCount = 0;
                        Array.Clear(this._fields, 0, this._fields.Length);
                        this._parseErrorFlag = false;
                        this._nextFieldIndex = 0;
                    }
                    else
                    {
                        this._fieldCount++;

                        if (this._fieldCount == this._fields.Length)
                        {
                            Array.Resize(ref this._fields, (this._fieldCount + 1) * 2);
                        }
                    }
                }

                // _fieldCount contains the last name index, but it must contains the name count,
                // so increment by 1
                this._fieldCount++;

                if (this._fields.Length != this._fieldCount)
                {
                    Array.Resize(ref this._fields, this._fieldCount);
                }

                this._initialized = true;

                // If headers are present, call ReadNextRecord again
                if (this._hasHeaders)
                {
                    // Don't count first sourceRecord as it was the headers
                    this._currentRecordIndex = -1;

                    this._firstRecordInCache = false;

                    this._fieldHeaders = new string[this._fieldCount];
                    this._fieldHeaderIndexes = new Dictionary<string, int>(this._fieldCount, _fieldHeaderComparer);

                    for (int i = 0; i < this._fields.Length; i++)
                    {
                        this._fieldHeaders[i] = this._fields[i];

                        if (this._fieldHeaderIndexes.ContainsKey(this._fields[i]))
                        {
                            // NOTE: Уже содержит поле возможно нарушена кодировка или действительно нарушена уникальность
                            var existsFields = string.Join(", ", this._fieldHeaderIndexes.Keys);
                            const string MessageFormat =
                                "Набор заголовков \"{0}\" уже содержит заголовок \"{1}\". Возможно не правильная кодировка.";
                            var message = string.Format(MessageFormat, existsFields, this._fields[i]);
                            throw new Exception(message);
                        }

                        this._fieldHeaderIndexes.Add(this._fields[i], i);
                    }

                    // Proceed to first sourceRecord
                    if (!onlyReadHeaders)
                    {
                        // Calling again ReadNextRecord() seems to be simpler, 
                        // but in fact would probably cause many subtle bugs because the derived does not expect a recursive behavior
                        // so simply do what is needed here and no more.
                        if (!this.SkipEmptyAndCommentedLines(ref this._nextFieldStart))
                        {
                            return false;
                        }

                        Array.Clear(this._fields, 0, this._fields.Length);
                        this._nextFieldIndex = 0;
                        this._eol = false;

                        this._currentRecordIndex++;

                        return true;
                    }
                }
                else
                {
                    if (onlyReadHeaders)
                    {
                        this._firstRecordInCache = true;
                        this._currentRecordIndex = -1;
                    }
                    else
                    {
                        this._firstRecordInCache = false;
                        this._currentRecordIndex = 0;
                    }

                    this._fieldHeaders = new string[this._fields.Length];
                    this._fieldHeaderIndexes = new Dictionary<string, int>(
                        this._fieldHeaders.Length, _fieldHeaderComparer);

                    for (int i = 0; i < this._fields.Length - 1; i++)
                    {
                        this._fieldHeaders[i] = i.ToString();
                        this._fieldHeaderIndexes.Add(this._fields[i], i);
                    }
                }
            }
            else
            {
                if (skipToNextLine)
                {
                    this.SkipToNextLine(ref this._nextFieldStart);
                }
                else if (this._currentRecordIndex > -1 && !this._missingFieldFlag)
                {
                    // If not already at end of sourceRecord, move there
                    if (!this._eol && !this._eof)
                    {
                        if (!this._supportsMultiline)
                        {
                            this.SkipToNextLine(ref this._nextFieldStart);
                        }
                        else
                        {
                            // a dirty trick to handle the case where extra fields are present
                            while (this.ReadField(this._nextFieldIndex, true, true) != null)
                            {
                            }
                        }
                    }
                }

                if (!this._firstRecordInCache && !this.SkipEmptyAndCommentedLines(ref this._nextFieldStart))
                {
                    return false;
                }

                if (this._hasHeaders || !this._firstRecordInCache)
                {
                    this._eol = false;
                }

                // Check to see if the first sourceRecord is in cache.
                // This can happen when initializing a mapReader with no headers
                // because one sourceRecord must be read to get the name count automatically
                if (this._firstRecordInCache)
                {
                    this._firstRecordInCache = false;
                }
                else
                {
                    Array.Clear(this._fields, 0, this._fields.Length);
                    this._nextFieldIndex = 0;
                }

                this._missingFieldFlag = false;
                this._parseErrorFlag = false;
                this._currentRecordIndex++;
            }

            return true;
        }

        private bool SkipEmptyAndCommentedLines(ref int pos)
        {
            if (pos < this._bufferLength)
            {
                this.DoSkipEmptyAndCommentedLines(ref pos);
            }

            while (pos >= this._bufferLength && !this._eof)
            {
                if (this.ReadBuffer())
                {
                    pos = 0;
                    this.DoSkipEmptyAndCommentedLines(ref pos);
                }
                else
                {
                    return false;
                }
            }

            return !this._eof;
        }

        private void DoSkipEmptyAndCommentedLines(ref int pos)
        {
            while (pos < this._bufferLength)
            {
                if (this._buffer[pos] == this._comment)
                {
                    pos++;
                    this.SkipToNextLine(ref pos);
                    this.commentAndEmptyLineSkipCount++;
                }
                else if (this._skipEmptyLines && this.ParseNewLine(ref pos))
                {
                    this.commentAndEmptyLineSkipCount++;
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        private bool SkipWhiteSpaces(ref int pos)
        {
            for (;;)
            {
                while (pos < this._bufferLength && this.IsWhiteSpace(this._buffer[pos]))
                {
                    pos++;
                }

                if (pos < this._bufferLength)
                {
                    break;
                }
                else
                {
                    pos = 0;

                    if (!this.ReadBuffer())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool SkipToNextLine(ref int pos)
        {
            // ((pos = 0) == 0) is a little trick to reset position inline
            while ((pos < this._bufferLength || (this.ReadBuffer() && ((pos = 0) == 0))) && !this.ParseNewLine(ref pos))
            {
                pos++;
            }

            return !this._eof;
        }

        private void HandleParseError(MalformedCsvException error, ref int pos)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this._parseErrorFlag = true;

            switch (this._defaultParseErrorAction)
            {
                case CsvParseErrorAction.ThrowException:
                    throw error;

                case CsvParseErrorAction.RaiseEvent:
                    CsvParseErrorEventArgs e = new CsvParseErrorEventArgs(error, CsvParseErrorAction.ThrowException);
                    this.OnParseError(e);

                    switch (e.Action)
                    {
                        case CsvParseErrorAction.ThrowException:
                            throw e.Error;

                        case CsvParseErrorAction.RaiseEvent:
                            throw new InvalidOperationException(
                                string.Format(
                                    CultureInfo.InvariantCulture, 
                                    Resources.CsvReaderParseErrorActionInvalidInsideParseErrorEvent, 
                                    e.Action), 
                                e.Error);

                        case CsvParseErrorAction.AdvanceToNextLine:

                            // already at EOL when fields are missing, so don't skip to next line in that case
                            if (!this._missingFieldFlag && pos >= 0)
                            {
                                this.SkipToNextLine(ref pos);
                            }

                            break;

                        default:
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture, 
                                    Resources.CsvReaderParseErrorActionNotSupported, 
                                    e.Action), 
                                e.Error);
                    }

                    break;

                case CsvParseErrorAction.AdvanceToNextLine:

                    // already at EOL when fields are missing, so don't skip to next line in that case
                    if (!this._missingFieldFlag && pos >= 0)
                    {
                        this.SkipToNextLine(ref pos);
                    }

                    break;

                default:
                    throw new NotSupportedException(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            Resources.CsvReaderParseErrorActionNotSupported, 
                            this._defaultParseErrorAction), 
                        error);
            }
        }

        private string HandleMissingField(string value, int fieldIndex, ref int currentPosition)
        {
            if (fieldIndex < 0 || fieldIndex >= this._fieldCount)
            {
                throw new ArgumentOutOfRangeException(
                    "fieldIndex", 
                    fieldIndex, 
                    string.Format(CultureInfo.InvariantCulture, Resources.FieldIndexOutOfRange, fieldIndex));
            }

            this._missingFieldFlag = true;

            for (int i = fieldIndex + 1; i < this._fieldCount; i++)
            {
                this._fields[i] = null;
            }

            if (value != null)
            {
                return value;
            }
            else
            {
                switch (this._missingFieldAction)
                {
                    case MissingFieldAction.ParseError:
                        this.HandleParseError(
                            new MissingFieldCsvException(
                                this.GetCurrentRawData(), 
                                currentPosition, 
                                Math.Max(0, this._currentRecordIndex), 
                                fieldIndex), 
                            ref currentPosition);
                        return value;

                    case MissingFieldAction.ReplaceByEmpty:
                        return string.Empty;

                    case MissingFieldAction.ReplaceByNull:
                        return null;

                    default:
                        throw new NotSupportedException(
                            string.Format(
                                CultureInfo.InvariantCulture, 
                                Resources.CsvReaderMissingFieldActionNotSupported, 
                                this._missingFieldAction));
                }
            }
        }

        private void ValidateDataReader(DataReaderValidations validations)
        {
            if ((validations & DataReaderValidations.IsInitialized) != 0 && !this._initialized)
            {
                throw new InvalidOperationException(Resources.CsvReaderNoCurrentRecord);
            }

            if ((validations & DataReaderValidations.IsNotClosed) != 0 && this._isDisposed)
            {
                throw new InvalidOperationException(Resources.CsvReaderClosed);
            }
        }

        private long CopyFieldToArray(
            int field, long fieldOffset, Array destinationArray, int destinationOffset, int length)
        {
            this.EnsureInitialize();

            if (field < 0 || field >= this._fieldCount)
            {
                throw new ArgumentOutOfRangeException(
                    "field", field, string.Format(CultureInfo.InvariantCulture, Resources.FieldIndexOutOfRange, field));
            }

            if (fieldOffset < 0 || fieldOffset >= int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("fieldOffset");
            }

            // Array.Copy(...) will do the remaining argument checks
            if (length == 0)
            {
                return 0;
            }

            string value = this[field];

            if (value == null)
            {
                value = string.Empty;
            }

            Debug.Assert(fieldOffset < int.MaxValue);

            Debug.Assert(destinationArray.GetType() == typeof(char[]) || destinationArray.GetType() == typeof(byte[]));

            if (destinationArray.GetType() == typeof(char[]))
            {
                Array.Copy(value.ToCharArray((int)fieldOffset, length), 0, destinationArray, destinationOffset, length);
            }
            else
            {
                char[] chars = value.ToCharArray((int)fieldOffset, length);
                byte[] source = new byte[chars.Length];
                

                for (int i = 0; i < chars.Length; i++)
                {
                    source[i] = Convert.ToByte(chars[i]);
                }

                Array.Copy(source, 0, destinationArray, destinationOffset, length);
            }

            return length;
        }

        private Exception GetCastException(Type type)
        {
            return
                new InvalidCastException(
                    string.Format(Resources.CsvReaderCannotConvertValue, _fieldType.FullName, type.FullName));
        }

        private void CheckIndex(int i)
        {
            if (i < 0 || i >= this._fieldCount)
            {
                throw new IndexOutOfRangeException(string.Format(Resources.CsvReaderFieldIndexNotFound, i));
            }
        }

        #endregion

        #region IDataReader Members

        int IDataReader.RecordsAffected
        {
            get
            {
                return -1;
            }
        }

        bool IDataReader.IsClosed
        {
            get
            {
                return this._eof;
            }
        }

        bool IDataReader.NextResult()
        {
            this.ValidateDataReader(DataReaderValidations.IsNotClosed);
            return false;
        }

        void IDataReader.Close()
        {
            this.Dispose();
        }

        public bool Read()
        {
            this.ValidateDataReader(DataReaderValidations.IsNotClosed);
            var retVal = this.ReadNextRecord(false, false);

            while (retVal && this.IsSkip())
            {
                retVal = this.ReadNextRecord(false, false);
            }

            return retVal;
        }

        private bool IsSkip()
        {
            if (!this._skipEmtpyRows)
            {
                return false;
            }

            for (int i = 0; i < this._fieldCount; i++)
            {
                if (this[i] != null)
                {
                    return false;
                }
            }

            return true;
        }

        int IDataReader.Depth
        {
            get
            {
                this.ValidateDataReader(DataReaderValidations.IsNotClosed);
                return 0;
            }
        }

        DataTable IDataReader.GetSchemaTable()
        {
            this.EnsureInitialize();
            this.ValidateDataReader(DataReaderValidations.IsNotClosed);

            DataTable schema = new DataTable("SchemaTable");
            schema.Locale = CultureInfo.InvariantCulture;
            schema.MinimumCapacity = this._fieldCount;

            schema.Columns.Add(SchemaTableColumn.AllowDBNull, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseTableName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnSize, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.DataType, typeof(object)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsAliased, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsExpression, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsKey, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsLong, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsUnique, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericPrecision, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericScale, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ProviderType, typeof(int)).ReadOnly = true;

            schema.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)).ReadOnly = true;

            string[] columnNames;

            if (this._hasHeaders)
            {
                columnNames = this._fieldHeaders;
            }
            else
            {
                columnNames = new string[this._fieldCount];

                for (int i = 0; i < this._fieldCount; i++)
                {
                    columnNames[i] = "Column" + i.ToString(CultureInfo.InvariantCulture);
                }
            }

            // null marks columns that will change for each row
            object[] schemaRow = new object[]
                                 {
                                     true, // 00- AllowDBNull
                                     null, // 01- BaseColumnName
                                     string.Empty, // 02- BaseSchemaName
                                     string.Empty, // 03- BaseTableName
                                     null, // 04- ColumnName
                                     null, // 05- ColumnOrdinal
                                     int.MaxValue, // 06- ColumnSize
                                     typeof(string), // 07- DataType
                                     false, // 08- IsAliased
                                     false, // 09- IsExpression
                                     false, // 10- IsKey
                                     false, // 11- IsLong
                                     false, // 12- IsUnique
                                     DBNull.Value, // 13- NumericPrecision
                                     DBNull.Value, // 14- NumericScale
                                     (int)DbType.String, // 15- ProviderType

                                     string.Empty, // 16- BaseCatalogName
                                     string.Empty, // 17- BaseServerName
                                     false, // 18- IsAutoIncrement
                                     false, // 19- IsHidden
                                     true, // 20- IsReadOnly
                                     false // 21- IsRowVersion
                                 };

            for (int i = 0; i < columnNames.Length; i++)
            {
                schemaRow[1] = columnNames[i]; // Base column stepName
                schemaRow[4] = columnNames[i]; // Column stepName
                schemaRow[5] = i; // Column ordinal

                schema.Rows.Add(schemaRow);
            }

            return schema;
        }

        #endregion

        #region IDataRecord Members

        object IDataRecord.this[string name]
        {
            get
            {
                this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
                return this[name];
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
                return this[i];
            }
        }

        bool IDataRecord.GetBoolean(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Boolean));
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
            return this.CopyFieldToArray(i, fieldOffset, buffer, bufferoffset, length);
        }

        byte IDataRecord.GetByte(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Byte));
        }

        char IDataRecord.GetChar(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Char));
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
            return this.CopyFieldToArray(i, fieldoffset, buffer, bufferoffset, length);
        }

        IDataReader IDataRecord.GetData(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(IDataReader));
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
            return _fieldType.FullName;
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(DateTime));
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Decimal));
        }

        double IDataRecord.GetDouble(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Double));
        }

        Guid IDataRecord.GetGuid(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Guid));
        }

        Type IDataRecord.GetFieldType(int i)
        {
            this.EnsureInitialize();
            this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);

            this.CheckIndex(i);
            return _fieldType;
        }

        float IDataRecord.GetFloat(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Single));
        }

        int IDataRecord.GetValues(object[] values)
        {
            this.ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
            for (int i = 0; i < this._fieldCount; i++)
            {
                values[i] = this[i];
            }

            return this._fieldCount;
        }

        bool IDataRecord.IsDBNull(int i)
        {
            this.CheckIndex(i);
            return false;
        }

        short IDataRecord.GetInt16(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Int16));
        }

        int IDataRecord.GetInt32(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Int32));
        }

        long IDataRecord.GetInt64(int i)
        {
            this.CheckIndex(i);
            throw this.GetCastException(typeof(Int64));
        }

        string IDataRecord.GetName(int i)
        {
            this.CheckIndex(i);

            if (this._hasHeaders)
            {
                return this._fieldHeaders[i];
            }
            else
            {
                return i.ToString();
            }
        }

        int IDataRecord.GetOrdinal(string name)
        {
            int index;
            if (!this._fieldHeaderIndexes.TryGetValue(name, out index))
            {
                throw new IndexOutOfRangeException(string.Format(Resources.FieldHeaderNotFound, name));
            }

            return index;
        }

        string IDataRecord.GetString(int i)
        {
            return this[i];
        }

        object IDataRecord.GetValue(int i)
        {
            return this[i];
        }

        #endregion

        #region IEnumerable<IDataRecord> Members

        public IEnumerator<IDataRecord> GetEnumerator()
        {
            return new DataRecordEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IDisposable members

#if DEBUG
        private StackTrace _allocStack;
#endif

        private bool _isDisposed;

        private readonly object _lock = new object();

        [Browsable(false)]
        public bool IsDisposed
        {
            get
            {
                return this._isDisposed;
            }
        }

        /// <summary>
        ///     Checks if the instance has been disposed of, and if it has, throws an
        ///     <see
        ///         cref="T:System.ComponentModel.ObjectDisposedException" />
        ///     ; otherwise, does nothing.
        /// </summary>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///     The instance has been disposed of.
        /// </exception>
        /// <remarks>
        ///     Derived classes should call this method at the start of all methods and properties that should not be accessed after a call to
        ///     <see
        ///         cref="M:Dispose()" />
        ///     .
        /// </remarks>
        private void CheckDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        /// <summary>
        ///     Releases all resources used by the instance.
        /// </summary>
        /// <remarks>
        ///     Calls <see cref="M:Dispose(Boolean)" /> with the disposing param set to <see langword="true" /> to free unmanaged and managed Resources.
        /// </remarks>
        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     Releases the unmanaged resources used by this instance and optionally releases the managed Resources.
        /// </summary>
        /// <param stepName="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged Resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            // Refer to http://www.bluebytesoftware.com/blog/PermaLink,guid,88e62cdf-5919-4ac7-bc33-20c06ae539ae.aspx
            // Refer to http://www.gotdotnet.com/team/libraries/whitepapers/resourcemanagement/resourcemanagement.aspx

            // No exception should ever be thrown except in critical scenarios.
            // Unhandled exceptions during finalization will tear down the process.
            if (!this._isDisposed)
            {
                try
                {
                    // Dispose-time code should call Dispose() on all owned objects that implement the IDisposable interface. 
                    // "owned" means objects whose lifetime is solely controlled by the container. 
                    // In cases where ownership is not as straightforward, techniques such as HandleCollector can be used.  
                    // Large managed object fields should be nulled out.

                    // Dispose-time code should also set references of all owned objects to null, after disposing them. This will allow the referenced objects to be garbage collected even if not all references to the "parent" are released. It may be a significant memory consumption win if the referenced objects are large, such as big arrays, collections, etc. 
                    if (disposing)
                    {
                        // Acquire a lock on the object while disposing.
                        if (this._sourceReader != null)
                        {
                            lock (this._lock)
                            {
                                if (this._sourceReader != null)
                                {
                                    this._sourceReader.Dispose();

                                    // _sourceReader = null;
                                    this._buffer = null;
                                    this._eof = true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Ensure that the flag is set
                    this._isDisposed = true;
                }
            }
        }

        ~CsvReader()
        {
#if DEBUG
            Debug.WriteLine("FinalizableObject was not disposed" + this._allocStack);
#endif

            this.Dispose(false);
        }

        #endregion
    }
}
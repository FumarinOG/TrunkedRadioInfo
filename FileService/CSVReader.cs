using System.Collections;
using System.IO;
using System.Text;

namespace FileService
{
    public sealed class CSVReader
    {
        private readonly TextReader _textStream;
        private bool _isEndOfStream;
        private bool _isEndOfLine;
        private readonly char[] _buffer = new char[4096];
        private int _position;
        private int _length;

        public CSVReader(TextReader stream)
        {
            _textStream = stream;
        }

        public string[] GetNextRow()
        {
            var textRow = new ArrayList();

            while (true)
            {
                var item = GetNextItem();

                if (item == null)
                {
                    if (textRow.Count == 0)
                    {
                        return null;
                    }

                    return (string[])textRow.ToArray(typeof(string));
                }

                textRow.Add(item);
            }
        }

        private string GetNextItem()
        {
            var isQuoted = false;
            var isPreData = true;
            var isPostData = false;
            var item = new StringBuilder();

            if (_isEndOfLine)
            {
                _isEndOfLine = false;
                return null;
            }

            while (true)
            {
                var nextChar = GetNextChar(true);

                if (_isEndOfStream)
                {
                    if (item.Length > 0)
                    {
                        return item.ToString();
                    }

                    return null;
                }

                if ((isPostData || !isQuoted) && nextChar == ',')
                {
                    return item.ToString();
                }

                if ((isPreData || isPostData || !isQuoted) && ((nextChar == '\x0A') || (nextChar == '\x0D')))
                {
                    _isEndOfLine = true;

                    if ((nextChar == '\x0D') && (GetNextChar(false) == '\x0A'))
                    {
                        GetNextChar(true);
                    }

                    return item.ToString();
                }

                if (isPreData && (nextChar == ' '))
                {
                    continue;
                }

                if (isPreData && (nextChar == '"'))
                {
                    isQuoted = true;
                    isPreData = false;
                    continue;
                }

                if (isPreData)
                {
                    isPreData = false;
                    item.Append(nextChar);
                    continue;
                }

                if ((nextChar == '"') & isQuoted)
                {
                    if (GetNextChar(false) == '"')
                    {
                        item.Append(GetNextChar(true));
                    }
                    else
                    {
                        isPostData = true;
                    }

                    continue;
                }

                item.Append(nextChar);
            }
        }

        private char GetNextChar(bool canAdvance)
        {
            if (_position >= _length)
            {
                _length = _textStream.ReadBlock(_buffer, 0, _buffer.Length);

                if (_length == 0)
                {
                    _isEndOfStream = true;
                    return '\0';
                }

                _position = 0;
            }

            if (canAdvance)
            {
                return _buffer[_position++];
            }

            return _buffer[_position];
        }
    }
}

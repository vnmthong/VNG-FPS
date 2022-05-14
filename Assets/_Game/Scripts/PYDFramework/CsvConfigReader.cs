using System.Collections.Generic;
using System.Globalization;

namespace PYDFramework.Config
{
    public class CsvConfigReader : IConfigReader
    {
        public List<List<List<string>>> data;
        public int idCount => data.Count;
        public int fieldCount { get; private set; }
        public int currentIdPosition { get; private set; }
        public int currentFieldPosition { get; private set; }

        public static char delimiter = ',';

        public CsvConfigReader(string textData)
        {
            var reader = new CsvReader(System.Text.Encoding.UTF8, textData.Trim(' ', '\n', '\r'));
            reader.HasHeaderRow = true;
            // read headers
            if(reader.ReadNextRecord())
            {
                fieldCount = reader.FieldCount ?? 0;
            }
            // read content
            data = new List<List<List<string>>>();
            List<List<string>> currentIdData = null;
            string prevId = "";
            while (reader.ReadNextRecord())
            {
                var fields = reader.Fields;
                if (fields.Count == 0)
                    continue;
                if (fields.Count != fieldCount)
                    continue;
                var id = fields[0];
                
                if (id != "" && id != prevId)
                {
                    prevId = id;
                    currentIdData = new List<List<string>>();
                    for(var i = 0; i < fieldCount; ++i)
                        currentIdData.Add(new List<string>());
                    data.Add(currentIdData);
                }
                for(var fieldIndex = 0; fieldIndex < fields.Count; ++fieldIndex)
                {
                    currentIdData[fieldIndex].Add(fields[fieldIndex]);
                }
            }
        }

        public bool Next()
        {
            if (!HasNext())
                return false;

            currentFieldPosition++;
            if (currentFieldPosition >= fieldCount)
            {
                currentFieldPosition = 0;
                currentIdPosition++;
            }
            return true;
        }

        public string Read()
        {
            var field = data[currentIdPosition][currentFieldPosition][0];
            Next();
            return field;
        }

        public bool HasNext()
        {
            if (currentIdPosition + 1 == idCount && currentFieldPosition + 1 == fieldCount)
                return false;
            return true;
        }

        public List<string> ReadArray()
        {
            var fields = data[currentIdPosition][currentFieldPosition];
            Next();
            return fields;
        }

        public bool ReadBool()
        {
            bool val = false;
            bool.TryParse(Read(), out val);
            return val;
        }

        public int ReadInt()
        {
            int val = 0;
            int.TryParse(Read(), out val);
            return val;
        }

        public long ReadLong()
        {
            long val = 0;
            long.TryParse(Read(), out val);
            return val;
        }

        public float ReadFloat()
        {
            float val = 0;
            float.TryParse(Read(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val);
            return val;
        }

        public double ReadDouble()
        {
            double val = 0;
            double.TryParse(Read(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val);
            return val;
        }

        public string ReadString()
        {
            return Read();
        }

        public bool[] ReadBoolArr()
        {
            return ReadArray().ConvertAll(str => {
                bool val = false;
                bool.TryParse(str, out val);
                return val;
            }).ToArray();
        }

        public int[] ReadIntArr()
        {
            return ReadArray().ConvertAll(str => {
                    int val = 0;
                    int.TryParse(str, out val);
                    return val;
                }).ToArray();
        }

        public long[] ReadLongArr()
        {
            return ReadArray().ConvertAll(str => {
                long val = 0;
                long.TryParse(str, out val);
                return val;
            }).ToArray();
        }

        public float[] ReadFloatArr()
        {
            return ReadArray().ConvertAll(str => {
                float val = 0;
                float.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val);
                return val;
            }).ToArray();
        }

        public double[] ReadDoubleArr()
        {
            return ReadArray().ConvertAll(str => {
                double val = 0;
                double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val);
                return val;
            }).ToArray();
        }

        public string[] ReadStringArr()
        {
            return ReadArray().ToArray();
        }
    }
}
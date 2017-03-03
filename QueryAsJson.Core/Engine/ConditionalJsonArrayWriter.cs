using Newtonsoft.Json;
using QueryAsJson.Core.Compiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Engine
{
    internal class ConditionalJsonArrayWriter
    {
        private bool arrayStarted;
        private JsonTextWriter jsonWriter;
        private MappedProperty mappedProperty;

        public ConditionalJsonArrayWriter(JsonTextWriter jsonWriter, MappedProperty mappedProperty)
        {
            this.jsonWriter = jsonWriter;
            this.mappedProperty = mappedProperty;
        }

        public void StartArray()
        {
            if (!arrayStarted)
            {
                arrayStarted = true;
                if (mappedProperty != null) jsonWriter.WritePropertyName(mappedProperty.TargetPropertyName);
                jsonWriter.WriteStartArray();
            }
        }

        public void EndArray()
        {
            if (arrayStarted) jsonWriter.WriteEndArray(); ;
        }
    }
}

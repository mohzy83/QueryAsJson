using QueryAsJson.Core.Compiled;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace QueryAsJson.Core
{
    /// <summary>
    /// Filemanager to save or load mappings to a json file
    /// </summary>
    public static class MappingFileManager
    {
        /// <summary>
        /// Loads the specified mapping file and generates a <see cref="MappedObject"/> with all its child.
        /// The <see cref="MappedObject"/> can be executed directly
        /// </summary>
        /// <param name="mappingFileName">Json file name. The Json must contain the mapping</param>
        /// <returns>fully populated <see cref="MappedObject"/> which can be executed by the mappingEngine</returns>
        public static MappedObject Load(string mappingFileName)
        {
            var json = File.ReadAllText(mappingFileName, Encoding.UTF8);
            var result = JsonConvert.DeserializeObject<MappedObject>(json);
            return result;
        }

        /// <summary>
        /// Saves a <see cref="MappedObject"/> including its childs to a json file
        /// </summary>
        /// <param name="mappedQuery">Root Object</param>
        /// <param name="fileName">filename of target file</param>
        public static void Save(this MappedObject mappedQuery, string fileName)
        {
            var json = JsonConvert.SerializeObject(mappedQuery, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(fileName, json, Encoding.UTF8);
        }
    }
}

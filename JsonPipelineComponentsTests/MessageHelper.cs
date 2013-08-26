using System.IO;

namespace JsonPipelineComponentsTests
{
    /// <summary>
    ///     Helper class for messages
    /// </summary>
    internal class MessageHelper
    {
        /// <summary>
        ///     Saves a message to disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        internal static void SaveMessage(string path, Stream data)
        {
            var rdr = new StreamReader(data);
            var writer = new StreamWriter(path);
            writer.Write(rdr.ReadToEnd());
            writer.Flush();
            data.Seek(0, SeekOrigin.Begin);
            writer.Close();
        }

        /// <summary>
        ///     Loads a message from disk
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Stream LoadMessage(string path)
        {
            var rdr = new StreamReader(path);
            return rdr.BaseStream;
        }
    }
}
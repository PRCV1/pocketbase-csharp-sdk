using pocketbase_csharp_sdk.Helper.Convert;
using System.Text.RegularExpressions;

namespace pocketbase_csharp_sdk.Models
{
    /// <summary>
    /// A Server-Sent message
    /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#fields"/>
    /// </summary>
    public class SseMessage
    {
        /// <summary>
        /// The event ID
        /// </summary>
        public string? Id { get; private set; }

        /// <summary>
        /// A string identifying the type of event described
        /// </summary>
        public string? Event { get; private set; }

        /// <summary>
        /// The data field for the message. 
        /// When the receives multiple consecutive lines that begin with data:, 
        /// it concatenates them, inserting a newline character between each one. 
        /// Trailing newlines are removed.
        /// </summary>
        public string? Data { get; private set; }

        /// <summary>
        /// The reconnection time. 
        /// If the connection to the server is lost, the browser will wait 
        /// for the specified time before attempting to reconnect. 
        /// This must be an integer, specifying the reconnection time in milliseconds. 
        /// If a non-integer value is specified, the field is ignored.
        /// </summary>
        public int? Retry { get; private set; }

        public override string ToString()
        {
            return $"Id:{Id}{Environment.NewLine}Event:{Event}{Environment.NewLine}Data:{Data}{Environment.NewLine}Retry:{Retry}";
        }

        /// <summary>
        /// Factory for the SseMessage from received message
        /// </summary>
        /// <param name="receivedMessage"></param>
        /// <returns></returns>
        public static async Task<SseMessage?> FromReceivedMessageAsync(string? receivedMessage)
        {
            if (receivedMessage == null)
                return null;
            var message = new SseMessage();
            string? line;
            using (var stringReader = new StringReader(receivedMessage))
            {
                while ((line = await stringReader.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("id:"))
                        message.Id = line["id:".Length..].Trim();
                    else if (line.StartsWith("event:"))
                        message.Event = line["event:".Length..].Trim();
                    else if (line.StartsWith("retry:"))
                        message.Retry = SafeConvert.ToInt(line["retry:".Length..].Trim());
                    else if (line.StartsWith("data:"))
                    {
                        // PocketBase returns multiple datas?
                        // If true, every data is a Json?
                        //      -> then it must be stored in a list of strings
                        var data = line["data:".Length..].Trim();
                        if (message.Data == null)
                            message.Data = data;
                        else
                            message.Data += Environment.NewLine + data;
                    }
                }
            }
            return message;
        }

        private static bool ProcessMessage(string? line, SseMessage message)
        {
            Regex regex = new Regex("^(\\w+)[\\s\\:]+(.*)?$");
            if (string.IsNullOrWhiteSpace(line))
            {
                return true;
            }

            var match = regex.Match(line);
            if (match is null)
            {
                return false;
            }

            var field = match.Groups[1].Value ?? "";
            var value = match.Groups[2].Value ?? "";

            switch (field)
            {
                case "id":
                    message.Id = value;
                    break;
                case "event":
                    message.Event = value;
                    break;
                case "retry":
                    message.Retry = SafeConvert.ToInt(value, 0);
                    break;
                case "data":
                    message.Data = value;
                    break;
            }

            return false;
        }
    }
}

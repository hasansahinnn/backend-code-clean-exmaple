using Newtonsoft.Json;

namespace Core.Extensions
{
    /// <summary>
    /// Represents details about an error, including the error message and status code.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code associated with the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Converts the ErrorDetails object to its equivalent JSON representation.
        /// </summary>
        /// <returns>A JSON string representing the ErrorDetails object.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

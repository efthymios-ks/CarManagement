using System.Diagnostics;

namespace CarManagement.Shared
{
    /// <summary>
    /// Basic Dto used for REST API.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ServiceResponse<T>
    {
        //Properties
        private string DebuggerDisplay { get { return ToString(); } }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

        //Constructors
        public ServiceResponse()
        {

        }

        public ServiceResponse(bool Success) : this(Success, string.Empty)
        {

        }

        public ServiceResponse(bool Success, string Message) : this(Success, Message, default(T))
        {
        }

        public ServiceResponse(bool Success, string Message, T Data)
        {
            this.Success = Success;
            this.Message = Message;
            this.Data = Data;
        }

        //Methods
        public override string ToString()
        {
            return $"[{(Success ? "Success" : "Failed")}] {Message}";
        }
    }
}

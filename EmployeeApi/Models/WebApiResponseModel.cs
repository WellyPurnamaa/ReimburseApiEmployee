using System;

namespace EmployeeApi.Models
{
    public class WebApiResponseModel
    {
        private string _statusCode;
        public string statusCode
        {
            get
            {
                return this._statusCode;
            }
            set
            {
                this._statusCode = value;
            }
        }

        private string _message;
        public string message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        private string _version;
        public string version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        private object _data;
        public object data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public WebApiResponseModel()
        {
            this.version = Environment.GetEnvironmentVariable("ASPNETCORE_VERSION");
        }
    }
}
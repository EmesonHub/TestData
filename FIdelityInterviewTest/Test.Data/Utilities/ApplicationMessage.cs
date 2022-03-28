using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data.Utilities
{
    public class ApplicationMessage
    {
        public const string OK = "Ok";
        public const string NOTFOUND = "Record not found. Its either the record have been deleted or the requested operation not allowed due to the status.";
        public const string EMPTYENTRY = "Empty input field(s) found, please review your entry for empty inputs.";
        public const string DUPLICATE = "Record already exist.";
        public const string SAVE = "Record saved successfully.";
        public const string UPDATE = "Record updated successfully.";
        public const string DELETE = "Record deleted successfully.";
        public const string INVALIDTOKEN = "Sorry, invalid session or current session have expired. Please re-login with your credentials.";
        public const string TRANSACTION_ERROR = "Error executing the initiated process. Please check your network connection/internet strength and try the process again. If error persist, contact the Admin.";
    }
}

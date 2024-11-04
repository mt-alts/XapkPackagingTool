/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.EventArg
{
    public class BuildFailedEventArgs : EventArgs
    {
        public string[] MessageFormatters { get; set; }

        public string Message { get; set; }

        public BuildFailedEventArgs(string message, string[] messageFormatters)
        {
            Message = message;
            MessageFormatters = messageFormatters;
        }
    }
}

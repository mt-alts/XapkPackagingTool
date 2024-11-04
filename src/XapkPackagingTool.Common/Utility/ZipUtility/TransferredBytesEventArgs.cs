/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public class TransferredBytesEventArgs
    {
        public long TransferredBytes { get; }

        public TransferredBytesEventArgs(long transferredBytes)
        {
            TransferredBytes = transferredBytes;
        }
    }
}

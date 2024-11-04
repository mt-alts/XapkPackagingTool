/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IProgressService
    {
        event EventHandler CancelRequired;
        void UpdateStatusMessageTitle(string message);
        void UpdateStatusMessage(string message, string title);
        void UpdateStatusMessage(string message);
        public void IndeterminateModeEnable(bool isDeterminate);
        void UpdateProgress(int progressValue);
        void UpdateProgress(int progressValue, string message);
        void ReportFailure(string message, string title);
        void ReportCompletion(string message, string title);
        void ShowProgress();
        void CloseProgress();
    }
}

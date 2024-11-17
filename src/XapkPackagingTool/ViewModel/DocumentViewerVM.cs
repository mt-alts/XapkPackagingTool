/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;

namespace XapkPackagingTool.ViewModel
{
    internal class DocumentViewerVM : ViewModelBase
    {
        public string DocumentName { get; }
        public string DocumentText { get; }

        public DocumentViewerVM()
        {
            DocumentName = "null";
            DocumentText = "null";
        }

        public DocumentViewerVM(string docPath)
        {
            try
            {
                if (!File.Exists(docPath))
                {
                    DocumentName = "StrFileNotFoundMessageTitle".Localize();
                    DocumentText = string.Format("StrFileNotFoundMessage".Localize(), docPath);
                }
                else
                {
                    DocumentName = Path.GetFileNameWithoutExtension(docPath);
                    DocumentText = System.IO.File.ReadAllText(docPath);
                }
            }
            catch (Exception exc)
            {
                DocumentName = "StrError".Localize();
                DocumentText = exc.Message;
            }
        }
    }
}

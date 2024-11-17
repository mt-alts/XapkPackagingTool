using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;
using XapkPackagingTool.Service;

namespace XapkPackagingTool.Utility.Validators.XapkPackage
{
    internal static class MetadataValidator
    {
        public static bool IsValid(IXapkManifest xapkManifest)
        {
            bool isValid 
                = 
                !string.IsNullOrWhiteSpace(xapkManifest.Name) &&
                !string.IsNullOrWhiteSpace(xapkManifest.VersionName) &&
                !string.IsNullOrWhiteSpace(xapkManifest.PackageName) &&
                !string.IsNullOrWhiteSpace(xapkManifest.TargetSdkVersion) &&
                !string.IsNullOrWhiteSpace(xapkManifest.MinSdkVersion) &&
                !string.IsNullOrWhiteSpace(xapkManifest.VersionCode);

            if (!isValid )
                throw new Exceptions.MetadataNotValidException("StrMetadataValidationError".Localize());
            
            return isValid;
        }
    }
}

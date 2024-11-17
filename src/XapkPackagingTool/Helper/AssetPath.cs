using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Helper
{
    internal class AssetPath
    {
        internal class Xapk
        {
            public static string PredefinedAppBinaryInterfaces
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetApplicationBinaryInterfaces
                    );
            }

            public static string BlankXapkConfigTemplate
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetBlankXapkConfigTemplate
                    );
            }

            public static string PredefinedDensityQualifiers
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetDensityQualifiers
                    );
            }

            public static string PredefinedExpansionInstallLocations
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetExpansionInstallLocations
                    );
            }

            public static string PredefinedLocaleCodes
            {
                get =>
                    Path.Combine(EnvironmentPaths.BasePath, Properties.Path.Default.AssetLocales);
            }

            public static string PredefinedPermissions
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetPredefinedPermissions
                    );
            }
        }

        internal class About
        {
            public static string ExternalComponents
            {
                get =>
                    Path.Combine(
                        EnvironmentPaths.BasePath,
                        Properties.Path.Default.AssetUsedComponentsJsonDataPath
                    );
            }
        }
    }
}

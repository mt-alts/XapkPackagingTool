﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharpXapkLib.Res {
    using System;
    
    
    /// <summary>
    ///   Yerelleştirilmiş dizeleri aramak gibi işlemler için, türü kesin olarak belirtilmiş kaynak sınıfı.
    /// </summary>
    // Bu sınıf ResGen veya Visual Studio gibi bir araç kullanılarak StronglyTypedResourceBuilder
    // sınıfı tarafından otomatik olarak oluşturuldu.
    // Üye eklemek veya kaldırmak için .ResX dosyanızı düzenleyin ve sonra da ResGen
    // komutunu /str seçeneğiyle yeniden çalıştırın veya VS projenizi yeniden oluşturun.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Bu sınıf tarafından kullanılan, önbelleğe alınmış ResourceManager örneğini döndürür.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SharpXapkLib.Res.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Tümü için geçerli iş parçacığının CurrentUICulture özelliğini geçersiz kular
        ///   CurrentUICulture özelliğini tüm kaynak aramaları için geçersiz kılar.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Access to the path &apos;{0}&apos; is denied. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string AccessDeniedMessage {
            get {
                return ResourceManager.GetString("AccessDeniedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The specified file could not be found: {0}. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string FileNotFoundExceptionMessage {
            get {
                return ResourceManager.GetString("FileNotFoundExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   An I/O error occurred while accessing the file: {0}. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string IOExceptionMessage {
            get {
                return ResourceManager.GetString("IOExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Package metadata could not be read from the file at location &apos;{0}&apos;. The file may be missing, corrupted, or improperly formatted. Please verify the integrity of the metadata file located at &apos;{0}&apos; and try again. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string MetadataCannotRead {
            get {
                return ResourceManager.GetString("MetadataCannotRead", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Package metadata cannot be created. System message: {0} benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string MetadataCreateError {
            get {
                return ResourceManager.GetString("MetadataCreateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The directory for packaging has not been specified. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string PackagingDirectoryNotSpecified {
            get {
                return ResourceManager.GetString("PackagingDirectoryNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The required resources for packaging have not been specified. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string PackagingResourcesNotSpecified {
            get {
                return ResourceManager.GetString("PackagingResourcesNotSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   An unexpected error has occurred. System message: {0} benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string UnexpectedErrorMessage {
            get {
                return ResourceManager.GetString("UnexpectedErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The file &apos;{1}&apos; could not be found in &apos;{0}&apos;. benzeri yerelleştirilmiş bir dize arar.
        /// </summary>
        internal static string ZipEntryNotFound {
            get {
                return ResourceManager.GetString("ZipEntryNotFound", resourceCulture);
            }
        }
    }
}

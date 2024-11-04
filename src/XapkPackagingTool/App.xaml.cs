/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;
using XapkPackagingTool.Common.Utility.FileSystemVirtualization;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Service.SystemServices;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.Utility.RecentItems;
using XapkPackagingTool.View;
using XapkPackagingTool.ViewModel;
using XapkPackagingTool.ViewModel.Main;
using XapkPackagingTool.ViewModel.Main.ApkVariants;
using XapkPackagingTool.ViewModel.Startup;

namespace XapkPackagingTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();
            LoadLanguage(FindSystemTwoLetterISOLanguageName());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var windService = ServiceProvider.GetRequiredService<IWindowService>();
            windService.ShowWindow<StartupViewModel>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IOpenFileService, OpenFileService>();
            services.AddSingleton<IConfirmDialogService, ConfirmDialogService>();
            services.AddSingleton<ISaveFileService, SaveFileService>();
            services.AddTransient<IViewModelHost, ViewModelHost>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IRecentManager, RecentManager>();
            services.AddSingleton<IProgressService, ProgressService>();
            services.AddSingleton<IPackageReader, PackageReader>();

            services.AddScoped<IXapkConfigService, XapkConfigService>();

            services.AddTransient<AboutViewModel>();
            services.AddTransient<PermissionsViewModel>();
            services.AddTransient<LocalesViewModel>();
            services.AddTransient<ExpansionsViewModel>();
            services.AddTransient<ApkVariantsViewModel>();
            services.AddTransient<MonolithicApkViewModel>();
            services.AddTransient<SplitsApkViewModel>();
            services.AddTransient<PackageMetadataViewModel>();
            services.AddTransient<PackagingOptionsViewModel>();
            services.AddTransient<PackageProgressViewModel>();
            services.AddSingleton<IMessageDialogService, MessageDialogService>();

            services.AddSingleton<IConfigService, ConfigService>();

            services.AddTransient<IVirtualFileSystem, VirtualFileSystem>();

            services.AddTransient<NewPackageViewModel>();
            services.AddTransient<GettingStartedViewModel>();

            services.AddTransient<StartupViewModel>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<NewPackageWindow>();
        }

        private ResourceDictionary FindDefaultLanguage()
        {
            var defaultLanguage = this.Resources.MergedDictionaries.FirstOrDefault(md =>
                md.Source != null
                && (md.Source.OriginalString.Equals("Resources/Strings/Resource.tr.xaml"))
            );
            return defaultLanguage;
        }

        private void LoadLanguage(string language)
        {
            var defaultLanguage = FindDefaultLanguage();

            if (defaultLanguage != null)
                this.Resources.MergedDictionaries.Remove(defaultLanguage);

            var resourceUri = new Uri(
                $"Resources/Strings/Resource.{(language == "tr" ? "tr" : "en")}.xaml",
                UriKind.Relative
            );

            var newLanguageDict = new ResourceDictionary { Source = resourceUri };
            this.Resources.MergedDictionaries.Add(newLanguageDict);
        }

        private static string FindSystemTwoLetterISOLanguageName()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            string systemLanguage = currentCulture.TwoLetterISOLanguageName;

            return systemLanguage;
        }
    }
}

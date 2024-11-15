using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Rehm2024.Common;
using Rehm2024.Service;
using Rehm2024.Tools;
using Rehm2024.ViewModels;
using Rehm2024.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Rehm2024
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            string url = ToolHelper.LoadSettings().Url; 
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@url, serviceKey: "webUrl");
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.RegisterForNavigation<LogInfoView, LogInfoViewModel>();
            containerRegistry.RegisterForNavigation<ConfigView, ConfigViewModel>();
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>();


            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();


            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IRehmService,RehmService>();


        }

        

    }
}
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Rehm.Shared.Dtos;
using Rehm2024.Common;
using Rehm2024.Common.Models;
using Rehm2024.Event;
using Rehm2024.Extensions;
using Rehm2024.Service;
using Rehm2024.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Rehm2024.ViewModels
{
    public class MainViewModel:BindableBase, IConfigureService
    {
        
        public MainViewModel(IRegionManager regionManager,IEventAggregator aggregator, IDialogHostService dialogHostService, IRehmService rehmService)
        {
            
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
            this.aggregator = aggregator;


            aggregator.GetEvent<SettingEvent>().Subscribe(UpdateData);

            ToolHelper.LoadSettings();

            this.dialogHostService = dialogHostService;
            this.rehmService = rehmService;
            timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));


            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3); // 每5秒触发一次  
            _timer.Tick += Timer_Tick;
            _timer.Start();

            MenuBar = new ObservableCollection<MenuBar>();
            CreateMenuBar();


            Sqlhelp.UpdateTable();


        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
            
        }

        private void UpdateData(AppSettings settings)
        {

            Task task = Task.Factory.StartNew(() =>
            {
                roi = new Roi(aggregator);

            });
            Thread.Sleep(1500);
            //roi.Dispose();
            //task.Wait();
            //task.Dispose();  
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
           
            
        }

        private async void TimerCallback(object state)
        {
            var res = await rehmService.Heartbeat(new HeartbeatDto()
            {
                messageId = Guid.NewGuid().ToString("N")
            }) ;
            //aggregator.GetEvent<MessagesEvent>().Publish("AAAAABBBBCCCC");

           
        }


        private void Navigate(string obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj)) return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
            
        }

       

        /// <summary>
        /// 用户控件页面导航
        /// </summary>
        public DelegateCommand<MenuBar> NavigateCommand { private set; get; }

        public DelegateCommand<string> LogComand { private set; get; }

        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;
        private IRegionNavigationJournal journal;


        private IDialogHostService dialogHostService;
        private readonly IRehmService rehmService;
        private System.Threading.Timer timer;

        private DispatcherTimer _timer;

        private Roi roi;

        private ObservableCollection<MenuBar> menuBar;
        public ObservableCollection<MenuBar> MenuBar
        {
            get { return menuBar; }
            set { menuBar = value; RaisePropertyChanged(); }
        }
        void CreateMenuBar()
        {
            MenuBar.Add(new MenuBar() { Icon = "Home", Title = "主页", NameSpace = "LogInfoView" });
            MenuBar.Add(new MenuBar() { Icon = "Cog", Title = "配置", NameSpace = "ConfigView" });
            MenuBar.Add(new MenuBar() { Icon = "Info", Title = "测试", NameSpace = "TestView" });
        }

        /// <summary>
        /// 配置首页初始化
        /// </summary>
        public void Configure()
        {
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("LogInfoView");
        }



       
    }
}

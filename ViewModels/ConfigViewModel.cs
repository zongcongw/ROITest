using Prism.Commands;
using Prism.Mvvm;
using Rehm.Shared.Dtos;
using Rehm2024.Common.Models;
using Rehm2024.Service;
using Rehm2024.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace Rehm2024.ViewModels
{
    public class ConfigViewModel : BindableBase
    {

        public ConfigViewModel(ILoginService loginService,IRehmService rehmService)
        {
          
            ItemsMenus = new ObservableCollection<ItemsMenu>();
           
            EnterSelectCommand = new DelegateCommand<string>(enterSelect);

            SaveCommand = new DelegateCommand(SaveParams);

            CancelCommand = new DelegateCommand(Cancel);

            loadParams();

            CreateItemBar();

            this.loginService = loginService;
            this.rehmService = rehmService; 

        }

        private void Cancel()
        {
            loadParams();
            itemsMenu[4].NameSpace = "Administrator";
        }

       
        private async void enterSelect(string obj)
        {

            // var updateResult = await toDoService.UpdateAsync(obj);

            var res = await rehmService.Heartbeat(new HeartbeatDto()
            {
                messageId = "cc46e9b9ebca48b9a2ec6f24ecb0875f2"
            }) ;

            var loginResult = await loginService.Login(new Rehm.Shared.Dtos.UserDto()
            {
                Account = "121",
                PassWord = "1212"
            });

            //await RehmService.


        }

        private void loadParams()
        {
            AppSettings appSettings = ToolHelper.LoadSettings("RehmConfig.json");
            url = appSettings.Url;
            workshop = appSettings.WorkshopNo;
            LineNo = appSettings.LineNo;
            equipmentName = appSettings.EquipmentName;
            equipmentNo = appSettings.EquipmentNo;
            userNo = appSettings.UserNo;
        }

        private void SaveParams()
        {
            AppSettings appSettings = new AppSettings
            {
                Url = url,
                WorkshopNo = itemsMenu[0].NameSpace,
                LineNo = itemsMenu[1].NameSpace,
                EquipmentName = itemsMenu[2].NameSpace,
                EquipmentNo = itemsMenu[3].NameSpace,
                UserNo = itemsMenu[4].NameSpace,

            };
            ToolHelper.SaveConfiguration(appSettings, "RehmConfig.json");
        }





        private string url;
        private string workshop;
        private string lineNo;
        private string equipmentName;
        private string equipmentNo;
        private string userNo;
        public string Url
        {
            get { return url; }
            set { url = value; RaisePropertyChanged(); }
        }

        public string Workshop
        {
            get { return workshop; }
            set { workshop = value; RaisePropertyChanged(); }
        }

        public string LineNo
        {
            get { return lineNo; }
            set { lineNo = value; RaisePropertyChanged(); }
        }

        public string EquipmentName
        {
            get { return equipmentName; }
            set { equipmentName = value; RaisePropertyChanged(); }
        }

        public string EquipmentNo
        {
            get { return equipmentNo; }
            set { equipmentNo = value; RaisePropertyChanged(); }
        }

        public string UserNo
        {
            get { return userNo; }
            set { userNo = value; RaisePropertyChanged(); }
        }

        private readonly IRehmService rehmService;  
        private readonly ILoginService loginService;
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand<MenuBar> LoaddataCommand { get; set; }
        public DelegateCommand<string> EnterSelectCommand { private set; get; }

        private ObservableCollection<ItemsMenu> itemsMenu;
        public ObservableCollection<ItemsMenu> ItemsMenus
        {
            get { return itemsMenu; }
            set { itemsMenu = value; RaisePropertyChanged(); }
        }
        void CreateItemBar()
        {
            ItemsMenus.Add(new ItemsMenu() { Title = "车间编号", NameSpace = Workshop });
            ItemsMenus.Add(new ItemsMenu() { Title = "产线编号", NameSpace = LineNo });
            ItemsMenus.Add(new ItemsMenu() { Title = "设备名称", NameSpace = EquipmentName });
            ItemsMenus.Add(new ItemsMenu() { Title = "设备编号", NameSpace = EquipmentNo });
            ItemsMenus.Add(new ItemsMenu() { Title = "用户编号", NameSpace = UserNo });
        }

       
    }
}

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Rehm2024.Common;
using Rehm2024.Common.Models;
using Rehm2024.Event;
using Rehm2024.Extensions;
using Rehm2024.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rehm2024.ViewModels
{
    public class TestViewModel : BindableBase
    {
        public TestViewModel(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            Segmentlist = new ObservableCollection<string>();
            SaveCommand = new DelegateCommand(SaveParams);
            ConnCommand = new DelegateCommand<string>(ConnHost);
            LoadParams();
            this.aggregator = aggregator;
            this.dialogHostService = dialogHostService;
        }

        private void ConnHost(string obj)
        {
            try
            {
                if (obj == "Connect")
                {
                    //第一步：调用socket()函数创建一个用于通讯的套接字
                    listensocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //第二步：给已经创建好的套接字绑定一个端口号，这一般通过设置网络套接口地址和调用blind()函数来实现
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip_Socket.Trim()), int.Parse(port_Socket.Trim()));
                    try
                    {
                        listensocket.Bind(endPoint);

                    }
                    catch (Exception ex)
                    {
                        aggregator.SendMessage(ex.Message);
                        return;

                    }
                    //第三步：调用listen()函数使套接字成为一个监听套接字

                    listensocket.Listen(1);//最大允许1个客户端进行连接


                    //开启线程监听
                     Task.Run(new Action(() =>
                        {
                            Listenconnection();
                        }));
                    IsEnable = false;
                    aggregator.SendMessage("打开服务端成功");

                }
                else if (obj == "Disconn")
                {
                    if (listensocket != null)
                    {
                        listensocket.Close();
                        listensocket.Dispose();
                    }
                        
                    if (clientsocket!=null)
                        clientsocket.Close();

                    IsEnable = true;

                }
                else if (obj == "Send")
                {
                    if (clientsocket != null)
                        clientsocket.Send(ToolHelper.HexStringToByteArray("02" + ToolHelper.StringToHexString(content) + "0D"));

                }
            }
            catch (Exception)
            {

                
            }
            
        }

        private void Listenconnection()
        {
            while (true)
            {
                try
                {
                    clientsocket = listensocket.Accept();//创建1个新的socket为新创建的连接

                    string ip = clientsocket.RemoteEndPoint.ToString();

                    //开启线程
                    Task.Run(() => ReceiveMsg(clientsocket));
                }
                catch (Exception)
                {

                    break;
                }
               

            }
        }

        /// <summary>
        /// 接收方法
        /// </summary>
        /// <param name="clientsocket"></param>
        private void ReceiveMsg(Socket clientsocket)
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024];//定义1MB容量缓存区
                int length = -1;
                try
                {
                    length = clientsocket.Receive(buffer);
                }
                catch (Exception)
                {

                    //客户端下线了
                    //更新在线列表
                    string ip = clientsocket.RemoteEndPoint.ToString();

                    break;
                }

                if (length > 0)
                {

                    string info = Encoding.ASCII.GetString(buffer, 0, length).Trim();

                    if(info == "\u0002S")
                    {
                        if (clientsocket != null) 
                            clientsocket.Send(ToolHelper.HexStringToByteArray("02" + ToolHelper.StringToHexString(DateTime.Now.ToFileTimeUtc().ToString()) + "0D"));
                        Console.Write(ToolHelper.StringToHexString(DateTime.Now.ToFileTimeUtc().ToString()));
                    }



                }
            }
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        private void LoadParams()
        {
            AppSettings appSettings = ToolHelper.LoadSettings();
            url = appSettings.Url;
            workshop = appSettings.WorkshopNo;
            lineNo = appSettings.LineNo;
            equipmentName = appSettings.EquipmentName;
            equipmentNo = appSettings.EquipmentNo;
            userNo = appSettings.UserNo;


            Segmentlist.Clear();
            Segmentlist.Add("item1121");
            Segmentlist.Add("item1122");
            Segmentlist.Add("item1123");
            Segment = segmentlist.FirstOrDefault();

            port_Socket = "8080";
            ip_Socket = "192.168.41.44";
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        private void SaveParams()
        {
            AppSettings appSettings = new AppSettings{
                Url = url,
                WorkshopNo = workshop,
                LineNo = lineNo,
                EquipmentName = equipmentName,
                UserNo = userNo,
                EquipmentNo = equipmentNo,
            };
            ToolHelper.SaveConfiguration(appSettings);


            aggregator.GetEvent<SettingEvent>().Publish(appSettings);

        }

        private bool isEnable=true;

        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; RaisePropertyChanged(); }
        }



        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand<string> ConnCommand { get; set; }

        private Socket listensocket;//监听socket

        private Socket clientsocket;

        private string url;


        private string ip_Socket;

        public string IP_Socket
        {
            get { return ip_Socket; }
            set { ip_Socket = value; RaisePropertyChanged(); }
        }

        private string port_Socket;

        public string Port_Socket
        {
            get { return port_Socket; }
            set { port_Socket = value; RaisePropertyChanged(); }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        }


        public string Url
        {
            get { return url; }
            set { url = value; RaisePropertyChanged(); }
        }

        

        private string workshop;

        public string Workshop
        {
            get { return workshop; }
            set { workshop = value; RaisePropertyChanged(); }
        }

        private string lineNo;

        public string LineNo
        {
            get { return lineNo; }
            set { lineNo = value; RaisePropertyChanged(); }
        }

        private string equipmentName;

        public string EquipmentName
        {
            get { return equipmentName; }
            set { equipmentName = value; RaisePropertyChanged(); }
        }

        private string equipmentNo;

        public string EquipmentNo
        {
            get { return equipmentNo; }
            set { equipmentNo = value; RaisePropertyChanged(); }
        }

        private string userNo;

        public string UserNo
        {
            get { return userNo; }
            set { userNo = value; }
        }





        private string segment;

        public string Segment
        {
            get { return segment; }
            set { segment = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<string> segmentlist;
        private readonly IDialogHostService dialogHostService;

        public ObservableCollection<string> Segmentlist
        {
            get { return segmentlist; }
            set { segmentlist = value; RaisePropertyChanged(); }
        }

        public IEventAggregator aggregator { get; }
    }
}

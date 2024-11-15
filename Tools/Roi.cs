using ImTools;
using Newtonsoft.Json;
using Prism.Events;
using Rehm.Shared.Dtos;
using Rehm2024.Event;
using Rehm2024.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rehm2024.Tools
{
    public class Roi
    {
        private System.Threading.Timer timer;

        public Roi(IEventAggregator aggregator)
        {

            this.ClientLink = new SocketClientAsync(RoiIp, RoiPort, 1);
      
            GetTimer = new System.Threading.Timer(GetTimer_Tick, null, 0, 300000);


            Task T1 = new Task(new Action(LinkSocketSerFunc));
            T1.Start();

            Action action2 = () =>
            {
                
            };
            action2.BeginInvoke(null, null);
            this.aggregator = aggregator;
        }

       

        /// <summary>
        /// 重连服务端线程
        /// </summary>
        private void LinkSocketSerFunc()
        {
            object lockobj = new object();
            ClientLink = new SocketClientAsync(RoiIp, RoiPort, 0);
            bool NotFirstIn = false;
            while (ThreadContinue)
            {
                try
                {
                    if (!ClientLinkRes)
                    {
                        if (NotFirstIn)
                        {
                            ClientLink.CloseLinkServer();
                            ClientLink = new SocketClientAsync(RoiIp, RoiPort, 0);
                        }
                        NotFirstIn = true;
                        ClientLink.OnMsgReceived += new SocketClientAsync.ReceiveMsgHandler(Client_OnMsgReceived);//绑定接受到服务端消息的事件
                        ClientLinkRes = ClientLink.ConnectServer();
                        
                        if (ClientLinkRes) 
                        {
                            aggregator.GetEvent<MessagesEvent>().Publish("Roi连接成功");
                            //InfoShow(ClientLink.clientSocket.RemoteEndPoint.ToString() + "连接成功" + "\r\n", true);
                        }
                        else
                        {
                            aggregator.GetEvent<MessagesEvent>().Publish("Roi连接失败");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    aggregator.GetEvent<MessagesEvent>().Publish(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        private async void Client_OnMsgReceived(byte[] info, int i)
        {
            if (info.Length > 1)
            {
                try
                {
                    
                    string xmlString = Encoding.ASCII.GetString(info).Trim() ;
                    ToolHelper.WriteLog(xmlString);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlNode rehmNode = doc.SelectSingleNode("/REHM");
                    string RoiId = rehmNode.Attributes["n_id"].Value;

                    aggregator.GetEvent<MessagesEvent>().Publish(Encoding.ASCII.GetString(info).Trim());


                    //STATE
                    if (doc.SelectSingleNode("//STATE") != null)
                    {
                        XmlSerializer serializer1 = new XmlSerializer(typeof(StatusDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var status = (StatusDto)serializer1.Deserialize(reader);
                            string statusStr = JsonConvert.SerializeObject(status);
                            aggregator.GetEvent<MessagesEvent>().Publish(statusStr);
                            
                        }
                    }
                    //EVENT
                    if(doc.SelectSingleNode("//EVENT") != null)
                    {
                        XmlSerializer serializer7 = new XmlSerializer(typeof(AlarmDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var alarmEvent = (AlarmDto)serializer7.Deserialize(reader);
                            string eventStr = JsonConvert.SerializeObject(alarmEvent);
                            aggregator.GetEvent<MessagesEvent>().Publish(eventStr);
                        }
                    }
                    //PART_IN
                    if (doc.SelectSingleNode("//PART_IN") != null)
                    {
                        XmlSerializer serializer8 = new XmlSerializer(typeof(PartInDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var partIn = (PartInDto)serializer8.Deserialize(reader);
                            string partInStr = JsonConvert.SerializeObject(partIn);
                            aggregator.GetEvent<MessagesEvent>().Publish(partInStr);
                        }
                       
                    }
                    //PART_OUT
                    if (doc.SelectSingleNode("//PART_OUT") != null) 
                    {
                        XmlSerializer serializer8 = new XmlSerializer(typeof(PartInDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var partIn = (PartInDto)serializer8.Deserialize(reader);
                            string partInStr = JsonConvert.SerializeObject(partIn);
                            aggregator.GetEvent<MessagesEvent>().Publish(partInStr);
                            ToolHelper.WriteLog(partInStr);
                        }
                       
                        
                    }
                   

                    //PROCESS_INTERLOCK_REQ
                    if(doc.SelectSingleNode("//PROCESS_INTERLOCK_REQ") != null)
                    {
                        XmlSerializer serializer16 = new XmlSerializer(typeof(InterlockDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var InterlockDto = (InterlockDto)serializer16.Deserialize(reader);
                            string InterlockStr = JsonConvert.SerializeObject(InterlockDto);
                            aggregator.GetEvent<MessagesEvent>().Publish(InterlockStr);

                            Thread.Sleep(1000);

                            InterlockRespDto interlockRespDto = new InterlockRespDto();

                            string respxml = ToolHelper.ObjectToXml(new InterlockRespDto
                            {
                                EntryCheckResp = new PROCESS_INTERLOCK_RESP()
                                {
                                    barcode = InterlockDto.PartIn.Barcode,
                                    product = InterlockDto.PartIn.program,
                                    program = InterlockDto.PartIn.program,
                                    productRevision = "",
                                    programRevision = "",
                                    error="",
                                    lot="",
                                    result = "true"
                                }

                            }
                             ) ;

                            string compactXml = XDocument.Parse(respxml).ToString(SaveOptions.DisableFormatting);
                            if (ClientLinkRes)
                            {
                               await ClientLink.SendAsync(compactXml);


                            }
                        }
                    }

                    //TRACE
                    if (doc.SelectSingleNode("//TRACE") != null)
                    {
                        XmlSerializer serializer20 = new XmlSerializer(typeof(TraceDto));
                        using (StringReader reader = new StringReader(xmlString))
                        {
                            var TraceData = (TraceDto)serializer20.Deserialize(reader);
                            string TraceStr = JsonConvert.SerializeObject(TraceData);
                            aggregator.GetEvent<MessagesEvent>().Publish(TraceStr);
                        }
                        
                    }



                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message+"-----"+ex.ToString());
                }

            }
            else
            {
                ClientLinkRes = false;
            }
        }

        private void GetTimer_Tick(object state)
        {
            if (ClientLinkRes)
            {
                SendMessage();
                //byte[] mgs = Encoding.ASCII.GetBytes("<REHM><STATE_REQ/></REHM>");
                //ClientLink.Send(mgs);
            }
        }
        


        private async void SendMessage()
        {
           await ClientLink.SendAsync("<REHM><STATE_REQ/></REHM>");

        }




        private string RoiIp="192.168.45.116";

        private SocketClientAsync ClientLink;//客户端连接对象

        private int RoiPort = 55555;//服务端监听的端口号
        private bool ClientLinkRes = false;//服务器通讯状态标志
        private bool ThreadContinue = true;//线程轮询标志
        private System.Threading.Timer GetTimer;

        private int ii = 0;
        private readonly IEventAggregator aggregator;
        /// <summary>
        /// 终止服务
        /// </summary>
        public void StopServer()
        {
            if (ClientLinkRes)
            {
                ClientLink.CloseLinkServer();
                ClientLink.Dispose();
            }
        }


        public void Dispose()
        {
            StopServer();
            ClientLinkRes = false;
            ThreadContinue = false;
            //Client_Td.Abort();
            GetTimer.Dispose();
        }
    }
    
    
}

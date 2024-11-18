using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rehm2024.Tools
{
    public class SocketClientAsync
    {
        #region 声明变量
        public string IPAdress;
        public bool connected = false;
        public Socket clientSocket;
        private IPEndPoint hostEndPoint;
        private int Flag = 0;
        public AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
        private SocketAsyncEventArgs lisnterSocketAsyncEventArgs;

        public delegate void StartListeHandler();
        public event StartListeHandler StartListen;

        public delegate void ReceiveMsgHandler(byte[] info, int i);
        public event ReceiveMsgHandler OnMsgReceived;

        private List<SocketAsyncEventArgs> s_lst = new List<SocketAsyncEventArgs>();
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="i"></param>
        public SocketClientAsync(string hostName, int port, int i)
        {
            Flag = i;
            IPAdress = hostName;
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
            this.hostEndPoint = new IPEndPoint(hostAddresses[hostAddresses.Length - 1], port);
            this.clientSocket = new Socket(hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        }
        #endregion
        #region 开始连接服务端
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        private bool Connect()
        {
            using (SocketAsyncEventArgs args = new SocketAsyncEventArgs())
            {
                args.UserToken = this.clientSocket;
                args.RemoteEndPoint = this.hostEndPoint;
                args.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnConnect);
                this.clientSocket.ConnectAsync(args);
                bool flag = autoConnectEvent.WaitOne(50000);//阻止当前线程 等待OnConnect执行完成 
                if (connected)//判断是否连接成功 成功则创建连接对象和接受数据事件
                {
                    this.lisnterSocketAsyncEventArgs = new SocketAsyncEventArgs();
                    byte[] buffer = new byte[1024*10];
                    this.lisnterSocketAsyncEventArgs.UserToken = this.clientSocket;
                    this.lisnterSocketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
                    this.lisnterSocketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnReceive);
                    this.StartListen();//触发事件
                    return true;
                }
                return false;
            }
        }

        internal int Receive(byte[] buffer)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 判断有没有连接上服务端
        /// <summary>
        /// 判断有没有连接上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            this.connected = (e.SocketError == SocketError.Success);
            autoConnectEvent.Set();
        }
        #endregion

        Encoding Encoding = Encoding.Default;

        #region 发送数据到服务端
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="mes"></param>
        public void Send(byte[] mes)
        {
            if (this.connected)
            {
                EventHandler<SocketAsyncEventArgs> handler = null;
                //byte[] buffer = Encoding.Default.GetBytes(mes);
                byte[] buffer = new byte[1024];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0x00;
                }
                Array.Copy(mes, 0, buffer, 0, mes.Length);
                SocketAsyncEventArgs senderSocketAsyncEventArgs = null;
                lock (s_lst)
                {
                    if (s_lst.Count > 0)
                    {
                        senderSocketAsyncEventArgs = s_lst[s_lst.Count - 1];
                        s_lst.RemoveAt(s_lst.Count - 1);

                    }
                }
                if (senderSocketAsyncEventArgs == null)
                {
                    senderSocketAsyncEventArgs = new SocketAsyncEventArgs();
                    senderSocketAsyncEventArgs.UserToken = this.clientSocket;
                    senderSocketAsyncEventArgs.RemoteEndPoint = this.clientSocket.RemoteEndPoint;
                    if (handler == null)
                    {
                        handler = delegate (object sender, SocketAsyncEventArgs _e)
                        {
                            lock (s_lst)
                            {
                                s_lst.Add(senderSocketAsyncEventArgs);
                            }
                        };
                    }
                    senderSocketAsyncEventArgs.Completed += handler;
                }
                senderSocketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
                this.clientSocket.SendAsync(senderSocketAsyncEventArgs);
                Thread.Sleep(100);
            }
            else
            {
                this.connected = false;
            }
        }
        #endregion

        public async Task SendAsync(string message)
        {

            if(message == null) return; message = message.Trim();
            // 将消息转换为字节数组
            byte[] data = Encoding.ASCII.GetBytes(message);

            // 异步发送数据
            await SendDataAsync(clientSocket, data);


        }

        #region 断开服务端的连接
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        private int Disconnect()
        {
            int res = 0;
            try
            {
                this.clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }
            try
            {
                this.clientSocket.Close();
            }
            catch (Exception)
            {
            }
            this.connected = false;
            return res;
        }
        #endregion
        #region 数据接收
        /// <summary>
        /// 数据接受
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0)
            {
                if (clientSocket.Connected)
                {
                    try
                    {
                        this.clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (this.clientSocket.Connected)
                        {
                            this.clientSocket.Close();
                        }
                    }
                }
                byte[] info = new Byte[] { 0 };
                this.OnMsgReceived(info, Flag);
            }
            else
            {
                byte[] buffer = new byte[e.BytesTransferred*3];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesTransferred);
                this.OnMsgReceived(buffer, Flag);
                Listen();
            }
        }
        #region 监听服务端
        /// <summary>
        /// 监听服务端
        /// </summary>
        public void Listen()
        {
            if (this.connected && this.clientSocket != null)
            {
                try
                {
                    (lisnterSocketAsyncEventArgs.UserToken as Socket).ReceiveAsync(lisnterSocketAsyncEventArgs);
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion
        #endregion
        #region 建立连接服务端的方法
        /// <summary>
        /// 建立连接的方法
        /// </summary>
        /// <returns></returns>
        public bool ConnectServer()
        {
            bool flag = false;
            this.StartListen += new StartListeHandler(SocketClient_StartListen);
            // this.OnMsgReceived += new ReceiveMsgHandler(SocketClient_OnMsgReceived);
            flag = this.Connect();
            if (!flag)
            {
                return flag;
            }
            return true;
        }
        #endregion
        #region 关闭与服务端之间的连接
        /// <summary>
        /// 关闭连接的方法
        /// </summary>
        /// <returns></returns>
        public int CloseLinkServer()
        {
            return this.Disconnect();
        }
        #endregion
        #region 监听方法
        /// <summary>
        /// 监听的方法
        /// </summary>
        private void SocketClient_StartListen()
        {
            this.Listen();
        }
        #endregion
        #region IDispose member
        public void Dispose()
        {
            if (this.clientSocket.Connected)
            {
                this.clientSocket.Close();
            }
        }
        #endregion
        #region 析构函数
        ~SocketClientAsync()
        {
            try
            {
                if (this.clientSocket.Connected)
                {
                    this.clientSocket.Close();
                }
            }
            catch
            {

            }
            finally
            {

            }
        }
        #endregion


        private static async Task SendDataAsync(Socket socket, byte[] data)
        {
            // 异步发送数据
            await socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        }
    }
}

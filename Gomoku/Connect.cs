using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;

namespace Gomoku
{
    class connect 
    {
        
        public  int activer, row, column;
        public  string copy;
        public  chessBoard board;
        public void connected(Quobject.SocketIoClientDotNet.Client.Socket socket, string name)
        {
     
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                MessageBox.Show("connected", "Thông báo ");

            });
            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                MessageBox.Show(data.ToString());
                
          
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                MessageBox.Show(data.ToString());
            });


            socket.On("ChatMessage", (data) =>
            {
                MessageBox.Show(data.ToString());
                if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Welcome!")
                {
                    socket.Emit("MyNameIs", name);
                    socket.Emit("ConnectToOtherPlayer");
                }
                string mes = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                if (mes.Contains("You are the first player!") == true)
                {
                        activer = 2; 
                }

            });
          
            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                MessageBox.Show(data.ToString());
            });


            socket.On("NextStepIs", (data) =>
            {

                MessageBox.Show(data.ToString());
                var o = JObject.Parse(data.ToString());
                activer = (int)o["player"];
                row = (int)o["row"];
                column = (int)o["col"];
                MessageBox.Show(row + "  " + column + "   " + activer);
                if (activer == 1)
                {
                   
                        board.play(column, row, activer);
                        activer = 2;
                   
                    
                }

                if (activer == 2)
                {
                    
                        activer = 2;
                    
                    
                }
            });
            
            
        }
        public  void guitoado(Quobject.SocketIoClientDotNet.Client.Socket socket, int row, int col)
        {

            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                MessageBox.Show(data.ToString());
            });

            socket.Emit("MyStepIs", JObject.FromObject(new { row = row, col = col }));
        }
        public  void changname(Quobject.SocketIoClientDotNet.Client.Socket socket, string name)
        {
                  socket.Emit("MyNameIs", name);
                  socket.Emit("message:", copy + "is now called" + name);
                  copy = name;
        }
        public  void sendmessage (Quobject.SocketIoClientDotNet.Client.Socket socket, string name, string txt)
        {
            
                socket.Emit("ChatMessage", txt);
                socket.Emit("message:"+txt, "from:" +name);
           
        }
    }
}

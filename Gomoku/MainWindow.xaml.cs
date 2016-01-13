using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using _Gomoku;
using System.Configuration;
using System.ComponentModel;

namespace Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void sleep(object sender, DoWorkEventArgs e) 
        {
            Thread.Sleep(500);
            
        } 
        public MainWindow()
        {
            InitializeComponent();


            worker.DoWork += sleep;
            worker.RunWorkerCompleted += AIplay;
            chess_Board.view = chessGrid;
                    
                #region ketnoi

                socket = IO.Socket(System.Configuration.ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
                socket.On(Socket.EVENT_CONNECT, () =>
                {


                });
                socket.On(Socket.EVENT_MESSAGE, (data) =>
                {

                });
                socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
                {

                });
                socket.On("ChatMessage", (data) =>
                {

                    if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Welcome!")
                    {
                        socket.Emit("MyNameIs", name);
                        socket.Emit("ConnectToOtherPlayer");
                    }
                    string mes = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                    if (mes.Contains("You are the first player!") == true)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (numplayer == 1)
                            {
                                activer = 1;
                                socket.Emit("MyStepIs", JObject.FromObject(new { row = 5, col = 5 }));
                                ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), "You are the first player!");
                                listView.Items.Add(chatMessage);
                            }
                            else
                            {
                                activer = 0;
                                ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), "You are the first player!");
                                listView.Items.Add(chatMessage);
                            }

                        }));
                    }

                    mes = data.ToString();
                    if (mes.Contains("from") == true)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                    {
                        string temp = "";
                        var o = JObject.Parse(data.ToString());
                        temp = (string)o["from"];
                        string mess = (string)o["message"];
                        ChatMessage chatMessage = new ChatMessage(temp, DateTime.Now.ToString("hh:mm:ss tt"), mess);
                        listView.Items.Add(chatMessage);
                    }));
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                    {
                        mes = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                        ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), mes);
                        listView.Items.Add(chatMessage);
                    }));
                    }



                });

                socket.On("EndGame", (data) =>
                {
                    this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (numplayer == 2)
                            {
                                isEnd = true;
                                mes = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                                ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), mes);
                                listView.Items.Add(chatMessage);
                            }
                            if (numplayer == 1)
                            {
                                isEnd = true;
                                mes = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                                ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), mes);
                                listView.Items.Add(chatMessage);
                            }
                        }));
                });

                socket.On(Socket.EVENT_ERROR, (data) =>
                {
                    //MessageBox.Show(data.ToString());
                });


                socket.On("NextStepIs", (data) =>
                {

                    // MessageBox.Show(data.ToString());
                    var o = JObject.Parse(data.ToString());
                    activer = (int)o["player"];
                    row = (int)o["row"];
                    column = (int)o["col"];
                    //MessageBox.Show(row + "  " + column + "   " + activer);
                    if (activer == 1)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (chess_Board.view == null)
                                chess_Board.view = chessGrid;
                            chess_Board.play(column, row, activer);
                            activer = 0;
                        }));
                    }

                    if (activer == 0)
                    {
                        if (numplayer == 2)
                        {
                            if (rec == true)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {

                                    activer = 1;
                                    rec = false;
                                }));
                            }
                            else
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    activer = 0;
                                }));
                            }
                        }
                        else
                        {
                            if (rec == true)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {

                                    activer = 1;
                                    rec = false;
                                }));
                            }
                            else
                            {
                                rec = true;
                                Random rd = new Random();
                                int _r = 0, _c = 0;
                                _r = rd.Next(0, 11);
                                _c = rd.Next(0, 11);
                                while(chess_Board.Board[_r, _c] != 0)
                                {
                                    _r = rd.Next(0, 11);
                                    _c = rd.Next(0, 11);
                                    chess_Board.play(_c, _r, 2);
                                    socket.Emit("MyStepIs", JObject.FromObject(new { row = _r, col = _c }));
                                }
                            }
                        }
                    }

                });

                #endregion

            


        }



        Socket socket;
        
        bool isStart = false;
        bool rec;
        bool isEnd = false;
        bool isOnl = true;
        static string nameGuest = "";
        static string step = "";
        static string mes = "";
        static int row;
        static int column;
        static int activer = 2;
        string name = "Guest";
        string Message = "";
        string time = "";
        int winner = 0;
        int numplayer = 2;
        static chessBoard chess_Board = new chessBoard();
        static AI ai = new AI();
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public static void AIplay(object sender, RunWorkerCompletedEventArgs e)
        {

            Thread.Sleep(2000);
            ai.isActi = true;
            ai.play(chess_Board);
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string copy = name;
            name = textName.Text;
            if (isOnl == true)
            {
                socket.Emit("MyNameIs", name);
                socket.Emit("message:", copy + "is now called" + name);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Message = messageText.Text;
            if (isOnl == true)
            {
                
                socket.Emit("ChatMessage", Message);
                socket.Emit("message:" + Message, "from:" + name);
                messageText.Text = "Type your message here...";
            }
            else
            {
                ChatMessage chatMessage = new ChatMessage("Sever", DateTime.Now.ToString("hh:mm:ss tt"), Message);
                listView.Items.Add(chatMessage);
                messageText.Text = "Type your message here...";
            }

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (isOnl == false)
            {
                #region offline
                
                # region nguoi choi
                if (numplayer == 2)
                {
                    var element = (UIElement)e.Source;

                    int c = Grid.GetColumn(element);
                    int r = Grid.GetRow(element);
                    string temp = "Column : ";
                    temp += c;
                    temp += "Row:";
                    temp += r;
                    if (chess_Board.Board[c, r] == 0 && winner == 0)
                    {
                        double width = chessGrid.Width / 12;
                        double height = chessGrid.Height / 12;

                        chess_Board.play(c, r, activer);



                        if (chess_Board.checkWin(c, r) != 0)
                        {
                            MessageBox.Show("Winner is player" + activer);
                            winner = activer;
                        }
                        if (activer == 1)
                            activer = 2;
                        else
                            activer = 1;
                    }
                    else
                    {
                        if (winner == 0)
                            MessageBox.Show("o nay da duoc danh");
                        else
                            MessageBox.Show("da co nguoi thang cuoc");
                    }
                }
                #endregion

                # region may choi voi nguoi
                else
                {

                    if (activer == 1)
                        ai.isActi = false;
                    var element = (UIElement)e.Source;

                    int c = Grid.GetColumn(element);
                    int r = Grid.GetRow(element);
                    if (ai.isWin == true)
                        MessageBox.Show("Winner is computer");
                    else if (chess_Board.Board[c, r] == 0 && winner == 0 && ai.isWin == false)
                    {
                        chess_Board.play(c, r, 1);
                        if (chess_Board.checkWin(c, r) != 0)
                        {
                            MessageBox.Show("Winner is player" + activer);
                            winner = activer;
                        }
                        else
                        {
                            
                                worker.RunWorkerAsync();
                           
                        }
                    }
                    
                    
                }
                #endregion
                #endregion

            }
            else
            {
                
                #region online

           
                    if (isEnd == true)
                    {
                        MessageBox.Show("Da end game");
                    }
                    else if (activer == 1 || activer == 2)
                        MessageBox.Show("chua den luot");
                    else if (activer == 0 && rec == false)
                    {
                        var element = (UIElement)e.Source;
                        int c = Grid.GetColumn(element);
                        int r = Grid.GetRow(element);
                        if (chess_Board.view == null)
                            chess_Board.view = chessGrid;
                        chess_Board.play(c, r, 2);
                        socket.Emit("MyStepIs", JObject.FromObject(new { row = r, col = c }));
                        activer = 1;
                        rec = true;
                    
                    }
           
          
                #endregion
                 
            }

        }

        

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            
            
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void computer_Click(object sender, RoutedEventArgs e)
        {
            numplayer = 1;
        }

        private void Human_click(object sender, RoutedEventArgs e)
        {
            numplayer = 2;
        }

       

        
        private void messageText_GotFocus(object sender, RoutedEventArgs e)
        {
            messageText.Text = "";
        }

      
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            isOnl = true;
        }

        private void offlineButton_Click(object sender, RoutedEventArgs e)
        {
            socket.Disconnect();
            isOnl = false;
        }

       

     



        }

        
    
}


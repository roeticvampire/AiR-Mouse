using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MouseMotion
{

    


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        #endregion

        Thread clientReceiveThread;
        static float[] floatArray2;
        static bool isServerOn;
        String s = "Click the Start Button.\nUI Updates will be pushed after the Beta Testing is successful";
        string ServerStartedMessage = "The Server is now Running!\nHost IP Address: ";
        int sensitivity; //obviosly to be linked to a slider that does it's thing y'know
        float minthreshold; // another slider, basically this one ignores the small jittery movements as per your preference... ranging only 0f to 1f

        public MainWindow()
        {
           
            InitializeComponent();
            textBlock1.Text = s;
            floatArray2 = new float[6];
            for (int i = 0; i < 6; i++)
                floatArray2[i] = 0;
            sensitivity = 100;
            minthreshold = 0.1f;

            
        }
        
        //This is our Start Button
        public void Button_Click(object sender, RoutedEventArgs e)
        {

            if(!isServerOn)
            {
                if (clientReceiveThread != null)
                {
                    clientReceiveThread.Abort();
                }
                clientReceiveThread = new Thread(new ThreadStart(ServerisOnBitch))
                {
                    IsBackground = true
                };
                clientReceiveThread.Start();
                isServerOn = true;
                
            }
            else
            {
                Console.WriteLine("Server already running berti");
                //Create a writing function to be invoked at this moment
            }
            
        }


        
       
        /// <summary>
        /// This function initiates a TCP Listener and is thereby run on a seperate thread to rpevent the application from freezing
        /// </summary>
        void ServerisOnBitch()
        {
            string hostName = Dns.GetHostName();
            string hostIpAdd = Dns.GetHostEntry(hostName).AddressList[Dns.GetHostEntry(hostName).AddressList.Length-1].ToString();
            Console.WriteLine("Initiating Local TCP/IP Server\n");
            Console.WriteLine("Host IP Address :" + hostIpAdd);
            TcpListener server = new TcpListener(IPAddress.Parse(hostIpAdd), 41900);
            ServerStartedMessage += hostIpAdd+"\nPort: 41900";
            TcpClient client = default(TcpClient);
            UpdateText();
            try
            {
                server.Start();
                
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            
            while (true)
            {
                //Console.Clear();
                client = server.AcceptTcpClient();
                //if (client.Connected)
                   // UpdateText("\nClient: Connected");
                byte[] recievedBuffer = new byte[24];
                NetworkStream stream = client.GetStream();
                stream.Read(recievedBuffer, 0, recievedBuffer.Length);
                floatArray2 = new float[recievedBuffer.Length / 4];
                Buffer.BlockCopy(recievedBuffer, 0, floatArray2, 0, recievedBuffer.Length);
                MouseMotion();
                

            }
        }


        /// <summary>
        /// The function that handles the actual interpretation of inoming data and the subsequent mouse movement
        /// </summary>
        private void MouseMotion()
        {
            //This one is for the arrow keys
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X + (int)(sensitivity * floatArray2[1] / 2), System.Windows.Forms.Cursor.Position.Y - (int)(sensitivity * floatArray2[0] / 2));

            //checking to counter the minimum threshold for AirMouseMotion
            if (floatArray2[2] < minthreshold && floatArray2[2] > -minthreshold)
                floatArray2[2] = 0;
            if (floatArray2[3] < minthreshold && floatArray2[3] > -minthreshold)
                floatArray2[3] = 0;

            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X + (int)(sensitivity * floatArray2[3]), System.Windows.Forms.Cursor.Position.Y - (int)(sensitivity * floatArray2[2]));

            if (floatArray2[4] > 0)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            if (floatArray2[5] > 0)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)// this is close button
        {
           
            if(clientReceiveThread!=null)
            { if (clientReceiveThread.IsAlive)
                clientReceiveThread.Abort(); }
            Close(); 
        }

        

        void UpdateText()
        {   if(isServerOn)
            this.Dispatcher.Invoke(() =>
            {
                textBlock1.Text = ServerStartedMessage+"\nSensitivity: "+sensitivity.ToString() + "\nMinimum Threshold for AirMouse: " + minthreshold.ToString("0.00"); // your code here.
});
            
        }
      /*  void UpdateText(string str)
        {   if(isServerOn)
            this.Dispatcher.Invoke(() =>
            {
                textBlock1.Text = ServerStartedMessage +str + "\nSensitivity: " + sensitivity.ToString() +"\nMinimum Threshold for AirMouse: "+ minthreshold.ToString("0.00"); // your code here.
            });
            

        }*/
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sensitivity= (int) Slider1.Value;
            UpdateText();
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            minthreshold = (float) SliderThreshold.Value;
            UpdateText();
        }
    }

    





    }


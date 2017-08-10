using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using k = kcpwarpper;
using static kcpwarpper.KCP;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
namespace Test
{
    public unsafe partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
        }
        kcpwarpper.IKCPCB* kcp1;
        Socket udpsocket;
        IPEndPoint localipep;
        IPEndPoint remoteipep;
        private unsafe void button_init_Click(object sender, EventArgs e)
        {
            var userid = uint.Parse(textBox_sid.Text);
            var arr = textBox_local.Text.Split(":"[0]);
            localipep = new IPEndPoint(IPAddress.Parse(arr[0]), int.Parse(arr[1]));
            arr = textBox_remote.Text.Split(":"[0]);
            remoteipep = new IPEndPoint(IPAddress.Parse(arr[0]), int.Parse(arr[1]));

            kcp1 = ikcp_create(userid, (void*)userid);

            kcp1->output = Marshal.GetFunctionPointerForDelegate(new k.d_output(udp_output));

            ikcp_wndsize(kcp1, 128, 128);
            ikcp_nodelay(kcp1, 1, 10, 2, 1);
            kcp1->rx_minrto = 10;
            kcp1->fastresend = 1;

            udpsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpsocket.Blocking = false;
            udpsocket.Bind(localipep);



        }

        int udp_output(byte* buf, int len, k.IKCPCB* kcp, void* user)
        {
            byte[] buff = new byte[len];
            Marshal.Copy(new IntPtr(buf), buff, 0, len);
            udpsocket.SendTo(buff, 0, len, SocketFlags.None, remoteipep);
            return 0;
        }

        EndPoint ipep = new IPEndPoint(0, 0);
        byte[] b = new byte[1400];
        byte[] kb = new byte[1400];

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (udpsocket == null || kcp1 == null)
            {
                return;
            }
            ikcp_update(kcp1, (uint)Environment.TickCount);
            if (udpsocket.Available == 0)
            {
                return;
            }
            int cnt = udpsocket.ReceiveFrom(b, ref ipep);
            if (cnt > 0)
            {
                fixed (byte* p = &b[0])
                {
                    ikcp_input(kcp1, p, cnt);
                }
            }
            else
            {
                Console.WriteLine("cnt:" + cnt);
            }
            fixed (byte* p = &kb[0])
            {
                var kcnt = ikcp_recv(kcp1, p, kb.Length);
                if (kcnt > 0)
                {
                    Console.WriteLine("rec:" + Encoding.UTF8.GetString(kb, 0, kcnt));
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var buff = Encoding.UTF8.GetBytes(textBox_msg.Text);
            fixed (byte* p = &buff[0])
            {
                var ret = ikcp_send(kcp1, p, buff.Length);
                Console.WriteLine("send:" + ret);
            }
        }
    }
}

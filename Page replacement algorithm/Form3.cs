using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Page_replacement_algorithm
{
    public partial class resultForm : Form
    {
        public resultForm()
        {
            InitializeComponent();
        }

        List<string> opt1 = new List<string>();
        List<List<string>> opt2 = new List<List<string>>();
        List<string> fifo1 = new List<string>();
        List<List<string>> fifo2 = new List<List<string>>();
        List<string> lru1 = new List<string>();
        List<List<string>> lru2 = new List<List<string>>();



        Graphics g;
        Pen pen;
        Font font = new Font("宋体", 10, FontStyle.Bold);
        Brush brush = new SolidBrush(Color.Black);
        Brush brushred = new SolidBrush(Color.Red);

        private void resultForm_Load(object sender, EventArgs e)
        {
            opt1 = principleForm.informationBarOpt;
            opt2 = principleForm.informationFormOpt;
            fifo1 = principleForm.informationBarFifo;
            fifo2 = principleForm.informationFormFifo;
            lru1 = principleForm.informationBarLru;
            lru2 = principleForm.informationFormLru;
            g = CreateGraphics();
            pen = new Pen(Color.Black);
            g.Clear(this.BackColor);

            //opt表格
            for(int i=0;i<opt1.Count;i++)
            {
                listBoxOpt.Items.Add(opt1[i]);
            }
            //listBoxOpt.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss"));
            listBoxOpt.TopIndex = listBoxOpt.Items.Count - 1;
            
            if (opt2.Count != 0 && opt2[0].Count != 0)
            {
                int opti = 0, optj = 0;
                for (int y = 40; y < 40 + 19 * opt2[0].Count; y += 19)
                {

                    g.DrawRectangle(pen, 130, y, 100, 19);
                    g.DrawString(opt2[opti][optj], font, brush, 135, y + 2);
                    optj++;
                }
                opti = 1;
                for (int x = 230; x < 230 + 80 * (opt2.Count - 1); x += 80)
                {
                    optj = 0;
                    for (int y = 40; y < 40 + 19 * opt2[opti].Count; y += 19)
                    {

                        g.DrawRectangle(pen, x, y, 80, 19);
                        g.DrawString(opt2[opti][optj], font, brush, x + 5, y + 2);
                        optj++;
                    }
                    opti++;
                }
            }






            //fifo表格
            for (int i = 0; i < fifo1.Count; i++)
            {
                listBoxFifo.Items.Add(fifo1[i]);
            }
            //listBoxOpt.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss"));
            listBoxFifo.TopIndex = listBoxFifo.Items.Count - 1;

            if(fifo2.Count!=0&&fifo2[0].Count!=0)
            {
                int fifoi = 0, fifoj = 0;
                for (int y = 260; y < 260 + 19 * fifo2[0].Count; y += 19)
                {

                    g.DrawRectangle(pen, 130, y, 100, 19);
                    g.DrawString(fifo2[fifoi][fifoj], font, brush, 135, y + 2);
                    fifoj++;
                }
                fifoi = 1;
                for (int x = 230; x < 230 + 80 * (fifo2.Count - 1); x += 80)
                {
                    fifoj = 0;
                    for (int y = 260; y < 260 + 19 * fifo2[fifoi].Count; y += 19)
                    {

                        g.DrawRectangle(pen, x, y, 80, 19);
                        g.DrawString(fifo2[fifoi][fifoj], font, brush, x + 5, y + 2);
                        fifoj++;
                    }
                    fifoi++;
                }
            }
            



            //lru表格
            for (int i = 0; i < lru1.Count; i++)
            {
                listBoxLru.Items.Add(lru1[i]);
            }
            //listBoxOpt.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss"));
            listBoxLru.TopIndex = listBoxLru.Items.Count - 1;

            if(lru2.Count!=0&&lru2[0].Count!=0)
            {
                int lrui = 0, lruj = 0;
                for (int y = 475; y < 475 + 19 * lru2[0].Count; y += 19)
                {

                    g.DrawRectangle(pen, 130, y, 100, 19);
                    g.DrawString(lru2[lrui][lruj], font, brush, 135, y + 2);
                    lruj++;
                }
                lrui = 1;
                for (int x = 230; x < 230 + 80 * (lru2.Count - 1); x += 80)
                {
                    lruj = 0;
                    for (int y = 475; y < 475 + 19 * lru2[lrui].Count; y += 19)
                    {

                        g.DrawRectangle(pen, x, y, 80, 20);
                        g.DrawString(lru2[lrui][lruj], font, brush, x + 5, y + 2);
                        lruj++;
                    }
                    lrui++;
                }
            }
            

        }

        //private void buttonOPT_Click(object sender, EventArgs e)
        //{
        //    g = CreateGraphics();
        //    pen = new Pen(Color.Black);
        //    g.Clear(this.BackColor);
        //    int i = 0, j = 0;
        //    for(int x= 130;x<130+80*aaa.Count;x+=80)
        //    {
        //        for(int y=40;y<40+19*aaa[i].Count;y+=19)
        //        {
        //            g.DrawRectangle(pen, x, y, 80, 19);
        //            g.DrawString(aaa[9][9], font, brush, x+5, y+2);
        //            j++;
        //        }
        //        i++;
        //    }

            
        //    listBoxOpt.Items.Add(DateTime.Now.ToString("HH:mm:ss"));
        //    //listBoxOpt.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss"));
        //    listBoxOpt.TopIndex = listBoxOpt.Items.Count - 1;
        //}

        //private void buttonFIFO_Click(object sender, EventArgs e)
        //{
        //    g = CreateGraphics();
        //    pen = new Pen(Color.Black);
        //    g.Clear(this.BackColor);
        //    int i = 0, j = 0;
        //    for (int x = 130; x < 130 + 80 * aaa.Count; x += 80)
        //    {
        //        for (int y = 260; y <260 + 19 * aaa[i].Count; y += 19)
        //        {
        //            g.DrawRectangle(pen, x, y, 80, 19);
        //            g.DrawString(aaa[9][9], font, brush, x + 5, y + 2);
        //            j++;
        //        }
        //        i++;
        //    }
        //}

        //private void buttonLRU_Click(object sender, EventArgs e)
        //{
        //    g = CreateGraphics();
        //    pen = new Pen(Color.Black);
        //    g.Clear(this.BackColor);
        //    int i = 0, j = 0;
        //    for (int x = 130; x < 130 + 80 * aaa.Count; x += 80)
        //    {
        //        for (int y = 475; y < 475 + 19 * aaa[i].Count; y += 19)
        //        {
        //            g.DrawRectangle(pen, x, y, 80, 20);
        //            g.DrawString(aaa[9][9], font, brush, x + 5, y + 2);
        //            j++;
        //        }
        //        i++;
        //    }
        //}
    }
}

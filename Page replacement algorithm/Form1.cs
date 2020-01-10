using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Page_replacement_algorithm
{
    public partial class principleForm : Form
    {
        public principleForm()
        {
            InitializeComponent();
        }

        //全局变量及页面共享数据
        int time = 1;//播放的图片选择
        PictureBox pictureBoxMove = new PictureBox();
        //bool complete = false;//用于图片移动时先水平移动再垂直移动
        Random random = new Random();//随机声明
        int algorithmType = 1;//窗体间传递数据时用于判断是哪种算法，1为OPT，2为FIFO，3为LRU
        //List<string> sequenceListUnassigned = new List<string>();//全局未赋值的序列二维数组,其中里面一层为4个，分别是一个地址的4个字符，最外一层是定义的访问的页面数
        int situation = 0;//用于动画，记录页面情况，1代表有快表且页面存在于快表，2代表有快表且页面不存在于快表，3代表没有快表且页面存在于页表，4代表无快表且页面不存在于页表
        int signalTime = 0;//用于记录每次页面的单独时间
        int timersituation = 0;//用于针对中断时，某个timer执行完后执行哪个timer，保证timer顺序单序执行，1代表sitution=2时，timer4*到timer2*到timer1；2代表sitution=4时，timer4*运行后运行timer1
        //int pictureNumberPrevious = 0;//上一个页面的移动图片是第几张
        //线程的定义
        Thread threadOpt = null;
        Thread threadFifo = null;
        Thread threadLru = null;

        
        //opt算法
        public static List<string> informationBarOpt = new List<string>();//opt算法的运行结果的listbox中的数据
        public static List<List<string>> informationFormOpt = new List<List<string>>();//opt算法的绘制表格信息
        List<string> sequenceListOpt = new List<string>();//Opt算法序列号数组
        string sequenceOpt ="";//opt算法的访问序列字符串
        int sequenceNumberOpt = 13;//OPT算法的序列的页数
        int residentSetNumberOpt = 3;//opt算法的驻留集数目
        bool quickWatchOpt = true;//opt算法有没有快表
        int fastWatchTimeOpt = 2;//opt算法快表的时间
        int memoryTimeOpt = 40;//opt算法访问内存的时间
        int interruptTimeOpt = 1500;//opt算法的中断更新时间
        int totalTimeOpt = 0;//opt算法的总时间
        List<int> pageFrameNumberOpt = new List<int>();//opt算法的页框号数组
        int missPageOpt = 0;//opt算法的缺页数
        double pageRateOpt = 0.0;//opt算法的缺页率
        


        //FIFO算法
        public static List<string> informationBarFifo = new List<string>();//FIFO算法的运行结果的listbox中的数据
        public static List<List<string>> informationFormFifo = new List<List<string>>();//FIFO算法的绘制表格信息
        List<string> sequenceListFifo = new List<string>();//FIFO算法序列号数组
        string sequenceFifo = "";//FIFO算法的访问序列字符串
        int sequenceNumberFifo = 13;//FIFO算法的序列的页数
        int residentSetNumberFifo = 3;//FIFO算法的驻留集数目
        bool quickWatchFifo = true;//FIFO算法有没有快表
        int fastWatchTimeFifo = 2;//FIFO算法快表的时间
        int memoryTimeFifo = 40;//FIFO算法访问内存的时间
        int interruptTimeFifo = 1500;//FIFO算法的中断更新时间
        int totalTimeFifo = 0;//FIFO算法的总时间
        List<int> pageFrameNumberFifo = new List<int>();//FIFO算法的页框号数组
        int missPageFifo = 0;//FIFO算法的缺页数
        double pageRateFifo = 0.0;//FIFO算法的缺页率


        //LRU算法
        public static List<string> informationBarLru = new List<string>();//LRU算法的运行结果的listbox中的数据
        public static List<List<string>> informationFormLru = new List<List<string>>();//LRU算法的绘制表格信息
        List<string> sequenceListLru = new List<string>();//LRU算法序列号数组
        string sequenceLru = "";//LRU算法的访问序列字符串
        int sequenceNumberLru = 13;//LRU算法的序列的页数
        int residentSetNumberLru = 3;//LRU算法的驻留集数目
        bool quickWatchLru = true;//LRU算法有没有快表
        int fastWatchTimeLru = 2;//LRU算法快表的时间
        int memoryTimeLru = 40;//LRU算法访问内存的时间
        int interruptTimeLru = 1500;//LRU算法的中断更新时间
        int totalTimeLru = 0;//LRU算法的总时间
        List<int> pageFrameNumberLru = new List<int>();//LRU算法的页框号数组
        int missPageLru = 0;//LRU算法的缺页数
        double pageRateLru = 0.0;//LRU算法的缺页率



        //主窗体加载
        private void principleForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("欢迎进入卡牌游戏，请点击游戏介绍查看游戏规则呦，么=么=哒=(~￣▽￣)~", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign);
            oPTToolStripMenuItem.Checked = true;
            haveQuickWatchToolStripMenuItem.Checked = true;
            timerPicturePlay.Interval =100; //设置间隔时间
            timerPicturePlay.Enabled = true; //让timer控件开始工作

            textBoxPageSequence.ForeColor = Color.DarkGray;
            this.textBoxPageSequence.Text = "序号大小在1000H-DFFFH范围，数目在1-13范围";
            textBoxNumOfResidentSet.ForeColor = Color.DarkGray;
            this.textBoxNumOfResidentSet.Text = "1-5";
            textBoxNumOfSerialAddress.ForeColor = Color.DarkGray;
            this.textBoxNumOfSerialAddress.Text = "1-13";
            textBoxNumOfMinAddress.ForeColor = Color.DarkGray;
            this.textBoxNumOfMinAddress.Text = "10000H";
            textBoxNumOfMaxAddress.ForeColor = Color.DarkGray;
            this.textBoxNumOfMaxAddress.Text = "DFFFFH";
            textBoxTimeOfQuickWatch.ForeColor = Color.DarkGray;
            this.textBoxTimeOfQuickWatch.Text = "1-3";
            textBoxTimeOfMemory.ForeColor = Color.DarkGray;
            this.textBoxTimeOfMemory.Text = "30-50";
            textBoxTimeOfInterrupt.ForeColor = Color.DarkGray;
            this.textBoxTimeOfInterrupt.Text = "1000-2000";

        }


        //关于按钮
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("欢迎使用此游戏，此游戏为猿族崛起团队开发","关于我们",MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign);
        }

        //OPT算法按钮
        private void oPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalselect(sender);
        }

        //FIFO算法按钮
        private void fIFOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalselect(sender);
        }

        //LRU算法按钮
        private void lRUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalselect(sender);
        }

        //菜单栏中算法的选择，只能单选，每个按钮调用的函数
        private void signalselect(object sender)
        {
            oPTToolStripMenuItem.Checked = false;
            fIFOToolStripMenuItem.Checked = false;
            lRUToolStripMenuItem.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
        }

        

        //游戏介绍按钮
        private void introduceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("游戏规则:需要先输入或生成取牌序列，手中（可设置有无）、书包中、卡库中均有扑克牌，且有取牌时间以及顺序，对于每张要取的牌，需要先查找手上的牌，有则出牌，无则从书包找牌；书包中有则出牌，无则从卡库中找牌；将卡库找到的牌根据游戏模式更新书包及手中牌面，再进行从手中找牌并出牌。每取一次牌，需要您选择取牌方式及输入所用时间，直至序列取牌完成。祝您成功！");
            
            introduceForm child = new introduceForm();
            child.ShowDialog();
        }

        //手中有牌（有快表）按钮
        private void haveQuickWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalQuickWatch(sender);
        }

        //手中无牌（无快表）按钮
        private void noHaveQuickWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalQuickWatch(sender);
        }

        //菜单栏中是否有快表的设置，单选
        private void signalQuickWatch(object sender)
        {
            haveQuickWatchToolStripMenuItem.Checked = false;
            noHaveQuickWatchToolStripMenuItem.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
        }


        
        //访问序列输入框的提示信息
        private void textBoxPageSequence_MouseLeave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxPageSequence.Text))
            {
                //textBoxPageSequence.ForeColor = Color.DarkGray;
                this.textBoxPageSequence.Text = "序号大小在1000H-DFFFH范围，数目在1-13范围";
            }
        }
        private void textBoxPageSequence_MouseUp(object sender, MouseEventArgs e)
        {
            if(textBoxPageSequence.Text== "序号大小在1000H-DFFFH范围，数目在1-13范围")
            {
                //textBoxPageSequence.ForeColor = Color.Black;
                this.textBoxPageSequence.Text = "";
            }
        }

        //书包卡牌数目（驻留集数目）提示信息
        private void textBoxNumOfResidentSet_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxNumOfResidentSet.Text))
            {
                //textBoxNumOfResidentSet.ForeColor = Color.DarkGray;
                this.textBoxNumOfResidentSet.Text = "1-5";
            }
        }
        private void textBoxNumOfResidentSet_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxNumOfResidentSet.Text == "1-5")
            {
                //textBoxNumOfResidentSet.ForeColor = Color.Black;
                this.textBoxNumOfResidentSet.Text = "";
            }
        }

        //访问序列数目提示消息
        private void textBoxNumOfSerialAddress_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxNumOfSerialAddress.Text))
            {
                //textBoxNumOfSerialAddress.ForeColor = Color.DarkGray;
                this.textBoxNumOfSerialAddress.Text = "1-13";
            }
        }
        private void textBoxNumOfSerialAddress_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxNumOfSerialAddress.Text == "1-13")
            {
                //textBoxNumOfSerialAddress.ForeColor = Color.Black;
                this.textBoxNumOfSerialAddress.Text = "";
            }
        }

        //地址范围最小提示消息
        private void textBoxNumOfMinAddress_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxNumOfMinAddress.Text))
            {
                //textBoxNumOfMinAddress.ForeColor = Color.DarkGray;
                this.textBoxNumOfMinAddress.Text = "10000H";
            }
        }
        private void textBoxNumOfMinAddress_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxNumOfMinAddress.Text == "10000H")
            {
                //textBoxNumOfMinAddress.ForeColor = Color.Black;
                this.textBoxNumOfMinAddress.Text = "";
            }
        }

        //地址范围最大提示消息
        private void textBoxNumOfMaxAddress_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxNumOfMaxAddress.Text))
            {
                //textBoxNumOfMaxAddress.ForeColor = Color.DarkGray;
                this.textBoxNumOfMaxAddress.Text = "DFFFFH";
            }
        }
        private void textBoxNumOfMaxAddress_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxNumOfMaxAddress.Text == "DFFFFH")
            {
                //textBoxNumOfMaxAddress.ForeColor = Color.Black;
                this.textBoxNumOfMaxAddress.Text = "";
            }
        }

        //快表访问时间提示消息
        private void textBoxTimeOfQuickWatch_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTimeOfQuickWatch.Text))
            {
                //textBoxTimeOfQuickWatch.ForeColor = Color.DarkGray;
                this.textBoxTimeOfQuickWatch.Text = "1-3";
            }
        }
        private void textBoxTimeOfQuickWatch_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxTimeOfQuickWatch.Text == "1-3")
            {
                //textBoxTimeOfQuickWatch.ForeColor = Color.Black;
                this.textBoxTimeOfQuickWatch.Text = "";
            }
        }

        //访问页表或内存时间提示消息
        private void textBoxTimeOfMemory_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTimeOfMemory.Text))
            {
                //textBoxTimeOfMemory.ForeColor = Color.DarkGray;
                this.textBoxTimeOfMemory.Text = "30-50";
            }
        }
        private void textBoxTimeOfMemory_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxTimeOfMemory.Text == "30-50")
            {
                //textBoxTimeOfMemory.ForeColor = Color.Black;
                this.textBoxTimeOfMemory.Text = "";
            }
        }

        //中断时间提示消息
        private void textBoxTimeOfInterrupt_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTimeOfInterrupt.Text))
            {
                //textBoxTimeOfInterrupt.ForeColor = Color.DarkGray;
                this.textBoxTimeOfInterrupt.Text = "1000-2000";
            }
        }
        private void textBoxTimeOfInterrupt_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxTimeOfInterrupt.Text == "1000-2000")
            {
                //textBoxTimeOfInterrupt.ForeColor = Color.Black;
                this.textBoxTimeOfInterrupt.Text = "";
            }
        }

        //页框号提示消息
        private void textBoxPageframeNumber_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPageframeNumber.Text))
            {
                this.textBoxPageframeNumber.Text = "以逗号分隔";
            }
        }
        private void textBoxPageframeNumber_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBoxPageframeNumber.Text == "以逗号分隔")
            {
                this.textBoxPageframeNumber.Text = "";
            }
        }


        //查看运行结果按钮
        private void buttonResult_Click(object sender, EventArgs e)
        {
            resultForm child = new resultForm();
            child.ShowDialog();
        }

        
      
        //图片播放的timer控件
        private void timerPicturePlay_Tick(object sender, EventArgs e)
        {
            pictureBox31.Image = imageList1.Images[time];
            if(time==12)
            {
                time = 0;
            }
            else
            {
                time++;
            }
            
        }

        //生成picturebox并移动
        private void button1_Click(object sender, EventArgs e)
        {
            //pictureBoxMove.Location = new Point(110, 250);
            //pictureBoxMove.Height = 87;
            //pictureBoxMove.Width = 57;
            pictureBoxMove.Location = new Point(pictureBox31.Location.X+10,pictureBox31.Location.Y+15);
            pictureBoxMove.Height = pictureBox11.Height;
            pictureBoxMove.Width = pictureBox11.Width;
            pictureBoxMove.Image = imageList1.Images[12];
            pictureBoxMove.Refresh();
            pictureBoxMove.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBoxMove);

            timerPictureMove.Interval = 1; //设置间隔时间
            timerPictureMove.Enabled = true; //让timer控件开始工作
                        
        }


        //pictureBox31{X=570,Y=360}
        //pictureBoxMove产生地址{X=580,Y=375}
        //pictureBox11{X=110,Y=255}
        //pictureBox1.BringToFront();//将控件放置所有控件最前端  
        //pictureBox1.SendToBack();//将控件放置所有控件最底端 
        //pictureBox1.Visible = true;//显示
        //pictureBox1.Visible = false;//隐藏

       

        //在char数组中找到指定char值所在的索引
        private int checkPosition(char [] shuzu ,char mubiao)
        {
            int i = 0;
             foreach(char key in shuzu )
            {
                if(key==mubiao )
                {
                    return i;
                    
                }
                i++;
            }
            return -1;
        }


        //随机生成按钮，生成一个访问序列
        private void buttonRandom_Click(object sender, EventArgs e)
        {
            char [] letterPageNumber = { '1', '2', '3','4','5','6','7','8','9','A','B','C','D' };
            char[] letter = { '0','1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            string xulie = "";
            if (textBoxNumOfSerialAddress.Text!=""&& textBoxNumOfMinAddress.Text!=""&& textBoxNumOfMaxAddress.Text!="")
            {
                for(int i=0;i<int.Parse(textBoxNumOfSerialAddress.Text);i++)
                {
                    
                    char[] cMin = textBoxNumOfMinAddress.Text.ToCharArray();
                    char[] cMax = textBoxNumOfMaxAddress.Text.ToCharArray();
                    char char1 = letterPageNumber[random.Next(checkPosition(letterPageNumber, cMin[0]), checkPosition(letterPageNumber, cMax[0]))];
                    //sequenceListUnassigned[i][0] = char1;
                    if (char1 == cMin[0])
                    {
                        xulie += char1.ToString();
                        for (int j=1;j<5;j++)
                        {
                            int check = checkPosition(letter, cMin[j]);
                            char char2 = letter[random.Next(checkPosition(letter, cMin[j]), 15)];
                            xulie += char2.ToString();
                            //sequenceListUnassigned[i][j] = char2;
                        }
                        xulie += "H,";
                    }
                    else if (char1 == cMax[0])
                    {
                        xulie += char1.ToString();
                        for(int j=1;j<5;j++)
                        {
                            char char2 = letter[random.Next(0, checkPosition(letter, cMax[j]))];
                            xulie += char2.ToString();
                            //sequenceListUnassigned[i][j] = char2;
                        }
                        xulie += "H,";
                    }
                    else
                    {
                        xulie += char1.ToString();
                        for(int j=1;j<5;j++)
                        {
                            char char2 = letter[random.Next(0, 14)];
                            xulie += char2.ToString();
                            //sequenceListUnassigned[i][j] = char2;
                        }
                        xulie += "H,";
                    }
                }
                textBoxPageSequence.Text = xulie;
                //if(oPTToolStripMenuItem.Checked)
                //{
                //    sequenceListOpt = sequenceListUnassigned;
                //}
                //else if(fIFOToolStripMenuItem.Checked)
                //{
                //    sequenceListFifo = sequenceListUnassigned;
                //}
                //else
                //{
                //    sequenceListLru = sequenceListUnassigned;
                //}
                //textBox1.Text = sequenceListOpt[3][3].ToString();
            }
            else
            {
                MessageBox.Show("取牌序列数目或卡牌大小范围不能为空");
            }
            


        }


        //将访问序列中的每个地址都截取为6个字符，并存储到string数组中
        private List<string> changeAddressToString(string address, int num)
        {

            List<string> result = new List<string>();
            for (int i = 0; i < num; i++)
            {
                result.Add(address.Substring(i * 7, 6));
                
            }
            return result;
            
        }


        //初始化按钮
        private void buttonInitialization_Click(object sender, EventArgs e)
        {
            oPTToolStripMenuItem.Checked = true;
            fIFOToolStripMenuItem.Checked = false;
            lRUToolStripMenuItem.Checked = false;
            //beginToolStripMenuItem.Checked = false;
            
            //suspendToolStripMenuItem.Checked = false;
            //suspendToolStripMenuItem.Enabled = false;
            //continueToolStripMenuItem.Checked = false;
            //continueToolStripMenuItem.Enabled = false;
            haveQuickWatchToolStripMenuItem.Checked = true;
            noHaveQuickWatchToolStripMenuItem.Checked = false;
            textBoxNumOfResidentSet.Text = "3";
            textBoxNumOfSerialAddress.Text = "8";
            textBoxNumOfMinAddress.Text = "10000H";
            textBoxNumOfMaxAddress.Text = "DFFFFH";
            textBoxTimeOfQuickWatch.Text = "2";
            textBoxTimeOfMemory.Text = "40";
            textBoxTimeOfInterrupt.Text = "1500";
            textBoxPageframeNumber.Text = "1,4,9";
            buttonRandom_Click(sender,e);
        }

        //开始按钮点击操作
        private void beginToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //suspendToolStripMenuItem.Enabled = true;
            //continueToolStripMenuItem.Enabled = false;
            //sequenceListUnassigned = changeAddressToString(textBoxPageSequence.Text, int.Parse(textBoxNumOfSerialAddress.Text));



            pictureBox11.Image = null;
            pictureBox11.Refresh();
            pictureBox12.Image = null;
            pictureBox12.Refresh();
            pictureBox13.Image = null;
            pictureBox13.Refresh();
            pictureBox14.Image = null;
            pictureBox14.Refresh();
            pictureBox15.Image = null;
            pictureBox15.Refresh();
            pictureBox21.Image = null;
            pictureBox21.Refresh();
            pictureBox22.Image = null;
            pictureBox22.Refresh();
            pictureBox23.Image = null;
            pictureBox23.Refresh();
            pictureBox24.Image = null;
            pictureBox24.Refresh();
            pictureBox25.Image = null;
            pictureBox25.Refresh();



            if (oPTToolStripMenuItem.Checked)
            {
                informationBarOpt = new List<string>();
                informationFormOpt = new List<List<string>>();
                //全局变量的赋值
                sequenceListOpt = changeAddressToString(textBoxPageSequence.Text, int.Parse(textBoxNumOfSerialAddress.Text));
                sequenceOpt = textBoxPageSequence.Text;
                sequenceNumberOpt = int.Parse(textBoxNumOfSerialAddress.Text);
                residentSetNumberOpt = int.Parse(textBoxNumOfResidentSet.Text);
                if(haveQuickWatchToolStripMenuItem.Checked)
                {
                    quickWatchOpt = true;
                }
                else
                {
                    quickWatchOpt = false;
                }
                fastWatchTimeOpt=int.Parse( textBoxTimeOfQuickWatch.Text);
                memoryTimeOpt = int.Parse(textBoxTimeOfMemory.Text);
                interruptTimeOpt = int.Parse(textBoxTimeOfInterrupt.Text);
                string[] strs = textBoxPageframeNumber.Text.Split(',');
                for (int i=0;i<residentSetNumberOpt;i++)
                {
                    pageFrameNumberOpt.Add(int.Parse(strs[i]));
                }
                

                //fIFOToolStripMenuItem.Enabled = false;
                //lRUToolStripMenuItem.Enabled = false;

                //线程
                threadOpt = new Thread(new ThreadStart(OPT)) {  };
                threadOpt.Start();

                
                //fIFOToolStripMenuItem.Enabled = true;
                //lRUToolStripMenuItem.Enabled = true;

            }
            else if (fIFOToolStripMenuItem.Checked)
            {
                informationBarFifo = new List<string>();
                informationFormFifo = new List<List<string>> ();
                //全局变量的赋值
                sequenceListFifo = changeAddressToString(textBoxPageSequence.Text, int.Parse(textBoxNumOfSerialAddress.Text));
                sequenceFifo = textBoxPageSequence.Text;
                sequenceNumberFifo = int.Parse(textBoxNumOfSerialAddress.Text);
                residentSetNumberFifo = int.Parse(textBoxNumOfResidentSet.Text);
                if (haveQuickWatchToolStripMenuItem.Checked)
                {
                    quickWatchFifo = true;
                }
                else
                {
                    quickWatchFifo = false;
                }
                fastWatchTimeFifo = int.Parse(textBoxTimeOfQuickWatch.Text);
                memoryTimeFifo = int.Parse(textBoxTimeOfMemory.Text);
                interruptTimeFifo = int.Parse(textBoxTimeOfInterrupt.Text);
                string[] strs = textBoxPageframeNumber.Text.Split(',');
                for (int i = 0; i < residentSetNumberFifo; i++)
                {
                    pageFrameNumberFifo.Add(int.Parse(strs[i]));
                }

                //oPTToolStripMenuItem.Enabled = false;
                //lRUToolStripMenuItem.Enabled = false;

                threadFifo = new Thread(new ThreadStart(FIFO)) {};
                threadFifo.Start();

                //oPTToolStripMenuItem.Enabled = true;
                //lRUToolStripMenuItem.Enabled = true;
            }
            else if(lRUToolStripMenuItem.Checked)
            {
                informationBarLru = new List<string>();
                informationFormLru = new List<List<string>>();
                //全局变量的赋值
                sequenceListLru = changeAddressToString(textBoxPageSequence.Text, int.Parse(textBoxNumOfSerialAddress.Text));
                sequenceLru = textBoxPageSequence.Text;
                sequenceNumberLru = int.Parse(textBoxNumOfSerialAddress.Text);
                residentSetNumberLru = int.Parse(textBoxNumOfResidentSet.Text);
                if (haveQuickWatchToolStripMenuItem.Checked)
                {
                    quickWatchLru = true;
                }
                else
                {
                    quickWatchLru = false;
                }
                fastWatchTimeLru = int.Parse(textBoxTimeOfQuickWatch.Text);
                memoryTimeLru = int.Parse(textBoxTimeOfMemory.Text);
                interruptTimeLru = int.Parse(textBoxTimeOfInterrupt.Text);
                string[] strs = textBoxPageframeNumber.Text.Split(',');
                for (int i = 0; i < residentSetNumberLru; i++)
                {
                    pageFrameNumberLru.Add(int.Parse(strs[i]));
                }

                //oPTToolStripMenuItem.Enabled = false;
                //fIFOToolStripMenuItem.Enabled = false;

                threadLru = new Thread(new ThreadStart(LRU)) { };
                threadLru.Start();

                //oPTToolStripMenuItem.Enabled = true;
                //fIFOToolStripMenuItem.Enabled = true;
            }
            else
            {
                MessageBox.Show("未选择线程！");
            }
            
        }

        //暂停按钮点击事件
        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (oPTToolStripMenuItem.Checked)
            {
                //try
                //{
                //    threadOpt.Suspend();
                //}
                //catch { }
                if(threadOpt.ThreadState==ThreadState.Running)
                {
                    threadOpt.Suspend();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = false;
                    //continueToolStripMenuItem.Enabled = true;
                }
                


            }
            else if (fIFOToolStripMenuItem.Checked)
            {
                if (threadFifo.ThreadState == ThreadState.Running)
                {
                    threadFifo.Suspend();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = false;
                    //continueToolStripMenuItem.Enabled = true;
                }
               

            }
            else if (lRUToolStripMenuItem.Checked)
            {
                if (threadLru.ThreadState == ThreadState.Running)
                {
                    threadLru.Suspend();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = false;
                    //continueToolStripMenuItem.Enabled = true;
                }
                

            }
            else
            {
                MessageBox.Show("未选择线程！");
            }
        }

        //继续按钮点击事件
        private void continueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (oPTToolStripMenuItem.Checked)
            {
                if (threadOpt.ThreadState ==  ThreadState.Suspended)
                {
                    threadOpt.Resume();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = true;
                    //continueToolStripMenuItem.Enabled = false;
                }

            }
            else if (fIFOToolStripMenuItem.Checked)
            {
                if (threadFifo.ThreadState == ThreadState.Suspended)
                {
                    threadFifo.Resume();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = true;
                    //continueToolStripMenuItem.Enabled = false;
                }
                

            }
            else if (lRUToolStripMenuItem.Checked)
            {
                if (threadLru.ThreadState == ThreadState.Suspended)
                {
                    threadLru.Resume();
                    //beginToolStripMenuItem.Enabled = false;
                    //suspendToolStripMenuItem.Enabled = true;
                    //continueToolStripMenuItem.Enabled = false;
                }
               

            }
            else
            {
                MessageBox.Show("未选择线程！");
            }
        }

        

        //提交按钮点击事件
        private void buttonSubmission_Click(object sender, EventArgs e)
        {
            labelAnswer.Text = "";
            //bool all = false;
            if((situation==1&&radioButton1.Checked==true)||(situation==2&&radioButton2.Checked==true)|| (situation == 3 && radioButton3.Checked == true)|| (situation == 4 && radioButton4.Checked == true))
            {
                labelAnswer.Text += "好聪明，取牌方式正确哟^^\n";
                //all = true;
            }
            else
            {
                labelAnswer.Text += "好遗憾，取牌方式应该是第" + situation + "个选项。\n";
            }
            if (textBoxAnswerTime.Text.Equals(signalTime.ToString()))
            {
                labelAnswer.Text += "恭喜时间计算正确！";
                //if(all)
                //{
                //    all = true;
                //}
                
            }
            else
            {
                labelAnswer.Text += "时间计算错误欧，应该是"+signalTime+"S";
            }
            //if(all)
            //{
            //    labelAnswer.Text += "\n 太棒了！都回答正确了！";
            //}
            if (threadOpt!=null&&threadOpt.ThreadState == ThreadState.Suspended)
            {
                threadOpt.Resume();
                
            }
            else if (threadFifo!=null&&threadFifo.ThreadState == ThreadState.Suspended)
            {
                threadFifo.Resume();

            }
            else if(threadLru!=null&&threadLru.ThreadState == ThreadState.Suspended)
            {
                threadLru.Resume();

            }
            else
            {
                MessageBox.Show("当前没有线程挂起");
            }

        }

        //用于根据页号找到相应的第几个图片，返回第几个
        private int checkPicture(string num)
        {
            int num2=0;
            switch(num)
            {
                case "1":
                    num2 = 0;
                    break;
                case "2":
                    num2 = 1;
                    break;
                case "3":
                    num2 = 2;
                    break;
                case "4":
                    num2 = 3;
                    break;
                case "5":
                    num2 = 4;
                    break;
                case "6":
                    num2 = 5;
                    break;
                case "7":
                    num2 = 6;
                    break;
                case "8":
                    num2 = 7;
                    break;
                case "9":
                    num2 = 8;
                    break;
                case "A":
                    num2 = 9;
                    break;
                case "B":
                    num2 = 10;
                    break;
                case "C":
                    num2 = 11;
                    break;
                case "D":
                    num2 = 12;
                    break;
                default:
                    MessageBox.Show("无法确定更新的牌号");
                    break;
            }
            return num2;
            
        }

        //用于截取序列字符串中的每个地址，返回string数组
        private List<string> changeStringToAddress(string str,int num)
        {
            List<string> result = new List<string>();
            for(int i=0;i<num;i++)
            {
                result.Add(str.Substring(i * 7, 6));
            }
            return result;
        }


        //操作非当前线程的控件的方法，两种
        //textBoxAnswerTime.Invoke(new Action<string>((str) => { textBoxAnswerTime.Text = str; }), "1");
        //this.BeginInvoke(new Action(() =>
        //        {
        //    textBoxAnswerTime.Text = "1";
        //}));

        //图片移动的timer控件
        private void timerPictureMove_Tick(object sender, EventArgs e)
        {
            //String dizhi = pictureBox31.Location.ToString() + pictureBoxMove.Location.ToString() + pictureBox11.Location.ToString();
            //textBox1.Text = dizhi;
            //timerPictureMove.Enabled = false;
            //if(psituation11)
            //{

            //    pictureBoxMove.Visible = true;
            //    pictureBoxMove.BringToFront();
                
            //    if (pictureBoxMove.Location.Y > 250)
            //    {
            //        pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 3);
            //    }
            //    else
            //    {
            //        pictureBoxMove.Visible = false;
            //        timerPictureMove.Enabled = false;
            //    }
            //}
            
            
            
        }

        //移动的图片的picturebox的生成及定义
        private void pictureMoveCreate(int picturenumber,int stuation,int picturelocation, int replacePosition)
        {
            pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
            pictureBoxMove.Height = pictureBox11.Height;
            pictureBoxMove.Width = pictureBox11.Width;
            pictureBoxMove.Image = imageList1.Images[picturenumber];
            pictureBoxMove.Refresh();
            pictureBoxMove.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBoxMove);
           
            if (stuation==1&&picturelocation==1)
            {
                //pictureBox11.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox11.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox11.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 1 && picturelocation == 2)
            {
                //pictureBox12.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox12.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox12.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 1 && picturelocation == 3)
            {
                //pictureBox13.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox13.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox13.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 1 && picturelocation == 4)
            {
                //pictureBox14.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox14.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox14.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 1 && picturelocation == 5)
            {
                //pictureBox15.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox15.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox15.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 2 && replacePosition == 1)
            {
                pictureBox11.Image = null;
                pictureBox11.Refresh();
                pictureBox21.Image = null;
                pictureBox21.Refresh();

                timer21.Enabled = false;
                timer1.Enabled = false;
                timer41.Interval = 1; //设置间隔时间
                timer41.Enabled = true; //让timer控件开始工作
                timersituation = 1;

                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                //timer21.Interval = 1; //设置间隔时间
                //timer21.Enabled = true; //让timer控件开始工作
                //timer1.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;

                pictureBox11.Image = imageList1.Images[picturenumber];
                pictureBox11.Refresh();
                pictureBox21.Image = imageList1.Images[picturenumber];
                pictureBox21.Refresh();
                //pictureBox11.BorderStyle = BorderStyle.Fixed3D;
                //pictureBox11.BringToFront();
                labelQuickWatch.Text = "更新完牌面后，手中有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox11.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox11.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;

            }
            else if (stuation == 2 && replacePosition == 2)
            {
                pictureBox12.Image = null;
                pictureBox12.Refresh();
                pictureBox22.Image = null;
                pictureBox22.Refresh();

                timer22.Enabled = false;
                timer1.Enabled = false;
                timer42.Interval = 1; //设置间隔时间
                timer42.Enabled = true; //让timer控件开始工作
                timersituation = 1;

                timer21.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                //timer22.Interval = 1; //设置间隔时间
                //timer22.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer1.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
                pictureBox12.Image = imageList1.Images[picturenumber];
                pictureBox12.Refresh();
                pictureBox22.Image = imageList1.Images[picturenumber];
                pictureBox22.Refresh();
                labelQuickWatch.Text = "更新完牌面后，手中有牌了。卡库出牌！";
                //pictureBox22.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 1001; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox12.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox12.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 2 && replacePosition == 3)
            {
                pictureBox13.Image = null;
                pictureBox13.Refresh();
                pictureBox23.Image = null;
                pictureBox23.Refresh();

                timer1.Enabled = false;
                timer23.Enabled = false;
                timer43.Interval = 1; //设置间隔时间
                timer43.Enabled = true; //让timer控件开始工作
                timersituation = 1;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                //timer23.Interval = 1; //设置间隔时间
                //timer23.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer1.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
                pictureBox13.Image = imageList1.Images[picturenumber];
                pictureBox13.Refresh();
                pictureBox23.Image = imageList1.Images[picturenumber];
                pictureBox23.Refresh();
                labelQuickWatch.Text = "更新完牌面后，手中有牌了。卡库出牌！";
                //pictureBox13.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox13.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox13.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 2 && replacePosition == 4)
            {
                pictureBox14.Image = null;
                pictureBox14.Refresh();
                pictureBox24.Image = null;
                pictureBox24.Refresh();

                timer1.Enabled = false;
                timer24.Enabled = false;
                timer44.Interval = 1; //设置间隔时间
                timer44.Enabled = true; //让timer控件开始工作
                timersituation = 1;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer45.Enabled = false;

                //timer24.Interval = 1; //设置间隔时间
                //timer24.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer1.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
                pictureBox14.Image = imageList1.Images[picturenumber];
                pictureBox14.Refresh();
                pictureBox24.Image = imageList1.Images[picturenumber];
                pictureBox24.Refresh();
                labelQuickWatch.Text = "更新完牌面后，手中有牌了。卡库出牌！";
                //pictureBox14.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox14.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox14.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 2 && picturelocation == 5)
            {
                pictureBox15.Image = null;
                pictureBox15.Refresh();
                pictureBox25.Image = null;
                pictureBox25.Refresh();

                timer1.Enabled = false;
                timer25.Enabled = false;
                timer45.Interval = 1; //设置间隔时间
                timer45.Enabled = true; //让timer控件开始工作
                timersituation = 1;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                

                //timer25.Interval = 1; //设置间隔时间
                //timer25.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer1.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
                pictureBox15.Image = imageList1.Images[picturenumber];
                pictureBox15.Refresh();
                pictureBox25.Image = imageList1.Images[picturenumber];
                pictureBox25.Refresh();
                labelQuickWatch.Text = "更新完牌面后，手中有牌了。卡库出牌！";
                //pictureBox15.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox15.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox15.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 3 && picturelocation == 1)
            {
                // pictureBox21.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox21.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox21.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 3 && picturelocation == 2)
            {
                //pictureBox22.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox22.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox22.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 3 && picturelocation == 3)
            {
                //pictureBox23.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {
                    if (i % 50 == 0)
                    {
                        pictureBox23.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox23.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 3 && picturelocation == 4)
            {
                //pictureBox24.BorderStyle = BorderStyle.Fixed3D;

                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox24.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox24.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 3 && picturelocation == 5)
            {

                //pictureBox25.BorderStyle = BorderStyle.Fixed3D;
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox25.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox25.BorderStyle = BorderStyle.None;
                    }
                }
                timer1.Interval = 1; //设置间隔时间
                timer1.Enabled = true; //让timer控件开始工作
                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;
            }
            else if (stuation == 4 && replacePosition == 1)
            {
                pictureBox21.Image = null;
                pictureBox21.Refresh();
                timer1.Enabled = false;
                timer41.Interval = 1; //设置间隔时间
                timer41.Enabled = true; //让timer控件开始工作
                timersituation = 2;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                pictureBox21.Image = imageList1.Images[picturenumber];
                //pictureBox21.BorderStyle = BorderStyle.Fixed3D;
                pictureBox21.Refresh();
                labelPageTable.Text = "更新完牌面后，书包有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox21.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox21.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;

            }
            else if (stuation == 4 && replacePosition == 2)
            {
                pictureBox22.Image = null;
                pictureBox22.Refresh();
                timer1.Enabled = false;
                timer42.Interval = 1; //设置间隔时间
                timer42.Enabled = true; //让timer控件开始工作
                timersituation = 2;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                pictureBox22.Image = imageList1.Images[picturenumber];
                //pictureBox22.BorderStyle = BorderStyle.Fixed3D;
                pictureBox22.Refresh();
                labelPageTable.Text = "更新完牌面后，书包有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox22.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox22.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 4 && replacePosition == 3)
            {
                pictureBox23.Image = null;
                pictureBox23.Refresh();
                timer1.Enabled = false;
                timer43.Interval = 1; //设置间隔时间
                timer43.Enabled = true; //让timer控件开始工作
                timersituation = 2;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer44.Enabled = false;
                timer45.Enabled = false;

                pictureBox23.Image = imageList1.Images[picturenumber];
                //pictureBox23.BorderStyle = BorderStyle.Fixed3D;
                pictureBox23.Refresh();
                labelPageTable.Text = "更新完牌面后，书包有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox23.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox23.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 4 && replacePosition == 4)
            {
                pictureBox24.Image = null;
                pictureBox24.Refresh();
                timer1.Enabled = false;
                timer44.Interval = 1; //设置间隔时间
                timer44.Enabled = true; //让timer控件开始工作
                timersituation = 2;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer45.Enabled = false;

                pictureBox24.Image = imageList1.Images[picturenumber];
                //pictureBox24.BorderStyle = BorderStyle.Fixed3D;
                pictureBox24.Refresh();
                labelPageTable.Text = "更新完牌面后，书包有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox24.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox24.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }
            else if (stuation == 4 && replacePosition == 5)
            {
                pictureBox25.Image = null;
                pictureBox25.Refresh();
                timer1.Enabled = false;
                timer45.Interval = 1; //设置间隔时间
                timer45.Enabled = true; //让timer控件开始工作
                timersituation = 2;

                timer21.Enabled = false;
                timer22.Enabled = false;
                timer23.Enabled = false;
                timer24.Enabled = false;
                timer25.Enabled = false;
                timer41.Enabled = false;
                timer42.Enabled = false;
                timer43.Enabled = false;
                timer44.Enabled = false;
                

                pictureBox25.Image = imageList1.Images[picturenumber];
                //pictureBox25.BorderStyle = BorderStyle.Fixed3D;
                pictureBox25.Refresh();
                labelPageTable.Text = "更新完牌面后，书包有牌了。卡库出牌！";
                for (int i = 0; i <= 501; i++)
                {

                    if (i % 50 == 0)
                    {
                        pictureBox25.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        pictureBox25.BorderStyle = BorderStyle.None;
                    }
                }
                //timer1.Interval = 1; //设置间隔时间
                //timer1.Enabled = true; //让timer控件开始工作
                //timer21.Enabled = false;
                //timer22.Enabled = false;
                //timer23.Enabled = false;
                //timer24.Enabled = false;
                //timer25.Enabled = false;
                //timer41.Enabled = false;
                //timer42.Enabled = false;
                //timer43.Enabled = false;
                //timer44.Enabled = false;
                //timer45.Enabled = false;
            }

        }
        
            
        //图片移动动画
        private void pictureAnimation(int situation,int picturelocation,int picturenumber,int replacePosition)
        {
            if(situation==1)//有快表，不中断
            {
                labelQuickWatch.Text = "";
                labelPageTable.Text = "";
                string str = "在手中位置"+picturelocation+"处找到牌了！卡库出牌！";
                labelQuickWatch.Text =str;
                pictureMoveCreate(picturenumber, situation, picturelocation, replacePosition);
                //
            }
            else if(situation==2)//有快表，中断
            {
                labelQuickWatch.Text = "";
                labelPageTable.Text = "";
                labelQuickWatch.Text = "竟然手中没有此牌！找书包！";
                labelPageTable.Text = "什么！书包也没有，等我会儿啊，我去卡库中找，别走开啊>*$*<";
                pictureMoveCreate(picturenumber, situation, picturelocation, replacePosition);
                //
               // 
               
            }
            else if(situation==3)
            {
                labelQuickWatch.Text = "";
                labelPageTable.Text = "";
                string str = "书包位置" + picturelocation + "是此张牌，卡库出牌！";
                labelPageTable.Text = str;
                pictureMoveCreate(picturenumber, situation, picturelocation, replacePosition);
                
                //
            }
            else if(situation==4)
            {
                labelQuickWatch.Text = "";
                labelPageTable.Text = "";
                labelPageTable.Text = "书包中没此牌，待我去书包查找一番。";
                pictureMoveCreate(picturenumber, situation, picturelocation, replacePosition);
                //
            }
            else
            {
                MessageBox.Show("游戏模式不确定，动画无法运行");
            }
        }


        //将1-15转换到1-F
        private string changeNumber(string m)
        {
            string str="";
            switch(m)
            {
                case "10":
                    str = "A";
                    break;
                case "11":
                    str = "B";
                    break;
                case "12":
                    str = "C";
                    break;
                case "13":
                    str = "D";
                    break;
                case "14":
                    str = "E";
                    break;
                case "15":
                    str = "F";
                    break;
                default:
                    str = m;
                    break;

            }
            return str;
        }
       

        //OPT算法函数lo
        private void OPT()
        {
            

            Dictionary<int, string> map = new Dictionary<int, string>();//驻留集内容
            //int i;//用于记录访问序列的页面当前数目
            int t;//
           
            int totalTime = 0;//用于记录总时间
            int unFind = 0;//缺页数

            List<string> list1 = new List<string>();
            list1.Add("卡牌序号");
            list1.Add("扑克牌号");
            for (int y = 1; y <= residentSetNumberOpt; y++)
            {
                list1.Add("书包位置" + y + "号");
            }

            list1.Add("对应卡库位置号");
            list1.Add("卡库具体位置");
            list1.Add("取本牌时间");
            informationFormOpt.Add(list1);

            for (int i = 0; i < sequenceNumberOpt; i++)
            {
                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = true;
                    buttonSubmission.Enabled = false;
                }));

                if (threadOpt.ThreadState == ThreadState.Running)
                {
                    threadOpt.Suspend();
                }
                
                //textBoxAnswerTime.Invoke(new Action<string>((str) => { textBoxAnswerTime.Text = str; }), "1");

                //this.Invoke(new Action(() =>
                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = false;
                    buttonSubmission.Enabled = true;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    //labelAnswer.Text = "";
                  
                    labelTip.Text = "请选出序号为" + sequenceListOpt[i] + "的取牌方式并计算取牌时间，加油！";
                }));


                //radioButton1.Invoke(new Action<bool>((yes) => { radioButton1.Checked = yes; }), false);
                //radioButton2.Invoke(new Action<bool>((yes) => { radioButton2.Checked = yes; }), false);
                //radioButton3.Invoke(new Action<bool>((yes) => { radioButton3.Checked = yes; }), false);
                //radioButton4.Invoke(new Action<bool>((yes) => { radioButton4.Checked = yes; }), false);
                //labelAnswer.Invoke(new Action<string>((str) => { labelAnswer.Text = str; }), "");
                //this.BeginInvoke(new Action(() =>
                //{
                //    //清空答题区
                //    radioButton1.Checked = false;
                //    radioButton2.Checked = false;
                //    radioButton3.Checked = false;
                //    radioButton4.Checked = false;
                //    labelAnswer.Text = "";
                //    int num4 = sequenceNumberOpt;
                //    int num1 = i;
                //    int num2 = sequenceListOpt.Count;
                //    string num3 = sequenceListOpt[i];
                //    labelTip.Text = "请选出序号为" + sequenceListOpt[i] + "的取牌方式并计算取牌时间，加油！";
                //    //
                //}));


                int picturelocation = 0;//当存在找到时，确定在驻留集的什么位置
                int picturenumber = checkPicture(sequenceListOpt[i].Substring(0, 1));//确定移动的图片是哪个
                int replacePosition = 0;//当缺页中断时，替换驻留集的位置
                string pageLogical = sequenceListOpt[i].Substring(0, 1);//每个页号的逻辑页号
                string pagePhysical = "-1";//每页的物理页号
                signalTime = 0;
                //是否开启快表
                if (quickWatchOpt)
                {
                    signalTime += fastWatchTimeOpt;//每页时间
                    totalTime += fastWatchTimeOpt;//总时间加上访问快表时间

                    if (map.ContainsValue(pageLogical))//页面是否存在于快表
                    {
                        totalTime += memoryTimeOpt;
                        signalTime += memoryTimeOpt;
                        //将每次访问的页面的数据进行保存
                        pagePhysical=pageFrameNumberOpt[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();


                        List<string> list2 = new List<string>();
                        list2.Add(sequenceListOpt[i]);
                        list2.Add(pageLogical);
                        for (int y=0; y<residentSetNumberOpt;y++)
                        {
                            if(map.ContainsKey(y))
                            {
                                 list2.Add(map[y]);
                            }
                            else
                            {
                                 list2.Add(" ");
                            }

                        }
                        list2.Add(pagePhysical);
                        list2.Add(changeNumber(pagePhysical) + sequenceListOpt[i].Substring(1, 4)+"H");
                        list2.Add(signalTime.ToString()+"S");
                        informationFormOpt.Add(list2);


                        situation = 1;//有快表，且存在于快表
                        picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;

                        //将线程暂停，等待作答
                        if (threadOpt.ThreadState==ThreadState.Running)
                        {
                            threadOpt.Suspend();
                        }

                        //动画
                        this.Invoke(new EventHandler(delegate
                        {
                            pictureAnimation(situation, picturelocation, picturenumber, 0);
                        }));

                        continue;//继续循环下一个
                    }
                    else
                    {
                        totalTime += memoryTimeOpt;
                        signalTime += memoryTimeOpt;
                        situation = 2;
                    }
                    
                }
                else
                {
                    signalTime += memoryTimeOpt;
                    totalTime += memoryTimeOpt;
                }

                //当在有快表的情况下，只有快表中没有页号才运行到此处，因为此程序设定的为快表与页面保持一致，所以此处总时间加上访问页表时间，快表没有，则也页表也不存在，所以此处不再写访问页表的过程代码
                //在没有快表的情况下，总时间先加上访问页表的时间，在接下来进行判断是否在页表及中断
                
                //if (situation == 1)
                //{
                    
                //}
                //else
                //{
                //    situation = 3;
                //}


                //在页表中没有找到该页号
                if (!map.ContainsValue(pageLogical))
                {
                    //if(situation==3)
                    //{
                    //    situation = 4;
                    //}
                    unFind++;
                    totalTime += interruptTimeOpt;//加上中断时间
                    signalTime += interruptTimeOpt;

                    //如果存放的大小小于最大驻留集数，将其放入map中
                    if (map.Count() < residentSetNumberOpt)
                    {
                        replacePosition = map.Count()+1;
                        map.Add(map.Count(),pageLogical);

                    }
                    //如果驻留集已经满了，则需要移除下一次最晚的那个要访问的那个
                    else
                    {
                        int min = 0;
                        int location = 0;
                        //遍历一遍驻留集中的元素
                        for (int j = 0; j < residentSetNumberOpt; j++)
                        {
                            //找到每一个驻留集对应数组中的下一个元素的位置
                            string tempLetter = map.First(x => x.Key == j).Value;
                            for (t = i + 1; t <sequenceNumberOpt; t++)
                            {
                                string letter = sequenceListOpt[t].Substring(0,1);
                                if (letter.Equals(tempLetter))
                                {
                                    if (t - i > min)
                                    {
                                        location = j;
                                        min = t - i;
                                    }
                                    break;
                                }
                            }
                            if (t == sequenceNumberOpt)
                            {
                                location = j;
                                min = t - i;
                            }
                        }
                        replacePosition = location + 1;
                        map.Remove(location);
                        map.Add(location, pageLogical);
                    }
                    

                    //更新页表和快表，重新查询
                    if (quickWatchOpt)
                    {
                        situation = 2;
                        pagePhysical=pageFrameNumberOpt[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();
                        signalTime += fastWatchTimeOpt;//加上快表和内存取数据时间
                        signalTime += memoryTimeOpt;
                        totalTime += fastWatchTimeOpt;
                        totalTime += memoryTimeOpt;
                    }
                    else
                    {
                        situation = 4;
                        pagePhysical = pageFrameNumberOpt[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();
                        signalTime += 2 * memoryTimeOpt;//加上页表和内存取数据时间
                        totalTime += 2*memoryTimeOpt;
                    }
                }
                //在内存页中找到了该页号，直接进行下一步
                else
                {
                    situation = 3;
                    pagePhysical = pageFrameNumberOpt[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();
                    picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;
                    signalTime +=  memoryTimeOpt;//加上内存取数据时间
                    totalTime += memoryTimeOpt;
                }

                //保存本页产生的数据
                List<string> list3 = new List<string>();
                list3.Add(sequenceListOpt[i]);
                list3.Add(pageLogical);
                for(int y=0;y<residentSetNumberOpt;y++)
                {
                    if(map.ContainsKey(y))
                    {
                        list3.Add(map[y]);
                    }
                    else
                    {
                        list3.Add(" ");
                    }
                }
                list3.Add(pagePhysical);
                list3.Add(changeNumber(pagePhysical) + sequenceListOpt[i].Substring(1, 4)+"H");
                list3.Add(signalTime.ToString()+"S");
                informationFormOpt.Add(list3);



                //将线程暂停，等待作答
                if (threadOpt.ThreadState == ThreadState.Running)
                {
                    threadOpt.Suspend();
                }

                //动画
                if (situation == 2|| situation == 4)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, 0, picturenumber, replacePosition);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, 0, picturenumber, replacePosition);
                    //}));
                }
                else if (situation == 3)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, picturelocation, picturenumber, 0);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, picturelocation, picturenumber, 0);
                    //}));
                }
                else
                {
                    MessageBox.Show("opt算法函数中情况分类出错");
                }
            }
            double rate = (double)unFind / (double)sequenceNumberOpt;


            //保存序列完成的总体信息
            totalTimeOpt = totalTime;
            missPageOpt = unFind;
            pageRateOpt = rate;

            //将数据存储到信息栏中，用于结果窗体的信息显示
            informationBarOpt.Add("OPT游戏模式的取牌序列为：" + sequenceOpt);
            if (quickWatchOpt)
            {
                informationBarOpt.Add("当前游戏手中 有 扑克牌");
            }
            else
            {
                informationBarOpt.Add("当前游戏手中 无 扑克牌");
            }
            informationBarOpt.Add("当前游戏从手中取牌的时间为：" + fastWatchTimeOpt.ToString() + "S");
            informationBarOpt.Add("当前游戏从书包取牌、卡库找牌及出牌的时间均为：" + memoryTimeOpt.ToString() + "S");
            informationBarOpt.Add("手中及书包中均没有卡牌的更新牌面时间为：" + interruptTimeOpt.ToString() + "S");
            informationBarOpt.Add("完成此次取牌序列的总时间为：" + totalTimeOpt.ToString() + "S");
            string yekuang = "";
            for (int y = 1; y <= residentSetNumberOpt; y++)
            {
                yekuang += "书包位置" + y + "号对应卡库" + pageFrameNumberOpt[y-1] + "号；";
            }
            informationBarOpt.Add(yekuang);
            informationBarOpt.Add("此次游戏共取牌 " + sequenceNumberOpt.ToString() + " 次");
            informationBarOpt.Add("此次游戏从卡库中找牌并更新牌面共 " + missPageOpt.ToString() + " 次");
            informationBarOpt.Add("此次游戏更新牌面率为：" + pageRateOpt.ToString());
            
        }


        //FIFO算法函数
        private void FIFO()
        {
            
            Queue<int> queue = new Queue<int>();
            Dictionary<int, string> map = new Dictionary<int, string>();
            int i;//用于记录访问序列的页面当前数目
            int unFind = 0;
            int totalTime = 0;//记录总时间

            //保存数据
            List<string> list1 = new List<string>();
            list1.Add("卡牌序号");
            list1.Add("扑克牌号");
            for (int y = 1; y <= residentSetNumberFifo; y++)
            {
                list1.Add("书包位置" + y + "号");
            }

            list1.Add("对应卡库位置号");
            list1.Add("卡库具体位置");
            list1.Add("取本牌时间");
            informationFormFifo.Add(list1);


            for (i = 0; i < sequenceNumberFifo; i++)
            {
                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = true;
                    buttonSubmission.Enabled = false;
                }));
                if (threadFifo.ThreadState == ThreadState.Running)
                {
                    threadFifo.Suspend();
                }

                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = false;
                    buttonSubmission.Enabled = true;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    //labelAnswer.Text = "";
                    labelTip.Text = "请选出序号为" + sequenceListFifo[i] + "的取牌方式并计算取牌时间，加油！";
                }));
                //this.BeginInvoke(new Action(() =>
                //{
                //    //清空答题区
                //    radioButton1.Checked = false;
                //    radioButton2.Checked = false;
                //    radioButton3.Checked = false;
                //    radioButton4.Checked = false;
                //    labelAnswer.Text = "";
                //    labelTip.Text = "请选出序号为" + sequenceListFifo[i] + "的取牌方式并计算取牌时间，加油！";
                //}));


                int picturelocation = 0;//当存在找到时，确定在驻留集的什么位置
                int picturenumber = checkPicture(sequenceListFifo[i].Substring(0, 1));//确定移动的图片是哪个
                int replacePosition = 0;//当缺页中断时，替换驻留集的位置
                string pageLogical = sequenceListFifo[i].Substring(0, 1);//每个页号的逻辑页号
                string pagePhysical ="-1";//每页的物理页号
                signalTime = 0;


                //是否开启快表
                if (quickWatchFifo)
                {
                    signalTime += fastWatchTimeFifo;//每页时间
                    totalTime += fastWatchTimeFifo;//总时间加上
                    if (map.ContainsValue(pageLogical))
                    {
                        totalTime += memoryTimeFifo;
                        signalTime += memoryTimeFifo;
                        pagePhysical = pageFrameNumberFifo[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号

                        //将每次访问的页面的数据进行保存
                        List<string> list2 = new List<string>();
                        list2.Add(sequenceListFifo[i]);
                        list2.Add(pageLogical);
                        for (int y = 0; y < residentSetNumberFifo; y++)
                        {
                            if (map.ContainsKey(y))
                            {
                                list2.Add(map[y]);
                            }
                            else
                            {
                                list2.Add(" ");
                            }

                        }
                        list2.Add(pagePhysical);
                        list2.Add(changeNumber(pagePhysical) + sequenceListFifo[i].Substring(1, 4)+"H");
                        list2.Add(signalTime.ToString()+"S");
                        informationFormFifo.Add(list2);

                        situation = 1;//有快表，且存在于快表
                        picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;



                        //将线程暂停，等待作答
                        if (threadFifo.ThreadState == ThreadState.Running)
                        {
                            threadFifo.Suspend();
                        }

                        //动画
                        this.Invoke(new EventHandler(delegate
                        {
                            pictureAnimation(situation, picturelocation, picturenumber, 0);
                        }));
                        //this.BeginInvoke(new Action(() =>
                        //{
                        //    pictureAnimation(situation, picturelocation, picturenumber, 0);
                        //}));

                        continue;
                    }
                    else
                    {
                        totalTime += memoryTimeFifo;
                        signalTime += memoryTimeFifo;
                        situation = 2;
                    }
                }
                else
                {
                    signalTime += memoryTimeFifo;
                    totalTime += memoryTimeFifo;
                }

                //访问页表
                //signalTime += memoryTimeFifo;
                //totalTime += memoryTimeFifo;
                //if (situation == 1)
                //{
                //    situation = 2;
                //}
                //else
                //{
                //    situation = 3;
                //}

                //页表中不存在
                if (!map.ContainsValue(pageLogical))
                {
                    //if (situation == 3)
                    //{
                    //    situation = 4;
                    //}

                    //缺页中断
                    unFind++;
                    totalTime += interruptTimeFifo;//加上中断时间
                    signalTime += interruptTimeFifo;

                    if (map.Count() < residentSetNumberFifo)
                    {
                        int location = map.Count;
                        replacePosition = map.Count() + 1;
                        map.Add(location, pageLogical);
                        queue.Enqueue(location);
                    }
                    else
                    {
                        int index = queue.Dequeue();
                        replacePosition = index  + 1;
                        map.Remove(index);
                        map.Add(index, pageLogical);
                        queue.Enqueue(index);
                    }

                    //更新页表和快表，重新查询
                    if (quickWatchFifo)
                    {
                        situation = 2;
                        pagePhysical = pageFrameNumberFifo[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                        signalTime += fastWatchTimeFifo;//加上快表和内存取数据时间
                        signalTime += memoryTimeFifo;
                        totalTime += fastWatchTimeFifo;
                        totalTime += memoryTimeFifo;
                    }
                    else
                    {
                        situation = 4;
                        pagePhysical = pageFrameNumberFifo[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                        signalTime += 2 * memoryTimeFifo;//加上页表和内存取数据时间
                        totalTime += 2 * memoryTimeFifo;
                    }
                }//页表中存在
                else
                {
                    situation = 3;
                    pagePhysical = pageFrameNumberFifo[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                    picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;
                    signalTime += memoryTimeFifo;//加上内存取数据时间
                    totalTime += memoryTimeFifo;

                }

                //保存本页产生的数据
                List<string> list3 = new List<string>();
                list3.Add(sequenceListFifo[i]);
                list3.Add(pageLogical);
                for (int y = 0; y < residentSetNumberFifo; y++)
                {
                    if (map.ContainsKey(y))
                    {
                        list3.Add(map[y]);
                    }
                    else
                    {
                        list3.Add(" ");
                    }
                }
                list3.Add(pagePhysical);
                list3.Add(changeNumber(pagePhysical) + sequenceListFifo[i].Substring(1, 4)+"H");
                list3.Add(signalTime.ToString()+"S");
                informationFormFifo.Add(list3);



                //将线程暂停，等待作答
                if (threadFifo.ThreadState == ThreadState.Running)
                {
                    threadFifo.Suspend();
                }

                //动画
                if (situation == 2 || situation == 4)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, 0, picturenumber, replacePosition);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, 0, picturenumber, replacePosition);
                    //}));
                }
                else if (situation == 3)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, picturelocation, picturenumber, 0);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, picturelocation, picturenumber, 0);
                    //}));
                }
                else
                {
                    MessageBox.Show("Fifo算法函数中情况分类出错");
                }
            }
            double rate = (double)unFind / (double)sequenceNumberFifo;

            //保存序列完成的总体信息
            totalTimeFifo = totalTime;
            missPageFifo = unFind;
            pageRateFifo = rate;

            //将数据存储到信息栏中，用于结果窗体的信息显示
            informationBarFifo.Add("Fifo游戏模式的取牌序列为：" + sequenceFifo);
            if (quickWatchFifo)
            {
                informationBarFifo.Add("当前游戏手中 有 扑克牌");
            }
            else
            {
                informationBarFifo.Add("当前游戏手中 无 扑克牌");
            }
            informationBarFifo.Add("当前游戏从手中取牌的时间为：" + fastWatchTimeFifo.ToString() + "S");
            informationBarFifo.Add("当前游戏从书包取牌、卡库找牌及出牌的时间均为：" + memoryTimeFifo.ToString() + "S");
            informationBarFifo.Add("手中及书包中均没有卡牌的更新牌面时间为：" + interruptTimeFifo.ToString() + "S");
            informationBarFifo.Add("完成此次取牌序列的总时间为：" + totalTimeFifo.ToString() + "S");
            string yekuang = "";
            for (int y = 1; y <= residentSetNumberFifo; y++)
            {
                yekuang += "书包位置" + y + "号对应卡库" + pageFrameNumberFifo[y - 1] + "号；";
            }
            informationBarFifo.Add(yekuang);
            informationBarFifo.Add("此次游戏共取牌 " + sequenceNumberFifo.ToString() + " 次");
            informationBarFifo.Add("此次游戏从卡库中找牌并更新牌面共 " + missPageFifo.ToString() + " 次");
            informationBarFifo.Add("此次游戏更新牌面率为：" + pageRateFifo.ToString());


        }

        //LRU算法函数
        private void LRU()
        {
            Dictionary<int, string> map = new Dictionary<int, string>();
            List<string> linkedList = new List<string>();

            int totalTime = 0;
            int unFind = 0;

            //保存数据
            List<string> list1 = new List<string>();
            list1.Add("卡牌序号");
            list1.Add("扑克牌号");
            for (int y = 1; y <= residentSetNumberLru; y++)
            {
                list1.Add("书包位置" + y + "号");
            }

            list1.Add("对应卡库位置号");
            list1.Add("卡库具体位置");
            list1.Add("取本牌时间");
            informationFormLru.Add(list1);
            

            for (int i = 0; i < sequenceNumberLru; i++)
            {
                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = true;
                    buttonSubmission.Enabled = false;
                }));
                if (threadLru.ThreadState == ThreadState.Running)
                {
                    threadLru.Suspend();
                }

                this.Invoke(new EventHandler(delegate
                {
                    buttonReply.Enabled = false;
                    buttonSubmission.Enabled = true;
                    //清空答题区
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    //labelAnswer.Text = "";
                    labelTip.Text = "请选出序号为" + sequenceListLru[i] + "的取牌方式并计算取牌时间，加油！";
                }));
                //this.BeginInvoke(new Action(() =>
                //{
                //    //清空答题区
                //    radioButton1.Checked = false;
                //    radioButton2.Checked = false;
                //    radioButton3.Checked = false;
                //    radioButton4.Checked = false;
                //    labelAnswer.Text = "";
                //    labelTip.Text = "请选出序号为" + sequenceListLru[i] + "的取牌方式并计算取牌时间，加油！";
                //}));

                int picturelocation = 0;//当存在找到时，确定在驻留集的什么位置
                int picturenumber = checkPicture(sequenceListLru[i].Substring(0, 1));//确定移动的图片是哪个
                int replacePosition = 0;//当缺页中断时，替换驻留集的位置
                string pageLogical = sequenceListLru[i].Substring(0, 1);//每个页号的逻辑页号
                string pagePhysical = "-1";//每页的物理页号
                signalTime = 0;

                //是否开启快表
                if (quickWatchLru)
                {
                    signalTime += fastWatchTimeLru;//每页时间
                    totalTime += fastWatchTimeLru;//总时间加上访问快表时间

                    if (map.ContainsValue(pageLogical))
                    {
                        totalTime += memoryTimeLru;
                        signalTime += memoryTimeLru;
                        pagePhysical = pageFrameNumberLru[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号

                        //将每次访问的页面的数据进行保存
                        List<string> list2 = new List<string>();
                        list2.Add(sequenceListLru[i]);
                        list2.Add(pageLogical);
                        for (int y = 0; y < residentSetNumberLru; y++)
                        {
                            if (map.ContainsKey(y))
                            {
                                list2.Add(map[y]);
                            }
                            else
                            {
                                list2.Add(" ");
                            }

                        }
                        list2.Add(pagePhysical);
                        list2.Add(changeNumber(pagePhysical) + sequenceListLru[i].Substring(1, 4)+"H");
                        list2.Add(signalTime.ToString()+"S");
                        informationFormLru.Add(list2);


                        //将线程暂停，等待作答
                        if (threadLru.ThreadState == ThreadState.Running)
                        {
                            threadLru.Suspend();
                        }

                        situation = 1;//有快表，且存在于快表
                        picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;


                        //动画
                        this.Invoke(new EventHandler(delegate
                        {
                            pictureAnimation(situation, picturelocation, picturenumber, 0);
                        }));
                        //this.BeginInvoke(new Action(() =>
                        //{
                        //    pictureAnimation(situation, picturelocation, picturenumber, 0);
                        //}));


                        continue;
                    }
                    else
                    {
                        totalTime += memoryTimeLru;
                        signalTime += memoryTimeLru;
                        situation = 2;
                    }
                }
                else
                {
                    signalTime += memoryTimeLru;
                    totalTime += memoryTimeLru;
                }

                //查询页表
                //signalTime += memoryTimeLru;
                //totalTime += memoryTimeLru;
                //if (situation == 1)
                //{
                //    situation = 2;
                //}
                //else
                //{
                //    situation = 3;
                //}

                //页表不存在
                if (!map.ContainsValue(pageLogical))
                {
                    //if (situation == 3)
                    //{
                    //    situation = 4;
                    //}

                    //缺页中断
                    unFind++;
                    totalTime += interruptTimeLru;//加上中断时间
                    signalTime += interruptTimeLru;


                    if (map.Count() < residentSetNumberLru)
                    {
                        replacePosition = map.Count() + 1;
                        map.Add(map.Count(), pageLogical);
                        linkedList.Add(pageLogical);//添加位置 
                    }
                    else
                    {
                        string removePageIndex = linkedList.ElementAt(0);
                        int index = map.First(x => x.Value == removePageIndex).Key;
                        replacePosition = index + 1;
                        map.Remove(index);
                        map.Add(index, pageLogical);
                        linkedList.RemoveAt(0);
                        linkedList.Add(pageLogical);

                    }

                    //更新页表和快表，重新查询
                    if (quickWatchLru)
                    {
                        situation = 2;
                        pagePhysical = pageFrameNumberLru[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                        signalTime += fastWatchTimeLru;//加上快表和内存取数据时间
                        signalTime += memoryTimeLru;
                        totalTime += fastWatchTimeLru;
                        totalTime += memoryTimeLru;
                    }
                    else
                    {
                        situation = 4;
                        pagePhysical = pageFrameNumberLru[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                        signalTime += 2 * memoryTimeLru;//加上页表和内存取数据时间
                        totalTime += 2 * memoryTimeLru;
                    }
                }
                else
                {
                    //如果包含这个值，把这个值拿走并在后面压入
                    int index = linkedList.IndexOf(pageLogical);
                    linkedList.RemoveAt(index);
                    linkedList.Add(pageLogical);//添加位置

                    picturelocation = map.FirstOrDefault(q => q.Value == pageLogical).Key + 1;

                    situation = 3;
                    pagePhysical = pageFrameNumberLru[map.FirstOrDefault(q => q.Value == pageLogical).Key].ToString();//每页的物理页号
                    signalTime += memoryTimeLru;//加上内存取数据时间
                    totalTime += memoryTimeLru;
                }

                //保存本页产生的数据
                List<string> list3 = new List<string>();
                list3.Add(sequenceListLru[i]);
                list3.Add(pageLogical);
                for (int y = 0; y < residentSetNumberLru; y++)
                {
                    if (map.ContainsKey(y))
                    {
                        list3.Add(map[y]);
                    }
                    else
                    {
                        list3.Add(" ");
                    }
                }
                list3.Add(pagePhysical);
                list3.Add(changeNumber(pagePhysical) + sequenceListLru[i].Substring(1, 4)+"H");
                list3.Add(signalTime.ToString()+"S");
                informationFormLru.Add(list3);



                //将线程暂停，等待作答
                if (threadLru.ThreadState == ThreadState.Running)
                {
                    threadLru.Suspend();
                }

                //动画
                if (situation == 2 || situation == 4)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, 0, picturenumber, replacePosition);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, 0, picturenumber, replacePosition);
                    //}));
                }
                else if (situation == 3)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        pictureAnimation(situation, picturelocation, picturenumber, 0);
                    }));
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    pictureAnimation(situation, picturelocation, picturenumber, 0);
                    //}));
                }
                else
                {
                    MessageBox.Show("Lru算法函数中情况分类出错");
                }


            }
            double rate = (double)unFind / (double)sequenceNumberLru;

            //保存序列完成的总体信息
            totalTimeLru = totalTime;
            missPageLru = unFind;
            pageRateLru = rate;

            //将数据存储到信息栏中，用于结果窗体的信息显示
            informationBarLru.Add("Lru游戏模式的取牌序列为：" + sequenceLru);
            if (quickWatchLru)
            {
                informationBarLru.Add("当前游戏手中 有 扑克牌");
            }
            else
            {
                informationBarLru.Add("当前游戏手中 无 扑克牌");
            }
            informationBarLru.Add("当前游戏从手中取牌的时间为：" + fastWatchTimeLru.ToString() + "S");
            informationBarLru.Add("当前游戏从书包取牌、卡库找牌及出牌的时间均为：" + memoryTimeLru.ToString() + "S");
            informationBarLru.Add("手中及书包中均没有卡牌的更新牌面时间为：" + interruptTimeLru.ToString() + "S");
            informationBarLru.Add("完成此次取牌序列的总时间为：" + totalTimeLru.ToString() + "S");
            string yekuang = "";
            for (int y = 1; y <= residentSetNumberLru; y++)
            {
                yekuang += "书包位置" + y + "号对应卡库" + pageFrameNumberLru[y - 1] + "号；";
            }
            informationBarLru.Add(yekuang);
            informationBarLru.Add("此次游戏共取牌 " + sequenceNumberLru.ToString() + " 次");
            informationBarLru.Add("此次游戏从卡库中找牌并更新牌面共 " + missPageLru.ToString() + " 次");
            informationBarLru.Add("此次游戏更新牌面率为：" + pageRateLru.ToString());

        }


        //timer1,用于出牌的动画（类似从内存中找数据）
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.Y > 250)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 4);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                timer1.Enabled = false;
            }
        }

        //缺页中断，更新页表及快表的位置1
        private void timer21_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X>pictureBox11.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X-10, pictureBoxMove.Location.Y);
            }
            else if(pictureBoxMove.Location.Y > pictureBox11.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                timer1.Interval = 1;
                timer1.Enabled = true;
                timer21.Enabled = false;


            }
        }

        private void timer22_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox12.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y > pictureBox12.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                timer1.Interval = 1;
                timer1.Enabled = true;
                timer22.Enabled = false;
            }
        }

        private void timer23_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox13.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y > pictureBox13.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                timer1.Interval = 1;
                timer1.Enabled = true;
                timer23.Enabled = false;
            }
        }

        private void timer24_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox14.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y > pictureBox14.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                timer1.Interval = 1;
                timer1.Enabled = true;
                timer24.Enabled = false;
            }
        }

        private void timer25_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox15.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y > pictureBox15.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y - 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                timer1.Interval = 1;
                timer1.Enabled = true;
                timer25.Enabled = false;
            }
        }

        //缺页中断，更新页表位置1的动画
        private void timer41_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox21.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y < pictureBox21.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y + 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                if(timersituation==1)
                {
                    timer21.Interval = 1;
                    timer21.Enabled = true;
                }
                else if(timersituation==2)
                {
                    timer1.Interval = 1;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("timer调用出错");
                }
                timer41.Enabled = false;
            }
        }

        private void timer42_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox22.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y < pictureBox22.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y + 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                

                if (timersituation == 1)
                {
                    timer22.Interval = 1;
                    timer22.Enabled = true;
                }
                else if (timersituation == 2)
                {
                    timer1.Interval = 1;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("timer调用出错");
                }
                timer42.Enabled = false;

            }
        }

        private void timer43_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox23.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y < pictureBox23.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y + 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                if (timersituation == 1)
                {
                    timer23.Interval = 1;
                    timer23.Enabled = true;
                }
                else if (timersituation == 2)
                {
                    timer1.Interval = 1;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("timer调用出错");
                }
                timer43.Enabled = false;
            }
        }

        private void timer44_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox24.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y < pictureBox24.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y + 10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                if (timersituation == 1)
                {
                    timer24.Interval = 1;
                    timer24.Enabled = true;
                }
                else if (timersituation == 2)
                {
                    timer1.Interval = 1;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("timer调用出错");
                }
                timer44.Enabled = false;
            }
        }

        private void timer45_Tick(object sender, EventArgs e)
        {
            pictureBoxMove.Visible = true;
            pictureBoxMove.BringToFront();
            pictureBoxMove.Refresh();
            if (pictureBoxMove.Location.X > pictureBox25.Location.X)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X - 10, pictureBoxMove.Location.Y);
            }
            else if (pictureBoxMove.Location.Y < pictureBox25.Location.Y)
            {
                pictureBoxMove.Location = new Point(pictureBoxMove.Location.X, pictureBoxMove.Location.Y +10);
            }
            else
            {
                pictureBoxMove.Visible = false;
                pictureBoxMove.Location = new Point(pictureBox31.Location.X + 10, pictureBox31.Location.Y + 15);
                pictureBoxMove.Refresh();
                if (timersituation == 1)
                {
                    timer25.Interval = 1;
                    timer25.Enabled = true;
                }
                else if (timersituation == 2)
                {
                    timer1.Interval = 1;
                    timer1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("timer调用出错");
                }
                timer45.Enabled = false;
            }
        }

        private void buttonReply_Click(object sender, EventArgs e)
        {
            if (threadOpt != null && threadOpt.ThreadState == ThreadState.Suspended)
            {
                threadOpt.Resume();

            }
            else if (threadFifo != null && threadFifo.ThreadState == ThreadState.Suspended)
            {
                threadFifo.Resume();

            }
            else if (threadLru != null && threadLru.ThreadState == ThreadState.Suspended)
            {
                threadLru.Resume();

            }
            else
            {
                MessageBox.Show("当前没有线程挂起");
            }
        }
    }
}

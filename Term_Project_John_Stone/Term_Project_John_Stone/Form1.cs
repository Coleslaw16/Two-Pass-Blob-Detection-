// By John Stone
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Term_Project_John_Stone
{
    public partial class Form1 : Form
    {
        string filename = "";
        Image picture;
        int linklistcount = 0;
        ArrayList holderArray = new ArrayList();
        ArrayList relevence = new ArrayList();
        int[,] labelHolder;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filename = dlg.FileName;
                openImage();
            }
        }

        private void openImage()
        {
            picture = Image.FromFile(filename);
            pictureBox1.Image = picture;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(picture);
            labelHolder = new int[temp.Width,temp.Height];
            Console.WriteLine(labelHolder.GetLength(0) + " " + labelHolder.GetLength(1));
            Color bl = Color.FromArgb(255, 0, 0, 0);
            for(int i=0;i<temp.Height;i++)
            {
                for(int j=0;j<temp.Width;j++)
                {
                    Color pixelColor = temp.GetPixel(j,i);
                    if(i==0 && j==0)
                    {
                        if(pixelColor != bl)
                        {
                            xyPosition item = new xyPosition(j,i);
                            createLinkedList(item);
                        }
                    }
                    else if(i==0 && j !=0)
                    {
                        if(pixelColor != bl)
                        {
                            if(pixelColor == temp.GetPixel(j-1,i))
                            {
                                step1(j,i);
                            }
                            if (pixelColor != temp.GetPixel(j-1, i))
                            {
                                xyPosition xyclass = new xyPosition(j,i);
                                createLinkedList(xyclass);
                            }
                        }
                    }
                    else if(j==0 && i != 0)
                    {
                        if(pixelColor != bl)
                        {
                            if(pixelColor == temp.GetPixel(j,i-1))
                            {
                                step3(j,i);
                            }
                            if(pixelColor != temp.GetPixel(j,i-1))
                            {
                                xyPosition xyclass = new xyPosition(j,i);
                                createLinkedList(xyclass);
                            }
                        }
                    }
                    else
                    {
                        if(pixelColor != bl)
                        {
                            if(pixelColor == temp.GetPixel(j-1,i))
                            {
                                step1(j,i);
                            }
                            if(pixelColor == temp.GetPixel(j-1,i) && pixelColor == temp.GetPixel(j,i-1) && labelHolder[j,i-1] != labelHolder[j-1,i])
                            {
 //                               Console.WriteLine(temp.GetPixel(j - 1, i) + " " + temp.GetPixel(j, i - 1) + " " + labelHolder[j, i - 1] + " " + labelHolder[j - 1, i]);
                                step2(j,i);
                            }
                            if(pixelColor != temp.GetPixel(j-1,i) && pixelColor == temp.GetPixel(j,i-1))
                            {
                                step3(j,i);
                            }
                            if (pixelColor != temp.GetPixel(j - 1, i) && pixelColor != temp.GetPixel(j, i - 1))
                            {
                                xyPosition xyclass = new xyPosition(j,i);
                                createLinkedList(xyclass);   
                            }
                        }
                    }
                }
            }

            for(int i=relevence.Count-1;i>0;i--)
            {
                if((int)(relevence[i]) != i)
                {
                    LinkedList<xyPosition> first = (LinkedList<xyPosition>)(holderArray[(int)(relevence[i])]);
                    LinkedList<xyPosition> second = (LinkedList<xyPosition>)(holderArray[i]);
                    foreach(xyPosition p in second)
                    {
                        first.AddLast(p);
                    }
                    holderArray[(int)(relevence[i])] = first;
                    holderArray.RemoveAt(i);
                }
            }

//This code implements coloring the identified shapes. In the current implementation it does seven different colors. 
//This code is not necessary for the algorithm just presentation
//If more shapes need to be colored this code should be changed.
            Color[] colorArray = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan, Color.White };
            int count = 0;
            foreach(LinkedList<xyPosition> ll in holderArray)
            {
                Color randomColor = colorArray[count];
                foreach(xyPosition p in ll)
                {
                    temp.SetPixel(p.X, p.Y, randomColor);
                }
                count++;
           }
//Use this code if need more than 7 shapes 
            /*
            foreach (LinkedList<xyPosition> ll in holderArray)
            {
                Random randomGen = new Random();
                KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                KnownColor randomColorName = names[randomGen.Next(names.Length)];
                Color ranCol = Color.FromKnownColor(randomColorName);
                foreach (xyPosition p in ll)
                {
                    temp.SetPixel(p.X, p.Y, ranCol);
                }
            }
              */
            pictureBox1.Image = null;
            label2.Text = holderArray.Count.ToString();
            pictureBox1.Image = temp;
//            Console.WriteLine("WE DONE");
        }

        //this method implements step 4 of the algorithm
        private void createLinkedList(xyPosition item)
        {
            item.setH(item);
            linklistcount++;
            LinkedList<xyPosition> temp = new LinkedList<xyPosition>();
            temp.AddFirst(item);
            holderArray.Add(temp); 
            labelHolder[item.X,item.Y] = linklistcount;
            relevence.Add(linklistcount-1);
        }

        private void step1(int j, int i)
        {
            int count = 0;
            labelHolder[j,i] = labelHolder[j-1,i];
            int target = labelHolder[j-1, i] - 1;
            foreach(LinkedList<xyPosition> ll in holderArray)
            {
                if(count == target)
                {
                    xyPosition xyclass = new xyPosition(j, i);
                    ll.AddLast(xyclass);
                    break;
                }
                count++;
            }
        }

        private void step3(int j, int i)
        {
            int count = 0;
            labelHolder[j,i] = labelHolder[j,i-1];
            int target = labelHolder[j, i - 1]-1;
            foreach(LinkedList<xyPosition> ll in holderArray)
            {
                if(count == target)
                {
                    xyPosition xyclass = new xyPosition(j, i);
                    ll.AddLast(xyclass);
                    break;
                }
                count++;
            }
        }

        private void step2(int j, int i)
        {
            int num1 = labelHolder[j,i-1];
            int num2 = labelHolder[j-1,i];
            int count = 0;
            labelHolder[j, i] = Math.Min(num1, num2);
            int target = Math.Min(num1, num2)-1;
            int notTarget = Math.Max(num1,num2)-1;
            relevence[notTarget] = target;
            foreach (LinkedList<xyPosition> ll in holderArray)
            {
                if (count == target)
                {
                    xyPosition xyclass = new xyPosition(j, i);
                    ll.AddLast(xyclass);
                    break;
                }
                count++;
            }
        }
    }
}

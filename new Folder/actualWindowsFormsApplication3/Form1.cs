using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication3
{
    public class freqt {
        public String s;
        public int freq;
        public freqt(String s,int freq) {
            this.s = s;
            this.freq = freq;
        }
    }

    public partial class Form1 : Form
    {
        
        public ListBox cb = new ListBox();
        List<freqt> list = new List<freqt>();
        
        
        //  public ListView lv = new ListView();
        static string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "wordlist.txt");
        static string path1 = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "bi-grams.txt");
        string Text = System.IO.File.ReadAllText(path);
        string Bigrams = System.IO.File.ReadAllText(path1);
        Trie root = new Trie();

        public String[] inp;

        public Form1()
        {
            //Console.WriteLine(list.Any());
            list.Add(new freqt("a",456));
            list.Add(new freqt("b", 156));
            list.Add(new freqt("t",756));
            list.First();
            
            //Console.WriteLine(list.Any());
            list.Sort(
                 delegate(freqt o1,freqt o2){
                     return o1.freq > o2.freq ? -1 : 1;
                 }   
                );
            foreach(freqt f in list){
                Console.WriteLine(f.s+" "+f.freq);
            }
            cb.KeyDown += new KeyEventHandler(richTextBox1_KeyDown);
            Controls.Add(cb);
            //spellings = Text.Split('\n');
            string[] ii = Text.Split('\n');
            int len = ii.Length;
           // root.insert("Korea",cb);
           // root.spelltraverse("Korea",cb);
            for (int i = 0; i < len; ++i)
            {
                if (!ii[i].Equals(""))
                {
                   // Console.WriteLine(ii[i].TrimEnd());
                    root.insert(ii[i].TrimEnd(), cb);
                }
            }
            string[] bgrams = Bigrams.Split('\n');
            len = bgrams.Length;
            for (int i = 0; i < len; ++i)
            {
                if (bgrams[i] != null)
                {
                    string[] input=bgrams[i].Split(' ');
                    root.insert_bigrams(input[1],input[2],Int32.Parse(input[0]));
                }
            }
            InitializeComponent();
            richTextBox1.Focus();

            cb.Visible = false;
        }
        private void richTextBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Console.Write("in");
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                cb.Focus();
                cb.SelectedIndex = 0;
                
            }
            else if (e.KeyCode == Keys.Space)
            {
                cb.Visible = false;

            }
            else if (e.KeyCode == Keys.Enter && sender == cb)
            {
                richTextBox1.Focus();




                String text = richTextBox1.Text;
                String[] inp = text.Split(' ');
                int len = inp.Length;
             //   Console.WriteLine(root.s.child[97].end + "fffff");
                if (inp[len - 1].Equals(""))
                {
                    if (sender == cb)
                    {
                        richTextBox1.AppendText(cb.GetItemText(cb.SelectedItem));
                        //richTextBox1.AppendText(cb.GetSelected().ToString().IndexOf(inp[len-2]))
                       // Console.WriteLine();
                    }
                    // Console.Write("gotya");
                }
                else
                {
                    // root.traverse("a",cb);
                    //  Console.WriteLine(root.s.child[97].end+"ff");
                    //root.suggest_rankwise(inp[len-1],cb);
                    richTextBox1.AppendText(cb.GetItemText(cb.SelectedItem));
                }
              //  e.Handled = false;


                //richTextBox1.AppendText(cb.SelectedItem.ToString().Split('\n')[0]);
                //Console.Write();
            }
            else
            {
                richTextBox1.Focus();
                //  richTextBox1_TextChanged(richTextBox1,e);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            cb.Items.Clear();


            Point p = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            Point cursorPt = Cursor.Position;
            p.Y += (int)Math.Ceiling(this.richTextBox1.Font.GetHeight()) + 30;

            cb.Location = p;
            List<String> list = new List<String>();
            String text = richTextBox1.Text;
            String[] inp = text.Split(' ');
            int len = inp.Length;
          //  Console.WriteLine(root.s.child[97].end+"fffff");
            if (inp[len - 1].Equals(""))
            {
                // Console.Write("got ya");
                
                if (len > 1)
                {
                    root.insert(inp[len - 2], cb);
                    cb.Visible = false;
                    root.suggest_biagrams(inp[len - 2], cb);
                }
            }
            else
            {

               // root.traverse("a",cb);
              //  Console.WriteLine(root.s.child[97].end+"ff");
                //root.suggest_rankwise(inp[len-1],cb);
         //       root.suggest_rankwise(inp[len-1],cb);
                root.spells(inp[len - 1], cb);
            }
            if (cb.Items.Count == 0)
            {
                cb.Visible = false;
            }
            else
            {
                cb.Visible = true;
            }
        }
    }
    public class Forstarting {
        public Node temp;
        public String me;
        public Forstarting(Node temp, String me) {
            this.temp = temp;
            this.me = me;
        }
    }
    public class Trie
    {
        public Node s = new Node(" ");
        public void suggest_rankwise(String s,ListBox cb) {
            Node temp = this.s;
            int len = s.Length;
            int i = 0;
           // Console.WriteLine("look");
            while (i < len){
                char c = s.ElementAt(i);
                if (temp.on[(int)c])
                {
             //       Console.WriteLine((int)c+"dd"+temp.end);
                    temp = temp.child[(int)c];
                }
                else {
                    return;
                }
                ++i;
            }
            int count = 0;
            int tostop = 0;
            List<Forstarting> list=new List<Forstarting>();
         //   Console.WriteLine(temp.end+" good "+temp.c);
            Forstarting t = new Forstarting(temp,s);
            list.Add(t);
            while(count<=5  && list.Any()){
                tostop++;
                
                Forstarting temp1 = list.First();
                list.RemoveAt(0);
                if (tostop == 100)
                    break;
                
           //     Console.WriteLine(temp1.me+" "+temp1.temp.c+" "+temp1.temp.end);
                if (temp1.temp.end == true)
                {
                    cb.Items.Add(temp1.me);
                    count++;
                }
                for (int j = 0; j < 256; ++j) {
                   
                    if (temp1.temp.on[j]) {
                        list.Add(new Forstarting(temp1.temp.child[j],temp1.me+""+(char)j));
                    }
                }
                list.Sort(
                        delegate(Forstarting n1,Forstarting n2) {
                            return n1.temp.freq > n2.temp.freq ? -1 : 1;
                        }
                    );
            }
            
        }
        public void spells(String ins, ListBox cb) { 
            string[] word;
            
            HashSet<String> deletes=new HashSet<String>(),inserts=new HashSet<String>(),replaces=new HashSet<String>();
            int len = ins.Length;
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < len-1; ++i) {
                deletes.Add(ins.Substring(0, i + 1) + ins.Substring(i + 2));
            }
            for (int i = 0; i < len; ++i) {
                string first = ins.Substring(0, i + 1), second = ins.Substring(i+1);
                for (int j = 0; j < 26; ++j)
                {
                    inserts.Add(first + alpha.ElementAt(j) + second);
                }
            }
            for (int i = 0; i < len - 1; ++i)
            {
                string first = ins.Substring(0, i + 1), second = ins.Substring(i + 2);
                for (int j = 0; j < 26; ++j)
                {
                    replaces.Add(first +alpha.ElementAt(j)+second);
                }
            }
            inserts.UnionWith(deletes);
            inserts.UnionWith(replaces);
            //Console.WriteLine("inserts");
            foreach (string si in inserts){
                spelltraverse(si, cb);
            //    Console.WriteLine(si); 
            }
            //Console.WriteLine("deletes");
        }
        public void spelltraverse(String ins, ListBox cb) {

            Node temp = this.s;
//            Console.WriteLine(temp.child[(int)'a'].end + "whatisthis");
            char c = ins.ElementAt(0);
            int len = ins.Length;
            int i = 0;
            String ans = "found";
            while (i < len)
            {
                c = ins.ElementAt(i);
             //   Console.Write((int)c);
                if (temp.on[(int)c])
                {
                    ++i;
                    temp = temp.child[(int)c];
                }
                else
                {
                    return;
                }
            }
            //Console.WriteLine(temp.c+" "+temp.end);
            if (temp.end == true)
            {
                cb.Items.Add(ins);
            }  
        }
        public void traverse(String ins, ListBox cb)
        {
            Node temp = this.s;
            //Console.WriteLine(temp.child[(int)'a'].end+"whatisthis");
            char c = ins.ElementAt(0);
            int len = ins.Length;
            int i = 0;
            String ans = "found";
            while (i < len)
            {
                c = ins.ElementAt(i);
                if (temp.on[(int)c])
                {
                    ++i;
                    temp = temp.child[(int)c];
                }
                else
                {
                    ans = "Not found";
                    break;
                }
            }
            if (ans.Equals("found"))
            {
              //  Console.WriteLine("in");
                suggestion(temp, ins, cb);
            }
        }
        public void insert_bigrams(String s, String toadd, int freq)
        {
            Node temp = this.s;
            char c = s.ElementAt(0);
            int i = 0;
            int len = s.Length;
            // Console.Write(s);
            while (i < len)
            {
                c = s.ElementAt(i);
                int index=Encoding.Default.GetBytes(s)[i];
                try
                {
                    if (temp.on[index])
                    {
                        temp = temp.child[index];
                        ++i;
                    }
                    else
                    {
                        temp.child[index] = new Node(c + "");
                        temp.on[index] = true;
                        temp = temp.child[index];
                        ++i;
                    }
                }
                catch (Exception e) {
                    Console.Write((int)s.ElementAt(i)+" "+c);
                    Console.WriteLine(Encoding.Default.GetBytes(s)[i]);
                    break;
                }
            }
            ++i;
            try
            {
                temp.bigrams.Add(toadd, freq);
                if (temp.max1 < freq)
                {
                    int swap = temp.max1;
                    temp.max1 = freq;
                    freq = swap;
                }
                if (temp.max2 < freq)
                {
                    int swap = temp.max2;
                    temp.max2 = freq;
                    freq = swap;
                }
                if (temp.max3 < freq)
                {
                    int swap = temp.max3;
                    temp.max3 = freq;
                    freq = swap;
                }
                if (temp.max4 < freq)
                {
                    int swap = temp.max4;
                    temp.max4 = freq;
                    freq = swap;
                }
                if (temp.max5 < freq)
                {
                    int swap = temp.max5;
                    temp.max5 = freq;
                    freq = swap;
                }
            }
            catch (Exception e)
            {
            }
        }
        public void suggest_biagrams(String s, ListBox cb)
        {
            Node temp = this.s;
            int len = s.Length;
            for (int i = 0; i < len; ++i)
            {
                int index = Encoding.Default.GetBytes(s)[i];
                if (temp.on[index])
                {
                    temp = temp.child[index];
                }
                else
                {
                    return;
                }
            }
            cb.Items.Clear();
            foreach (KeyValuePair<String,int> ad in temp.bigrams)
            {
                // Console.Write(ad);
                if(ad.Value>=temp.max5){
                    cb.Items.Add(ad.Key);
                }
                
            }
            cb.Visible = true;
        }
        public void insert(String ins, ListBox cb)
        {
            if (ins.Length == 0)
                return;
            Node temp = this.s;
            char c = ins.ElementAt(0);
            int len = ins.Length;
            int i = 0;
            String ans = "found";
            
            while (i < len)
            {
                c = ins.ElementAt(i);
                int index = Encoding.Default.GetBytes(ins)[i];
               // Console.Write(index+" ");
                if (temp.on[index])
                {
                    ++i;
                    temp = temp.child[index];
                    ++temp.freq;
                }
                else
                {
                    ans = "Not found";
                    temp.child[index] = new Node(c + "");
                    temp.on[index] = true;
                    temp = temp.child[index];
                    ++temp.freq;
                    ++i;
                }
            }
          //  Console.WriteLine(temp.c);
            temp.end = true;
        }
        public static void suggestion(Node temp, String s, ListBox cb)
        {
            int on = 0;
            
          //  Console.WriteLine(temp.end+" good "+temp.c);
            if (temp.end == true)
            {
                //Console.WriteLine(s);
             //   Console.WriteLine("c"+s+"dddddddd"+temp.end+" "+s.Equals("a"));
                cb.Items.Add(s);
            }
            for (int i = 0; i < 256; ++i)
            {

                if (temp.on[i])
                {
                    ++on;
                    suggestion(temp.child[i], s + (char)i, cb);
                }
            }

        }

    }
    public class Pair
    {
        String word;
        int count = 0;
        Pair(String s, int count)
        {
            word = s;
            this.count = count;
        }
    }
    public class Node
    {
        public char c;
        public Dictionary<String, int> bigrams;
        //String me;
        public Boolean end = false;
        public Boolean[] on = new Boolean[256];
        public Node[] child = new Node[256];
        public int freq = 0;
        public int max1 = Int16.MinValue, max2 = Int16.MinValue, max3 = Int16.MinValue, max4 = Int16.MinValue, max5 = Int16.MinValue;
        public Node(String c)
        {
            bigrams = new Dictionary<String, int>();
            this.c = c.ElementAt(0);
        }
    }
}

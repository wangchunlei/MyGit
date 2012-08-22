using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedObjects;

namespace PokeInDesktopSample
{
    //We define this class on PokeIn DesktopClient and let server side call the methods from this class
    public class DesktopTest123
    {
        Form1 frm;
        internal bool subscribed = false;

        public DesktopTest123(Form1 f)
        {
            frm = f;
        }

        public void ServerTimeUpdated(DateTime tm)
        {
            frm.textBox1.Invoke(new DUpdateString(UpdateString), tm);
        }

        void UpdateString(DateTime tm)
        {
            frm.textBox1.Text = tm.ToLongTimeString();
        }

        public void ParameterTest(TestClass test)
        {
            frm.listBox1.Invoke(new DAddParam(AddParam), "Number:" + test.number.ToString());
            frm.listBox1.Invoke(new DAddParam(AddParam), "ItemsCount:" + test.items.Count.ToString());
            frm.listBox1.Invoke(new DAddParam(AddParam), "M:" + test.text);

            //Select last item on listbox
            frm.listBox1.SelectedIndex = frm.listBox1.Items.Count - 1;
        }

        public void TestClassReceived(int testNumber, int itemCount, string text, string dtText)
        {
            frm.listBox1.Invoke(new DAddParam(AddParam), "TestClass received by Server");
            frm.listBox1.Invoke(new DAddParam(AddParam), "Number:" + testNumber.ToString());
            frm.listBox1.Invoke(new DAddParam(AddParam), "ItemsCount:" + itemCount.ToString());
            frm.listBox1.Invoke(new DAddParam(AddParam), "M:" + text);
            frm.listBox1.Invoke(new DAddParam(AddParam), "DT:" + dtText);

            //Select last item on listbox
            frm.listBox1.SelectedIndex = frm.listBox1.Items.Count - 1;

        }

        void AddParam(string t)
        {
            frm.listBox1.Items.Add(t);
        }

        public void Subscribed()
        {
            frm.listBox1.Invoke(new DUpdateStatus(SubscribeStatus), true);
        }

        public void UnSubscribed()
        {
            frm.listBox1.Invoke(new DUpdateStatus(SubscribeStatus), false);
        }

        void SubscribeStatus(bool active)
        {
            subscribed = active;
            if (active)
            {
                frm.listBox1.Items.Add("Subscribed");
                frm.button1.Text = @"UnSubscribe";
            }
            else
            {
                frm.listBox1.Items.Add("UnSubscribed");
                frm.button1.Text = @"Subscribe To TimeServer";
            }

            //Select last item on listbox
            frm.listBox1.SelectedIndex = frm.listBox1.Items.Count - 1;
        }
    }

}

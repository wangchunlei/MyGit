using System;
using System.Windows.Forms;
using PokeIn.Caching;
using System.IO;
namespace Cache_Sample
{
    public partial class Form1 : Form
    {
        internal string cacheFile;
        PCache _cache;

        //RESTORE
        public Form1()
        {
            cacheFile = Application.StartupPath + "\\MyObj.bin";
            InitializeComponent();

            _cache = new PCache();
            if (File.Exists(cacheFile))
            {
                FileStream fs = new FileStream(cacheFile, FileMode.Open, FileAccess.Read, FileShare.None);
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, (Int32)fs.Length);
                fs.Close();
                
                //below simple method converts saved byte data into cache object
                if(array.Length>0)
                    _cache.RestoreCache(array, true);
            } 

            CleanView(true);
        }


        //ADD  / UPDATE
        private void Button1Click(object sender, EventArgs e)
        {
            if ( txtName.Text.Trim().Length == 0 )
            {
                MessageBox.Show("Enter a name!");
                return;
            } 

            _cache[txtName.Text] = new Records(txtName.Text, txtAddr.Text, txtTel.Text, dtBirth.Value);
            CleanView(true);  
        }

        void CleanView(bool refreshList)
        {
            txtName.Text = "";
            txtTel.Text = "";
            txtAddr.Text = "";
            dtBirth.Value = DateTime.Now;

            if (refreshList)
            {
                lstNames.Items.Clear();
                string[] names = _cache.GetKeys();
                foreach (string name in names)
                    lstNames.Items.Add(name);
            }
        }

        //BACKUP
        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            FileStream fs = new FileStream(cacheFile, FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] array;
            //below simple methods converts all the cache objects into byte array
            _cache.BackupCache(out array, true);
            fs.Write(array, 0, array.Length);
            fs.Close(); 
        }

        private void LstNamesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstNames.SelectedIndex < 0)
                return;
            Records record = (Records) _cache[lstNames.SelectedItem.ToString()];

            txtName.Text = record.Name;
            txtAddr.Text = record.Address;
            txtTel.Text = record.Tel;
            dtBirth.Value = record.Birth;
        }

        private void Button3Click(object sender, EventArgs e)
        {
            CleanView(false);
        }

        //DELETE RECORD
        private void Button2Click(object sender, EventArgs e)
        {
            if (lstNames.SelectedIndex < 0)
                return;

            _cache.Remove(lstNames.SelectedItem.ToString()); 
            CleanView(true);
        }
    }
}

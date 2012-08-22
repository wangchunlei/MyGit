using System;
using System.Collections.Generic;
using System.Text;

namespace Cache_Sample
{
    [Serializable]
    class Records
    {
        public string Name = String.Empty,
                      Address = String.Empty,
                      Tel = String.Empty;
        public DateTime Birth = DateTime.MinValue;

        public Records()
        {
        }

        public Records(string name, string address, string tel, DateTime birth)
        {
            Name = name;
            Tel = tel;
            Address = address;
            Birth = birth;
        }
    }
}

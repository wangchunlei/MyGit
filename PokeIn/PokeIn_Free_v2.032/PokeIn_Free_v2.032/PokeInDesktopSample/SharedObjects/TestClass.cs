using System;
using System.Collections.Generic;

namespace SharedObjects
{
    //We send this test class from desktop to server (vice versa) to show you the "remote method" capabilities of PokeIn

    [Serializable]
    public class TestClass
    {
        public int number = 0;
        public string text = "";
        public List<string> items = new List<string>();
        public TestClass()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            number = rnd.Next(50, 100);
            text = rnd.Next(0, 1000).ToString() + " text test";
            for (int i = 0; i < number; i++)
                items.Add(i.ToString());
        }
    }
}

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SocialMedia.Test
{
    [TestClass]
    public class ProfileTest
    {
        [TestMethod]
        public void PostDate()
        {

            var TenSceondsAgo = DateTime.Now.Ticks;
            var date = new DateTime(TenSceondsAgo);
            Thread.Sleep(1000);
            
            var input = FormatDate(date);

            var expected = 1 + " Seconds Ago";
            
            Assert.AreEqual(input[0], expected);
        }




       



 
        public string[] FormatDate(DateTime? inputDate)
        {
            DateTime? currentTime = DateTime.Now;

            var daysAgo = currentTime - inputDate;

            string[] output = new string[4];


            if (daysAgo.Value.Minutes == 0)
            {
                output[0] = daysAgo.Value.Seconds + " Seconds Ago";
                output[1] = inputDate.Value.ToLongDateString();
                output[2] = inputDate.Value.ToShortTimeString();
                output[3] = daysAgo.Value.Seconds + "s";
            }
            else if (daysAgo.Value.Hours == 0)
            {
                output[0] = daysAgo.Value.Minutes + " Minutes Ago";
                output[1] = inputDate.Value.ToLongDateString();
                output[2] = inputDate.Value.ToShortTimeString();
                output[3] = daysAgo.Value.Minutes + "m";
            }
            else if (daysAgo.Value.Days == 0)
            {
                output[0] = daysAgo.Value.Hours + " Hours Ago";
                output[1] = inputDate.Value.ToLongDateString();
                output[2] = inputDate.Value.ToShortTimeString();
                output[3] = daysAgo.Value.Hours + "h";
            }
            else
            {
                output[0] = daysAgo.Value.Days + " Days Ago";
                output[1] = inputDate.Value.ToLongDateString();
                output[2] = inputDate.Value.ToShortTimeString();
                output[3] = daysAgo.Value.Hours + "d";
            }


            return output;

        }
    }
}

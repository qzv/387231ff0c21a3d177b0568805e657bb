using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace steam
{
    /* 
    ~ moschino, premium smart quantum computing level steam username checker that returns very useful information
    by me the j

     credits: 
     valve
     steam re
     rezonans65
    */

    class Program
    {
        private static moschino moschino;
        private static Config.Settings Settings;

        static void Main(string[] args)
        {
            Settings = new Config.Settings()
            {
                requestLimit = 1
            };

            //pull up
            moschino = new moschino(Settings);
          
            while (true)
            {
                Console.Title = string.Format("hack the planet, Accounts Found: {0}", moschino.AccountsFound);
                Thread.Sleep(100);
            }
        }
    }
}

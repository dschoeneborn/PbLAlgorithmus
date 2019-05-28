using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbLAlgorithmus
{
    class Program
    {
        public static List<Modul> Modules = new List<Modul>();

        static void Main(string[] args)
        {
            Modules.Add(new Modul("1",
                new LED("1", LedPosition.Up, LedStatus.Phase1, LedStatus.Phase1, LedStatus.Off),
                new LED("1", LedPosition.Down)));
            try
            {
                Modules.First().AddFlashLight(Color.YELLOW);
            }
            catch(Exception ex)
            {

            }

            Modules.First().AddFlashLight(Color.WHITE);

            try
            {
                Modules.First().AddFlashLight(Color.RED);
            }
            catch (Exception ex)
            {

            }

            Modules.First().RemoveFlashLight(Color.WHITE);
        }

    }
}

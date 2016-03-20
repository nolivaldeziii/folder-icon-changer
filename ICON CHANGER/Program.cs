using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICON_CHANGER
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ICON CHANGER V1.0.2\n");
            Console.WriteLine("NOTE: this program deletes the desktop.ini files of specific folders!\n");
            Console.WriteLine("This program changes the folder's icon and its subfolders\n" +
                "Make sure that the .ico file mathes the name of the folder \nand is inside their respective folder\n"+
                "\nIF the proccess failed, please maunally delete the desktop.ini  file\n"+
                "\nPress Any Key To Continue...\n");
            Console.ReadKey();

            IconChanger change = new IconChanger(".\\");

            Console.WriteLine("\nPress Any Key To Continue...");
            Console.ReadKey();
        }
    }
}

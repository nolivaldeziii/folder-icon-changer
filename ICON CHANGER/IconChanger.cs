using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ICON_CHANGER
{
    public delegate void FolderVisitEvent(string location);
    public struct Setting
    {
        public bool HideIcoFile;
        public bool IncludeSubfolder;
        public bool IcoInFolder;

        public string Extension;
        public string IcoSourceFolder;
    }
    public struct Data
    {
        public Queue<string> CandidateFolderName;
        public Queue<string> CandidateFolderFullName;
        public Queue<string> CandidateIcoFullName;

        public Queue<string> CheckedFolders;

        public Queue<string> SpecificFolder;
        public Queue<string> SpecificIco;

        public Queue<string> FailedProccess;
    }

    class FolderChecker
    {
        public FolderVisitEvent Visited;

        public FolderChecker(FolderVisitEvent newEvents)
        {
            Visited = newEvents;
        }

        public void VisitAllSubfolder(string Root)
        {
            string[] Folders = System.IO.Directory.GetDirectories(Root);
            //do delegate
            Visited(Root);
            /////////////
            if (Folders.Length != 0)    //General Condition 
                for (int i = 0; i < Folders.Length; i++)
                    VisitAllSubfolder(Folders[i]);
        }
 
    }

    class Finalizer
    {
        string currentLoc;
        string ICOlocation;
        public Finalizer(string WorkingLocation, string IcoLocation)
        {
            currentLoc = WorkingLocation;
            ICOlocation = IcoLocation;
        }
        public string DesktopINI()
        {
                System.IO.StreamWriter IniWrite = new System.IO.StreamWriter(currentLoc + "\\desktop.ini");
                IniWrite.WriteLine("[.ShellClassInfo]");
                IniWrite.WriteLine("IconFile=" + ICOlocation);
                IniWrite.WriteLine("IconIndex=0");
                IniWrite.WriteLine("InfoTip=TryCatch");
                IniWrite.Close();

                SetIniFileAttributes(currentLoc + "\\desktop.ini");
                SetFolderAttributes(currentLoc);
                return "Success!";
        }

        public bool SolveError()
        {
            int retryCount = 0;
            resolve:
            System.Diagnostics.Process.Start("CMD.exe", string.Format("/C attrib -r -a -s -h {0}\\desktop.ini", currentLoc));
            System.Diagnostics.Process.Start("CMD.exe", string.Format("/C del {0}\\desktop.ini", currentLoc));
            try
            {
                File.Delete(currentLoc + "\\desktop.ini");
                return true;
            }
            catch (Exception)
            {
                if (retryCount < 3)
                {
                    retryCount++;
                    goto resolve;
                }
                else
                {       
                    data.FailedProccess.Enqueue(currentLoc);
                    return false;
                }
            }    
        }

        public void HideIco()
        {
            SetIniFileAttributes(ICOlocation);
        }

        private bool SetFolderAttributes(string FolderPath)
        {


            // Set folder attribute to "System"
            if ((File.GetAttributes(FolderPath) & FileAttributes.System)
                   != FileAttributes.System)
            {
                File.SetAttributes(FolderPath, File.GetAttributes
                                  (FolderPath) | FileAttributes.System);
            }

            return true;

        }
        private bool SetIniFileAttributes(string IniPath)
        {

            // Set ini file attribute to "Hidden"
            if ((File.GetAttributes(IniPath) & FileAttributes.Hidden)
                   != FileAttributes.Hidden)
            {
                File.SetAttributes(IniPath, File.GetAttributes(IniPath)
                                   | FileAttributes.Hidden);
            }

            // Set ini file attribute to "System"
            if ((File.GetAttributes(IniPath) & FileAttributes.System)
                   != FileAttributes.System)
            {
                File.SetAttributes(IniPath, File.GetAttributes(IniPath)
                                    | FileAttributes.System);
            }

            return true;

        }
    }

    class DataGatherer
    {
        Data newData = new Data();
        Setting Settings;
        public DataGatherer(Setting set)
        {
            Settings = set;
            newData.CandidateFolderFullName = new LinkedList<string>();
            newData.CandidateFolderName = new LinkedList<string>();
            newData.CandidateIcoFullName = new LinkedList<string>();
            newData.CheckedFolders = new Queue<string>();
            newData.FailedProccess = new Queue<string>();
            newData.SpecificFolder = new Queue<string>();
            newData.SpecificIco = new Queue<string>();
        }
        public void getCandidate(string currentLoc)
        {
            System.IO.DirectoryInfo getICOname = new System.IO.DirectoryInfo(currentLoc);
            int LastIndexofSlashInDir = getICOname.FullName.LastIndexOf('\\') + 1;
            string NameOfCurrentFolder = getICOname.FullName.Remove(0, LastIndexofSlashInDir);
            System.IO.FileInfo[] icoINFO = getICOname.GetFiles(Settings.Extension);

            for (int i = 0; i < icoINFO.Length; i++)
            {
                //IF This Folder Has ICO in it
                if (NameOfCurrentFolder == icoINFO[i].ToString().Remove(icoINFO[i].ToString().LastIndexOf('.'), 4))
                {
                    newData.CandidateFolderName.AddLast(NameOfCurrentFolder);
                    newData.CandidateFolderFullName.AddLast(getICOname.FullName);
                    newData.CandidateIcoFullName.AddLast(getICOname.FullName + "\\" + icoINFO[i]);
                }
            }
        }
        
    }
    class IconChanger
    {
        string RootFolder;
        Setting Settings = new Setting();
        Data newData = new Data();
        Queue<string> FailedProccess = new Queue<string>();


        public IconChanger(string RootF) 
        {
            RootFolder = RootF;
            VisitAllSubFolders(RootF);
            Settings.Extension = "*.ico";
            printFail();
        }
        void VisitAllSubFolders(string Sub)
        {

        }
        void writeINI(string ICOlocation, string currentLoc)
        {
            
        }
        void GetReady()
        {
            DataGatherer gatherNewData = new DataGatherer(Settings);
            FolderVisitEvent newEvent = new FolderVisitEvent(gatherNewData.getCandidate);
            FolderChecker CheckFolders = new FolderChecker(newEvent);
            CheckFolders.VisitAllSubfolder(RootFolder);
        }
        void changeICON()
        {
            GetReady();
            Finalizer FProccess;
            for (int i = 0; i < newData.CandidateFolderName.Count; i++)
            {
                FProccess = new Finalizer(newData.CandidateFolderFullName.Dequeue, newData.CandidateIcoFullName.Dequeue);
            }

            for (int i = 0; i < icoINFO.Length; i++)
            {               
                if (NameOfCurrentFolder == icoINFO[i].ToString().Remove(icoINFO[i].ToString().LastIndexOf('.'), 4))
                {
                    Console.WriteLine(string.Format("changing icon for {0}", NameOfCurrentFolder));
                    string ICOlocation = getICOname.FullName + "\\" + icoINFO[i];
                    
                        try
                        {
                            writeINI(ICOlocation, Root);
                        }
                        catch (UnauthorizedAccessException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Resolving...");
                            int retryCount = 0;
                            resolve:
                            System.Diagnostics.Process.Start("CMD.exe", string.Format("/C attrib -r -a -s -h {0}\\desktop.ini", Root));
                            System.Diagnostics.Process.Start("CMD.exe", string.Format("/C del {0}\\desktop.ini", Root));
                            try
                            {
                                
                                File.Delete(Root + "\\desktop.ini");
                                writeINI(ICOlocation, Root);
                                
                            }
                            catch (Exception f)
                            {
                                if(retryCount==0)
                                    Console.WriteLine(f.Message);
                                Console.WriteLine("Retrying...");
                                if (retryCount < 3)
                                {
                                    retryCount++;
                                    goto resolve;
                                }
                                else
                                {
                                    Console.WriteLine("Failed :(");
                                    FailedProccess.Enqueue(getICOname.FullName);
                                    break;
                                }
                            }                     
                        }
                    break;
                }
            }   
        }
        void printFail()
        {
            if (FailedProccess.Count > 0)
            {
                Console.WriteLine("\nCannot Change the icon for the following folder(s):");
                while (FailedProccess.Count > 0)
                {
                    Console.WriteLine(FailedProccess.Dequeue());
                }
            }
        }

       
    }

}

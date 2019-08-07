using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MNApp.Lib
{
    public class EMailExtract
    {
        private string currentDir;

        public string CurrentDir
        {
            get
            {
                if (String.IsNullOrEmpty(currentDir))   CurrentDir = Directory.GetCurrentDirectory();
                
                return currentDir;
            }
            set { currentDir = value; }
        }


        private string inputDir;

        public string InputDir
        {
            get
            {
                if (String.IsNullOrEmpty(inputDir))
                {
                    InputDir = $"{CurrentDir}/Input";
                }
                return inputDir;
            }
            set { inputDir = value; }
        }

        public bool InputDirExists
        {
            get
            {                
                return Directory.Exists(InputDir);
            }
        }

        public string[] Files
        {
            get
            {                
                return Directory.GetFiles(InputDir, "*.txt", SearchOption.TopDirectoryOnly);
            }
        }
        
        public List<FileInfo> FileDetails
        {
            get
            {
                List<FileInfo> fileInfos = new List<FileInfo>();
                foreach(string file in Files)
                {
                    fileInfos.Add(new FileInfo(file));
                }
                return fileInfos;
            }
        }
    }
}

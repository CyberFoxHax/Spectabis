﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spectabis_WPF.Domain
{
    //Used only for commandline argument launching

    class LaunchPCSX2
    {
        public static void LaunchGame(string game)
        {
            string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string gamePath = $"{BaseDirectory}resources\\configs\\{game}";
            
            var _gameIni = new IniFile(gamePath + @"\spectabis.ini");
            var _isoDir = _gameIni.Read("isoDirectory", "Spectabis");

            //Launch arguments
            var _nogui = _gameIni.Read("nogui", "Spectabis");
            var _fullscreen = _gameIni.Read("fullscreen", "Spectabis");
            var _fullboot = _gameIni.Read("fullboot", "Spectabis");
            var _nohacks = _gameIni.Read("nohacks", "Spectabis");

            string _launchargs = "";

            if (_nogui == "1") { _launchargs = "--nogui "; }
            if (_fullscreen == "1") { _launchargs = _launchargs + "--fullscreen "; }
            if (_fullboot == "1") { _launchargs = _launchargs + "--fullboot "; }
            if (_nohacks == "1") { _launchargs = _launchargs + "--nohacks "; }

            Console.WriteLine($"{_launchargs} {_isoDir} --cfgpath={gamePath}");

            //Paths in PCSX2 command arguments have to be in quotes...
            const string quote = "\"";

            Process PCSX = new Process();

            //PCSX2 Process
            if(File.Exists(Properties.Settings.Default.emuDir))
            {
                PCSX.StartInfo.FileName = Properties.Settings.Default.emuDir;
                PCSX.StartInfo.Arguments = $"{_launchargs} {quote}{_isoDir}{quote} --cfgpath={quote}{gamePath}{quote}";

                PCSX.Start();

                //Elevate Process
                PCSX.PriorityClass = ProcessPriorityClass.AboveNormal;

                Application.Current.Shutdown();
            }
            else
            {
                Console.WriteLine(Properties.Settings.Default.emuDir + " does not exist!");
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech;
using System.Speech.Synthesis;
using System.IO;
using System.Threading;
using System.Reflection;

namespace TTM
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            try
            {
                if(args[0] == "/getvoices")
                {
                    SpeechSynthesizer temp = new SpeechSynthesizer();
                    string output = "Installed voices: \n";
                    foreach (InstalledVoice voice in temp.GetInstalledVoices())
                    {
                        output = output + voice.VoiceInfo.Name + "\n";
                    }
                    File.WriteAllText(folderpath + "\\voices.txt", output);
                    Environment.Exit(0);
                }
            }
            catch (Exception) {}
            //speak all lines out loud
            Dictionary<string, SpeechSynthesizer> voices = new Dictionary<string, SpeechSynthesizer>();
            string[] fileinput = File.ReadLines(folderpath + "\\script.txt").ToArray<string>();
            bool defining = false;
            foreach (string command in fileinput)
            {
                string[] temparr;
                string line;
                if (command.Contains("//"))
                {
                    continue;
                }
                else if (command == "[define]")
                {
                    defining = true;
                }
                else if(defining & command.Contains("="))
                {
                    temparr = command.Split('=');
                    voices.Add(temparr[0], new SpeechSynthesizer());
                    voices[temparr[0]].SelectVoice(temparr[1]);
                    voices[temparr[0]].Rate = 1;
                    voices[temparr[0]].Volume = 100;
                }
                else if (command.Contains("sleep|"))
                {
                    temparr = command.Split('|');
                    Thread.Sleep(int.Parse(temparr[1]));
                }
                else if (command.Contains("|"))
                {
                    temparr = command.Split('|');
                    voices[temparr[0]].Speak(temparr[1]);
                }
            }
        }
    }
}
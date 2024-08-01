using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Infrastructure.FFMPEG
{
    public class FFMPEG
    {
        #region Properties
        private string _ffmpegExecutableLocation;
        public string FfmpegExecutableLocation
        {
            get
            {
                return _ffmpegExecutableLocation;
            }
            set
            {
                _ffmpegExecutableLocation = value;
            }
        }
        #endregion

        #region Constructors
        public FFMPEG()
        {
            Initialize();
        }
        public FFMPEG(string ffmpegExePath)
        {
            _ffmpegExecutableLocation = ffmpegExePath;
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            //first make sure we have a value for the ffexe file setting
            if (string.IsNullOrEmpty(_ffmpegExecutableLocation))
            {
                object ffmpegExecutableLocation = Settings.Current.FFMPEGLocation;

                if (ffmpegExecutableLocation == null)
                {
                    throw new Exception(
                        "Could not find the location of the ffmpeg exe file.  The path for ffmpeg.exe " +
                        "can be passed in via a constructor of the ffmpeg class (this class) or by setting in the app.config or web.config file.  " +
                        "in the appsettings section, the correct property name is: ffmpeg:ExeLocation"
                    );
                }
                else
                {
                    if (string.IsNullOrEmpty(ffmpegExecutableLocation.ToString()))
                    {
                        throw new Exception("No value was found in the app setting for ffmpeg:ExeLocation");
                    }

                    _ffmpegExecutableLocation = ffmpegExecutableLocation.ToString();
                }
            }

            if (!File.Exists(_ffmpegExecutableLocation))
            {
                throw new Exception("Could not find a copy of ffmpeg.exe");
            }
        }
        #endregion

        #region Run the process
        public string RunCommand(string Parameters, DataReceivedEventHandler action)
        {
            //create a process info
            ProcessStartInfo processStartInfo = new ProcessStartInfo(_ffmpegExecutableLocation, Parameters)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            //Create the output and streamreader to get the output
            string output = null;
            StreamReader processStandardErrorStreamReader = null;
            //try the process
            try
            {
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                processStartInfo.WorkingDirectory = _ffmpegExecutableLocation.Replace("ffmpeg.exe", "");
                process.OutputDataReceived += action;
                process.Start();

                process.WaitForExit();

                //get the output
                processStandardErrorStreamReader = process.StandardError;

                //now put it in a string
                output = processStandardErrorStreamReader.ReadToEnd();

                process.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                //now, if we succeded, close out the streamreader
                if (processStandardErrorStreamReader != null)
                {
                    processStandardErrorStreamReader.Close();
                    processStandardErrorStreamReader.Dispose();
                }
            }
            return output;
        }

        public string RunMultipleCommand(string parameters, DataReceivedEventHandler action)
        {
            //create a process info
            //System.Diagnostics.Process.Start("CMD.EXE", $"/C chcp 65001&CD E:\\FFMPEG\\bin&ffmpeg.exe {parameters}");

            //return string.Empty;
            var multipleCommands = $"/C chcp 65001&CD E:\\FFMPEG\\bin&ffmpeg.exe {parameters}";

            ProcessStartInfo processStartInfo = new ProcessStartInfo("CMD.EXE", multipleCommands)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            //Create the output and streamreader to get the output
            string output = null;
            StreamReader processStandardErrorStreamReader = null;
            //try the process
            try
            {
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                //processStartInfo.WorkingDirectory = _ffmpegExecutableLocation.Replace("ffmpeg.exe", "");
                process.OutputDataReceived += action;
                process.Start();

                process.WaitForExit();

                //get the output
                processStandardErrorStreamReader = process.StandardError;

                //now put it in a string
                output = processStandardErrorStreamReader.ReadToEnd();

                process.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                //now, if we succeded, close out the streamreader
                if (processStandardErrorStreamReader != null)
                {
                    processStandardErrorStreamReader.Close();
                    processStandardErrorStreamReader.Dispose();
                }
            }
            return output;
        }

        #endregion

    }

}

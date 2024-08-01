using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Infrastructure.FFMPEG
{
    public class FrapperWrapper
    {
        /// <summary>
        /// Gets or sets the frapper.
        /// </summary>
        /// <value>
        /// The frapper.
        /// </value>
        private FFMPEG Frapper
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrapperWrapper"/> class.
        /// </summary>
        /// <param name="frapper">The frapper.</param>
        public FrapperWrapper(FFMPEG frapper)
        {
            Frapper = frapper;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public string ExecuteCommand(string command, DataReceivedEventHandler action)
        {
            return Frapper.RunCommand(command, action);
        }

        /// <summary>
        /// Excute Multiple Command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string ExecuteMultipleCommand(string command, DataReceivedEventHandler action)
        {
            return Frapper.RunMultipleCommand(command, action);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.UX.ErrorList
{
    public class ErrorListDataEntry
    {
        public string Description { get; set; }
        public ErrorListLevel Level
        {
            get { return _level; }
            set
            {
                _level = value;
                switch (_level)
                {
                    case ErrorListLevel.Error:
                        this.ErrorIconSrc = ErrorIconRelPath;
                        break;
                    case ErrorListLevel.Warning:
                        this.ErrorIconSrc = WarningIconRelPath;
                        break;
                    case ErrorListLevel.Information:
                        this.ErrorIconSrc = InformationIconRelPath;
                        break;
                }
            }
        }

        public string ErrorIconSrc { get; private set; }

        private ErrorListLevel _level = ErrorListLevel.Error;

        private const string ErrorIconRelPath = "./Resources/Images/Error.png";
        private const string WarningIconRelPath = "./Resources/Images/Warning.png";
        private const string InformationIconRelPath = "./Resources/Images/Information.png";
    }
}

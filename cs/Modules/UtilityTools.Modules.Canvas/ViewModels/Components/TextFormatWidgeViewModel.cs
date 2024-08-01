using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using UtilityTools.Core.Mvvm;
using DelegateCommand = UtilityTools.Core.Mvvm.DelegateCommand;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class TextFormatWidgeViewModel : BaseUXItemDescription
    {

        //original text
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        private string result;
        public string FormatResult
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                RaisePropertyChanged("FormatResult");
            }
        }

        public ICommand FormatCommand => new DelegateCommand((obj) =>
        {

            var sb = new StringBuilder();
            var lines = Text.Split('\r');
            sb.Append(lines[0]);
            foreach(var line in lines.Skip(1))
            {
                var newLine = line.Replace("\n","");
                if (string.IsNullOrWhiteSpace(newLine))
                    continue;

                if(newLine.StartsWith("A:") || newLine.StartsWith("B:"))
                {
                    sb.Append("\r\n"+ newLine);
                }
                else
                {
                    sb.Append(" "+newLine);
                }
            }

            FormatResult = sb.ToString();
        });

        public ICommand RemoveLetterCommand => new DelegateCommand((obj) =>
        {
            FormatResult = FormatResult.Replace("A:", "").Replace("B:", "");
        });


        public TextFormatWidgeViewModel()
        {

        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.UX.ErrorList;
using UtilityTools.Services.Interfaces;

namespace UtilityTools.Core.Mvvm
{
    public abstract class BaseUXItemDescription : ViewModelBase, IBaseUXItemDescription
    {
        protected TaskStage taskstage;
        protected bool isWaiting;
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public MessageLevel MessageLevel { get; set; }
        public MessageOwner MessageOwner { get; set; }

        protected virtual void OnStageChanged(TaskStage TaskStage) { }

        public bool IsWaiting
        {
            get
            {
                return isWaiting;
            }
            set
            {
                isWaiting = value;
                RaisePropertyChanged("IsWaiting");
            }
        }

        /// <summary>
        /// log
        /// </summary>
        public ObservableCollection<ErrorListDataEntry> MessageListData
        {
            get; set;
        } = new ObservableCollection<ErrorListDataEntry>();



        /// <summary>
        /// Gets or sets a enum value indicating task stage
        /// </summary>
        public TaskStage TaskStage
        {
            get
            {
                return taskstage;
            }
            set
            {
                taskstage = value;
                OnStageChanged(taskstage);
                RaisePropertyChanged("TaskStage");

               
                if (taskstage == TaskStage.Done)
                {

                    //NativeWindows.PlaySound(@"", 0, NativeWindows.SND_ASYNC | NativeWindows.SND_FILENAME);
                    //SoundPlayer p = new SoundPlayer();
                    //p.SoundLocation = Application.StartupPath + "//Sounds/download-complete.wav";
                    //p.Load();
                    //p.Play();

                }
            }
        }


    }
}

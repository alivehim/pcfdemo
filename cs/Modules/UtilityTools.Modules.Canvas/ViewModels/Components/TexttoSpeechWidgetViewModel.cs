using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Polly;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class TexttoSpeechWidgetViewModel : BaseUXItemDescription
    {
        static string Key = "d844adc5de4b4e9895fd52453f2b4753";
        static string Key2 = "6863155d1c864f4fbf54b8f81aaf352e";
        static string Location = "eastus";

        public ObservableCollection<OneNoteItemViewModel> NoteBookItems { get; set; } = new ObservableCollection<OneNoteItemViewModel>();
        public ObservableCollection<OneNotePageViewModel> OnenoteSectionPages { get; set; } = new ObservableCollection<OneNotePageViewModel>();

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

        private string rawText;
        public string RawText
        {
            get
            {
                return rawText;
            }
            set
            {
                rawText = value;
                RaisePropertyChanged("RawText");
            }
        }

        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                RaisePropertyChanged("Content");
            }
        }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private readonly IGraphOnenoteService graphOnenoteService;

        public TexttoSpeechWidgetViewModel(IGraphOnenoteService graphOnenoteService)
        {
            this.graphOnenoteService = graphOnenoteService;
        }

        public ICommand ClearCommand => new DelegateCommand((obj) =>
        {
            Title = string.Empty;
            Text = string.Empty;
        });

        public ICommand PlayCommand => new DelegateCommand(async (obj) =>
        {
            await ConvertTextToSpeech(Text);
        });

        public ICommand SaveCommand => new DelegateCommand((obj) =>
       {
           IsWaiting = true;
           Task.Run(async () =>
           {
               if (!string.IsNullOrEmpty(Settings.Current.TTSFolder) && !string.IsNullOrEmpty(Title))
               {
                   string fn = $"{Path.Combine(Settings.Current.TTSFolder, Title)}.wav";
                   await ConvertTextToAudioFile(Text, fn);
               }

           }).ContinueWith(res =>
           {
               IsWaiting = false;
           }, TaskScheduler.FromCurrentSynchronizationContext());

       });

        public ICommand OpenTTSMakerCommand => new DelegateCommand((obj) =>
        {
            ToolsContext.Current.PublishEvent<ChangeBrowserAddressEvent, BrowserChange>(new BrowserChange
            {
                Url = "https://ttsmaker.com"
                //Url = "https://ttsmaker.cn"
            });
        });

        public ICommand TestCommand => new DelegateCommand((obj) =>
        {

            Content = File.ReadAllText(@"D:\\test.txt");

            Console.WriteLine(RawText);

            Content = RawText;
        });

        /// <summary>
        /// load onenote books
        /// </summary>
        public ICommand LoadNoteCommand => new DelegateCommand((obj) =>
          {
              IsWaiting = true;
              NoteBookItems.Clear();


              Task.Run(async () =>
              {
                  var result = await graphOnenoteService.GetOnenoteBooksAsync();
                  return result;
              }).ContinueWith((res) =>
              {
                  IsWaiting = false;

                  NoteBookItems.AddRange(res.Result.Select(p => new OneNoteItemViewModel
                  {

                      Name = p.displayName,
                      Url = p.sectionsUrl
                  }));

              }, TaskScheduler.FromCurrentSynchronizationContext());


          });

        public ICommand SelectSectionsCommand => new DelegateCommand((obj) =>
        {
            var dialogService = ToolsContext.Current.ResolveService<IDialogService>();


            dialogService.ShowDialog("CanvasOnenoteSectionDialog", new DialogParameters($"message=Onenote"), r =>
            {
                if (r.Result == ButtonResult.OK)
                {

                    IsWaiting = true;
                    var pagesUrl = r.Parameters.GetValue<string>("pagesUrl");
                    var sectionName = r.Parameters.GetValue<string>("sectionName");

                    Task.Run(async () =>
                    {
                        var graphOnenoteService = ToolsContext.Current.ResolveService<IGraphOnenoteService>();

                        var pages = await graphOnenoteService.GetOnenotePagesAsync(pagesUrl);

                        return pages;


                    }).ContinueWith((res) =>
                   {
                       if (res.Exception != null)
                       {
                           IsWaiting = false;
                           return;
                       }

                       OnenoteSectionPages.Clear();

                       OnenoteSectionPages.AddRange(res.Result.value.Select(p => new OneNotePageViewModel
                       {
                           Name = p.title,
                           ContentUrl = p.contentUrl
                       }));

                       IsWaiting = false;

                   }, TaskScheduler.FromCurrentSynchronizationContext());


                }

            });
        });

        public ICommand OpenPageCommand => new DelegateCommand((obj) =>
        {
            var item = obj as OneNotePageViewModel;

            IsWaiting = true;

            Task.Run(async () =>
            {
                var graphOnenoteService = ToolsContext.Current.ResolveService<IGraphOnenoteService>();

                var content = await graphOnenoteService.GetOnenoteContentAsync(item.ContentUrl);

                return content;


            }).ContinueWith((res) =>
           {
               if (res.Exception != null)
               {
                   IsWaiting = false;
                   return;
               }

               try
               {
                   Content = res.Result;

                   //Console.WriteLine(RawText);
                   //var dispatcher = Application.Current.Dispatcher;
                   //await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                   //{
                   //    Content = res.Result;

                   //    Console.WriteLine(RawText);

                   //    Text = RawText;

                   //}));




                   ToolsContext.Current.PublishEvent<ChangeBrowserAddressEvent, BrowserChange>(new BrowserChange
                   {
                       Url = "https://ttsmaker.com",
                       Parameter = RawText,
                       Parameter2 = item.Name
                   });

               }
               catch
               {

               }
               finally
               {
                   IsWaiting = false;
               }





           }, TaskScheduler.FromCurrentSynchronizationContext());


        });

        public ICommand OpenSectionsCommand => new DelegateCommand((obj) =>
        {

            var dialogService = ToolsContext.Current.ResolveService<IDialogService>();


            dialogService.ShowDialog("CanvasOnenoteSectionDialog", new DialogParameters($"message=Onenote"), r =>
            {
                if (r.Result == ButtonResult.OK)
                {

                    IsWaiting = true;
                    var pagesUrl = r.Parameters.GetValue<string>("pagesUrl");
                    var sectionName = r.Parameters.GetValue<string>("sectionName");

                    Task.Run(async () =>
                    {
                        //get Content
                        //var cloudResource = ToolsContext.Current.ResolveService<IContentCrawlingServiceFactory>();
                        //var onenoteServiceFactory = ToolsContext.Current.ResolveService<IOnenoteServiceFactory>();
                        var graphOnenoteService = ToolsContext.Current.ResolveService<IGraphOnenoteService>();
                        //var graphOnenoteService = onenoteServiceFactory.GetHandler(OnenoteSource.MicrosoftGraph);

                        //var extractor = cloudResource.GetContentExtractor(Url);
                        //var content = await extractor.GetContentAsync(Url);
                        ////write page

                        //await graphOnenoteService.CreatePageAsync(pagesUrl, Name, content);




                        //item.Value.Reverse();

                        var result = new List<PageItem>();

                        var pages = await graphOnenoteService.GetOnenotePagesAsync(pagesUrl);


                        var nextLink = pages?.nextLink;
                        while (!string.IsNullOrEmpty(nextLink))
                        {
                            var tmpresult = await graphOnenoteService.GetOnenotePagesAsync(nextLink);
                            result.AddRange(tmpresult.value);
                            nextLink = tmpresult?.nextLink;
                        }

                        return result;


                    }).ContinueWith(async (res) =>
                    {
                        if (res.Exception != null)
                        {
                            IsWaiting = false;
                            return;
                        }

                        var folder = @$"{Settings.Current.TTSFolder}/{sectionName}";
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(@$"{Settings.Current.TTSFolder}/{sectionName}");
                        }

                        res.Result.Reverse();

                        foreach (var page in res.Result)
                        {
                            await Process(page, folder);
                        }

                        IsWaiting = false;

                    }, TaskScheduler.FromCurrentSynchronizationContext());


                }

            });

        });

        public ICommand TTSCommand => new DelegateCommand((obj) =>
       {
           Task.Run(async () =>
           {

               IsWaiting = true;
               var book = obj as OneNoteItemViewModel;

               var dic = new Dictionary<string, List<PageItem>>();

               var result = await graphOnenoteService.GetOnenoteSectionsAsync(book.Url);

               foreach (var item in result)
               {
                   dic.Add(item.displayName, new List<PageItem>());
                   var pages = await graphOnenoteService.GetOnenotePagesAsync(item.pagesUrl);


                   foreach (var page in pages.value)
                   {
                       dic[item.displayName].Add(page);
                   }
               }

               return dic;



           }).ContinueWith(async (res) =>
           {
               if (res.Exception != null)
               {
                   IsWaiting = false;
                   return;
               }

               foreach (var item in res.Result)
               {
                   var folder = @$"{Settings.Current.TTSFolder}/{item.Key}";
                   if (!Directory.Exists(folder))
                   {
                       Directory.CreateDirectory(@$"{Settings.Current.TTSFolder}/{item.Key}");
                   }

                   //item.Value.Reverse();
                   foreach (var page in item.Value)
                   {
                       await Process(page, folder);
                   }
               }

               IsWaiting = false;
           });


       });



        private async Task Process(PageItem page, string folder)
        {
            Title = page.title.Replace(" - Training | Microsoft Learn", "")
                .Replace(":", " ")
                .Replace("-", " ");
            string filePath = $"{Path.Combine(folder, Title)}.wav";

            if (!File.Exists(filePath))
            {

                try
                {

                    var pageContent = await graphOnenoteService.GetOnenoteContentAsync(page.contentUrl);

                    var dispatcher = Application.Current.Dispatcher;
                    await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        Content = pageContent;

                        Console.WriteLine(RawText);

                        Text = RawText;

                    }));


                    await Task.Delay(5000);

                    try
                    {
                        var pollyContext = new Context("Retry 503");
                        var policy = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(
                                2,
                                _ => TimeSpan.FromMilliseconds(15500),
                                (result, timespan, retryNo, context) =>
                                {
                                    ToolsContext.Current.PostMessage($"{context.OperationKey}: Retry number {retryNo} within " +
                                        $"{timespan.TotalMilliseconds}ms. Original status code: 503");
                                }
                            );

                        var response = await policy.ExecuteAsync(async ctx =>
                        {
                            var result = await ConvertTextToAudioFile(Text, filePath);
                            if (!result)
                            {
                                throw new Exception("retry");
                            }
                            return result;
                        }, pollyContext);
                    }
                    catch
                    {

                    }

                }
                catch
                {

                }


            }
            else
            {
                ToolsContext.Current.PostMessage($"skip {Title}");
            }
        }



        public async Task ConvertTextToSpeech(string text)
        {

            var confg = SpeechConfig.FromSubscription(Key, Location);

            using (var converter = new SpeechSynthesizer(confg))
            {
                using (var r = await converter.SpeakTextAsync(text))
                {
                    if (r.Reason ==
                    ResultReason.SynthesizingAudioCompleted)
                        Console.WriteLine($"Speech converted " + Title +
                        $"to speaker for text [{text}]");
                    else if (r.Reason == ResultReason.Canceled)
                    {
                        var canc =
                       SpeechSynthesisCancellationDetails.FromResult(r);
                        Console.WriteLine($"CANCELED: " +
                         $"Reason={canc.Reason}");
                        if (canc.Reason ==
                        CancellationReason.Error)
                        {
                            Console.WriteLine($"Cancelled with " +
                            $"Error Code {canc.ErrorCode}");
                            Console.WriteLine($"Cancelled with " +
                            $"Error Details " +
                           $"[{canc.ErrorDetails}]");
                        }
                    }
                }
            }
        }

        public async Task<bool> ConvertTextToAudioFile(string text, string filePath)
        {
            var config = SpeechConfig.FromSubscription(Key, Location);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter wr = new BinaryWriter(fs))
            {
                wr.Write(
                System.Text.Encoding.ASCII.GetBytes("RIFF"));
            }

            using (var fw = AudioConfig.FromWavFileOutput(filePath))
            using (var ss = new SpeechSynthesizer(config, fw))
                return await Conversion(text, ss);
        }

        private async Task<bool> Conversion(string text, SpeechSynthesizer synthesizer)
        {
            using (var r = await synthesizer.SpeakTextAsync(text))
            {
                if (r.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    ToolsContext.Current.PostMessage($"Speech converted " + Title + $"to speaker for text succeed.");

                    return true;
                }
                else if (r.Reason == ResultReason.Canceled)
                {
                    var cancellation =
                    SpeechSynthesisCancellationDetails.FromResult(r);
                    ToolsContext.Current.PostMessage($"CANCELED: " +
                    $"Reason={cancellation.Reason}");
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        ToolsContext.Current.PostMessage($"Cancelled with " +
                        $"Error Code {cancellation.ErrorCode}");
                        ToolsContext.Current.PostMessage($"Cancelled with " +
                        $"Error Details " +
                        $"[{cancellation.ErrorDetails}]");
                    }

                }
                return false;
            }
        }

    }
}

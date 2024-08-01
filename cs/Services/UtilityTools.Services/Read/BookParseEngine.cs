using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models.Read;
using UtilityTools.Core.Utilites;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Read;

namespace UtilityTools.Services.Read
{
    public class BookParseEngine : IBookParseEngine
    {
        //章节索引
        private BookInfo _book;
        public BookInfo CurrentBook
        {
            get
            {
                return _book;
            }
        }

        public string FileName { get; private set; }

        private IList<PageInfo> RootPages { get; set; } = new List<PageInfo>();

        #region private method

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string LoadBookContent(string path,out string fileName)
        {
            var content= FileHelper.GetFileContent(path,out string name);
            fileName = name;
            return content;
        }

        public async Task Load(string fullName, IList<ProfileImage> keywordList, int width, int height, int lineHeight, int fontsize)
        {
           var fileContent = LoadBookContent(fullName,out string name);
            FileName = name;
            GetBook(fileContent, keywordList);
 
           await LoadChapters(width, height, lineHeight, fontsize);
        }

        private bool GetBook(string bookdetail, IList<ProfileImage> keywordList)
        {
            if (keywordList != null && keywordList.Count != 0)
            {

                // 是否存在大于4个字的关键词
                var keys = (
                            from n in keywordList
                            where n.Key.Length > 4
                            select n.Key).ToList();

                _book = TextParser.GetBook(bookdetail, out Dictionary<string, string> dics, keys);

                if (keys != null)
                {
                    var originKeys = 
                                     from n in keywordList
                                     where n.Key.Length > 4
                                     select n;


                    foreach (var item in originKeys)
                    {
                        item.FrontName = dics[item.Key];
                    }


                    return true;
                }
            }
            else
            {
                _book = TextParser.GetBook(bookdetail, out Dictionary<string, string> dics);
            }

            return false;

        }

        private async Task LoadChapters(int width, int height, int lineHeight, int fontsize)
        {
            if (CurrentBook.Chapters.Count == 0)
                return;


            //清空所有页
            RootPages.Clear();

            //计算出每页中 每行多少字，每页多少行
            var countData = DeviceHelper.GetCounts(width, height, lineHeight, fontsize);

            int NumItemsRead = 0;

            await CurrentBook.Chapters.ParallelForEachAsync(  (item) =>
            {
                TextParser.GetChapterPage(item, countData.lineCountOfPage, countData.countOfLine);

                Interlocked.Increment(ref NumItemsRead);
                double ProgressReported = (NumItemsRead * 100.0 / CurrentBook.Chapters.Count);
            });


        }



        #endregion
    }
}

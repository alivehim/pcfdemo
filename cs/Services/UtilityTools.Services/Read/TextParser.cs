using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.Read;

namespace UtilityTools.Services.Read
{
    public class TextParser
    {
        static Regex chapterRegex = new Regex(@"(?:^\s*|^\s*第.*?)(第[^\s,.，。]*?[章篇]\s?.*)");

        static int ReplaceIndex = 10;
        static string prefix = "";

        public static string ReplaceContentByKeys(string content, Dictionary<string, string> replaceDic = null)
        {
            if (replaceDic == null || replaceDic.Count == 0)
                return content;
            else
            {
                foreach (var item in replaceDic)
                {
                    content = content.Replace(item.Key, item.Value);
                }
                return content;
            }
        }

        public static string PrepareReplaceKey(string key)
        {
            var repName = ReplaceIndex++.NumberToChinese();
            return $"{prefix}{repName}";
        }

        public static Dictionary<string, string> PrepareReplaceKeys(IList<string> replaceNames = null)
        {
            if (replaceNames != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var key in replaceNames)
                {
                    var repName = ReplaceIndex++.NumberToChinese();
                    dic.Add(key, $"{prefix}{repName}");
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 章节定位 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static BookInfo GetBook(string content, out Dictionary<string, string> dics, IList<string> replaceNames = null)
        {
            ReplaceIndex = 10;
            BookInfo result = new BookInfo();
            int chapterindex = 0;
            Chapter beforeItem = new Chapter() { ChapterNo = chapterindex++ };
            dics = PrepareReplaceKeys(replaceNames);

            StringBuilder chapterContent = new StringBuilder();
            if (!string.IsNullOrEmpty(content))
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        try
                        {
                            string line = null;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrEmpty(line))
                                    continue;

                                var match = chapterRegex.Match(line);

                                if (match.Success)
                                {
                                    //发现新章节
                                    Chapter nitem = new Chapter() { ChapterNo = chapterindex++ };
                                    nitem.Title = match.Value;


                                    //上一章节内容
                                    beforeItem.Context = chapterContent.ToString();
                                    if (!string.IsNullOrEmpty(beforeItem.Context))
                                    {
                                        result.Chapters.Add(beforeItem);
                                    }
                                    //清空chapterContent
                                    chapterContent.Clear();

                                    beforeItem = nitem;
                                    chapterContent.Append(match.Value + "\r\n");
                                }
                                else
                                {
                                    if (dics != null && dics.Count > 0)
                                    {
                                        var newline = ReplaceContentByKeys(line, dics);
                                        chapterContent.Append(newline + "\r\n");
                                    }
                                    else
                                    {
                                        chapterContent.Append(line + "\r\n");
                                    }

                                }
                            }

                            //章节内容
                            beforeItem.Context = chapterContent.ToString();
                            //清空chapterContent
                            chapterContent.Clear();
                        }
                        catch (NullReferenceException ex)
                        {
                            ToolsContext.Current.PostMessage(ex.ToString());
                        }
                    }
                }
            }
            if (result.Chapters.Count == 0)
            {
                result.Chapters.Add(beforeItem);
            }

            else
            {
                result.Chapters.Add(beforeItem);
            }
            return result;
        }

        /// <summary>
        /// 根据章节内容获取页
        /// </summary>
        /// <param name="str">章节内容</param>
        /// <param name="lineNum">每行多少字</param>
        /// <param name="wordPerLine">每页多少行</param>
        /// <returns></returns>
        public static IList<PageInfo> GetChapterPages(string str, int lineNum, int wordPerLine)
        {
            IList<PageInfo> pages = new List<PageInfo>();
            str = str.Replace(",", "，")
                .Replace(".", "。");

            // 章节多少页有多少行
            var lines = Regex.Split(Regex.Replace(str, "\r", ""), @"[\n]").Where(p => !string.IsNullOrEmpty(p)).ToList();
            int lineLength = lines.Count;
            int start = 0;
            PageInfo page = new PageInfo();
            while (start < lineLength)
            {
                var content = lines[start];
                if (!content.StartsWith("　　"))
                {
                    content = "　　" + content.Replace(" ", "");
                }
                var lineRows = GetRowsEx(content, lineNum, wordPerLine);
                if (page.Row.Count + lineRows.Count >= lineNum)
                {
                    int leftnum = lineNum - page.Row.Count;
                    page.Row.AddRange(lineRows.Take(leftnum));
                    pages.Add(page);
                    page = new PageInfo();
                    //如果余下的行数还是大于page行数

                    var pageNextCount = lineRows.Count - leftnum;
                    if (pageNextCount <= lineNum)
                    {
                        page.Row.AddRange(lineRows.Skip(leftnum));
                    }
                    else
                    {
                        var xxleftLines = lineRows.Skip(leftnum);
                        var pageNumber = pageNextCount % lineNum;
                        for (int i = 0; i < pageNumber; i++)
                        {
                            page = new PageInfo();
                            pages.Add(page);
                            page.Row.AddRange(xxleftLines.Skip(i * lineNum).Take(lineNum));
                        }
                    }
                }
                else
                {
                    page.Row.AddRange(lineRows);
                }
                start++;
            }

            //检查是否是空白

            var cnt = page.Row.Count(p => string.IsNullOrEmpty(p));
            if (cnt != page.Row.Count)
            {
                pages.Add(page);
            }


            return pages;
        }

        private static IList<string> GetRowsEx(string lineContent, int lineNum, int wordPerLine)
        {
            IList<string> rows = new List<string>();

            int lineWidth = (wordPerLine * 2);
            int num = 1;   //当前行数
            float currentByteCount = 0;  //当前行所占字节数
            int totalCharNum = 0;  //总字符长度
            int length = 0;  //当前游标长度
            int start = 0;
            int startIndex = start;
            byte[] totalBytes = Encoding.GetEncoding("utf-8").GetBytes(lineContent);

            int c = 0;
            //int w = 0;
            int l = 0;
            while (c < totalBytes.Length - 1)
            {
                int b = totalBytes[c] & 0xff;

                if (b > 0x7f)
                { //汉字
                    var offset = lineWidth - currentByteCount;
                    if (offset >= 3.0f)
                    {
                        currentByteCount += 3.0f;
                    }
                    else if (Math.Abs(3.0f - offset) <= 0.5)
                    {
                        currentByteCount += 3.0f;
                    }
                    else
                    {
                        num++;
                        rows.Add(lineContent.Substring(startIndex, length));
                        startIndex = rows.Sum(p => p.Count());
                        length = 0;

                        currentByteCount = 3.0f;
                    }
                    c += 3;
                    l += 3;
                }
                else
                { //英文
                    var offset = lineWidth - currentByteCount;
                    if (offset >= 1.5f)
                    {
                        currentByteCount += 1.5f;
                    }
                    else if (Math.Abs(1.5f - offset) <= 0.5)
                    {
                        currentByteCount += 1.5f;
                    }
                    else
                    {
                        num++;
                        rows.Add(lineContent.Substring(startIndex, length));
                        startIndex = length;
                        length = 0;

                        currentByteCount = 1.5f;
                    }
                    c += 2;
                    l += 2;
                }
                totalCharNum++;
                length++;
            }



            if (startIndex != start)
            {
                rows.Add(lineContent.Substring(startIndex));
            }
            if (rows.Count == 0)
            {
                rows.Add(lineContent);
            }
            return rows;
        }


        public static void GetChapterPage(Chapter chapter, int lineCountOfPage, int wordCountOfLine)
        {
            //byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(chapter.Context);//得到字节数组
            var items = GetChapterPages(chapter.Context, lineCountOfPage, wordCountOfLine);
            chapter.Pages.AddRange(items);
        }
    }
}

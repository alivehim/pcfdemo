using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    [MessageOwner(MessageOwner.WebresourceManager)]
    public class WebresourceExtractDescriptor : BaseSimpleExtractDescriptor<FileDataDescriptor>
    //public class WebresourceExtractDescriptor : ISimpleExtractDescriptor<FileDataDescriptor> 
    {
        //public override IExtractResult<FileDataDescriptor> Run(ITaskContext taskContext)
        //{
        //    var allowedExtensions = new[] { ".html", ".js", ".css", ".txt" };
        //    List<FileDataDescriptor> files = new List<FileDataDescriptor>();
        //    void AddSubDirectory(DirectoryInfo subFolder)
        //    {
        //        foreach (FileInfo c in subFolder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
        //        {
        //            files.Add(new FileDataDescriptor { FileName = c.Name, FullName = c.FullName });
        //        }
        //    }

        //    var folder = new DirectoryInfo(taskContext.Key);
        //    foreach (DirectoryInfo NextFolder in folder.GetDirectories())
        //    {
        //        AddSubDirectory(NextFolder);


        //        foreach (var subFolder in NextFolder.GetDirectories())
        //        {
        //            AddSubDirectory(subFolder);
        //        }
        //        //var innerFolder = new DirectoryInfo(NextFolder.FullName);


        //        //foreach (FileInfo c in innerFolder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
        //        //{
        //        //    files.Add(new FileDataDescriptor { FileName = c.Name, FullName = c.FullName });
        //        //}
        //    }

        //    foreach (FileInfo NextFile in folder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
        //    {
        //        files.Add(new FileDataDescriptor { FileName = NextFile.Name, FullName = NextFile.FullName });
        //    }

        //    return Result(files);
        //}

        public override Task RunAsync(ITaskContext taskContext)
        {

            var allowedExtensions = new[] { ".html", ".js", ".css", ".txt" };
            List<FileDataDescriptor> files = new List<FileDataDescriptor>();
            void AddSubDirectory(DirectoryInfo subFolder)
            {
                foreach (FileInfo c in subFolder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
                {
                    files.Add(new FileDataDescriptor { FileName = c.Name, FullName = c.FullName });
                }
            }

            var folder = new DirectoryInfo(taskContext.Key);
            foreach (DirectoryInfo NextFolder in folder.GetDirectories())
            {
                AddSubDirectory(NextFolder);


                foreach (var subFolder in NextFolder.GetDirectories())
                {
                    AddSubDirectory(subFolder);
                }
                //var innerFolder = new DirectoryInfo(NextFolder.FullName);


                //foreach (FileInfo c in innerFolder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
                //{
                //    files.Add(new FileDataDescriptor { FileName = c.Name, FullName = c.FullName });
                //}
            }

            foreach (FileInfo NextFile in folder.GetFiles().Where(p => allowedExtensions.Contains(p.Extension)))
            {
                files.Add(new FileDataDescriptor { FileName = NextFile.Name, FullName = NextFile.FullName });
            }

            return Task.FromResult(Result(files));

        }
    }
}

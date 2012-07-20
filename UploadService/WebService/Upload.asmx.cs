using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Lanxum.Domas.Extension.LogManager;
using System.IO;
using System.Web.Hosting;

namespace WebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Upload : System.Web.Services.WebService
    {
        private ILogger logger = null;

        [WebMethod(Description = "获取文件大小")]
        public long GetFileSize(string sourceFile)
        {
            try
            {
                string sourcefile = GetTargetFilePath(sourceFile);
                using (var fs = new FileStream(sourcefile, FileMode.Open, FileAccess.Read))
                {
                    return fs.Length;
                }
            }
            catch
            {
                return 0;
            }
        }

        protected virtual string GetUploadFolder()
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
            if (string.IsNullOrEmpty(folder))
                folder = "Upload";
            if (Directory.Exists(folder) == true)
            {

            }
            else
            {
                //根目录，相对位置；
                folder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(@HostingEnvironment.ApplicationPhysicalPath), "BackUp");
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    logger = LogManager.GetLogger("创建备份目录");
                    logger.Error("创建备份出错:" + ex.ToString());
                }
            }
            return folder;
        }

        protected string GetTempFilePath(string fileName)
        {
            return Path.Combine(GetUploadFolder(), fileName);
        }

        protected string GetTargetFilePath(string fileName)
        {
            return Path.Combine(GetUploadFolder(), fileName);
        }

        private void SaveFileByByte(byte[] FileByte, FileStream fs)
        {
            fs.Write(FileByte, 0, FileByte.Length);
        }

        [WebMethod(Description = "上传文件")]
        public bool UploadFileBybyte(string fileName, byte[] fileByte, bool isFirst, bool isLast)
        {
            logger = LogManager.GetLogger("uploadfileByByte");
            bool result = false;
            string _tempExtension = "_temp";
            try
            {
                string uploadFolder = GetUploadFolder();
                string tempFileName = fileName + _tempExtension;
                string tempPath = GetTempFilePath(tempFileName);
                string targetPath = GetTargetFilePath(fileName);
                if (isFirst)
                {
                    logger.Info(fileName + " 第一块数据文件到达!");
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);
                }
                using (FileStream fs = File.Open(tempPath, FileMode.Append))
                {
                    logger.Info("已经上传文件长度" + fs.Length);
                    SaveFileByByte(fileByte, fs);
                    fs.Close();
                    result = true;
                }
                if (isLast)
                {
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);
                    logger.Info(fileName + " 最后一块数据文件到达!");
                    File.Move(tempPath, targetPath);
                    result = true;
                    logger.Info(fileName + " 文件传输成功!");
                    //Finish stuff....
                    // FinishedFileUpload(_fileName, _parameters);
                }
            }
            catch (Exception e)
            {
                logger.Error("上传" + fileName + "出错：" + e.ToString());
                result = false;
                throw;
            }
            finally
            {
            }
            return result;
        }

        [WebMethod(Description = "下载文件")]
        public byte[] DownFile(string fileName, long startPosition, long chunkSize)
        {
            logger = LogManager.GetLogger("DownFile");
            string sourceFile = GetTargetFilePath(fileName);
            logger.Info("开始下载文件" + sourceFile + "开始位置" + startPosition);
            byte[] filebyte = new byte[chunkSize];
            using (var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                fs.Position = startPosition;
                fs.Read(filebyte, 0, filebyte.Length);
                if (startPosition + chunkSize >= fs.Length)
                {
                    logger.Info("下载文件" + sourceFile + "完成");
                }
            }
            return filebyte;
        }

        [WebMethod(Description = "删除服务器文件")]
        public void DelelteFileByFileName(string fileName)
        {
            logger = LogManager.GetLogger("DelelteFileByFileName");
            try
            {
                File.Delete(GetTargetFilePath(fileName));
            }
            catch (Exception ex)
            {
                logger.Error("删除文件" + fileName + "出错：" + ex.ToString());
            }
        }


    }
}
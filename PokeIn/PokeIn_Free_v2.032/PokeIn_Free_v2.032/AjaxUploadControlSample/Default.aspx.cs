using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using PokeIn;
using PokeIn.Comet;

namespace AjaxUploadControlSample
{
    public class Dummy:IDisposable
    {
        internal string ClientID;
        internal string FilePath;
        public Dummy(string clientId, string filePath)
        {
            ClientID = clientId;
            FilePath = filePath;
        }

        public void Dispose()
        {
            //Not Implemented
        } 

        public void DeleteFilesInFolder()
        {
            int fileCount = 0;
            try
            {
                string[] files = System.IO.Directory.GetFiles(FilePath+ "\\Upload");
                fileCount = files.Length;
                foreach (string file in files)
                    System.IO.File.Delete(file);
            }
            catch(Exception e)
            {
                CometWorker.SendToClient(ClientID, JSON.Method("Error", e.Message));
            }
            finally
            {
                CometWorker.SendToClient(ClientID, JSON.Method("FilesDeleted", fileCount));
            }
        }

        static Dummy()
        {
            CometWorker.FileUploadRequested += new OnFileUploadRequest(CometWorker_FileUploadRequested);
        }

        static void CometWorker_FileUploadRequested(ref HttpFile file)
        {
            string fileName = file.ServerMapPath + "\\Upload\\" + file.FileName;
            file.SaveAs(fileName);
            string resourceName = file.FileName.Replace("&", "_");
            ResourceManager.AddReplaceResource(fileName, resourceName, ResourceType.Image, file.ClientId);
            CometWorker.SendToClient(file.ClientId, JSON.Method("ShowImage", resourceName));
        }
    }
    public class _Default : Page
    {
        static _Default()
        {
            CometWorker.OnClientConnected += new DefineClassObjects(CometWorker_OnClientConnected);
        }

        static void CometWorker_OnClientConnected(ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("Dummy", new Dummy(details.ClientId, HttpContext.Current.Server.MapPath("")));
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        } 
    }
}

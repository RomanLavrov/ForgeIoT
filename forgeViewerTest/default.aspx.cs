using Autodesk.Forge;
using Autodesk.Forge.Client;
using Autodesk.Forge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace forgeViewerTest
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void Upload_Click(object sender, EventArgs e)
        {
            // create a randomg bucket name (fixed prefix + randomg guid)
            string bucketKey = "forgeapp" + Guid.NewGuid().ToString("N").ToLower();

            // upload the file (to your server)
            string fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), bucketKey, FileUpload.FileName);
           // LargeFileUpload(FileUpload, Page, fileSavePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fileSavePath));
            FileUpload.SaveAs(fileSavePath);

            // get a write enabled token
            TwoLeggedApi oauthApi = new TwoLeggedApi();
            dynamic bearer = await oauthApi.AuthenticateAsync(
                WebConfigurationManager.AppSettings["FORGE_CLIENT_ID"],
                WebConfigurationManager.AppSettings["FORGE_CLIENT_SECRET"],
                "client_credentials",
                new Scope[] { Scope.BucketCreate, Scope.DataCreate, Scope.DataWrite, Scope.DataRead });

            // create the Forge bucket
            PostBucketsPayload postBucket = new PostBucketsPayload(bucketKey, null, PostBucketsPayload.PolicyKeyEnum.Transient /* erase after 24h*/ );
            BucketsApi bucketsApi = new BucketsApi();
            bucketsApi.Configuration.AccessToken = bearer.access_token;
            dynamic newBucket = await bucketsApi.CreateBucketAsync(postBucket);

            // upload file (a.k.a. Objects)
            ObjectsApi objectsApi = new ObjectsApi();
            oauthApi.Configuration.AccessToken = bearer.access_token;
            dynamic newObject;
            //using (StreamReader fileStream = new StreamReader(fileSavePath))
            //{
            //    newObject = await objectsApi.UploadObjectAsync(bucketKey, FileUpload.FileName, (int)fileStream.BaseStream.Length, fileStream.BaseStream,
            //        "application/octet-stream");               
            //}

            //--------------------------------------------------------------------------------------
            long fileSize = FileUpload.PostedFile.ContentLength;
            long chunkSize = 2 * 1024 * 1024;
            string sessionId = RandomString(12);
            long chunkQuantity = (long)Math.Round(0.5 + (double)fileSize / (double)chunkSize);
           

            ApiResponse<dynamic> response = null;

            using (FileStream streamReader = new FileStream(fileSavePath, FileMode.Open))
            {               
                for (int i = 0; i < chunkQuantity; i++)
                {                    
                    long start = i * chunkSize;
                    long end = Math.Min(fileSize, (i + 1) * chunkSize) - 1;

                    string range = "bytes " + start + "-" + end + "/" + fileSize;
                    long length = end - start + 1;

                    byte[] buffer = new byte[length];
                    MemoryStream memoryStream = new MemoryStream(buffer);

                    int parts = streamReader.Read(buffer, 0, (int)length);
                    memoryStream.Write(buffer, 0, parts);
                    memoryStream.Position = 0;
                    
                    
                    response = await objectsApi.UploadChunkAsyncWithHttpInfo(bucketKey, FileUpload.FileName, (int)length, range, sessionId, memoryStream, "application/octet-stream");
                    //System.IO.File.WriteAllText("data/" + bucketKey + "." + FileUpload.FileName + ".json", response.Data.ToString() as string);
                    MessageBox.Show(Page, response.StatusCode.ToString());                    
                }

                //MessageBox.Show(Page, "BusketKey=" + bucketKey + "; FileSize=" + fileSize + "; ChunkQuantity=" + chunkQuantity + "; SessionId=" + sessionId + "; StreamReaderLength=" + streamReader.Length);
            }
            newObject = response.Data;
            //---------------------------------------------------------------------------------------

            // translate file
            string objectIdBase64 = ToBase64(newObject.objectId);
            List<JobPayloadItem> postTranslationOutput = new List<JobPayloadItem>()
                {
                    new JobPayloadItem(
                    JobPayloadItem.TypeEnum.Svf /* Viewer*/,
                    new List<JobPayloadItem.ViewsEnum>()
                    {
                       JobPayloadItem.ViewsEnum._3d,
                       JobPayloadItem.ViewsEnum._2d
                    })
                };

            JobPayload postTranslation = new JobPayload(
                new JobPayloadInput(objectIdBase64),
                new JobPayloadOutput(postTranslationOutput));
            DerivativesApi derivativeApi = new DerivativesApi();
            derivativeApi.Configuration.AccessToken = bearer.access_token;
            dynamic translation = await derivativeApi.TranslateAsync(postTranslation);

            // check if is ready
            int progress = 0;
            do
            {
                System.Threading.Thread.Sleep(1000); // wait 1 second
                try
                {
                    dynamic manifest = await derivativeApi.GetManifestAsync(objectIdBase64);
                    progress = (string.IsNullOrWhiteSpace(Regex.Match(manifest.progress, @"\d+").Value) ? 100 : Int32.Parse(Regex.Match(manifest.progress, @"\d+").Value));
                }
                catch (Exception ex)
                {

                }
            } while (progress < 100);

            // ready!!!!

            // register a client-side script to show this model
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowModel", string.Format("<script>showModel('{0}');</script>", objectIdBase64));
            Page.ClientScript.RegisterStartupScript(this.GetType(), "InitApplication", string.Format("<script>initApplication('{0}');</script>", objectIdBase64));


            // clean up
            Directory.Delete(Path.GetDirectoryName(fileSavePath), true);
        }

        public string ToBase64(string input)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string RandomString(int size)
        {
            string array = "abcdefghijklmnopqrstuvwxyz";
            string generated = String.Empty;
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {

                var pos = random.Next(0, array.Length);
                generated += array[pos];
            }
            return generated;
        }        
    }

    public static class MessageBox
    {
        public static void Show(this Page Page, String Message)
        {
            Page.ClientScript.RegisterStartupScript(
               Page.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }
    }
}
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleCalendar.Credentials.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace GoogleCalendar.Credentials.Helper
{
    public class GoogleDriveHelper
    {
        private static readonly string[] _scopes =
        {
            "https://www.googleapis.com/auth/drive",
            "https://www.googleapis.com/auth/drive.appdata",
            "https://www.googleapis.com/auth/drive.file",
            "https://www.googleapis.com/auth/drive.metadata"
        };

        public GoogleDriveHelper() 
        {
        }


        // ************************* service section *******************************
        // Create a service to use google drive (v3):
        public async static Task<Google.Apis.Drive.v3.DriveService> GetServiceV3()
        {
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials", "credentials_drive.json"), FileMode.Open, FileAccess.Read))
            {
                string credentialsPath = "token_drive.json";

#pragma warning disable CS0618 // Type or member is obsolete
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialsPath, true)
                );
#pragma warning restore CS0618 // Type or member is obsolete
            }

            Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "google_drive_v3"
            });

            return service;
        }

        // Create a service to use google drive (v2):
        public async static Task<Google.Apis.Drive.v2.DriveService> GetServiceV2()
        {
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "credentials", "credentials_drive.json"), FileMode.Open, FileAccess.Read))
            {
                string credentialsPath = "token_drive.json";

#pragma warning disable CS0618 // Type or member is obsolete
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credentialsPath, true)
                );
#pragma warning restore CS0618 // Type or member is obsolete
            }

            Google.Apis.Drive.v2.DriveService service = new Google.Apis.Drive.v2.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "google_drive_v2",
            });

            return service;
        }


        // ************************* gets section *********************************
        public async static Task<List<GoogleDriveFile>> GetFolderContent(String folderId)
        {
            List<string> ChildList = new();
            Google.Apis.Drive.v2.DriveService serviceV2 = await GetServiceV2(); 
            ChildrenResource.ListRequest request = serviceV2.Children.List(folderId);

            do
            {
                try
                {
                    ChildList children = request.Execute();

                    if (children.Items != null && children.Items.Count > 0)
                    {
                        foreach (var file in children.Items)
                        {
                            ChildList.Add(file.Id);
                        }
                    }

                    request.PageToken = children.NextPageToken;
                }
                catch (Exception)
                {
                    throw;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));


            //Get All File List
            List<GoogleDriveFile> AllFileList = await GetAllDriveFiles();
            List<GoogleDriveFile> Filter_FileList = new();

            foreach (string Id in ChildList)
            {
                Filter_FileList.Add(AllFileList.Where(x => x.Id == Id).FirstOrDefault()!);
            }

            return Filter_FileList;
        }

        public async static Task<List<GoogleDriveFile>> GetAllDriveFiles()
        {
            Google.Apis.Drive.v3.DriveService service = await GetServiceV3();

            // Define parameters of request.
            Google.Apis.Drive.v3.FilesResource.ListRequest FileListRequest = service.Files.List();
            FileListRequest.Fields = "nextPageToken, files(createdTime, id, name, size, version, trashed, parents)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<GoogleDriveFile> FileList = new();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new()
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents
                    };

                    FileList.Add(File);
                }
            }

            return FileList;
        }

        public async static Task<bool> IsFileInFolder(UploadInFolder data)
        {
            try
            {
                Google.Apis.Drive.v2.DriveService serviceV2 = await GetServiceV2();
                var res = serviceV2.Children.Get(data.FolderId, data.FileId).Execute();

                if (res == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                throw;
            }
        }


        // ************************* posts section *********************************
        /* When you want to upload a file in the specific folder, you must specify the correct 
         * folder id in the parent property of the Google Drive file. */
        public async static Task<string> CreateFolderOnDrive(string Folder_Name)
        {
            try
            {
                Google.Apis.Drive.v3.DriveService service = await GetServiceV3();
                Google.Apis.Drive.v3.Data.File FileMetaData = new()
                {
                    Name = Folder_Name,
                    MimeType = "application/vnd.google-apps.folder"
                };
                Google.Apis.Drive.v3.FilesResource.CreateRequest request;

                request = service.Files.Create(FileMetaData);
                request.Fields = "id";
                var file = request.Execute();

                if(file.Id == null)
                {
                    throw new Exception("Ocurrio un error a la hora de crear un directorio en google drive.");
                }

                return $"Directorio {Folder_Name} creado exitosamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async static Task<string> FileUploadInFolder(string folderId, IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    Google.Apis.Drive.v3.DriveService service = await GetServiceV3();
                    string contentType;
                    new FileExtensionContentTypeProvider().TryGetContentType(Path.GetFileName(file.FileName), out contentType!);
                    var FileMetaData = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = Path.GetFileName(file.FileName),
                        MimeType = contentType,
                        Parents = new List<string> { folderId }
                    };

                    Google.Apis.Drive.v3.FilesResource.CreateMediaUpload request;
                    byte[] byteArray = System.IO.File.ReadAllBytes(Path.GetFileName(file.FileName));// File's content.
                    MemoryStream stream = new(byteArray);

                    request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                    request.Fields = "id";
                    request.Upload();

                    var response = request.ResponseBody;

                    if (response.Id == null)
                    {
                        throw new Exception("Ocurrio un error a la hora de agregar un archivo en google drive.");
                    }

                    return $"Directorio {Path.GetFileName(file.FileName)} creado exitosamente";
                }
                else
                {
                    throw new Exception("Ocurrio un error a la hora de agregar un archivo en google drive.");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}      


using Google.Apis.Drive.v2;
using GoogleCalendar.Credentials.Helper.GoogleDrive.credentials;
using Google.Apis.Drive.v2.Data;
using File = Google.Apis.Drive.v2.Data.File;
using GoogleServices.Credentials.Models.GoogleDrive;

namespace GoogleCalendar.Credentials.Helper.GoogleDrive.services
{
    public class GoogleDriveGetsHelper : GoogleDriveHelper
    {
        public async static Task<List<GoogleDriveFile>> GetFolderContent(string folderId)
        {
            List<string> ChildList = new();
            DriveService serviceV2 = await GetServiceV2();
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
            } while (!string.IsNullOrEmpty(request.PageToken));


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

            try
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<dynamic> GetFileById(string fileId)
        {
            DriveService service = await GetServiceV2();

            try
            {
                File file = await service.Files.Get(fileId).ExecuteAsync();

                if (file == null)
                {
                    throw new Exception("No existe el archivo.");
                }

                return file;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async static Task<bool> IsFileInFolder(UploadInFolder data)
        {
            if (data.FolderId == null) return false;
            if (data.FileId == null) return false;

            try
            {
                DriveService serviceV2 = await GetServiceV2();
                var res = await serviceV2.Children.Get(data.FolderId, data.FileId).ExecuteAsync();

                if (res == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return false;
            }
        }
    }
}

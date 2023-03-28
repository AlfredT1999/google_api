using GoogleCalendar.Credentials.Helper.GoogleDrive.credentials;
using Microsoft.AspNetCore.StaticFiles;

namespace GoogleCalendar.Credentials.Helper.GoogleDrive.services
{
    public class GoogleDrivePostsHelper : GoogleDriveHelper
    {
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

                if (file.Id == null)
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

        public async static Task<IList<string>> DriveMoveFileToFolder(string fileId, string folderId)
        {
            try
            {
                Google.Apis.Drive.v3.DriveService service = await GetServiceV3();

                // Retrieve the existing parents to remove
                var getRequest = service.Files.Get(fileId);
                getRequest.Fields = "parents";
                var file = getRequest.Execute();
                var previousParents = String.Join(",", file.Parents);

                // Move the file to the new folder
                var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);

                updateRequest.Fields = "id, parents";
                updateRequest.AddParents = folderId;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();

                return file.Parents;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

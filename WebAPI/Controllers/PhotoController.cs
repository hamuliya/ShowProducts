
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;
using System.Net;
using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;


namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]

public class PhotoController : ControllerBase
{
    private readonly IWebHostEnvironment? _environment;
    public PhotoController(IWebHostEnvironment? environment)
    {
        _environment = environment;
    }

    private string[] permittedExtensions = { ".jpg", ".jpeg" };

    private bool CheckFileValidity(string uploadedFileName)
    {
        bool result = true;
        var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            result = false;
        }
        return result;

    }

    [HttpPost("UploadPhoto")]
    public async Task<IResult> UploadPhoto([FromForm] PhotoAPIModel objFile)
    {

        PhotoAPIModel obj = new();
        try
        {
            obj.PhotoID = objFile.PhotoID;
            string? fileName =objFile?.Photo?.FileName;
            bool validFile = false;

            
            if (fileName is not null)
            {        
                int FileNameLength = fileName.Length;
                validFile= CheckFileValidity(fileName);
               
            }

            if (objFile?.Photo?.Length > 0 && validFile)
            {

                //string DirPath= $"{_environment?.WebRootPath}\\Upload\\{obj.PhotoID}\\";
                string dirPath = $"{_environment?.ContentRootPath}\\Photos\\{obj.PhotoID}\\";


               // string ext = Path.GetExtension(fileName).ToLowerInvariant();
                string filePath = Path.Combine(dirPath,Path.GetRandomFileName());
                


                if (!Directory.Exists(dirPath ))
                {
                    Directory.CreateDirectory(dirPath);
                }

                await using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await objFile.Photo.CopyToAsync(filestream);
                    filestream.Flush();
           
                }
            }
        }
        catch (Exception ex)
        {
            return Results.Problem( ex.Message);
        }
        return Results.Ok();
    }



    [HttpGet("DeletePhoto/{id}/{PhotoName}")]
    public Task<IResult> DeletePhoto(int id, string PhotoName)
    {
        
        string dirPath = $"{_environment?.ContentRootPath}\\Photos\\{id}\\";

        string photopath = dirPath + $"\\{PhotoName}"+".jpg";
        try
        {
            if (System.IO.File.Exists(photopath))
            {
                System.IO.File.Delete(photopath);
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult(Results.Problem(ex.Message));
        }

        return Task.FromResult(Results.Ok());
    }

    [HttpGet("UpdateFolder/{id}")]
    public Task<IResult> UpdateFolder(int id)
    {

        string dirPath = $"{_environment?.ContentRootPath}\\Photos\\0\\";
        try
        {
            if (System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.Move("0",id.ToString());
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult(Results.Problem(ex.Message));
        }

        return Task.FromResult(Results.Ok());
    }



    [HttpGet("GetPhotos/{id}")]

    public Task<IResult> GetPhotos(int id)
    {

        try
        {
            string dirPath = $"{_environment?.ContentRootPath}\\Photos\\{id}\\";
            List<string> photos = new();
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                string fileName = file.Name;
              
                photos.Add(fileName);
            }
            return Task.FromResult(Results.Ok(photos));

        }
        catch (Exception ex)
        {
            return Task.FromResult(Results.Problem(ex.Message));
        }

        }


    [HttpGet("GetPhoto/{id}/{PhotoName}")]
    public ActionResult GetPhoto(int id,string  PhotoName)
    {
        try
        {
            string filePath = $"{_environment?.ContentRootPath}\\Photos\\{id}\\{PhotoName}";

            if (filePath != null)
            {
               
                var image = System.IO.File.OpenRead(filePath);
                return  File(image, "image/jpeg");
            }
        }
        catch
        {
            throw;
        }
        return NotFound();
    }

}


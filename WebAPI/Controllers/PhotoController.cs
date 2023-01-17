
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;

using WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;


namespace WebAPI.Controllers;




[Route("[controller]")]
[ApiController]

public class PhotoController : ControllerBase
{
    private readonly IWebHostEnvironment? _environment;
    private readonly ILogger<PhotoController> _logger;

    public PhotoController(IWebHostEnvironment? environment, ILogger<PhotoController> logger)
    {
        _environment = environment;
        _logger = logger;
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


    [HttpDelete("photos/{id}/{photoName}")]
    public async Task<IActionResult> DeletePhoto(int id, string photoName)
    {
        // Build the file path using the environment's ContentRootPath and the provided id and photoName

        string filePath = Path.Combine(_environment?.ContentRootPath ?? "", "Photos", id.ToString(), $"{photoName}.jpg");
        

        try
        {
            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Delete the file
                System.IO.File.Delete(filePath);
                return Ok();
            }
            else
            {
                _logger.LogError( "Error from PhotoController=>DeletePhoto");
                // Return a 404 Not Found if the file does not exist
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            // Return a 500 Internal Server Error if there is an exception
            _logger.LogError(ex, "Error from PhotoController=>DeletePhoto");
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("UpdateFolder/{id}")]
    public async Task<IResult> UpdateFolder(int id)
    {
        try
        {
            // Validate the input
            if (id <= 0)
                return Results.Problem("Invalid folder id");

            // Construct the directory path
            var dirPath = Path.Combine(_environment?.ContentRootPath ?? "", "Photos", "0");
            var newDirPath = Path.Combine(_environment?.ContentRootPath ?? "", "Photos", id.ToString());

            if (!System.IO.Directory.Exists(dirPath))
                return Results.Problem($"Source folder {dirPath} does not exist");

            if (System.IO.Directory.Exists(newDirPath))
                return Results.Problem($"Target folder {newDirPath} already exists");

            // Move the folder
            await Task.Run(() => System.IO.Directory.Move(dirPath, newDirPath));
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError (ex,"Error from PhotoController=>UpdateFolder");
            return Results.Problem(ex.Message);
        }

        return Results.Ok();
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



using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;



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


    private bool IsValidFile(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        return !string.IsNullOrEmpty(extension) && permittedExtensions.Contains(extension);
    }



    [HttpPost("UploadPhoto")]
    public async Task<IActionResult> UploadPhoto([FromForm] PhotoAPIModel model)
    {
        try
        {
            // check if a file has been received
            if (model?.Photo == null || model.Photo.Length == 0)
            {
                return BadRequest("No file received.");
            }

            // check if the file is valid
            string fileName = model.Photo.FileName;
            if (!IsValidFile(fileName))
            {
                return BadRequest("Invalid file type.");
            }

            // create the directory if it doesn't exist
            string directoryPath = Path.Combine(_environment.ContentRootPath, "Photos", model.PhotoID.ToString());
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // generate a unique file name
            string filePath = Path.Combine(directoryPath, Path.GetRandomFileName());

            // save the file to the directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(stream);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            // return a 500 status code with the exception message
            return StatusCode(500, ex.Message);
        }
    }





    [HttpDelete("photos/{id}/{photoName}")]
    public async Task<IActionResult> DeletePhoto(int id, string photoName)
    {
        // Build the file path using the environment's ContentRootPath and the provided id and photoName

        string filePath = Path.Combine(_environment.ContentRootPath, "Photos", id.ToString(), $"{photoName}.jpg");


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
                _logger.LogError("404,An error occurred while delete photo");
                // Return a 404 Not Found if the file does not exist
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            // Return a 500 Internal Server Error if there is an exception
            _logger.LogError(ex, "500,An error occurred while delete photo.");
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("UpdateFolder/{id}")]
    public async Task<IActionResult> UpdateFolder(int id)
    {
        try
        {
            // Validate the input
            if (id <= 0)
                return BadRequest("Invalid folder id");

            // Construct the directory path
            var dirPath = Path.Combine(_environment.ContentRootPath, "Photos", "0");
            var newDirPath = Path.Combine(_environment.ContentRootPath, "Photos", id.ToString());

            if (!System.IO.Directory.Exists(dirPath))
                return NotFound($"Source folder {dirPath} does not exist");

            if (System.IO.Directory.Exists(newDirPath))
                return Conflict($"Target folder {newDirPath} already exists");

            // Move the folder
            await Task.Run(() => System.IO.Directory.Move(dirPath, newDirPath));
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "An error occurred while update foler name.");
            return StatusCode(500, ex.Message);
        }

        return Ok();
    }


    [HttpGet("GetPhotos/{id}")]
    public async Task<IActionResult> GetPhotos(int id)
    {
        // Check if the folder with the given ID exists
        string dirPath = Path.Combine(_environment.ContentRootPath, "Photos", id.ToString());
        if (!Directory.Exists(dirPath))
            return NotFound("Folder with ID " + id + " does not exist.");

        //try
        //{
            // Get all the files in the folder
            var files = Directory.GetFiles(dirPath);

            // Extract the file names from the full path
            var fileNames = files.Select(Path.GetFileName);

            return Ok(fileNames);
        //}
        //catch (Exception ex)
        //{
        //    // Log the exception and return a 500 Internal Server Error
        //    Console.WriteLine(ex);
        //    return StatusCode(500, "An error occurred while getting photos.");
        //}
    }




    [HttpGet("GetPhoto/{id}/{photoName}")]
    public IActionResult GetPhoto(int id, string photoName)
    {
        // Check if the file with the given ID and photoName exists
        string filePath = Path.Combine(_environment.ContentRootPath, "Photos", id.ToString(), photoName);
        if (!System.IO.File.Exists(filePath))
            return NotFound("The photo with name " + photoName + " and ID " + id + " does not exist.");

        try
        {
            // Open the image file
            var image = System.IO.File.OpenRead(filePath);

            // Get the file extension
            var fileExtension = Path.GetExtension(photoName).TrimStart('.');

            // Set the content type based on the file extension
            var contentType = "image/" + fileExtension;

            return File(image, contentType);
        }
        catch (Exception ex)
        {
            // Log the exception and return a 500 Internal Server Error
            
            _logger.LogError(ex, "An error occurred while getting the photo.");
            return StatusCode(500, "An error occurred while getting the photo.");
        }
    }
}
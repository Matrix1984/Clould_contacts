using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var MyAllowSpecificOrigins = "_MyAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_MyAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("localhost", "http://localhost:4200", "http://127.0.0.1:8080")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
});




var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images" // The URL path to access the files
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapPost("/Contact", async ([FromForm] InsertContactDto dto) =>
{
    var repo = new ContactRepo();

    if (dto.File != null)
        dto.ImagePath = await UploadFileAsync(dto.File);
    else
        dto.ImagePath = GetNoImage();

    int id = repo.InsertContact(dto);

    return repo.GetContactById(id);

}).DisableAntiforgery();



app.MapGet("/Contact", () =>
{
    var repo = new ContactRepo();

    return repo.GetAllContacts();
});


app.MapGet("/Contact/{id:int}", (int id) =>
{
    var repo = new ContactRepo();

    return repo.GetContactById(id);
});



app.MapPut("/Contact", async ([FromForm] UpdateContactDto dto) =>
{
    var repo = new ContactRepo();

    if (dto.File != null && String.IsNullOrEmpty(dto.ImagePath))
        dto.ImagePath = await UploadFileAsync(dto.File);
     

        repo.UpdateContactById(dto);

    return repo.GetContactById(dto.Id);

}).DisableAntiforgery();


app.MapDelete("/Contact/{id:int}", async (int id) =>
{ 
    var repo = new ContactRepo();
    repo.DeleteContact(id);
});


app.MapPost("/Contact/BulkInsert", async (List<InsertContactDto> dtos) =>
{
    var repo = new ContactRepo();

    foreach (var item in dtos)
    {
        repo.InsertContact(item);
    }

    return repo.GetAllContacts();
});

app.Run();



async Task<string> UploadFileAsync(IFormFile file)
{

    // !! Security Note: Never use the client-supplied file name directly for storage !!
    // Use a safe, unique name determined by the app to prevent path traversal attacks.
    var trustedFileName = Guid.NewGuid() + "_" + file.FileName;

    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", trustedFileName);

    // Ensure the directory exists
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Images"));

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream); // Copy the uploaded file stream to the new file stream
    }

    // You might save the trusted file name and original file name (for display) to a database
    return Configurations.SERVER_HOST + Configurations.IMAGE_UPLOAD_PATH + trustedFileName;
}

 string GetNoImage() =>
    Configurations.SERVER_HOST + Configurations.IMAGE_UPLOAD_PATH + "/No_image_available.svg.png";
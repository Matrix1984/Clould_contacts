public class InsertContactDto
{ 
    public string Name { get; set; }
    public string FullAddress { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Cell { get; set; }
    public string RegistrationDate { get; set; }

    public string ImagePath { get; set; }
    public IFormFile File { get; set; }

}

public class InsertContactOfflineDto
{
    public string Name { get; set; }
    public string FullAddress { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Cell { get; set; }
    public string RegistrationDate { get; set; }

    public string ImagePath { get; set; }
    public string File { get; set; }

}

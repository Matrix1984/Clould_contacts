using Dapper;
using Microsoft.Data.SqlClient; 

public class ContactRepo
{
    public IEnumerable<ContactBriefDto> GetAllContacts()
    {
        using var connection = new SqlConnection(Configurations.CONNECTION_STRING);

        var sql = "SELECT Id,Name,Email,Phone,ImagePath FROM Contact";

        IEnumerable<ContactBriefDto> contacts = connection.Query<ContactBriefDto>(sql);

        return contacts;
    }

    public ContactDto GetContactById(int contactId)
    {
        using var connection = new SqlConnection(Configurations.CONNECTION_STRING);

        var sql = "SELECT * FROM Contact WHERE Id=@id";

        var contact = connection.QuerySingle(sql, new { id = contactId });

        var contactDto = new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            FullAddress = contact.FullAddress,
            Email = contact.Email,
            Phone = contact.Phone,
            Cell = contact.Cell,
            RegistrationDate = contact.RegistrationDate,
            ImagePath = contact.ImagePath
        };
        return contactDto;
    }

    public void UpdateContactById(UpdateContactDto update)
    {
        using var connection = new SqlConnection(Configurations.CONNECTION_STRING);

        var sql = @"
UPDATE Contact SET
Name=@Name,
FullAddress=@FullAddress,
Email=@Email,
Phone=@Phone,
Cell=@Cell,
RegistrationDate=@RegistrationDate
 
";
        if(!string.IsNullOrEmpty(update.ImagePath))
            sql += ",ImagePath=@ImagePath";

        sql += " WHERE Id = @Id";
       

        connection.Execute(sql, update);
    }

    public int InsertContact(InsertContactDto insert)
    {
        using var connection = new SqlConnection(Configurations.CONNECTION_STRING);

        var sql = @" 
INSERT INTO Contact (Name,FullAddress,Email,Phone,Cell,RegistrationDate,ImagePath)
OUTPUT INSERTED.Id
VALUES (@Name,@FullAddress,@Email,@Phone,@Cell,@RegistrationDate,@ImagePath) 
";

        var id = connection.ExecuteScalar(sql, insert);

        return (int)id;
    }
 

    public int DeleteContact(int idd)
    {
        using var connection = new SqlConnection(Configurations.CONNECTION_STRING);

        var sql = @" 
DELETE FROM Contact WHERE Id=@id
";

        var id = connection.Execute(sql, new { id = idd });

        return (int)id;
    }
}
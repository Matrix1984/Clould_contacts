using System.Text.Json;


namespace TodoApi.Repositories.Contacts
{
    public class ContactFetcher
    {
        public async Task<Rootobject> GetRandomContactsAsync()
        {

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://randomuser.me/api/?results=10");
            response.EnsureSuccessStatusCode(); // Throws an exception if status code is not successful
            string responseBody = await response.Content.ReadAsStringAsync();

            Rootobject? randomUsers = JsonSerializer.Deserialize<Rootobject>(responseBody);

            return randomUsers;
        }


        public  List<InsertContactDto> ConvertToContacts(Rootobject rootobject)
        {
             var contacts = from n in rootobject.results
                                               select new InsertContactDto
                                               {
                                                   Name=n.name.first+ " "+n.name.last,
                                                   FullAddress=n.location.city + ", " + n.location.country,
                                                   Email=n.email,
                                                   Phone= n.phone,
                                                   Cell= n.cell,
                                                   RegistrationDate =DateTime.Now.ToString("yyyy-MM-dd"),
                                                   ImagePath= n.picture.medium
                                               };
            return contacts.ToList();
        }
    }
}
 
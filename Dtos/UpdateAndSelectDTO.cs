namespace TodoApi.Dtos
{
    public class UpdateAndSelectDTO
    {
        public List<InsertContactOfflineDto> InsertDtos { get; set; }
        public List<UpdateContactOfflineDto> UpdateDtos { get; set; }
        public List<int> DeletetIds { get; set; }
    }
}
 
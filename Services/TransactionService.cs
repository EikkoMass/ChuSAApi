using System.Text.Json;
using ChuSAApi.Domain.Entities;

namespace ChuSAApi.Services;

public class TransactionService
{
    public async Task<bool> isHoliday (DateTime date)
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://brasilapi.com.br/api/feriados/v1/" + DateTime.Now.Year);

        if (response.IsSuccessStatusCode)
        {
            var aa = response.Content.ReadAsStreamAsync();
            
            var content = await JsonSerializer.DeserializeAsync<List<Holidays>>(aa.Result);

            if (content != null)
            {
                return content.Find(holidays => date >= DateTime.Parse(holidays.date + " " + TimeOnly.MinValue) 
                                                && date <= DateTime.Parse(holidays.date + " " + TimeOnly.MaxValue)) != null;
            }
            
        }
        
        return false;
    }
}
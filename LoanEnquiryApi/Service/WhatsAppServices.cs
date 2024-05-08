using LoanEnquiryApi.Model.Enquiry;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class WhatsAppService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new();

    public WhatsAppService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendWhatsAppMessageAsync(List<ListRecommendedBankModel> recommendedBanks)
    {
        try
        {
            bool allMessagesSentSuccessfully = true;

            foreach (var responseData in recommendedBanks)
            {
                var cleanedContactNo = responseData.ContactNo.Replace(" ", "");
                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    recipient_type = "individual",
                    to = cleanedContactNo,
                    type = "template",
                    template = new
                    {
                        name = "atlas_advisory_message",
                        language = new
                        {
                            code = "en_US"
                        },
                        components = new object[]
                        {
                        new
                        {
                            type = "body",
                            parameters = new object[]
                            {
                                new { type = "text", text = responseData.BankName },
                                new { type = "text", text = $"https://atlasadv.com.sg/enquirystatus/{responseData.EnquiryId}" }
                            }
                        },
                        new
                        {
                            type = "button",
                            sub_type = "url",
                            index = 0,
                            parameters = new object[]
                            {
                                new { type = "text", text = $"{responseData.EnquiryId}" }
                            }
                        }
                        }
                    }
                };

                string? whatsappPhoneId = _configuration["VITE_WHATSAPP_PHONEID"];
                string? bearerToken = _configuration["VITE_WHATSAPP_TOKEN"];

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var response = await _httpClient.PostAsJsonAsync(
                    $"https://graph.facebook.com/v18.0/{whatsappPhoneId}/messages",
                    requestBody);

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                // If the response is not successful, set the flag to false
                if (!response.IsSuccessStatusCode)
                    allMessagesSentSuccessfully = false;
            }

            return allMessagesSentSuccessfully;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending WhatsApp message: {ex.Message}");
            throw; 
        }
    }
}
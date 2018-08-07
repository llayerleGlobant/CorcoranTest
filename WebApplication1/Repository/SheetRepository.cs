using CorcoranTest.WepApi.Dtos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CorcoranTest.WepApi.Repository
{
    /// <summary>
    /// GoogleSheet Repository
    /// </summary>
    public static class SheetRepository
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Clave de API 1";

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns>IEnumerable<PresidentInfoDto></returns>
        public static IEnumerable<PresidentInfoDto> GetAllData(string credentialsPath)
        {
            var result = new List<PresidentInfoDto>();
            UserCredential credential;


            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1i2qbKeasPptIrY1PkFVjbHSrLtKEPIIwES6m2l2Mdd8";
            String range = "A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);


            var response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var dto = new PresidentInfoDto()
                    {
                        President = row[0].ToString(),
                        Birthplace = row[2].ToString(),
                        Birthday = row[1].ToString() != null ? DateTime.Parse(row[1].ToString()) : default(DateTime),
                        Deathplace = row.Count > 3 ? row[4].ToString() : "",
                        Deathday = row.Count > 3 ? 
                                   row[3].ToString() != null ? 
                                        DateTime.Parse(row[3].ToString()) : 
                                        default(DateTime) : 
                                   default(DateTime)
                    };

                    result.Add(dto);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            return result;
        }
    }
}

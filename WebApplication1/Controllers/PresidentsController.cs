using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CorcoranTest.WepApi.Dtos;
using CorcoranTest.WepApi.Enums;
using CorcoranTest.WepApi.Repository;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CorcoranTest.WepApi.Controllers
{


    /// <summary>
    /// Presidents Controller
    /// </summary>
    public class PresidentsController : Controller
    {

        string credentialPath;
        private IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Controller Constructor
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public PresidentsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            credentialPath = Path.GetFullPath(Path.Combine(_hostingEnvironment.ContentRootPath, @"GoogleConfigurations/credentials.json"));
        }

        /// <summary>
        /// Get All Presidents in GoogleSheet
        /// </summary>
        /// <returns></returns>
        [Route("api/president/GetAll")]
        [HttpGet]
        public IEnumerable<PresidentInfoDto> Get()
        {
            return SheetRepository.GetAllData(credentialPath);
        }

        /// <summary>
        /// Get President by Name
        /// </summary>
        /// <param name="name">Name to search</param>
        /// <returns></returns>
        [Route("api/president/GetByName/{name}")]
        [HttpGet]
        public IEnumerable<PresidentInfoDto> GetByName(string name)
        {
            return SheetRepository
                .GetAllData(credentialPath)
                .Where(x => x.President.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Get all presidents ordered by Birth Date and Death Date
        /// </summary>
        /// <param name="birthDate">Order for Birth Date</param>
        /// <param name="deathDate">Order for Dearth Date</param>
        /// <returns></returns>
        [Route("api/president/GetAllSorted")]
        [HttpGet]
        public IEnumerable<PresidentInfoDto> GetAllSorted(SortOrder birthDate = SortOrder.Asc, SortOrder deathDate = SortOrder.Asc)
        {
            var result= SheetRepository.GetAllData(credentialPath);
            IOrderedEnumerable<PresidentInfoDto> order;

            if (birthDate == SortOrder.Asc)
                order = result.OrderBy(x => x.Birthday);
            else
                order = result.OrderByDescending(x => x.Birthday);

            if (birthDate == SortOrder.Asc)
                order = order.ThenBy(x => (x.Deathday ?? DateTime.MaxValue));
            else
                order = order.ThenBy(x => (x.Deathday ?? DateTime.MinValue));

            return result.ToList();
        }
    }
}
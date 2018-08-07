using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorcoranTest.WepApi.Dtos
{
    /// <summary>
    /// President Info Dto
    /// </summary>
    public class PresidentInfoDto
    {
        /// <summary>
        /// President Name
        /// </summary>
        public string President { get; set; }
        /// <summary>
        /// Birth Day
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// Birth Place
        /// </summary>
        public string Birthplace { get; set; }
        /// <summary>
        /// Death Day
        /// </summary>
        public DateTime? Deathday { get; set; }
        /// <summary>
        /// Death Place
        /// </summary>
        public string Deathplace { get; set; }
    }
}

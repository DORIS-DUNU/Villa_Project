﻿using System.Net;

namespace VilllaParks.Model.Dto
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessage = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;

        public List<string> ErrorMessage { get; set; }

        public object Result { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ProjetWeb.Models.DTO
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }
        
        public BaseResponse(T data)
        {
            Data = data;
        }
        
        public T Data { get; set; }
        
        public List<string> Errors { get; set; } = new List<string>();
    }
}
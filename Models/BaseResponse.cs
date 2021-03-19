using System;
using System.Collections.Generic;

namespace ProjetWeb.Models
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }
        
        public BaseResponse(T data)
        {
            Data = data;
        }
        
        public T Data { get; set; }
        
        public List<Exception> Errors { get; set; } = new List<Exception>();
    }
}
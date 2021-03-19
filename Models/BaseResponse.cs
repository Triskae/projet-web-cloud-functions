using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace ProjetWeb.Models
{
    public class Response<T>
    {
        public Response(T data)
        {
            Data = data;
        }
        
        public T Data { get; set; }
        
        public List<Error> Errors { get; set; } = new List<Error>();
        
        public void AddError(string code, string description, Exception e)
        {
            Errors.Add(new Error
            {
                Code = code,
                Description = description,
                Exception = e
            });
        }

    }

    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public Exception Exception { get; set; }
    }
}
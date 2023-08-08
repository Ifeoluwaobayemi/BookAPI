using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookApi.Core.DTOs
{
    public class Response<TData, TError>
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public bool Status { get; set; }
        public TError Errors { get; set; }
        public TData Data { get; set; }
    }
}

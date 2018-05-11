using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class PaginModel
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
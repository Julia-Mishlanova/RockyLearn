using Microsoft.AspNetCore.Hosting;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers.Interfaces
{
    public interface IMailTemplater
    {
        string GetMessageBody(ProductUserVM productUserVM, IWebHostEnvironment webHostEnvironment);
    }
}

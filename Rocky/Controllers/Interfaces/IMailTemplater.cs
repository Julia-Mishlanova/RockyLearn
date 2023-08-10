using Microsoft.AspNetCore.Hosting;
using Rocky_Models.ViewModels;

namespace Rocky.Controllers.Interfaces
{
    public interface IMailTemplater
    {
        string GetMessageBody(ProductUserVM productUserVM, IWebHostEnvironment webHostEnvironment);
    }
}

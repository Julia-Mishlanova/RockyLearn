using Microsoft.AspNetCore.Hosting;
using Rocky_Models.ViewModels;

namespace Rocky.Interfaces
{
    public interface IMailTemplater
    {
        string GetMessageBody(ProductUserVM productUserVM, IWebHostEnvironment webHostEnvironment);
    }
}

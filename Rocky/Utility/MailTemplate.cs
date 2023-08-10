using MailKit;
using Microsoft.AspNetCore.Hosting;
using Rocky.Interfaces;
using Rocky_Models.ViewModels;
using System.IO;
using System.Text;

namespace Rocky.Utility
{
    public class MailTemplate : IMailTemplater
    {
        public string GetMessageBody(ProductUserVM ProductUserVM, IWebHostEnvironment webHostEnvironment)
        {
            var PathToTemplate = webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products: {3}

            StringBuilder productListSB = new();
            foreach (var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            return messageBody;
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Rocky.Interfaces;
using Rocky.Utility.BrainTree;
using Rocky_DataAccess;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Models.ViewModels;
using System.Linq;

namespace Rocky.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepos;
        private readonly IOrderDetailRepository _orderDetailRepos;
        private readonly IMailSender _mailSender;
        private readonly IMailTemplater _mailTemplater;
        private readonly IMessageFormater _messageFormater;
        private readonly IBrainTreeGate _brain;

        public ProductUserVM ProductUserVM { get; set; }
        public OrderController(
            IOrderHeaderRepository orderHeaderRepos,
            IOrderDetailRepository orderDetailRepos,
            IMailSender mailSender,
            IMailTemplater mailTemplater,
            IMessageFormater messageFormater,
            IBrainTreeGate brain)
        {
            
            _orderHeaderRepos = orderHeaderRepos;
            _orderDetailRepos = orderDetailRepos;
            _mailSender = mailSender;
            _mailTemplater = mailTemplater;
            _messageFormater = messageFormater;
            _brain = brain;
        }
        public IActionResult Index()
        {
            OrderListVM orderListVM = new OrderListVM()
            {
                OrderHeaderList = _orderHeaderRepos.GetAll(),
                StatusList = WC.ListStatus.ToList().Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem 
                { 
                    Text = u, Value = u
                })
            };
            return View(orderListVM);
        }
    }
}

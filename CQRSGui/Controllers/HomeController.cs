using System;
using System.Web.Mvc;

using Lokad.Cqrs;

using SimpleCQRS.Messages;
using SimpleCQRS.ReadModel;

namespace CQRSGui.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IMessageSender bus;
        private readonly IReadModelFacade readmodel;

        public HomeController()
        {
            bus = ServiceLocator.Bus;
            readmodel = ServiceLocator.ReadModel;
        }

        public ActionResult Index()
        {
            ViewData.Model = readmodel.GetInventoryItems();

            return View();
        }

        public ActionResult Details(Guid id)
        {
            ViewData.Model = readmodel.GetInventoryItemDetails(id);
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
            bus.SendOne(new CreateInventoryItem(Guid.NewGuid(), name));

            return RedirectToAction("Index");
        }

        public ActionResult ChangeName(Guid id)
        {
            ViewData.Model = readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(Guid id, string name, int version)
        {
            var command = new RenameInventoryItem(id, name, version);
            bus.SendOne(command);

            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(Guid id, int version)
        {
            bus.SendOne(new DeactivateInventoryItem(id, version));
            return RedirectToAction("Index");
        }

        public ActionResult CheckIn(Guid id)
        {
            ViewData.Model = readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(Guid id, int number, int version)
        {
            bus.SendOne(new CheckInItemsToInventory(id, number, version));
            return RedirectToAction("Index");
        }

        public ActionResult Remove(Guid id)
        {
            ViewData.Model = readmodel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult Remove(Guid id, int number, int version)
        {
            bus.SendOne(new RemoveItemsFromInventory(id, number, version));
            return RedirectToAction("Index");
        }
    }
}

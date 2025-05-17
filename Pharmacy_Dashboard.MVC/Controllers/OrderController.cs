using AutoMapper;
using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Specifications.PharmacySpecifications;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_Dashboard.MVC.ViewModels.OrderViewModels;

namespace Pharmacy_Dashboard.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(OrderPageParams orderPageParams)/*(int pageNumber = 1, int pharmacyId = 1, OrderStatus? orderStatusFilter = null, DateTime? dateFilter = null)*/
        {
            // TODO : get registerd pharmacy Id
            if (!ModelState.IsValid)
            {
                return View(new OrdersListViewModel
                {
                    Orders = new List<OrderContent>(),
                    CurrentPage = 1,
                    TotalPages = 1,
                    CurrentFilter = new OrderFilter()
                });
            }

            int pageSize = 20;
            var ordersSpecs = new OrdersSpecifications(orderPageParams.pharmacyId, orderPageParams.pageNumber, pageSize, orderPageParams.orderStatusFilter, orderPageParams.dateFilter);
            var orders = await _unitOfWork.Repository<PharmacyOrder>().GetAllWithSpecAsync(ordersSpecs);
            var count = await _unitOfWork.Repository<PharmacyOrder>().GetCountAsync(new OrdersSpecifications(1, orderPageParams.orderStatusFilter, orderPageParams.dateFilter));

            if (orders is null)
                ModelState.AddModelError("", "Orders Is Empty");

            var viewModel = new OrdersListViewModel
            {
                Orders = _mapper.Map<List<OrderContent>>(orders),
                CurrentPage = orderPageParams.pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                CurrentFilter = new OrderFilter
                {
                    OrderStatus = orderPageParams.orderStatusFilter,
                    DateFilter = orderPageParams.dateFilter
                }
            };

            // return it to view
            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(int id)
        {
            // get the order from db
            var orderSpecs = new OrderWithMedicinesSpecs(id);
            var order = await _unitOfWork.Repository<PharmacyOrder>().GetWithSpecsAsync(orderSpecs);

            // map the order
            var viewModel = _mapper.Map<OrderCardViewModel>(order);

            // return
            return View("Card", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdatedOrderParams updatedOrder)
        {
            var medicinesDic = new Dictionary<int, int>();

            // load order from db 
            var order = await _unitOfWork.Repository<PharmacyOrder>().GetAsync(updatedOrder.Id);

            // update its status and delivery date
            order.Status = updatedOrder.OrderStatus;
            if (updatedOrder.OrderStatus == OrderStatus.Confirmed)
                order.DeliverDate = updatedOrder.DeliveryDate;

            // if status will be delivered
            // Decrease the medicines from stock table 
            if (updatedOrder.OrderStatus == OrderStatus.Completed)
                // get medicines,quantity from the order
                foreach (var item in order.MedicinePharmacyOrders)
                {
                    medicinesDic.Add(item.MedicineId, item.Quantity);
                }

                // Decrease it from the stock



            // save it 
            _unitOfWork.Repository<PharmacyOrder>().Update(order);
            await _unitOfWork.Repository<PharmacyOrder>().SaveAsync();

            TempData["Message"] = $"Order #{order.Id} has been {order.Status.ToString().ToLower()}.";

            // return to Orders Table
            return RedirectToAction("GetOrder", new { id = order.Id });
        }
    }
}

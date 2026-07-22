using Microsoft.AspNetCore.Mvc;
using Services.Api;
using Services.Models;

namespace MvcApp.Controllers
{
    public class GameController : Controller
    {
        private readonly IServiceGame service;

        public GameController(IServiceGame service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken token)
        {
            var response = await service.DisplayCreatePageAsync(token);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GameCreateRequest request, CancellationToken token)
        {
            await service.CreateAsync(request, token);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Update(GameDisplayUpdateRequest request, CancellationToken token)
        {
            var response = await service.DisplayUpdatePageAsync(request, token);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GameUpdateRequest request, CancellationToken token)
        {
            await service.UpdateAsync(request, token);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(GameDisplayDeleteRequest request, CancellationToken token)
        {
            var response = await service.DisplayDeletePageAsync(request, token);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GameDeleteRequest request, CancellationToken token)
        {
            await service.DeleteAsync(request, token);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Details(GameGetByRequest request, CancellationToken token)
        {
            var response = await service.GetByAsync(request, token);
            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> List(CancellationToken token)
        {
            var response = await service.GetAllAsync(token);
            return View(response);
        }
    }
}

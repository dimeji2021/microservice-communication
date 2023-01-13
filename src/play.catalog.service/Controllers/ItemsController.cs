using Microsoft.AspNetCore.Mvc;
using play.catalog.service.Dtos;
using play.catalog.service.Entities;
using play.catalog.service.Repositories;

namespace play.catalog.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //[Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok((await _itemsRepository.GetItemsAsync()).Select(item => item.AsDto()));
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok((await _itemsRepository.GetItemsAsync(id)).AsDto());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreatedItemDto createdItemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createdItemDto.Name,
                Description = createdItemDto.Description,
                Price = createdItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetAsync), new { id = item.Id }, item);
        }
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id, UpdatedItemDto updatedItemDto)
        {
            var existingItem = await _itemsRepository.GetItemsAsync(id);
            if (existingItem is not null)
            {
                existingItem.Name = updatedItemDto.Name;
                existingItem.Description = updatedItemDto.Description;
                existingItem.Price = updatedItemDto.Price;
                await _itemsRepository.UpdateAsync(existingItem);
                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingItem = await _itemsRepository.GetItemsAsync(id);
            if (existingItem is null) return NotFound();
            await _itemsRepository.RemoveAsync(existingItem.Id);
            return NoContent();
        }
    }
}
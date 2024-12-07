// This controller is purely for testing and has nothing to do with the actual project

using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/test/[controller]")]
    public class TestController : ControllerBase
    {
        // See this as a mock database, this is in memory only
        private static List<TestModel> _testItems = new List<TestModel>();

        // GET: Retrieve all items
        [HttpGet]
        public ActionResult<IEnumerable<TestModel>> GetAll()
        {
            return Ok(_testItems);
        }

        // POST: Add a new item
        [HttpPost]
        public ActionResult<TestModel> Create(TestModel newItem)
        {
            if (newItem == null || string.IsNullOrWhiteSpace(newItem.Name))
            {
                return BadRequest("Invalid data.");
            }

            newItem.Id = Guid.NewGuid();
            _testItems.Add(newItem);

            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        // GET by ID: Retrieve a single item
        [HttpGet("{id}")]
        public ActionResult<TestModel> GetById(Guid id)
        {
            var item = _testItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            return Ok(item);
        }

        // PUT: Update an existing item
        [HttpPut("{id}")]
        public ActionResult Update(Guid id, TestModel updatedItem)
        {
            var existingItem = _testItems.FirstOrDefault(x => x.Id == id);
            if (existingItem == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            existingItem.Name = updatedItem.Name;
            return NoContent();
        }

        // DELETE: Remove an item
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var item = _testItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            _testItems.Remove(item);
            return NoContent();
        }
    }

    // Model
    public class TestModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

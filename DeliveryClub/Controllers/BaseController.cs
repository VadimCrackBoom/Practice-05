using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T> : ControllerBase where T : class
{
    protected static List<T> _items = new List<T>();
    protected static int _nextId = 1;

    protected virtual int GetId(T item) => 0;
    protected virtual void SetId(T item, int id) { }

    [HttpGet]
    public ActionResult<IEnumerable<T>> GetAll()
    {
        return Ok(_items);
    }

    [HttpGet("{id}")]
    public ActionResult<T> GetById(int id)
    {
        var item = _items.FirstOrDefault(i => GetId(i) == id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<T> Create(T item)
    {
        SetId(item, _nextId++);
        _items.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = GetId(item) }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, T item)
    {
        var existingItem = _items.FirstOrDefault(i => GetId(i) == id);
        if (existingItem == null) return NotFound();

        _items.Remove(existingItem);
        SetId(item, id);
        _items.Add(item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _items.FirstOrDefault(i => GetId(i) == id);
        if (item == null) return NotFound();

        _items.Remove(item);
        return NoContent();
    }
}
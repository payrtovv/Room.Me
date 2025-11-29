using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Me.Data;
using Room.Me.Dtos;
using System.Threading.Tasks;
namespace Room.Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private static readonly Dictionary<string, List<object>> _tags = new()
        {
            ["personality"] = new List<object>
            {
                new { id = 1, label = "Extrovertido", value = "extrovert" },
                new { id = 2, label = "Ambivertido", value = "ambivert" },
                new { id = 3, label = "Introvertido", value = "introvert" }
            },
            ["schedule"] = new List<object>
            {
                new { id = 4,  label = "Madrugador", value = "early_bird" },
                new { id = 5,  label = "Horario Flexible", value = "flexible" },
                new { id = 6,  label = "Nocturno", value = "night_owl" }
            },
            ["cleanliness"] = new List<object>
            {
                new { id = 7,  label = "Super Ordenado", value = "neat" },
                new { id = 8,  label = "Orden Normal", value = "average" },
                new { id = 9,  label = "Desordenado", value = "messy" }
            },
            ["pets"] = new List<object>
            {
                new { id = 10, label = "Tengo Mascotas", value = "has_pets" },
                new { id = 11, label = "Acepto Mascotas", value = "ok_with" },
                new { id = 12, label = "Cero Mascotas", value = "none" }
            },
            ["visits"] = new List<object>
            {
                new { id = 13, label = "Casa de Fiesta", value = "party_house" },
                new { id = 14, label = "Visitas Moderadas", value = "occasional" },
                new { id = 15, label = "Sin Visitas", value = "private" }
            },
            ["habits"] = new List<object>
            {
                new { id = 16, label = "Fumador", value = "smoker" },
                new { id = 17, label = "Fumo afuera", value = "outside_only" },
                new { id = 18, label = "No fumador", value = "non_smoker" }
            }
        };

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                message = "Catálogo de tags obtenido correctamente",
                data = _tags
            });
        }

        // POST para agregar categoría o agregar items a una categoría existente
        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody] AddCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Category))
                return BadRequest(new { message = "La categoría es obligatoria." });

            if (request.Items == null || request.Items.Count == 0)
                return BadRequest(new { message = "Debe agregar al menos un item." });

            // Si la categoría no existe, se crea
            if (!_tags.ContainsKey(request.Category))
            {
                _tags[request.Category] = new List<object>();
            }

            // Generar IDs continuos
            int nextId = _tags.Values.Sum(list =>
                list.Count > 0 ? list.Count : 0
            ) + 1;

            foreach (var item in request.Items)
            {
                _tags[request.Category].Add(new
                {
                    id = nextId++,
                    label = item.Label,
                    value = item.Value
                });
            }

            return Ok(new
            {
                message = "Categoría/Items agregados correctamente",
                data = _tags[request.Category]
            });
        }
    }

    public class AddCategoryRequest
    {
        public string Category { get; set; } = "";
        public List<AddTagItem> Items { get; set; } = new();
    }

    public class AddTagItem
    {
        public string Label { get; set; } = "";
        public string Value { get; set; } = "";
    }
}

using Academia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academia.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly Academia.Data.ApplicationDbContext _context;

        public CreateModel(Academia.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FormaPagamentoId"] = new SelectList(_context.FormaPagamento, "Id", "Nome");
        ViewData["PersonalId"] = new SelectList(_context.Personal, "Id", "Nome");

            // BUSCA PELA ULTIMA MATRICULA CADASTRADA NO BANCO DE DADOS
            var ultimoCliente = _context.Cliente.OrderByDescending(c => c.Matricula).FirstOrDefault();

            if (ultimoCliente != null)
            {
                
                if (int.TryParse(ultimoCliente.Matricula, out int ultimoNumeroMatricula))
                {
                    ultimoNumeroMatricula++; 
                    Cliente = new Cliente { Matricula = ultimoNumeroMatricula.ToString() };
                }
                else
                {
                    Cliente = new Cliente { Matricula = "1" };
                }
            }
            else
            {
                Cliente = new Cliente { Matricula = "1" };
            }

            return Page();
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Cliente.Add(Cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

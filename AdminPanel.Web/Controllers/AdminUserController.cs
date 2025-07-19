// Controllers/AdminUserController.cs
using AdminPanel.DAL.Context;
using AdminPanel.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class AdminUserController : Controller
{
    private readonly AppDbContext _context;

    public AdminUserController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kullanıcı Yönetimi";
        return View(await _context.Users.ToListAsync());
    }
    public IActionResult Create()
    {
        ViewData["Title"] = "Yeni Kullanıcı Oluştur";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Email,Password")] User user)
    {
        if (ModelState.IsValid)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            TempData["successMessage"] = "Kullanıcı başarıyla eklendi!"; // <-- Bu satır var ve doğru
            return RedirectToAction(nameof(Index));
        }
        TempData["errorMessage"] = "Kullanıcı eklenirken bir hata oluştu. Lütfen formu kontrol edin."; // <-- Bu satır da var
        ViewData["Title"] = "Yeni Kullanıcı Oluştur";
        return View(user);
    }

    // GET: /AdminUser/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        ViewData["Title"] = "Kullanıcı Düzenle";
        return View(user);
    }

    // POST: /AdminUser/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["successMessage"] = "Kullanıcı bilgileri başarıyla güncellendi!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    TempData["errorMessage"] = "Kullanıcı bulunamadı veya başka bir kullanıcı tarafından silinmiş olabilir.";
                    return NotFound();
                }
                else
                {
                    TempData["errorMessage"] = "Kullanıcı güncellenirken bir çakışma oluştu. Lütfen tekrar deneyin.";
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        TempData["errorMessage"] = "Kullanıcı güncellenirken bir hata oluştu. Lütfen formu kontrol edin.";
        ViewData["Title"] = "Kullanıcı Düzenle";
        return View(user);
    }

    // GET: /AdminUser/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        ViewData["Title"] = "Kullanıcıyı Sil";
        return View(user);
    }

    // POST: /AdminUser/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["successMessage"] = "Kullanıcı başarıyla silindi!";
        }
        else
        {
            TempData["errorMessage"] = "Silinecek kullanıcı bulunamadı.";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
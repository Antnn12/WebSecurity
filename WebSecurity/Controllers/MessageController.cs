using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSecurity_Anton;
using WebSecurity_Anton.Models;

namespace WebSecurity.Controllers
{
    public class MessageController : Controller
    {
        private readonly SqlContext _context;
        private string[] _tags = new string[] { "<b>", "</b>", "<i>", "</i>" };

        public MessageController(SqlContext context)
        {
            _context = context;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }

            return View(messageEntity);
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,Name")] MessageEntity messageEntity)
        {
            if (ModelState.IsValid)
            {
                string encodedContent = HttpUtility.HtmlEncode(messageEntity.Message);
                foreach(var tag in _tags)
                {
                    var encodedTag = HttpUtility.HtmlEncode(tag);
                    encodedContent = encodedContent.Replace(encodedTag, tag);
                }
                messageEntity.Message = encodedContent;
                _context.Add(messageEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(messageEntity);
        }
        

        // GET: Message/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageEntity = await _context.Messages.FindAsync(id);
            if (messageEntity == null)
            {
                return NotFound();
            }
            return View(messageEntity);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,Name")] MessageEntity messageEntity)
        {
            if (id != messageEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(messageEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageEntityExists(messageEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(messageEntity);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageEntity == null)
            {
                return NotFound();
            }

            return View(messageEntity);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var messageEntity = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(messageEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageEntityExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}

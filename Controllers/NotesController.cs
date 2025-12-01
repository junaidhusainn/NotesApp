using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    public class NotesController(NotesDbContext context) : Controller
    {
        // List all notes in reverse chronological order
        public async Task<IActionResult> Index()
        {
            var notes = await context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return View(notes);
        }

        // Show create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save new note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            if (!ModelState.IsValid)
            {
                return View(note);
            }

            note.CreatedAt = DateTime.Now;
            context.Add(note);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var note = id.HasValue ? await context.Notes.FindAsync(id.Value) : null;
            return note is not null ? View(note) : NotFound();
        }

        // Update note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Note note)
        {
            if (!ModelState.IsValid) return View(note);

            var existingNote = await context.Notes.FindAsync(id);
            if (existingNote is null) return NotFound();

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.Priority = note.Priority;
            existingNote.UpdatedAt = DateTime.Now;

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Delete note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await context.Notes.FindAsync(id);
            if (note is not null)
            {
                context.Notes.Remove(note);
                await context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}

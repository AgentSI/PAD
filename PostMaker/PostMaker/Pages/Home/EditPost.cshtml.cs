using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditPostModel : PageModel
{
    private readonly PostContext _context;

    public EditPostModel(PostContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Post Post { get; set; }

    public void OnGet(int id)
    {
        Post = _context.Post.Find(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingPost = _context.Post.Find(Post.Id);

        if (existingPost == null)
        {
            return NotFound();
        }

        existingPost.Author = Post.Author;
        existingPost.Content = Post.Content;
        existingPost.Created = DateTime.Now;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var deletingPost = _context.Post.Find(Post.Id);

        if (deletingPost == null)
        {
            return NotFound();
        }

        _context.Post.Remove(deletingPost);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

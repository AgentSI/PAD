using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CreatePostModel : PageModel
{
    private readonly PostContext _context;

    public CreatePostModel(PostContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Post Post { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Post.Add(Post);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

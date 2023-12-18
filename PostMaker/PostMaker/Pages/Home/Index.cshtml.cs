using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class IndexModel : PageModel
{
    private readonly PostContext _context;

    public IndexModel(PostContext context)
    {
        _context = context;
    }

    public IList<Post> Posts { get;set; }

    public async Task OnGetAsync()
    {
        Posts = await _context.Post.AsNoTracking().ToListAsync();
    }
}

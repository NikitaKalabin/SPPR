using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_253503_Kalabin.UI.TagHelpers;

[HtmlTargetElement("pager")]
public class PagerTagHelper : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HtmlAttributeName("current-page")]
    public int CurrentPage { get; set; }

    [HtmlAttributeName("total-pages")]
    public int TotalPages { get; set; }

    [HtmlAttributeName("category")]
    public string? Category { get; set; }

    [HtmlAttributeName("admin")]
    public bool Admin { get; set; } = false;
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.Add("class", "pagination");

        if (CurrentPage > 1)
        {
            output.Content.AppendHtml(CreatePageItem(CurrentPage - 1, "Previous"));
        }

        for (int i = 1; i <= TotalPages; i++)
        {
            output.Content.AppendHtml(CreatePageItem(i, i.ToString()));
        }

        if (CurrentPage < TotalPages)
        {
            output.Content.AppendHtml(CreatePageItem(CurrentPage + 1, "Next"));
        }
    }
    
    private TagBuilder CreatePageItem(int page, string text)
    {
        var li = new TagBuilder("li");
        li.AddCssClass("page-item");
        if (page == CurrentPage)
        {
            li.AddCssClass("active");
        }

        var a = new TagBuilder("span");
        a.AddCssClass("page-link");
        a.Attributes["href"] = GeneratePageLink(page);
        a.InnerHtml.Append(text);

        li.InnerHtml.AppendHtml(a);
        return li;
    }
    
    private string GeneratePageLink(int page)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null.");
        }
        
        var values = new RouteValueDictionary
        {
            { "pageNo", page }
        };

        if (!string.IsNullOrEmpty(Category))
        {
            values["category"] = Category;
        }

        string? url = _linkGenerator.GetPathByAction(
            action: "Index",
            controller: "Clothes",
            values: values,
            httpContext: httpContext);


        return url ?? "#";
    }
}
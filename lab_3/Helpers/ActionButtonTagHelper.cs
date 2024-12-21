using Microsoft.AspNetCore.Razor.TagHelpers;

namespace lab_3.Helpers
{
    public class ActionButtonTagHelper : TagHelper
    {
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string RouteId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"/{Controller}/{Action}/{RouteId}");
            output.Attributes.SetAttribute("class", CssClass);
            output.Content.SetContent(Text);
        }
    }
}

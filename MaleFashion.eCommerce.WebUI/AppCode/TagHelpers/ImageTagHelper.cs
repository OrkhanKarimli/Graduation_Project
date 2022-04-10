using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.TagHelpers
{
    [HtmlTargetElement("image")]
    public class ImageTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-class")]
        public string CssClass { get; set; }

        [HtmlAttributeName("asp-path")]
        public string Path { get; set; }

        [HtmlAttributeName("asp-for")]
        public string Name { get; set; }

        [HtmlAttributeName("asp-caption")]
        public string Caption { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (!string.IsNullOrWhiteSpace(CssClass))
                output.Attributes.Add("class", CssClass);

            bool isReadnly = false;

            if (output.Attributes.ContainsName("readonly"))
            {
                isReadnly = true;
            }

            output.Content.SetHtmlContent($"<label class='control-label'>{Caption}</label>" +
                $"<label class='image-input' for='{Name}' {(string.IsNullOrWhiteSpace(Path) ? "" : $"data-imgurl='{Path}'")}>" +
                $"<input type='hidden' name='{Name}Temp' value='{(System.IO.Path.GetFileName(Path) ?? "")}' />" +
                (isReadnly ? "" : $"<span>&times;</span>") +
                $"</label>" +
                (isReadnly ? "" : $"<input name='{Name}' id='{Name}' type='file' accept='image/x-png,image/gif,image/jpeg' />"));

        }
    }


    /*
     <div class='form-group col-sm-12'>
                                <label id='blogFile-viewer' for='blogFile' data-imgurl='/uploads/images/@Model.ImagePath'>
                                    <input type='hidden' asp-for='ImagePathTemp' value='@Model.ImagePath' />
                                    <span>&times;</span>
                                </label>
                                <input type='file' name='blogImage' accept='image/x-png,image/gif,image/jpeg' id='blogFile' />
                            </div>
     */
}

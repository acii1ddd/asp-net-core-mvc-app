using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace lab_3.Helpers
{
    public static class MenuHelper
    {
        public static IHtmlContent RenderMenu(this IHtmlHelper htmlHelper, ClaimsPrincipal user)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("navbar-nav flex-grow-1");

            // добавляем пункты меню в зависимости от роли пользователя
            if (user.IsInRole("Passenger"))
            {
                ul.InnerHtml.AppendHtml(CreateMenuItem("Билеты", "Ticket", "Index"));
                ul.InnerHtml.AppendHtml(CreateMenuItem("Поезда", "Train", "Index"));
            }
            if (user.IsInRole("Manager"))
            {
                ul.InnerHtml.AppendHtml(CreateMenuItem("Пассажиры", "Passenger", "Index"));
                ul.InnerHtml.AppendHtml(CreateMenuItem("Билеты", "Ticket", "Index"));
                ul.InnerHtml.AppendHtml(CreateMenuItem("Поезда", "Train", "Index"));
            }
            if (user.IsInRole("Boss"))
            {
                ul.InnerHtml.AppendHtml(CreateMenuItem("Пользователи", "User", "Index"));
            }
            return ul;
        }

        private static TagBuilder CreateMenuItem(string text, string controller, string action)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("nav-item");

            var a = new TagBuilder("a");
            a.AddCssClass("nav-link text-dark btn");
            a.Attributes["href"] = "/" + controller + "/" + action;
            a.InnerHtml.Append(text);

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}

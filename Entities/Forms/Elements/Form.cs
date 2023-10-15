﻿using Microsoft.AspNetCore.Html;

namespace CMPSC487W_Project2.Entities.Forms.Elements
{
    public class Form : HtmlElement
    {
        public Form(IHtmlContent content, string id, string action, bool isAjax = false)
            : base("form", content, id, null)
        {
            FormAction = action;
            IsAjax = isAjax;
        }

        private string FormAction { get; set; }
        private bool IsAjax { get; set; }
        public override string GetUniqueAttributes()
        {
            string attrs = $"action=\"{FormAction}\" method=\"post\" ";
            if (IsAjax)
                attrs += "data-form=\"ajaxform\" ";
            return attrs;
        }
    }
}